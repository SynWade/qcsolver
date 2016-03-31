using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace qcsolver.Models
{
    [MetadataType(typeof(ConstructionSiteMetadata))]
    public partial class ConstructionSite : IValidatableObject
    {
        private onsightdbEntities db = new onsightdbEntities();



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            //checks if the required field is entered
            if (address == null || address.Trim() == "")
            {
                yield return new ValidationResult(string.Format("The address: {0}, is required", address), new[] { "address" });
            }
            else
            {
                //address validation
                address = address.Trim();
                Regex addressRegex = new Regex(@"[^A-Za-z0-9'\.\-\s\,]");
                if (!addressRegex.IsMatch(address))
                {
                    yield return new ValidationResult(string.Format("The address: {0}, needs no special characters", address), new[] { "address" });
                }
            }

            if (startDate != null)
            {
                startDate = startDate;
            }

            if (endDate != null)
            {
                endDate = endDate;
            }

            //checks if city has been selected..
            if (city == null || city.Trim() == "")
            {
                yield return new ValidationResult(string.Format("The city: {0}, is required", city), new[] { "city" });
            }
            else
            {
                //city validation
                city = city.Trim();
                Regex cityRegex = new Regex(" \b^[a-zA-Z]+$\b");
                if (!cityRegex.IsMatch(city))
                {
                    yield return new ValidationResult(string.Format("The city: {0}, should not contain a number!", city), new[] { "city" });
                }
            }
                
            //checks if company has been selected..
            if (company != null)
            {
                company = company;
                yield return new ValidationResult(string.Format("The company: {0}, has to been selected", company), new[] { "company" });
            }

            //checks if country has been selected
            if (country != null)
            {
                country = country;
                yield return new ValidationResult(string.Format("The country: {0}, has to been selected", country), new[] { "country" });
            }

            //checks if province has been selected
            if (province != null)
            {
                province = province;
                yield return new ValidationResult(string.Format("The province: {0}, has to be selected", province), new[] { "province" });
            }
        }
    }

    public class ConstructionSiteMetadata
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
