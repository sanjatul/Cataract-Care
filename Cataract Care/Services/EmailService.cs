using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace Cataract_Care.Services
{
    public class EmailService : IEmailService
    {
        public async Task<bool> SendEmailAsync(string to, string subject, string message)
        {
            // Configuration settings for your email account
            string fromEmail = "shsiam800@gmail.com"; // Use a config or environment variable
            string fromPassword = "ektfaeuqlbxwyyjq"; // Use a config or environment variable

            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(fromEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;
                mailMessage.To.Add(new MailAddress(to));

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential(fromEmail, fromPassword);
                    smtpClient.EnableSsl = true;

                    try
                    {
                        await smtpClient.SendMailAsync(mailMessage);
                        return true; // Email sent successfully
                    }
                    catch (Exception ex)
                    {
                        // Log the exception or handle it accordingly
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        return false; // Email sending failed
                    }
                }
            }
        }
    }
}
