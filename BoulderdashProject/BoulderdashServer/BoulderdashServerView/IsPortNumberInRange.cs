using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BoulderdashServerView
{
    internal class IsPortNumberInRange : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int port = 0;

            try
            {
                if (((string)value).Length > 0)
                    port = Int32.Parse((string)value);
            }
            catch (FormatException e)
            {
                return new ValidationResult(false, "Podano nieprawidłowe dane! Powinła zostać wprowadzona liczba całkowita!");
            }

            if ((port < Min) || (port > Max))
            {
                return new ValidationResult(false, $"Numer portu powinien być z przedziału: {Min}-{Max}.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
