using Cataract_Care.Data;
using Cataract_Care.Models;
using Cataract_Care.Models.VM;
using Cataract_Care.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Cataract_Care.Models.DTOs;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;


namespace Cataract_Care.Controllers
{
    public class UserSubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;
        private readonly UserManager<ApplicationUser> _userManager;
        IWebHostEnvironment environment;
        public UserSubscriptionController(ApplicationDbContext context,IFileService fileService, UserManager<ApplicationUser> userManager, IWebHostEnvironment env) {
            _context = context;
            _fileService = fileService;
            _userManager = userManager;
            environment = env;
        }
       
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            var userSubscription = await _context.UserSubscriptions
                .Include(us => us.Subscription)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (userSubscription != null)
            {
                if (userSubscription.ExpiresAt != null && userSubscription.ExpiresAt < DateTime.UtcNow)
                {
                    _context.UserSubscriptions.Remove(userSubscription);
                    await _context.SaveChangesAsync();
                    userSubscription = null;
                }

                else if (userSubscription.PhotoLimit != null && userSubscription.PhotoLimit <= 0)
                {
                    _context.UserSubscriptions.Remove(userSubscription);
                    await _context.SaveChangesAsync();
                    userSubscription = null;
                }
            }

            ViewData["Package"] = userSubscription;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserSubscriptionId,PatientName,Age,Gender,Eye,EyeImage")] DiagnosisVM diagnosis)
        {
            if (ModelState.IsValid)
            {
                // Fetch the current user
                var user = await _userManager.GetUserAsync(User);
                var userId = user?.Id;
                if (userId == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction(nameof(Index));
                }

                // Fetch the subscription package
                var subscribedPackage = await _context.UserSubscriptions.FirstOrDefaultAsync(x => x.UserId == userId);
                if (subscribedPackage == null)
                {
                    TempData["ErrorMessage"] = "No active subscription found.";
                    return RedirectToAction(nameof(Index));
                }

                // Check and update photo limit
                if (subscribedPackage.PhotoLimit != null)
                {
                    if (subscribedPackage.PhotoLimit <= 0)
                    {
                        TempData["ErrorMessage"] = "Photo limit exceeded.";
                        return RedirectToAction(nameof(Index));
                    }
                    subscribedPackage.PhotoLimit -= 1;
                    _context.UserSubscriptions.Update(subscribedPackage);
                }

                // Create Diagnosis model
                Diagnosis diagnosisModel = new Diagnosis()
                {
                    PatientName = diagnosis.PatientName,
                    Age = diagnosis.Age,
                    Gender = diagnosis.Gender,
                    Eye = diagnosis.Eye,
                    UserId = userId
                };

                // Save image
                if (diagnosis.EyeImage != null)
                {
                    var result = _fileService.SaveImage(diagnosis.EyeImage, "diagnosis_images");
                    if (result.Item1 == 1)
                    {
                        diagnosisModel.Image = result.Item2;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to save image.";
                        return RedirectToAction(nameof(Index));
                    }
                }

                // Call the Flask API for prediction
                PredictionResponse predictedResult = await CallFlaskApiForPrediction(diagnosisModel.Image);
                //diagnosisModel.IsCataract = predictedResult.PredictedClass;

                if (predictedResult.PredictedClass != null && !string.IsNullOrEmpty(predictedResult.PredictedClass))
                {
                    // Capitalize the first letter of the class name
                    var className = char.ToUpper(predictedResult.PredictedClass[0]) + predictedResult.PredictedClass.Substring(1);

                    // Get the confidence score
                    var confidence = predictedResult.ConfidenceScore;

                    // Format the IsCataract field as "ClassName with probability Confidence%"
                    diagnosisModel.IsCataract = $"{className} with probability {confidence:F2}%";
                    // Save the diagnosis to the database
                    await _context.Diagnosis.AddAsync(diagnosisModel);
                    await _context.SaveChangesAsync();

                    // Set success message
                    TempData["SuccessMessage"] = "Diagnosed successfully.";
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    TempData["ErrorMessage"] = "Prediction failed. No valid result returned.";
                    return RedirectToAction(nameof(Index));
                }

                
            }

            // Handle model state being invalid
            TempData["ErrorMessage"] = "Model state is invalid.";
            return RedirectToAction(nameof(Index));
        }



        private async Task<PredictionResponse> CallFlaskApiForPrediction(string imagePath)
        {
            using (var client = new HttpClient())
            {
                // Replace with your Flask API URL
                var apiUrl = "http://localhost:5000/upload-image";

                using (var form = new MultipartFormDataContent())
                {
                    var wwwPath = this.environment.WebRootPath;
                    var path = Path.Combine(wwwPath, "diagnosis_images");
                    var img = Path.Combine(path, imagePath);
                    using (var fileStream = new FileStream(img, FileMode.Open))
                    {
                        var streamContent = new StreamContent(fileStream);
                        streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        form.Add(streamContent, "file", Path.GetFileName(img));

                        var response = await client.PostAsync(apiUrl, form);
                        if (response.IsSuccessStatusCode)
                        {
                            var jsonResponse = await response.Content.ReadAsStringAsync();
                            // Deserialize into the PredictionResponse class
                            return JsonConvert.DeserializeObject<PredictionResponse>(jsonResponse);
                        }
                        else
                        {
                            // Handle error response from Flask API
                            TempData["ErrorMessage"] = "Error in image prediction: " + await response.Content.ReadAsStringAsync();
                            return null;
                        }
                    }
                }
            }
        }

      

        #region API CALLS
        [HttpPut]
        public async Task<IActionResult> Subscribe(int id)
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { data = false });
            }

            var package = await _context.Packages.FirstOrDefaultAsync(x => x.SubscriptionId == id);
            if (package == null)
            {
                return Json(new { data = false });
            }

            DateTime? expiresAt = null;
            if (package.ValidityPeriod.HasValue)
            {
                expiresAt = DateTime.UtcNow.AddDays(package.ValidityPeriod.Value);
            }

            var userSubscription = new UserSubscription()
            {
                UserId = user.Id,
                SubscriptionId = id,
                PackageName = package.PackageName,
                PhotoLimit = package.MaxPhotoLimit,
                ExpiresAt = expiresAt,
                SubscriptionDate = DateTime.UtcNow,
            };

            await _context.UserSubscriptions.AddAsync(userSubscription);
            await _context.SaveChangesAsync();

            return Json(new { data = true });
        }

        public async Task<IActionResult> GetAllHistory()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;
            List<Diagnosis> diagonisHistory = await _context.Diagnosis.Where(x=>x.UserId== userId).ToListAsync();
            return Json(new { data = diagonisHistory });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Invalid ID" });
            }
            var history = await _context.Diagnosis.FirstOrDefaultAsync(x => x.DiagnosisId == id);
            if (history == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }


            _context.Diagnosis.Remove(history);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Deleted Successful" });
        }



        public async Task<IActionResult> DownloadReport(int diagnosisId)
        {
            // Generate the PDF document in memory using a MemoryStream
            var stream = new MemoryStream();

            // Fetch diagnosis data based on the diagnosisId
            var diagnosis = await _context.Diagnosis
                .Include(d => d.User) // Include the ApplicationUser related to the Diagnosis
                .FirstOrDefaultAsync(x => x.DiagnosisId == diagnosisId);

            if (diagnosis == null)
            {
                TempData["ErrorMessage"] = "Diagnosis not found.";
                return RedirectToAction(nameof(Index));
            }

            // Parse the IsCataract field to determine diagnosis and confidence
            var isCataract = diagnosis.IsCataract;
            string diagnosisResult = isCataract.Contains("Cataract") ? "Cataract" : "Normal";
            string probabilityStr = isCataract.Substring(isCataract.LastIndexOf(' ') + 1).Trim('%');
            double probability = double.Parse(probabilityStr);

            // Determine the full path to the image
            var imagePath = Path.Combine(environment.WebRootPath, "diagnosis_images", diagnosis.Image);

            // Determine confidence level
            string confidenceLevel = probability switch
            {
                <= 65 => "Low",
                <= 79 => "Medium",
                _ => "High"
            };

            // Generate the PDF
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(16));

                    page.Header()
                        .Text($"Report: Cataract Detection for {diagnosis.PatientName}")
                        .SemiBold().FontSize(28).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(10);
                            x.Item().Text($"USER NAME: {diagnosis.User.FullName}");
                            x.Item().Text($"PATIENT NAME: {diagnosis.PatientName}");
                            x.Item().Text($"AGE: {diagnosis.Age}");
                            x.Item().Text($"GENDER: {diagnosis.Gender}");
                            x.Item().Text($"EYE: {diagnosis.Eye}");
                            x.Item().Text($"DATE & TIME: {diagnosis.UploadDate}");
                            x.Item().Text($"RESULT: {diagnosisResult} - {confidenceLevel} Confidence ({probability}%)");

                            // Load and display the actual image in the PDF
                            if (System.IO.File.Exists(imagePath))
                            {
                                x.Item().Height(400).Width(400).Image(imagePath); // Add the actual image
                            }
                            else
                            {
                                x.Item().Text("Image not found.");
                            }
                        });
                });
            }).GeneratePdf(stream);

            // Reset the stream position to the beginning
            stream.Position = 0;

            // Set the filename and return the file
            var filename = $"CataractReport_{diagnosis.PatientName}.pdf";
            return File(stream, "application/pdf", filename);
        }

        #endregion


    }
}
