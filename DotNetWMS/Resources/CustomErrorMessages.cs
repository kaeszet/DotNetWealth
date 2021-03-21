using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Resources
{
    /// <summary>
    /// The class responsible for storing custom error messages
    /// </summary>
    public class CustomErrorMessages
    {
        public const string FieldIsRequired = "Pole \"{0}\" jest wymagane!";
        public const string IncorrectEmailAdress = "Adres e-mail posiada nieprawidłowy format!";
        public const string NumberRange = "Wartość musi być większa od {1} i mniejsza od {2}";
        public const string MaxLength = "Proszę o podanie nie więcej niż {0} znaków.";
    }
}
