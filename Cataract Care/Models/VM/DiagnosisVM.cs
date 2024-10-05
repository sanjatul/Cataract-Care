using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cataract_Care.Models.VM
{
    public class DiagnosisVM
    {

        [StringLength(100)]
        [Display(Name ="Patient Name")]
        [MaxLength(100)]
        [Required(ErrorMessage = "Patient name is required")]
        public string PatientName { get; set; }
        public int UserSubscriptionId { get; set; }

        [Range(1, 120)]
        [Required(ErrorMessage = "Age is required")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Eye selection is required")]
        public string Eye { get; set; }

        [Required(ErrorMessage = "Eye image is required")]
        [Display(Name ="Upload Fundus Image of Eye")]
        public IFormFile EyeImage { get; set; }
        
    }
}
