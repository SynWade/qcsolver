using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;


namespace qcsolver.Models
{
    [MetadataType(typeof(CompanyMetadata))]
    public partial class Company : IValidatableObject
    {
        private onsightdbEntities db = new onsightdbEntities();


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //checks if required field is entered
            if (companyName == null || companyName.Trim() == "")
            {
                yield return new ValidationResult(string.Format("The name: {0}, is required", companyName), new[] { "companyName" });
            }
            else
            {
                companyName = companyName.Trim();
                //company name validation
                Regex nameRegex = new Regex(" \b^[a-zA-Z]+$\b");
                if (!nameRegex.IsMatch(companyName))
                {
                    yield return new ValidationResult(string.Format("The name: {0}, should not contain a number", companyName), new[] { "companyName" });
                }
            }

            //checks if required field is entered.....
            if (contactNumber == null || contactNumber.Trim() == "")
            {
                yield return new ValidationResult(string.Format("The number: {0}, is required", contactNumber), new[] { "contactNumber" });
            }
            else
            {
                //number validation
                contactNumber = contactNumber.Trim();
                Regex numberRegex = new Regex(@"^\d{3}-\d{3}-\d{4}$");
                if (!numberRegex.IsMatch(contactNumber))
                {
                    yield return new ValidationResult(string.Format("The number: {0}, need a valid phone number", contactNumber), new[] { "contactNumber" });
                }
            }
            
            //checks if required field is entered ..
            if (contactEmail == null || contactEmail.Trim() == "" )
            {
                yield return new ValidationResult(string.Format("The email: {0}, is required", contactEmail), new[] { "contactEmail" });
            }
            else
            {
                //email validation
                contactEmail = contactEmail.Trim();
                Regex emailRegex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
                if (!emailRegex.IsMatch(contactEmail))
                {
                    yield return new ValidationResult(string.Format("The email: {0}, need to be the right email format", contactEmail), new[] { "contactEmail" });
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
                    yield return new ValidationResult(string.Format("The address: {0}, needs no special characters", address), new[] {"address"});
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
                    yield return new ValidationResult(string.Format("Postal code: {0}, should be in the right format N2L 1C3 ", postalCode), new[] {"postalCode"});
                }
            }

           
            //checks if the province is selected
            if (province != null)
            {
                province = province;
                yield return new ValidationResult(string.Format("The province: {0}, you need to select your province!", province), new[] { "province" });
            }

            //checks if country is selected
            if (country != null)
            {
                country = country;
                yield return new ValidationResult(string.Format("The country: {0}, you need to select your country", country), new[] { "country" });
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
        }
    }
    public class CompanyMetadata
    {
       
        
        public int companyId { get; set; }

        [Display(Name = "company Name")]
        public string companyName { get; set; }

        [Display(Name = "contact Number")]
        public String contactNumber { get; set; }

        [Display(Name = "contact Email")]
        [Required(ErrorMessage = "The email address is required")]
        public string contactEmail { get; set; }

        [Display(Name = "address")]
        public string address { get; set; }

        [Display(Name = "postal Code")]
        public string postalCode { get; set; }

        [Display(Name = "city")]
        public string city { get; set; }


        [Display(Name = "country")]
        public int country { get; set; }


        [Display(Name = "province")]
        public int province { get; set; }
    }
}