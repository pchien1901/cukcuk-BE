using MISA.CUKCUK.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.CustomValidation
{
    public class MISADateLessThanToday: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null || value == "")
            {
                return ValidationResult.Success;
            }

            DateTime date;
            if(DateTime.TryParse(value.ToString(), out date))
            {
                // so sánh ngày hiện tại
                var todayDate = DateTime.Now;
                if(todayDate < date)
                {
                    //return new ValidationResult(ErrorMessage);
                    throw new MISAValidateException(ErrorMessage);
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                
            }
            return base.IsValid(value, validationContext);
        }
    }
}
