using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PawsNClaws.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        #region Addition of UserInfo
        /***************************************************************************/
        //[Required(ErrorMessage = "Please enter a first name")]
        //[Display(Name = "First Name")]
        //[StringLength(50, ErrorMessage = "Name needs to be less than 50 characters.")]
        public string FirstName { get; set; }

        //[Required(ErrorMessage = "Please enter a last name")]
        //[Display(Name = "Last Name")]
        //[StringLength(50, ErrorMessage = "Name needs to be less than 50 characters.")]
        public string LastName { get; set; }

        /***************************************************************************/
        #endregion


        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}