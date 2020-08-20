
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


    namespace WeddingPlanner.Models
    {
        public class User
        {
            [Key]
            public int UserId { get; set; }
            
            [Required (ErrorMessage = "First Name is required")]
            [MinLength(2)]
            [Display(Name = "First Name:")]
            public string FirstName { get; set; }

            [Required (ErrorMessage = "Last Name is required")]
            [MinLength(2)]
            [Display(Name = "Last Name:")]
            public string LastName { get; set; }

            [EmailAddress]
            [Required (ErrorMessage = "Email is required")]
            [Display(Name = "Email:")]
            public string Email { get; set; }

            [Required]
            [MinLength(8, ErrorMessage = "Password must be 8 characters or longer!")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [NotMapped]
            [Required]
            [MinLength(8)]
            [DataType(DataType.Password)]
            [Compare("Password")]
            public string Confirm {get;set;}

            public DateTime CreatedAt {get;set;} = DateTime.Now;
            public DateTime UpdatedAt {get;set;} = DateTime.Now;

            public List<Wedding> PlannedWeddings {get;set;}
            public List<Association> AttendingWeddings {get;set;}
        }
    }