using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cataract_Care.Models
{
    public class Diagnosis
    {
        [Key]
        public int DiagnosisId { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }

        
        [StringLength(100)]
        public string PatientName { get; set; }

        
        [Range(0, 120)] 
        public int Age { get; set; }


        public string Gender { get; set; } = string.Empty;

        [Required]
        public string Eye { get; set; } = string.Empty;

        [Required]
        public DateTime UploadDate { get; set; }=DateTime.Now;
        [Required]
        public string Image { get; set; }
        public string IsCataract { get; set; } = String.Empty;

    }
}
