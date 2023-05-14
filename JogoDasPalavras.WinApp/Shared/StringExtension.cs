using System.Globalization;
using System.Text;

namespace JogoDasPalavras.WinApp.Shared
{
    public static class StringExtension
    {
        public static string FirstLetterToUpperCase(this string text)
        {
            return new string(text.Substring(0, 1).ToUpper() + text.Substring(1));
        }

        public static string RemoveDiacritics(this string text)
        {
            return new string(text
                    .Normalize(NormalizationForm.FormD)
                    .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    .ToArray())
                    .Normalize(NormalizationForm.FormC);
        }
    }
}
