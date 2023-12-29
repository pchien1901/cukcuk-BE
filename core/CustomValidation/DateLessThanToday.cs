using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.CustomValidation
{
    public class DateLessThanToday: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null || value == "")
            {
                return ValidationResult.Success;
            }

            //var dateValue = String.IsNullOrEmpty(value.ToString()) ? ;
            DateTime date;
            if(DateTime.TryParse(value.ToString(), out date))
            {
                // so sánh ngày hiện tại
                var todayDate = DateTime.Now;
                if(todayDate < date)
                {
                    return new ValidationResult(ErrorMessage);
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
