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
        [Required]
        [Display(Name = "Subscribe", Prompt = "Enter your email adress")]
        public string Email { get; set; } = null!;

        public bool IsSubscribed { get; set; } = false;
    }
}
