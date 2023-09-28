#pragma warning disable 
using System.Globalization;
using System.Text.RegularExpressions;
using Delfi.Glo.Entities.Dto;

namespace Delfi.Glo.Common.Utility;

public static class Common
{
    public static DateTime? ParseDynameterDateTime(string dateTime)
    {
        string cleanedDateTimeString = Regex.Replace(dateTime, "(?<=\\d)(st|nd|rd|th)", "");
        var result = DateTime.TryParse(cleanedDateTimeString, out DateTime outObj);
        return result ? outObj : null;
    }


}
#pragma warning restore
