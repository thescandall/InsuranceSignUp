using CarInsuranceSignUp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarInsuranceSignUp.Controllers
{
    public class HomeController : Controller
    {
       private readonly string connectionString = @"GOES HERE";
        public ActionResult Index()
        {
            return View();
        }

        //Sign up method that takes in all fields from the signup form
        [HttpPost]
        public ActionResult Signup(string firstName, string lastName, string emailAddress, DateTime dateOfBirth, int carYear, string carMake, string carModel, 
                                   string dui, int? speedingTickets, string coverage)
        {
            //checks to make sure important fields are not empty
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName)|| string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(carMake) || string.IsNullOrEmpty(carModel)
                || string.IsNullOrEmpty(dui) || string.IsNullOrEmpty(coverage))
            {
                return View("~/Views/Shared/Error.cshtml");
            }

            if(dui.ToLower() != "yes" && dui.ToLower() != "no" )
            {
                return View("~/Views/Shared/Error.cshtml");
            }

            if (coverage.ToLower() != "full coverage" && coverage.ToLower() != "liability")
            {
                return View("~/Views/Shared/Error.cshtml");
            }

            if (speedingTickets < 0)
            {
                return View("~/Views/Shared/Error.cshtml");
            }

            else
            {
                var quoteTotal = 50;

                var currentYear = DateTime.Now.Year;
                var age = currentYear - dateOfBirth.Year;

                //checks age and adds to the total based on the result

                if (age > 100)
                {
                    quoteTotal += 25;
                }
                else if (age < 25)
                {
                    quoteTotal += 25;
                }
                else if (age < 25 && age < 18)
                {
                    quoteTotal += 100;
                }

                //checks car year and adds to the total based on the year

                if (carYear < 2000)
                {
                    quoteTotal += 25;
                }
                else if (carYear > 2015)
                {
                    quoteTotal += 25;
                }

                if (carMake.ToLower() == "porsche")
                {
                    quoteTotal += 25;
                }
                if (carMake.ToLower() == "porsche" && carMake.ToString().ToLower() == "911 Carrera")
                {
                    quoteTotal += 25;
                }

                if (speedingTickets > 0)
                {
                    for (int i = 0; i <= speedingTickets; i++)
                    {
                        quoteTotal += 10;
                    }
                }

                if (dui.ToLower() == "yes")
                {
                    quoteTotal += quoteTotal * 25 / 100;
                }

                if (coverage.ToString().ToLower() == "full coverage")
                {
                    quoteTotal += quoteTotal * 50 / 100;
                }


                string queryString = @"INSERT INTO Quotes (FirstName, LastName, EmailAddress, DateOfBirth, CarYear, CarMake, CarModel,
                                       Dui, Tickets, Coverage, QuoteTotal) VALUES (@FirstName, @LastName, @EmailAddress, @DateOfBirth, @CarYear, @CarMake,
                                       @CarModel, @Dui, @Tickets, @Coverage, @QuoteTotal)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add("@FirstName", SqlDbType.VarChar);
                    command.Parameters.Add("@LastName", SqlDbType.VarChar);
                    command.Parameters.Add("@EmailAddress", SqlDbType.VarChar);
                    command.Parameters.Add("@DateOfBirth", SqlDbType.Date);
                    command.Parameters.Add("@CarYear", SqlDbType.Int);
                    command.Parameters.Add("@CarMake", SqlDbType.VarChar);
                    command.Parameters.Add("@CarModel", SqlDbType.VarChar);
                    command.Parameters.Add("@Dui", SqlDbType.VarChar);
                    command.Parameters.Add("@Tickets", SqlDbType.Int);
                    command.Parameters.Add("@Coverage", SqlDbType.VarChar);
                    command.Parameters.Add("@QuoteTotal", SqlDbType.Money);
                    command.Parameters["@FirstName"].Value = firstName;
                    command.Parameters["@LastName"].Value = lastName;
                    command.Parameters["@EmailAddress"].Value = emailAddress;
                    command.Parameters["@DateOfBirth"].Value = dateOfBirth;
                    command.Parameters["@CarYear"].Value = carYear;
                    command.Parameters["@CarMake"].Value = carMake;
                    command.Parameters["@CarModel"].Value = carModel;
                    command.Parameters["@Dui"].Value = dui;
                    command.Parameters["@Tickets"].Value = speedingTickets;
                    command.Parameters["@Coverage"].Value = coverage;
                    command.Parameters["@QuoteTotal"].Value = quoteTotal;
                    

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();


                }

                    return View(Quote());
            }
        }
     
     

     public ActionResult Admin()
        {
            string queryString = @"SELECT Id, FirstName, LastName, EmailAddress from Quotes";

            List<InsuranceQuote> quotes = new List<InsuranceQuote>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand commmand = new SqlCommand(queryString, connection);

                connection.Open();

                SqlDataReader reader = commmand.ExecuteReader();

                while (reader.Read())
                {
                    var quote = new InsuranceQuote();
                    quote.Id = Convert.ToInt32(reader["Id"]);
                    quote.FirstName = reader["FirstName"].ToString();
                    quote.LastName = reader["LastName"].ToString();
                    quote.EmailAddress = reader["EmailAddress"].ToString();
                    quotes.Add(quote);
                }
            }

            return View(quotes);
        }

        public ActionResult Quote()
        {


            string queryString = @"SELECT TOP 1 * from Quotes ORDER BY Id DESC";

            var quotes = new List<InsuranceQuote>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand commmand = new SqlCommand(queryString, connection);

                connection.Open();

                SqlDataReader reader = commmand.ExecuteReader();

                while (reader.Read())
                {
                    var quote = new InsuranceQuote();

                    quote.Id = Convert.ToInt32(reader["Id"]);
                    quote.FirstName = reader["FirstName"].ToString();
                    quote.LastName = reader["LastName"].ToString();
                    quote.EmailAddress = reader["EmailAddress"].ToString();
                    quote.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                    quote.CarYear = Convert.ToInt32(reader["CarYear"]);
                    quote.CarMake = reader["CarMake"].ToString();
                    quote.CarModel = reader["CarModel"].ToString();
                    quote.Dui = reader["Dui"].ToString();
                    quote.Tickets = Convert.ToInt32(reader["Tickets"]);
                    quote.Coverage = reader["Coverage"].ToString();
                    quote.QuoteTotal = Convert.ToInt32(reader["QuoteTotal"]);
                    quotes.Add(quote);
                    
                }
            }

            return View(quotes);

        }
    }
}