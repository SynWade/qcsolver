using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace qcsolver.Models
{
    [MetadataType(typeof(ScheduleMetadata))]
    public partial class Schedule : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (scheduleId != null)
            {
                scheduleId = scheduleId;
                yield return ValidationResult.Success;
            }

            if (startDateTime != null)
            {
                startDateTime = startDateTime;
                yield return ValidationResult.Success;
            }

            if (duration != null)
            {
                duration = duration;
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

    public class ScheduleMetadata
    {
        [Display(Name = "Schedule Id")]
        public int scheduleId { get; set; }

        [Display(Name = "Start Date Time")]
        public System.DateTime startDateTime { get; set; }

        [Display(Name = "Duration")]
        public int duration { get; set; }

        [Display(Name = "Person")]
        public int person { get; set; }

        [Display(Name = "Construction Site")]
        public int constructionSite { get; set; }
    }
}