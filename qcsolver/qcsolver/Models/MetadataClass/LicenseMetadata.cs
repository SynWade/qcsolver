using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;


namespace qcsolver.Models
{
    [MetadataType(typeof(LicenseMetadata))]
    public partial class License : IValidatableObject
    {
        private onsightdbEntities db = new onsightdbEntities();


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //check if the required field is entered
            if (licenseName == null || licenseName.Trim() == "")
            {
                yield return new ValidationResult(string.Format("", licenseName), new[] { "licenseName" });
            }
            else
            {
                //license name validation
                licenseName = licenseName.Trim();
                Regex licenseRegex = new Regex(" \b^[a-zA-Z]+$\b");
                if (!licenseRegex.IsMatch(licenseName))
                {
                    yield return new ValidationResult(string.Format("", licenseName), new[] { "licenseName" });
                }
            }
           
           
            //need to work on this..
            if (dateIssued != null)
            {
                dateIssued = dateIssued;
                yield return ValidationResult.Success;
            }

            //checks if required field is entered.. 
            if (fileLocation == null || fileLocation.Trim() == "")
            {
                yield return new ValidationResult(string.Format("The file location: {0} is required", fileLocation), new[] { "fileLocation" });
            }

            //checks if person is selected
            if (person != null)
            {
                person = person;
                yield return new ValidationResult(string.Format("The person: {0} needs to be selected", person), new[] { "person" });
            }
        }
    }




    public class LicenseMetadata
    {
        [Display(Name = "License Id")]
        public int licenseId { get; set; }

        [Display(Name = "License Name")]
        public string licenseName { get; set; }

        [Display(Name = "Date Issued")]
        public System.DateTime dateIssued {get; set;}

        [Display(Name = "Expiration Date")]
        public Nullable<System.DateTime> expirationDate { get; set; }

        [Display(Name = "File Location")]
        public string fileLocation { get; set; }

        [Display(Name = "person")]
        public int person { get; set; }
    }
}