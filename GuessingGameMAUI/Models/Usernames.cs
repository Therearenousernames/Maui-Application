using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GuessingGameMAUI.Models
{
    public class Usernames
    {
        [StringLength(64, MinimumLength = 4, ErrorMessage ="The username should contain at least 4 characters")]
        [Required (ErrorMessage = "Required")]
        public string Username { get; set; }
    }
}
