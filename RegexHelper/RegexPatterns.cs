using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerbiSharp.Infrastructure.BaseInfrastructure.RegexHelper
{
    public class RegexPatterns
    {
        public const string NUMBER_REGEX_PATTERN = @"(?<WholeNumber>(?:\d{1,3})(?:,\d{3})+|\d+)\.?(?<FractionalNumber>\d+)?";
        public const string IP_REGEX_PATTERN = @"\b(?:\d{1,3}.){3}\d{1,3}\b";
        public const string SHEBA_NUMBER_PATTERN = @"(?<=IR)?\d{24}\b";
        public const string PROTECTED_EMAIL_STRUCTURE_PATTERN = @"^((?!\.)[\w\-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$";
    }
}
