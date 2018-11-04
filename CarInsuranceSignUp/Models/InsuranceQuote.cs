using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarInsuranceSignUp.Models
{
    public class InsuranceQuote
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CarYear { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public string Dui { get; set; }
        public int Tickets { get; set; }
        public string Coverage { get; set; }
        public DateTime CreatedAt { get; set; }
        public int QuoteTotal { get; set; }
    }
}