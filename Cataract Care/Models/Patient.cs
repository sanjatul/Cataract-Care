using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cataract_Care.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }

        [Required]
        [StringLength(100)]
        public string PatientName { get; set; }

        [Required]
        [Range(0, 120)] 
        public int Age { get; set; }

        [Required]
        public GenderType Gender { get; set; }

        [Required]
        public EyeType Eye { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }

        public bool? IsCataract { get; set; }

        public enum GenderType
        {
            Male,
            Female,
            Other
        }

        public enum EyeType
        {
            Left,
            Right
        }
    }
}
