using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cataract_Care.Models
{
    public class UserSubscription
    {
        [Key]
        public int UserSubscriptionId {  get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }

        [Required]
        public int SubscriptionId { get; set; }
        [ForeignKey("SubscriptionId")]
        [ValidateNever]
        public Package Subscription { get; set; }

        [Required]
        public DateTime SubscriptionDate { get; set; }


        [Required]
        [StringLength(100)]
        [DisplayName("Package Name")]
        public string PackageName { get; set; }

        [DisplayName("Expiry Date")]
        [Range(0, int.MaxValue)]
        public DateTime? ExpiresAt { get; set; }


        [DisplayName("Photo Limit")]
        [Range(0, int.MaxValue)]
        public int? PhotoLimit { get; set; }
    }
}
