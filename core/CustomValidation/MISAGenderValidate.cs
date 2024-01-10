using MISA.CUKCUK.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.CustomValidation
{
    public class MISAGenderValidate: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null || value == "") {
                return ValidationResult.Success;
            }
            if(int.TryParse(value.ToString(), out int gender))
            {
                if(gender is not (int)MISAEnum.Gender.MALE and not (int)MISAEnum.Gender.FEMALE and not (int)MISAEnum.Gender.OTHER)
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
