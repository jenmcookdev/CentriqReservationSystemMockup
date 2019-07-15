using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawsNClaws.DATA.EF//.Metadata
{
    #region Location Metadata
    public class LocationMetadata
    {
        [Required(ErrorMessage = "Location name is required")]
        [Display(Name = "Location")]
        [StringLength(50, ErrorMessage = "50 or less characters for location")]
        public string LocationName { get; set; }

        [Required(ErrorMessage = "Please enter address")]
        [StringLength(100, ErrorMessage = "Address should be less than 100 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(100, ErrorMessage = "City should be less than 50 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(2, ErrorMessage = "State Abbreviation should be 2 characters")]
        public string State { get; set; }

        [Required(ErrorMessage = "Please enter zipcode")]
        [Display(Name = "Postal Code")]
        [RegularExpression(@"[0-9]{5}", ErrorMessage = "Valid Postal Code Needed")]
        [StringLength(5, ErrorMessage = "5 digit zipcode")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Please enter Phone Number")]
        [Display(Name = "Phone Number")]
        [StringLength(11, ErrorMessage = "Phone number is no more than 11 digits")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Valid email required")]
        [StringLength(50, ErrorMessage = "Email must be 50 digits or less")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please set reservation limit")]
        [Range(1, 255, ErrorMessage = "Reservation limit must be between 1 to 255")]
        public byte ReservationLimit { get; set; }
    }
    [MetadataType(typeof(LocationMetadata))]
    public partial class Location { }

    #endregion

    #region OwnerAsset Metadata
    public class OwnerAssetMetadata
    {
        [Required(ErrorMessage = "Name of pet is required")]
        [Display(Name = "Pet Name")]
        [StringLength(50, ErrorMessage = "Pet name needs to be less than 50 characters.")]
        public string AssetName { get; set; }

        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        [Display(Name = "Pet Image")]
        [StringLength(50, ErrorMessage = "50 characters or less")]
        public string AssetPhoto { get; set; }

        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        [Display(Name = "Pet Details")]
        [UIHint("MultilineText")]
        [StringLength(300, ErrorMessage = "Maximum is 300 characters")]
        public string SpecialNotes { get; set; }

        [Display(Name = "Active Pet")]
        [Required(ErrorMessage = "Required Field")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [Display(Name = "Date Added")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
        public System.DateTime DateAdded { get; set; }
    }
    [MetadataType(typeof(OwnerAssetMetadata))]
    public partial class OwnerAsset { }


    #endregion

    #region Reservation Metadata
    public class ReservationMetadata
    {
        /*Foreign keys*/
        //public int LocationID { get; set; }
        //public int ServicesProvicedID { get; set; }
        //public int OwnerAssetID { get; set; }

        [MyDate(ErrorMessage = "Cannot enter a date in the past")]
        [Required(ErrorMessage = "Reservation Date is required")]
        [Display(Name = "Reservation Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]        
        public System.DateTime ReservationDate { get; set; }

        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        [UIHint("MultilineText")]
        [Display(Name = "Customer Notes")]
        public string Notes { get; set; }

    }

    public class MyDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime d = Convert.ToDateTime(value);
            return d >= DateTime.Now;
        }
    }

    [MetadataType(typeof(ReservationMetadata))]
    public partial class Reservation { }

    #endregion

    #region ServicesProvided Metadata
    public class ServicesProvidedMetadata
    {
        //public int ServicesProvidedID { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Services Provided")]
        [UIHint("MultilineText")]
        [StringLength(200, ErrorMessage = "Maximum is 200 characters")]
        public string ServicesProvided1 { get; set; }

    }
    [MetadataType(typeof(ServicesProvidedMetadata))]
    public partial class ServicesProvided { }

    #endregion

    #region UserDetail Metadata
    public class UserDetailMetadata
    {
        [Required(ErrorMessage = "User is required")]
        [Display(Name = "User")]
        [StringLength(128, ErrorMessage = "Maximum is 128 characters")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "Please enter a first name")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "Name needs to be less than 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a last name")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Name needs to be less than 50 characters.")]
        public string LastName { get; set; }
    }

    [MetadataType(typeof(UserDetailMetadata))]
    public partial class UserDetail
    {
        //This is a readonly field for display ONLY
        [Display(Name = "Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
    #endregion
}
