// (c) 2019 SharpCoding
// This code is licensed under MIT license (see LICENSE.txt for details)
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using SharpCoding.SharpHelpers.DomainModel;
using SharpCoding.SharpHelpers.PrivateMethods;

namespace SharpCoding.SharpHelpers
{
    public static class StringHelper
    {
        /// <summary>
        /// The method replaces all occurrences of the strings toReplace with the replaceWith
        /// </summary>
        /// <param name="istance"></param>
        /// <param name="toReplace"></param>
        /// <param name="replaceWith"></param>
        /// <returns></returns>
        public static string Replace(this string istance, string[] toReplace, string replaceWith)
        {
            return string.IsNullOrEmpty(istance)
                ? string.Empty
                : toReplace.Aggregate(istance, (current, str) => current.Replace(str, replaceWith));
        }

        /// <summary>
        ///  This method clears string to avoid SQLInjection
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public static string SqlInjectionSanitize(this string stringValue)
        {
            if (null == stringValue)
                return string.Empty;
            return stringValue.RegexReplace("-{2,}", "-") // transforms multiple --- in - use to comment in sql scripts
                .RegexReplace(@"[*/]+", string.Empty) // removes / and * used also to comment in sql scripts
                .RegexReplace(
                    @"(;|\s)(exec|execute|select|insert|update|delete|create|alter|drop|rename|truncate|backup|restore)\s",
                    string.Empty, RegexOptions.IgnoreCase).Trim();
        }

        /// <summary>
        /// Formats the string according to the specified mask
        /// </summary>
        /// <param name="istance">The input string.</param>
        /// <param name="mask">The mask for formatting. Like "A##-##-T-###Z"</param>
        /// <returns>The formatted string</returns>
        public static string FormatWithMask(this string istance, string mask)
        {
            if (string.IsNullOrEmpty(istance)) return istance;
            var output = string.Empty;
            var index = 0;
            foreach (var m in mask)
            {
                if (m == '#')
                {
                    if (index >= istance.Length) continue;
                    output += istance[index];
                    index++;
                }
                else
                    output += m;
            }
            return output;
        }

        private static string RegexReplace(this string stringValue, string matchPattern, string toReplaceWith)
        {
            return Regex.Replace(stringValue, matchPattern, toReplaceWith);
        }

        private static string RegexReplace(this string stringValue, string matchPattern, string toReplaceWith, RegexOptions regexOptions)
        {
            return Regex.Replace(stringValue, matchPattern, toReplaceWith, regexOptions);
        }

        /// <summary>
        /// This method return the last substring after the string split 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator">separator character</param>
        /// <returns></returns>
        public static string LastAfter(this string str, char separator)
        {
            return str.Split(separator).Last();
        }

        /// <summary>
        /// This method executes equals operation between two strings with no case sensitive 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="comparand"></param>
        /// <returns></returns>
        public static bool IsDbEqual(this string str, string comparand)
        {
            return string.Equals(str, comparand, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// This method executes not equals operation between two strings with no case sensitive 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="comparand"></param>
        /// <returns></returns>
        public static bool IsNotDbEqual(this string s, string comparand)
        {
            return !IsDbEqual(s, comparand);
        }

        /// <summary>
        /// Given a list of strings, the method check if the instance is contained in the collection, ignoring case
        /// </summary>
        /// <param name="s"></param>
        /// <param name="comparands"></param>
        /// <returns></returns>
        public static bool IsInDb(this string s, params string[] comparands)
        {
            return comparands.Any(x => IsDbEqual(x, s));
        }

        /// <summary>
        /// The method returns the substring of the instance
        /// </summary>
        /// <param name="s"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string SafeSubstringByLength(this string s, int maxLength)
        {
            if (string.IsNullOrEmpty(s) || s.Length <= maxLength || maxLength <= 0)
                return s;
            return s.Substring(0, maxLength);
        }

        /// <summary>
        /// The method truncates the instance and check if the substring result is shorter than the original string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength"></param>
        /// <param name="truncated"></param>
        /// <returns></returns>
        public static string Truncate(this string str, int maxLength, out bool truncated)
        {
            truncated = false;
            var substr = SafeSubstringByLength(str, maxLength);
            var length1 = substr?.Length;
            var length2 = str?.Length;
            if (length1.GetValueOrDefault() == length2.GetValueOrDefault())
                return substr;
            truncated = true;
            return substr;
        }

        /// <summary>
        /// The method parses the instance to a nullable int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int? ToNullableInt(this string str)
        {
            return int.TryParse(str, out var result) ? new int?(result) : null;
        }

        /// <summary>
        /// Return the Base64 encoding
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToBase64(this string str)
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes(str), Base64FormattingOptions.None);
        }

        /// <summary>
        /// Return the string from the Base64 encoding
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FromBase64(this string str)
        {
            return Encoding.Default.GetString(Convert.FromBase64String(str));
        }

        /// <summary>
        /// Returns a string from the left side with a fixed length of characters
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length">The number of characters </param>
        public static string Left(this string str, int length)
        {
            if (length == 0 || str.Length == 0) return string.Empty;
            var result = str;
            if (length < str.Length)
            {
                result = str.Substring(0, length);
            }
            return result;
        }

        /// <summary>
        /// Returns a string from the right side with a fixed length of characters
        /// </summary>
        /// <param name="str"></param>
        ///<param name="length">The number of characters </param>
        public static string Right(this string str, int length)
        {
            if (length == 0 || str.Length == 0) return string.Empty;
            var result = str;
            if (length < str.Length)
            {
                result = str.Substring(str.Length - length);
            }
            return result;
        }

        /// <summary>
        /// Returns a object from Json
        /// </summary>
        /// <param name="strJson"></param>
        public static T JsonToObject<T>(this string strJson)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(strJson, options);
        }

        /// <summary>
        /// Returns true if the string is in the email address 
        /// </summary>
        /// <param name="str"></param>
        public static bool IsEmail(this string str)
        {
            return Regex.Match(str,
                @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                RegexOptions.IgnoreCase).Success;
        }

        /// <summary>
        /// Returns true if the string is a numeric 
        /// </summary>
        /// <param name="str"></param>
        public static bool IsNumber(this string str)
        {
            return Regex.Match(str, @"^[0-9]\d{0,9}((\.|\,)\d{1,3})?%?$", RegexOptions.IgnoreCase).Success;
        }

        /// <summary>
        /// Returns true if the string is a Guid 
        /// </summary>
        /// <param name="str"></param>
        public static bool IsGuid(this string str)
        {
            return Regex.Match(str,
                @"^[A-Fa-f0-9]{32}$|" +
                "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$",
                RegexOptions.IgnoreCase).Success;
        }

        /// <summary>
        /// Count the specific word in a given string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="word"></param>
        public static int WordCount(this string str, string word)
        {
            return str.Contains(word) ? new Regex(word).Matches(str).Count : 0;
        }

        /// <summary>
        /// Returns true if the string is a tax code
        /// </summary>
        /// <param name="str"></param>
        public static bool IsValidFiscalCode(this string str)
        {
            return Regex.Match(str,
                @"^(?:[A-Z][AEIOU][AEIOUX]|[B-DF-HJ-NP-TV-Z]{2}[A-Z]){2}(?:[\dLMNP-V]{2}(?:[A-EHLMPR-T](?:[04LQ][1-9MNP-V]|[15MR][\dLMNP-V]|[26NS][0-8LMNP-U])|[DHPS][37PT][0L]|[ACELMRT][37PT][01LM]|[AC-EHLMPR-T][26NS][9V])|(?:[02468LNQSU][048LQU]|[13579MPRTV][26NS])B[26NS][9V])(?:[A-MZ][1-9MNP-V][\dLMNP-V]{2}|[A-M][0L](?:[1-9MNP-V][\dLMNP-V]|[0L][1-9MNP-V]))[A-Z]$",
                RegexOptions.IgnoreCase).Success;
        }

        /// <summary>
        /// Returns true if the string is a iban
        /// </summary>
        /// <param name="str"></param>
        public static bool IsValidIban(this string str)
        {
            return Regex.Match(str,
                @"[a-zA-Z]{2}[0-9]{2}[a-zA-Z0-9]{4}[0-9]{7}([a-zA-Z0-9]?){0,16}",
                RegexOptions.IgnoreCase).Success;
        }

        /// <summary>
        /// Returns true if the string is a url path
        /// </summary>
        /// <param name="str"></param>
        public static bool IsValidUrl(this string str)
        {
            return Regex.Match(str,
                @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?",
                RegexOptions.IgnoreCase).Success;
        }

        /// <summary>
        ///  This method clears string from Html Tags or from <script></script> block
        /// </summary>
        /// <param name="input"></param>
        /// <param name="mode">CleanTextMode option: AllHtmlTags/ScriptTags</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">Thrown when a not supported value is passed for <paramref name="mode"/></exception>  
        /// <returns>Cleared string</returns>
        public static string CleanText(this string input,CleanTextMode mode)
        {
            return mode switch
            {
                CleanTextMode.AllHtmlTags => input.StripHtml(),
                CleanTextMode.ScriptTagx => input.RemoveScriptTag(),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
            };
        }

        /// <summary>
        /// Gets the matches between delimiters.
        /// </summary>
        /// <param name="input">The source string.</param>
        /// <param name="beginDelim">The beginning string delimiter.</param>
        /// <param name="endDelim">The end string delimiter.</param>
        /// <returns></returns>

        public static IEnumerable<string> ExtractBetween(this string input,string beginDelim, string endDelim)
        {
            var reg = new Regex($"(?<={Regex.Escape(beginDelim)})(.+?)(?={Regex.Escape(endDelim)})");
            var matches = reg.Matches(input);
            return (from Match m in matches select m.Value).ToList();
        }

        /// <summary>
        /// Check the string for possible SqlInjection.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsSqlInjection(this string input)
        {
            string[] sqlCheckList = {
                "char","nchar","varchar","nvarchar", "alter","begin","cast","create",
                "cursor","declare","delete","drop", "end","exec","execute","fetch","insert",
                "kill","select","sys","sysobjects","syscolumns","table","update"
            };

            var checkString = input.ToLower().Replace("'", "''");
            for (var i = 0; i <= sqlCheckList.Length - 1; i++)
            {
                if (Regex.IsMatch(checkString, $"\\b{sqlCheckList[i]}\\b"))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns an int from an input string.
        ///  </summary>
        /// <returns></returns>
        public static int ToInt32(this string value,CultureInfo culture = null)
        {
            var isValidInt = int.TryParse(value, NumberStyles.AllowThousands |
                                                    NumberStyles.AllowParentheses |
                                                    NumberStyles.AllowCurrencySymbol|
                                                    NumberStyles.AllowLeadingSign, culture ?? CultureInfo.CurrentCulture,
                out var result);

            return isValidInt ? result : 0;
        }

        /// <summary>
        /// Remove all digits from an input string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveDigits(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return Regex.Replace(value, @"[\d-]", string.Empty)?.Trim();
        }

        /// <summary>
        /// Remove all chars from an input string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string OnlyDigits(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return Regex.Replace(value, @"[^0-9]", string.Empty)?.Trim();
        }
    }
}
