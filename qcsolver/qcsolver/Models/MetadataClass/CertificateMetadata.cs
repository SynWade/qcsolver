using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web;

namespace qcsolver.Models
{
    [MetadataType(typeof(CertificateMetadata))]
    public partial class Certificate : IValidatableObject
    {
        private onsightdbEntities db = new onsightdbEntities();


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (certificateName != null)
            {
                certificateName = certificateName.Trim();
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
