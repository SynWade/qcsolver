using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace qcsolver.Models
{
    [MetadataType(typeof(PersonMetadata))]
    public partial class Person : IValidatableObject
    {
        private onsightdbEntities db = new onsightdbEntities();


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            //if (Use)
            //{
                
            //}

            //checks if the required field is entered
            if (firstName == null || firstName.Trim() == "")
            {
                yield return new ValidationResult(string.Format("The First Name: {0}, is required", firstName), new[] {"firstName"});
            }
            else
            {
                //validates firstName and checks for space
                firstName = firstName.Trim();
                Regex firstNameRegex = new Regex(" \b^[a-zA-Z]+$\b");
                if (!firstNameRegex.IsMatch(firstName))
                {
                    yield return new ValidationResult(string.Format("The First Name: {0}, must not contain any number but just letters!", firstName), new[] { "firstName" });
                }
            }

            //checks if the required field is entered
            if (lastName == null || lastName.Trim() == "")
            {
                yield return new ValidationResult(string.Format("The Last Name: {0}, is required", lastName), new[] { "LastName" });
            }
            else
            {
                //validates lastName and checks for space
                lastName = lastName.Trim();
                Regex lastNameRegex = new Regex(" \b^[a-zA-Z]+$\b");
                if (!lastNameRegex.IsMatch(lastName))
                {
                    yield return new ValidationResult(string.Format("The Last Name: {0}, must not contain any number but just letters!", lastName), new[] { "lastName" });
                }
            }

            //checks if the required field is enetered 
            if (city == null || city.Trim() == "")
            {
                yield return new ValidationResult(string.Format("The city: {0}, enter your city", city), new[] { "city" });
            }
            else
            {
                //city validation
                city = city.Trim();
                Regex cityRegex = new Regex(" \b^[a-zA-Z]+$\b");
                if (!cityRegex.IsMatch(city))
                {
                    yield return new ValidationResult(string.Format("The city: {0}, should not contain a number!", city), new[] { "city" });
                }
            }

            //checks if the required field is entered
            if (address == null || address.Trim() == "")
            {
                yield return new ValidationResult(string.Format("The address: {0}, is required", address), new[] { "address" });
            }
            else
            {
                //address validation
                address = address.Trim();
                Regex addressRegex = new Regex(@"[^A-Za-z0-9'\.\-\s\,]");
                if (!addressRegex.IsMatch(address))
                {
                    yield return new ValidationResult(string.Format("The address: {0}, needs no special characters", address), new[] { "address" });
                }
            }

            //check the required field
            if (postalCode == null || postalCode.Trim() == "")
            {
                yield return new ValidationResult(string.Format("Postal code: {0}, is required", postalCode), new[] { "postalCode" });
            }
            else
            {
                //postal code validation
                postalCode = postalCode.Trim();
                Regex postalCodeRegex = new Regex(@"^[a-z]\d[a-z] ?\d[a-z]\d$");
                if (!postalCodeRegex.IsMatch(postalCode))
                {
                    yield return new ValidationResult(string.Format("Postal code: {0}, should be in the right format N2L 1C3 ", postalCode), new[] { "postalCode" });
                }
            }

            //checks if required field is entered.....
            if (phone == null || phone.Trim() == "")
            {
                yield return new ValidationResult(string.Format("The number: {0}, is required", phone), new[] { "phone" });
            }
            else
            {
                //phone number validation
                phone = phone.Trim();
                Regex phoneRegex = new Regex(@"^\d{3}-\d{3}-\d{4}$");
                if (!phoneRegex.IsMatch(phone))
                {
                    yield return new ValidationResult(string.Format("The number: {0}, need a valid phone number", phone), new[] { "phone" });
                }
            }

            //checks if required field is entered ..
            if (email == null || email.Trim() == "")
            {
                yield return new ValidationResult(string.Format("The email: {0}, is required", email), new[] { "email" });
            }
            else
            {
                //email validation
                email = email.Trim();
                Regex emailRegex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
                if (!emailRegex.IsMatch(email))
                {
                    yield return new ValidationResult(string.Format("The email: {0}, need to be the right email format", email), new[] { "email" });
                }
            }

            //checks if the required field is entered..
            if (password == null || password.Trim() == "")
            {
                yield return new ValidationResult(string.Format("The Password: {0}, is required", password), new[] { "password" });
            }
            else
            {
                Regex passwordRegex = new Regex(@"^[a-zA-Z'.]{1,40}$");
                if (passwordRegex.IsMatch(password))
                {
                    yield return new ValidationResult(string.Format("The Password: {0}, must have no space, atleast one uppercase, one number and a special character", password), new[] { "password" });
                }
            }

            //checks if the province is selected
            if (type == null)
            {
                type = type;
                yield return new ValidationResult(string.Format("The type: {0}, you need to select your type!", type), new[] { "type" });
            }

            //checks if the province is selected
            if (company == null)
            {
                company = company;
                yield return new ValidationResult(string.Format("The company: {0}, you need to select your company!", company), new[] { "company" });
            }

            //checks if the province is selected
            if (country == null)
            {
                country = country;
                yield return new ValidationResult(string.Format("The country: {0}, you need to select your country!", country), new[] { "country" });
            }

            //checks if the province is selected
            if (province == null)
            {
                province = province;
                yield return new ValidationResult(string.Format("The province: {0}, you need to select your province!", province), new[] { "province" });
            }


        }
    }
    public class PersonMetadata
    {
        [Display(Name= "user Name")]
        public string UserName { get; set; }

        [Display(Name= "Id")]
        public string Id { get; set; }

        [Display(Name= "Person Id")]
        public int personId { get; set; }

        [Display(Name= "First Name")]
        public string firstName { get; set; }

        [Display(Name= "Last Name")]
        public string lastName { get; set; }

        [Display(Name= "city")]
        public string city { get; set; }

        [Display(Name= "address")]
        public string address { get; set; }

        [Display(Name= "Picture Location")]
        public string pictureLocation { get; set; }

        [Display(Name= "Contract Location")]
        public string contractLocation { get; set; }

        [Display(Name= "Postal Code")]
        public string postalCode { get; set; }

        [Display(Name= "Phone")]
        public string phone { get; set; }

        [Display(Name= "email")]
        public string email { get; set; }

        [Display(Name="Password")]
        public string password { get; set; }

        [Display(Name= "Online")]
        public bool online { get; set; }

        [Display(Name= "Type")]
        public int type { get; set; }

        [Display(Name= "Company")]
        public int company { get; set; }

        [Display(Name= "Country")]
        public int country { get; set; }

        [Display(Name= "Province")]
        public int province { get; set; }
    }
}