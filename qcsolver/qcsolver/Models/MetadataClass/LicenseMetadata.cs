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
            if (licenseId != null)
            {
                licenseId = licenseId;
                yield return ValidationResult.Success;
            }

            if (licenseName != null)
            {
                licenseName = licenseName.Trim();
                yield return ValidationResult.Success;
            }

            if (dateIssued != null)
            {
                dateIssued = dateIssued;
                yield return ValidationResult.Success;
            }

            if (fileLocation != null)
            {
                fileLocation = fileLocation.Trim();
                yield return ValidationResult.Success;
            }

            if (person != null)
            {
                person = person;
                yield return ValidationResult.Success;
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