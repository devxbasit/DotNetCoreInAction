using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ModelValidation.CustomAttributes;

namespace ModelValidation.Models
{
    public class Student : IValidatableObject
    {
        [Required] [Range(10, 20)] public int Id { get; set; }

        [ValueStartsWith("B")] public string Name { get; set; }

        [EmailAddress] public string Email { get; set; }

        [Phone] public string Contact { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrEmpty(Email) && String.IsNullOrEmpty(Contact))
            {
                yield return new ValidationResult($"Either {nameof(Email)} or {nameof(Contact)} must be specified",
                    new[] { nameof(Email), nameof(Contact) });
            }
        }
    }
}