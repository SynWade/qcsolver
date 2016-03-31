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
           
            //checks if the start date time is selected
            if (startDateTime != null)
            {
                startDateTime = startDateTime;
                yield return new ValidationResult(string.Format("The start date time: {0} is required", startDateTime), new[] {"startDateTime"});
            }


            //checks if duration is between (1-24)
            if (duration > 0 && duration < 24)
            {
                yield return new ValidationResult(string.Format("The duration: {0} must be between 1 and 24", duration), new[] {"duration"});
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
                yield return new ValidationResult(string.Format("The construction site: {0} needs to be selected", constructionSite), new[] {"constructionSite"});
            }
        }
    }





    public class ScheduleMetadata
    {
        [Display(Name= "Schedule Id")]
        public int scheduleId { get; set; }

        [Display(Name= "Start Date Time")]
        public System.DateTime startDateTime { get; set; }

        [Display(Name= "Duration")]
        public int duration { get; set; }

        [Display(Name= "Person")]
        public int person { get; set; }

        [Display(Name= "Construction Site")]
        public int constructionSite { get; set; }
    }
}