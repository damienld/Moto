using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Moto.Data
{
    public class User
    {
        public User()
        {
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Name e.g. John Doe")]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [NotMapped]
        [Required]
        [Compare("Email")]
        public string RetypeEmail { get; set; }

        [DisplayName("Phone Number")]
        public string Phone { get; set; }

        /*[RegularExpression("^[0-9+]{5}-[0-9+]{7}-[0-9]{1}$")]
        [Required]
        public string Cnic { get; set; }*/

        [Range(10, 120)]
        public string Age { get; set; }

        [StringLength(35)]
        public string City { get; set; }

        public string Address { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]

        public string Password { get; set; }

        [NotMapped]
        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}