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
    public class MISAPhoneNumberValidate: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var phoneNumber = value.ToString();
            var pattern = "^[0-9]+$";
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(phoneNumber))
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
