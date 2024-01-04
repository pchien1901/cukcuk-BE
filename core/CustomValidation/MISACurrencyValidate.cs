using MISA.CUKCUK.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.CustomValidation
{
    public class MISACurrencyValidate: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null || value == "")
            {
                return ValidationResult.Success;
            }

            if(decimal.TryParse(value.ToString(), out decimal currency)) 
            {
                if (currency < 0)
                {
                    throw new MISAValidateException(ErrorMessage);
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            return base.IsValid(value, validationContext);
        }
    }
}
