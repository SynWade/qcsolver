using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace qcsolver.Models
{
    [MetadataType(typeof(ConstructionSitesMetadata))]
    public partial class ConstructionSite : IValidatableObject
    {
        private onsightdbEntities db = new onsightdbEntities();



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (constructionSiteId != null)
            {
                constructionSiteId = constructionSiteId;
                yield return ValidationResult.Success;
            }

            if (address != null)
            {
                address = address.Trim();
                yield return ValidationResult.Success;
            }

            if (startDate != null)
            {
                startDate = startDate;
            }

            if (endDate != null)
            {
                endDate = endDate;
            }
            if (city != null)
            {
                city = city.Trim();
                yield return ValidationResult.Success;
            }

            if (company != null)
            {
                company = company;
                yield return ValidationResult.Success;
            }

            if (country != null)
            {
                country = country;
                yield return ValidationResult.Success;
            }

            if (province != null)
            {
                province = province;
                yield return ValidationResult.Success;
            }
        }
    }







    public class ConstructionSitesMetadata
    {
        [Display(Name = "construction SiteId")]
        public int constructionSiteId { get; set; }

        [Display(Name = "address")]
        public string address { get; set; }

        [Display(Name = "start date")]
        public System.DateTime startDate { get; set; }

        [Display(Name = "end date")]
        public Nullable<System.DateTime> endDate { get; set; }

        [Display(Name = "city")]
        public string city { get; set; }

        [Display(Name = "company")]
        public int company { get; set; }

        [Display(Name = "country")]
        public int country { get; set; }

        [Display(Name = "province")]
        public int province { get; set; }
    }
}
