using System;
using System.Text.RegularExpressions;

namespace Fx_validator_email_phone.Validation
{
    public class Validator
    {
        public bool IsEmail(string email)
        {
          string pattern = "^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,6}";
          Match matchEmail = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
          return matchEmail.Success;
         
        }


        public bool IsCellPhoneNumber(string number) {

            if (IsNumeric(number) && number.Length == 10 && number.Substring(0, 1) == "3")
            {
                return true;
            }
            else {
                return false;
            }
        }

        public bool IsNumeric(string number)
        {
            double test;
            return double.TryParse(number, out test);
        }

        public bool IsEmpty(string value) {
            return String.IsNullOrEmpty(value);
        }

    }
}

