using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class SuberscribeViewModel
    {
        public int Id { get; set; } //nyligen tillagd
        [Required]
        [Display(Name = "Subscribe", Prompt = "Your email")]
        public string Email { get; set; } = null!;

        [Display(Name = "Daily Newsletter")]
        public bool DailyNewletter { get; set; }

        [Display(Name = "Advertising Updates")]
        public bool AdvertisingUpdates { get; set; }


        [Display(Name = "Week in Review")]
        public bool WeekInReview { get; set; }


        [Display(Name = "Event Updates")]
        public bool EventUpdates { get; set; }


        [Display(Name = "Startups Weekly")]
        public bool StartupsWeekly { get; set; }


        [Display(Name = "Podcasts")]
        public bool Podcasts { get; set; }


        public bool IsSubscribed { get; set; } = false;
    }
}
