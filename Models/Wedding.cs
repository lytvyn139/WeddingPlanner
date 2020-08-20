using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId {get;set;}

        [Required (ErrorMessage = "Wedder One Name is required.")]
        [Display(Name = "Wedder One:")]
        public string Wedder1 {get;set;}

        [Required (ErrorMessage = "Wedder Two Name is required.")]
        [Display(Name = "Wedder Two:")]
        public string Wedder2 {get;set;}

        [Required (ErrorMessage = "Wedder Date is required.")]
        [Display(Name = "Date:")]
        [DataType(DataType.Date)]
        [Future]
        public DateTime Date {get;set;}

         
        [Display(Name = "Address:")]
        public string Address {get;set;}


        public int UserId {get;set;}
        public User Planner {get;set;}

        public List<Association> Attendees {get;set;}

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}