

using System.ComponentModel.DataAnnotations;

    namespace WeddingPlanner.Models
    {
        public class LoginUser
        {
            [Required (ErrorMessage = "Login Email is required")]
            [EmailAddress]
            [Display(Name = "Email:")]
            public string LoginEmail { get; set; }

            [DataType(DataType.Password)]
            [Required (ErrorMessage = "Login Password is required")]
            [Display(Name = "Password:")]
            public string LoginPassword {get;set;}
        }
    }