using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cataract_Care.Models
{
    public class Package
    {
        [Key]
        public int SubscriptionId { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Package Name")]
        public string PackageName { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }


        [DisplayName("Validity Duration")]
        [Range(0, int.MaxValue)]
        public int? ValidityPeriod { get; set; }

       
        [DisplayName("Photo Limit")]
        [Range(0, int.MaxValue)]
        public int? MaxPhotoLimit { get; set; }
        public string? Description { get; set; }

        public bool IsActive { get; set; }= true;

    }
}
