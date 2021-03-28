using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Resources
{
    /// <summary>
    /// A class to generate login for <c>WMSIdentityUser</c> class
    /// </summary>
    class UserLoginGenerator
    {
        /// <summary>
        /// A method that normalizes sent string and invoke normalize
        /// </summary>
        /// <param name="word">String to normalize</param>
        /// <returns>Invoke method which send back normalized string</returns>
        private static string PrepareAndNormalize(string word)
        {
            
            word = word.Trim().ToLower();
            return Normalize(word);

        }
        /// <summary>
        /// A method contains logic to normalize sent string
        /// </summary>
        /// <param name="word">String to normalize</param>
        /// <returns>Normalized string</returns>
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
                normalizedArray[i] = NormalizeChar(charArray[i]);
            }
            return new string(normalizedArray);
        }
        /// <summary>
        /// A method to normalize polish characters
        /// </summary>
        /// <param name="c">Polish char to normalize</param>
        /// <returns>Normalized polish char</returns>
        private static char NormalizeChar(char c)
        {
            return c switch
            {
                'ą' => 'a',
                'ć' => 'c',
                'ę' => 'e',
                'ł' => 'l',
                'ń' => 'n',
                'ó' => 'o',
                'ś' => 's',
                'ż' => 'z',
                'ź' => 'z',
                _ => c,
            };
        }
        /// <summary>
        /// A method which uses derived arguments to generate login for <c>WMSIdentityUser</c> class
        /// </summary>
        /// <param name="name"><c>WMSIdentityUser</c>'s name</param>
        /// <param name="surname"><c>WMSIdentityUser</c>'s surname</param>
        /// <param name="employeeNumber"><c>WMSIdentityUser</c>'s employee number</param>
        /// <returns></returns>
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
