using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BoulderdashServerView
{
    public class IsIntegerRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                int valueInt = Convert.ToInt32(value);
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, "Podana wartośćnie jest liczbą całkowitą!");
            }

            return ValidationResult.ValidResult;
        }
    }
}