using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBookingSystem.Models
{
    [Table("tblUsers")]
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name should have characters only")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name should have characters only")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please select Gender")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username can only have characters and numbers")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email Address is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage ="Email Address is not valid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mobile Number is required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile Number must have numbers only")]
        [MaxLength(10,ErrorMessage = "Mobile Number must have 10 numbers")]
        [MinLength(10, ErrorMessage = "Mobile Number must have 10 numbers")]
        public string MobileNumber { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string AddressLine
        {
            get; set;
        }
        [Required(ErrorMessage = "Please select City")]
        public string City
        {
            get; set;
        }
        [Required(ErrorMessage = "Pincode is required")]
        [MaxLength(6, ErrorMessage = "Pincode must have 6 numbers")]
        [MinLength(6, ErrorMessage = "Pincode must have 6 numbers")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Pincode must be numeric")]
        public string PinCode
        {
            get; set;
        }
        [Required(ErrorMessage = "Please select Country")]
        public string Country
        {
            get; set;
        }
        public bool Isactive { get; set; }
        [NotMapped]
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList States { get; set; }
        [NotMapped]
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList Countries { get; set; }
    }
}
