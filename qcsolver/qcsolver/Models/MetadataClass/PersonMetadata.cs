using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace qcsolver.Models
{
    [MetadataType(typeof(PersonMetadata))]
    public partial class Person : IValidatableObject
    {
        private onsightdbEntities db = new onsightdbEntities();


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return ValidationResult.Success;
        }
    }
    public class PersonMetadata
    {
        public string UserName { get; set; }
        public string Id { get; set; }
        public int personId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string pictureLocation { get; set; }
        public string contractLocation { get; set; }
        public string postalCode { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool online { get; set; }
        public int type { get; set; }
        public Nullable<int> company { get; set; }
        public int country { get; set; }
        public int province { get; set; }
    }
}