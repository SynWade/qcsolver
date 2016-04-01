using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace qcsolver.Models
{
    [MetadataType(typeof(CertificateMetadata))]
    public partial class Certificate : IValidatableObject
    {
        private onsightdbEntities db = new onsightdbEntities();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //checks if the required field is entered
            if (certificateName == null || certificateName.Trim() == "")
            {
                yield return new ValidationResult(string.Format("The certificate name: {0} is required", certificateName), new[] { "certificateName" });
            }
            else
            {
                //certificate name validation
                certificateName = certificateName.Trim();
                Regex certificateRegex = new Regex(" \b^[a-zA-Z]+$\b");
                if (!certificateRegex.IsMatch(certificateName))
                {

                    yield return new ValidationResult(string.Format("The certificate name: {0} should not not contain a number", certificateName), new[] { "certificateName" });
                }
            }
            


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

    public class CertificateMetadata
    {
        
        [Display(Name = "Certificate Id")]
        public int certificateId { get; set; }

        [Display(Name = "Certificate Name")]
        public string certificateName { get; set; }

        [Display(Name = "Date Issued")]
        public System.DateTime dateIssued { get; set; }

        [Display(Name = "File Location")]
        public string fileLocation { get; set; }

        [Display(Name = "person")]
        public int person { get; set; }
    }
}
