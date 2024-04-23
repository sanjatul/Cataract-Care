using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cataract_Care.Models
{
    public class Subscription
    {
        [Key]
        public int SubscriptionId { get; set; }

        [Required]
        [StringLength(100)]
        public string PackageName { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(100)]
        public string ValidityPeriod { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int MaxPhotoLimit { get; set; }
        [NotMapped]
        public ICollection<ApplicationUser>? Users { get; set; }
    }
}
