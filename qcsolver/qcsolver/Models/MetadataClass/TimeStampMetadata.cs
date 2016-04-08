using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace qcsolver.Models
{
    [MetadataType(typeof(TimestampMetadata))]
    public partial class Timestamp : IValidatableObject
    {

        private onsightdbEntities db = new onsightdbEntities();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (timeIn != null)
            {
                timeIn = timeIn;
                yield return ValidationResult.Success;
            }

            //checks if person is selected
            if (person != null)
            {
                person = person;
                yield return new ValidationResult(string.Format("The person: {0} needs to be selected", person), new[] { "person" });
            }

            //checks if the construction site has been selected.. 
            if (constructionSite != null)
            {
                constructionSite = constructionSite;
                yield return new ValidationResult(string.Format("The construction site: {0} needs to be selected", constructionSite), new[] { "constructionSite" });
            }
        }
    }

    public class TimestampMetadata
    {
        [Display(Name = "Time StampId")]
        public int timestampId { get; set; }

        [Display(Name = "Time In")]
        public System.DateTime timeIn { get; set; }

        [Display(Name = "Time Out")]
        public Nullable<System.DateTime> timeOut { get; set; }

        [Display(Name = "Person")]
        public int person { get; set; }

        [Display(Name = "Construction Site")]
        public int constructionSite { get; set; }
    }
}