using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BoulderdashServerView
{
    public class IsPortAvailable : ValidationRule
    {
        public override System.Windows.Controls.ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || ((string)value).Equals(""))
            {
                return new ValidationResult(false, "Nie podano żadnej wartości!");
            }

            int port;

            try
            {
                port = Int32.Parse((string)value);
            }
            catch (FormatException e)
            {
                return new ValidationResult(false, "Podano nieprawidłowe dane! Powinna zostać wprowadzona liczba całkowita!");
            }

            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
            {
                if (tcpi.LocalEndPoint.Port == port)
                {
                    return new ValidationResult(false, "Podany port już jest zajęty!");
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
