using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        public Subscription Subscription { get; set; }

        [Required]
        public DateTime SubscriptionStartDate { get; set; }

        [Required]
        public DateTime SubscriptionEndDate { get; set; }
    }
}
