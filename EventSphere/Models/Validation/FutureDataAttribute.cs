using System;
using System.ComponentModel.DataAnnotations;

namespace EventSphere.Models.Validation
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public FutureDateAttribute()
        {
            ErrorMessage = "Date must be in the future.";
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            var dateTime = (DateTime)value;
            return dateTime > DateTime.Now;
        }
    }
}

