using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace qcsolver.Models
{
    [MetadataType(typeof(TimeStampMetadata))]
    public partial class Timestamp : IValidatableObject
    {

        private onsightdbEntities db = new onsightdbEntities();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (timestampId != null)
            {
                timestampId = timestampId;
                yield return ValidationResult.Success;
            }

            if (timeIn != null)
            {
                timeIn = timeIn;
                yield return ValidationResult.Success;
            }

            if (timeOut != null)
            {
                timeOut = timeOut;
                yield return ValidationResult.Success;
            }

            if (person != null)
            {
                person = person;
                yield return ValidationResult.Success;
            }

            if (constructionSite != null)
            {
                constructionSite = constructionSite;
                yield return ValidationResult.Success;
            }
        }
    }





    public class TimeStampMetadata
    {
        [Display(Name= "Time StampId")]
        public int timestampId { get; set; }

        [Display(Name= "Time In")]
        public System.DateTime timeIn { get; set; }

        [Display(Name= "Time Out")]
        public Nullable<System.DateTime> timeOut { get; set; }

        [Display(Name= "Person")]
        public int person { get; set; }

        [Display(Name= "Construction Site")]
        public int constructionSite { get; set; }
    }
}