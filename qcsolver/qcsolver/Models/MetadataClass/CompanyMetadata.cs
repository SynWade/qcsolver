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



            if (companyName != null)
            {
                companyName = companyName.Trim();
                yield return ValidationResult.Success;
            }

            if (companyId != null)
            {
                companyId = companyId;
                yield return ValidationResult.Success;
            }

            if (contactNumber != null)
            {
                contactNumber = contactNumber.Trim();
                yield return ValidationResult.Success;
            }

            if (contactEmail != null)
            {
                contactEmail = contactEmail.Trim();
                yield return ValidationResult.Success;
            }

            Regex emailRegex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
            if (!emailRegex.IsMatch(contactEmail))
            {
                yield return new ValidationResult("Email is not valid");
            }
            if (address != null)
            {
                address = address.Trim();
                yield return ValidationResult.Success;
            }

            Regex postalCodeRegex = new Regex("A[ABCEGHJKLMNPRSTVXY]\\d[A-Z] ?\\d[A-Z]\\d\\z");
            if (!postalCodeRegex.IsMatch(postalCode))
            {
                yield return new ValidationResult("postal code is not valid");
            }
            if (province != null)
            {
                province = province;
                yield return ValidationResult.Success;
            }

            if (country != null)
            {
                country = country;
                yield return ValidationResult.Success;
            }
            if (city != null)
            {
                city = city.Trim();
                yield return ValidationResult.Success;
            }


            yield return new ValidationResult("");
        }
    }
    public class CompanyMetadata
    {
        [Display(Name = "company ID")]
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