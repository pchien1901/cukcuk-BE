using MISA.CUKCUK.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.CustomValidation
{
    public class MISAEmailValidate: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var email = value.ToString();
            var pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            Regex emailRegex = new Regex(pattern);
            if(emailRegex.IsMatch(email))
            {
                return ValidationResult.Success;
            }
            else
            {
                throw new MISAValidateException(ErrorMessage);
            }

            return base.IsValid(value, validationContext);
        }
    }
   
}
