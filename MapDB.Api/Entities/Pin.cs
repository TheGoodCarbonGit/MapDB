using Microsoft.AspNetCore.Components.Routing;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MapDB.Api.Entities{
    public class Pin{ // record class has better support for immutable data?

        [BsonId]
        public Guid ID { get; set; } // guid: unique ID
        
        [Required(ErrorMessage = "Name cannot be empty.")]
        [MaxLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Coordinates cannot be empty.")]
        [MinLength(2, ErrorMessage = "Coordinates must hold 2 values.")]
        [MaxLength(2, ErrorMessage = "Coordinates must hold 2 values.")]
        [CoordRangeCheck]
        public double[] Coordinates { get; set; }
        
        [Required(ErrorMessage = "Category cannot be empty.")]
        public string Category { get; set; }
        
        [Required(ErrorMessage = "Description cannot be empty.")]
        public string Description { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }

    public class CoordRangeCheckAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var coords = value as int[];

            if (coords[0] > 180 || coords[0] < -180)
            {
                return new ValidationResult("Longitude must be between -180 and 180.");
            }
            if (coords[1] > 90 || coords[1] < -90)
            {
                return new ValidationResult("Latitude must be between -90 and 90.");
            }
            
            return ValidationResult.Success;
        }
    }
}