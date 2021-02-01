using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Resources
{
    public class UserLoginGenerator
    {
        private static string PrepareAndNormalize(string word)
        {
            
            word = word.Trim().ToLower();
            return Normalize(word);
            

        }
        private static string Normalize(string word)
        {
            if (word == null || "".Equals(word))
            {
                return word;
            }
            char[] charArray = word.ToCharArray();
            char[] normalizedArray = new char[charArray.Length];
            for (int i = 0; i < normalizedArray.Length; i++)
            {
                normalizedArray[i] = normalizeChar(charArray[i]);
            }
            return new string(normalizedArray);
        }
        private static char normalizeChar(char c)
        {
            switch (c)
            {
                case 'ą':
                    return 'a';
                case 'ć':
                    return 'c';
                case 'ę':
                    return 'e';
                case 'ł':
                    return 'l';
                case 'ń':
                    return 'n';
                case 'ó':
                    return 'o';
                case 'ś':
                    return 's';
                case 'ż':
                    return 'z';
                case 'ź':
                    return 'z';
            }
            return c;
        }

        public static string GenerateUserLogin(string name, string surname, string employeeNumber)
        {
            bool isSufficientNameLength = name.Length >= 3;
            bool isSufficientSurnameLength = surname.Length >= 5;

            name = PrepareAndNormalize(name);
            surname = PrepareAndNormalize(surname);

            if (!isSufficientNameLength && !isSufficientSurnameLength)
            {
                return $"{surname}{name}{employeeNumber.Substring(employeeNumber.Length - (5 - surname.Length) - (3 - name.Length) - 4)}";
            }
            else if (!isSufficientSurnameLength)
            {
                return $"{surname}{name.Substring(0, 3)}{employeeNumber.Substring(employeeNumber.Length - (5 - surname.Length) - 4)}";
            }
            else if (!isSufficientNameLength)
            {
                return $"{surname.Substring(0, 5)}{name}{employeeNumber.Substring(employeeNumber.Length - (3 - name.Length) - 4)}";
            }
            else
            {
                return $"{surname.Substring(0, 5)}{name.Substring(0, 3)}{employeeNumber.Substring(employeeNumber.Length - 4)}";
            }
        }
    }
}
