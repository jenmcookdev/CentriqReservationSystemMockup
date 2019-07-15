using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PawsNClaws.Models
{
    public class ContactViewModel
    {
        //fields

        //properties

        [Required(ErrorMessage = "Required")]
        [StringLength(128, ErrorMessage = "Maximum is 128 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "No more than 50 characters")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Valid email required")]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "No more than 50 characters")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(4000, ErrorMessage = "You have exceeded the maximum characters allowed")]
        [UIHint("MultilineText")]
        public string Message { get; set; }

        //ctors

        //methods

        //ToString()

    }
}