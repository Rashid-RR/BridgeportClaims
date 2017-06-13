using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using BridgeportClaims.Common.Utilities;

namespace BridgeportClaims.Common.Extensions
{
    public static class StringExtensions
    {
        #region FormatWith
        /// <summary>
        /// Formats a string with one literal placeholder.
        /// </summary>
        /// <param name="text">The extension text</param>
        /// <param name="arg0">Argument 0</param>
        /// <returns>The formatted string</returns>
        public static string FormatWith(this string text, object arg0)
        {
            return string.Format(text, arg0);
        }

        /// <summary>
        /// Formats a string with two literal placeholders.
        /// </summary>
        /// <param name="text">The extension text</param>
        /// <param name="arg0">Argument 0</param>
        /// <param name="arg1">Argument 1</param>
        /// <returns>The formatted string</returns>
        public static string FormatWith(this string text, object arg0, object arg1)
        {
            return string.Format(text, arg0, arg1);
        }

        /// <summary>
        /// Formats a string with tree literal placeholders.
        /// </summary>
        /// <param name="text">The extension text</param>
        /// <param name="arg0">Argument 0</param>
        /// <param name="arg1">Argument 1</param>
        /// <param name="arg2">Argument 2</param>
        /// <returns>The formatted string</returns>
        public static string FormatWith(this string text, object arg0, object arg1, object arg2)
        {
            return string.Format(text, arg0, arg1, arg2);
        }

        /// <summary>
        /// Formats a string with a list of literal placeholders.
        /// </summary>
        /// <param name="text">The extension text</param>
        /// <param name="args">The argument list</param>
        /// <returns>The formatted string</returns>
        public static string FormatWith(this string text, params object[] args)
        {
            return string.Format(text, args);
        }

        /// <summary>
        /// Formats a string with a list of literal placeholders.
        /// </summary>
        /// <param name="text">The extension text</param>
        /// <param name="provider">The format provider</param>
        /// <param name="args">The argument list</param>
        /// <returns>The formatted string</returns>
        public static string FormatWith(this string text, IFormatProvider provider, params object[] args)
        {
            return string.Format(provider, text, args);
        }
        #endregion

        #region XmlSerialize XmlDeserialize
        /// <summary>Serialises an object of type T in to an xml string</summary>
        /// <typeparam name="T">Any class type</typeparam>
        /// <param name="objectToSerialise">Object to serialise</param>
        /// <returns>A string that represents Xml, empty oterwise</returns>
        public static string XmlSerialize<T>(this T objectToSerialise) where T : class
        {
            var serialiser = new XmlSerializer(typeof(T));
            string xml;
            using (var memStream = new MemoryStream())
            {
                using (var xmlWriter = new XmlTextWriter(memStream, Encoding.UTF8))
                {
                    serialiser.Serialize(xmlWriter, objectToSerialise);
                    xml = Encoding.UTF8.GetString(memStream.GetBuffer());
                }
            }

            // ascii 60 = '<' and ascii 62 = '>'
            xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
            xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
            return xml;
        }

        /// <summary>Deserialises an xml string in to an object of Type T</summary>
        /// <typeparam name="T">Any class type</typeparam>
        /// <param name="xml">Xml as string to deserialise from</param>
        /// <returns>A new object of type T is successful, null if failed</returns>
        public static T XmlDeserialize<T>(this string xml) where T : class
        {
            var serialiser = new XmlSerializer(typeof(T));
            T newObject;

            using (var stringReader = new StringReader(xml))
            {
                using (var xmlReader = new XmlTextReader(stringReader))
                {
                    try
                    {
                        newObject = serialiser.Deserialize(xmlReader) as T;
                    }
                    catch (InvalidOperationException) // String passed is not Xml, return null
                    {
                        return null;
                    }

                }
            }

            return newObject;
        }
        #endregion

        #region To X conversions
        /// <summary>
        /// Parses a string into an Enum
        /// </summary>
        /// <typeparam name="T">The type of the Enum</typeparam>
        /// <param name="value">String value to parse</param>
        /// <returns>The Enum corresponding to the stringExtensions</returns>
        public static T ToEnum<T>(this string value)
        {
            return ToEnum<T>(value, false);
        }

        /// <summary>
        /// Parses a string into an Enum
        /// </summary>
        /// <typeparam name="T">The type of the Enum</typeparam>
        /// <param name="value">String value to parse</param>
        /// <param name="ignorecase">Ignore the case of the string being parsed</param>
        /// <returns>The Enum corresponding to the stringExtensions</returns>
        public static T ToEnum<T>(this string value, bool ignorecase)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            value = value.Trim();

            if (value.Length == 0)
                throw new ArgumentNullException("Must specify valid information for parsing in the string.", "value");

            var t = typeof(T);
            if (!t.IsEnum)
                throw new ArgumentException("Type provided must be an Enum.", "T");

            return (T)Enum.Parse(t, value, ignorecase);
        }

        /// <summary>
        /// Toes the integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static int ToInteger(this string value, int defaultvalue)
        {
            return (int)ToDouble(value, defaultvalue);
        }
        /// <summary>
        /// Toes the integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int ToInteger(this string value)
        {
            return ToInteger(value, 0);
        }

        ///// <summary>
        ///// Toes the U long.
        ///// </summary>
        ///// <param name="value">The value.</param>
        ///// <returns></returns>
        //public static ulong ToULong(this string value)
        //{
        //    ulong def = 0;
        //    return value.ToULong(def);
        //}
        ///// <summary>
        ///// Toes the U long.
        ///// </summary>
        ///// <param name="value">The value.</param>
        ///// <param name="defaultvalue">The defaultvalue.</param>
        ///// <returns></returns>
        //public static ulong ToULong(this string value, ulong defaultvalue)
        //{
        //    return (ulong)ToDouble(value, defaultvalue);
        //}

        /// <summary>
        /// Toes the double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static double ToDouble(this string value, double defaultvalue)
        {
            double result;
            return double.TryParse(value, out result) ? result : defaultvalue;
        }

        /// <summary>
        /// Toes the double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static double ToDouble(this string value)
        {
            return ToDouble(value, 0);
        }

        /// <summary>
        /// Toes the date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string value, DateTime? defaultvalue)
        {
            DateTime result;
            return DateTime.TryParse(value, out result) ? result : defaultvalue;
        }

        /// <summary>
        /// Toes the date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string value)
        {
            return ToDateTime(value, null);
        }

        /// <summary>
        /// Converts a string value to bool value, supports "T" and "F" conversions.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>A bool based on the string value</returns>
        public static bool? ToBoolean(this string value)
        {
            if (String.Compare("T", value, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return true;
            }
            if (String.Compare("F", value, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return false;
            }
            bool result;
            if (bool.TryParse(value, out result))
            {
                return result;
            }
            else return null;
        }
        #endregion

        #region ValueOrDefault
        public static string GetValueOrEmpty(this string value)
        {
            return GetValueOrDefault(value, string.Empty);
        }
        public static string GetValueOrDefault(this string value, string defaultvalue)
        {
            if (value != null) return value;
            return defaultvalue;
        }
        #endregion

        #region ToUpperLowerNameVariant
        /// <summary>
        /// Converts string to a Name-Format where each first letter is Uppercase.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns></returns>
        public static string ToUpperLowerNameVariant(this string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            var valuearray = value.ToLower().ToCharArray();
            var nextupper = true;
            for (var i = 0; i < (valuearray.Count() - 1); i++)
            {
                if (nextupper)
                {
                    valuearray[i] = char.Parse(valuearray[i].ToString().ToUpper());
                    nextupper = false;
                }
                else
                {
                    switch (valuearray[i])
                    {
                        case ' ':
                        case '-':
                        case '.':
                        case ':':
                        case '\n':
                            nextupper = true;
                            break;
                        default:
                            nextupper = false;
                            break;
                    }
                }
            }
            return new string(valuearray);
        }
        #endregion

        #region Encrypt Decrypt
        /// <summary>
        /// Encrypts a string using the supplied key. Encoding is done using RSA encryption.
        /// </summary>
        /// <param name="stringToEncrypt">String that must be encrypted.</param>
        /// <param name="key">Encryptionkey.</param>
        /// <returns>A string representing a byte array separated by a minus sign.</returns>
        /// <exception cref="ArgumentException">Occurs when stringToEncrypt or key is null or empty.</exception>
        public static string Encrypt(this string stringToEncrypt, string key)
        {
            if (string.IsNullOrEmpty(stringToEncrypt))
            {
                throw new ArgumentException("An empty string value cannot be encrypted.");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cannot encrypt using an empty key. Please supply an encryption key.");
            }

            var cspp = new CspParameters {KeyContainerName = key};

            var rsa = new RSACryptoServiceProvider(cspp) {PersistKeyInCsp = true};

            var bytes = rsa.Encrypt(Encoding.UTF8.GetBytes(stringToEncrypt), true);

            return BitConverter.ToString(bytes);
        }

        /// <summary>
        /// Decrypts a string using the supplied key. Decoding is done using RSA encryption.
        /// </summary>
        /// <param name="stringToDecrypt"></param>
        /// <param name="key">Decryptionkey.</param>
        /// <returns>The decrypted string or null if decryption failed.</returns>
        /// <exception cref="ArgumentException">Occurs when stringToDecrypt or key is null or empty.</exception>
        public static string Decrypt(this string stringToDecrypt, string key)
        {
            if (string.IsNullOrEmpty(stringToDecrypt))
            {
                throw new ArgumentException("An empty string value cannot be encrypted.");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cannot decrypt using an empty key. Please supply a decryption key.");
            }

            var cspp = new CspParameters {KeyContainerName = key};

            var rsa = new RSACryptoServiceProvider(cspp) {PersistKeyInCsp = true};

            var decryptArray = stringToDecrypt.Split(new string[] { "-" }, StringSplitOptions.None);
            var decryptByteArray = Array.ConvertAll<string, byte>(decryptArray, (s => Convert.ToByte(byte.Parse(s, NumberStyles.HexNumber))));


            var bytes = rsa.Decrypt(decryptByteArray, true);

            var result = Encoding.UTF8.GetString(bytes);

            return result;
        }
        #endregion

        #region IsValidUrl
        /// <summary>
        /// Determines whether it is a valid URL.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is valid URL] [the specified text]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidUrl(this string text)
        {
            var rx = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            return rx.IsMatch(text);
        }
        #endregion

        #region IsValidEmailAddress
        /// <summary>
        /// Determines whether it is a valid email address
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is valid email address] [the specified s]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEmailAddress(this string email)
        {
            var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(email);
        }
        #endregion

        #region Email
        /// <summary>
        /// Send an email using the supplied string.
        /// </summary>
        /// <param name="body">String that will be used i the body of the email.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="sender">The email address from which the message was sent.</param>
        /// <param name="recipient">The receiver of the email.</param> 
        /// <param name="server">The server from which the email will be sent.</param>  
        /// <returns>A boolean value indicating the success of the email send.</returns>
        public static bool Email(this string body, string subject, string sender, string recipient, string server)
        {
            try
            {
                // To
                var mailMsg = new MailMessage();
                mailMsg.To.Add(recipient);

                // From
                var mailAddress = new MailAddress(sender);
                mailMsg.From = mailAddress;

                // Subject and Body
                mailMsg.Subject = subject;
                mailMsg.Body = body;

                // Init SmtpClient and send
                var smtpClient = new SmtpClient(server);
                var credentials = new NetworkCredential();
                smtpClient.Credentials = credentials;

                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Could not send mail from: {sender} to: {recipient} thru smtp server: {server}\n\n{ex.Message}", ex);
            }

            return true;
        }
        #endregion

        #region Truncate

        /// <summary>
        /// Truncates the string to a specified length and replace the truncated to a ...
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLength">total length of characters to maintain before the truncate happens</param>
        /// <returns>truncated string</returns>
        public static string Truncate(this string text, int maxLength)
        {
            // replaces the truncated string to a ...
            const string suffix = "...";
            var truncatedString = text;

            if (maxLength <= 0) return truncatedString;
            var strLength = maxLength - suffix.Length;

            if (strLength <= 0) return truncatedString;

            if (text == null || text.Length <= maxLength) return truncatedString;

            truncatedString = text.Substring(0, strLength);
            truncatedString = truncatedString.TrimEnd();
            truncatedString += suffix;
            return truncatedString;
        }
        #endregion
        
        #region Format
        /// <summary>
        /// Replaces the format item in a specified System.String with the text equivalent
        /// of the value of a specified System.Object instance.
        /// </summary>
        /// <param name="arg">The arg.</param>
        /// <param name="additionalArgs">The additional args.</param>
        public static string Format(this string format, object arg, params object[] additionalArgs)
        {
            if (additionalArgs == null || additionalArgs.Length == 0)
            {
                return string.Format(format, arg);
            }
            else
            {
                return string.Format(format, new object[] { arg }.Concat(additionalArgs).ToArray());
            }
        }
        #endregion

        #region IsNullOrEmpty
        /// <summary>
        /// Determines whether [is not null or empty] [the specified input].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is not null or empty] [the specified input]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotNullOrEmpty(this string input)
        {
            return !String.IsNullOrEmpty(input);
        }

        public static bool IsNullOrWhiteSpace(this string input)
        {
            return String.IsNullOrWhiteSpace(input);
        }

        public static bool IsNotNullOrWhiteSpace(this string input)
        {
            return !String.IsNullOrWhiteSpace(input);
        }

        #endregion

        /// <summary>
        /// Get substring of specified number of characters on the right. Mimicking SQL's "RIGHT" function.
        /// </summary>
        public static string Right(this string value, int length)
        {
            if (String.IsNullOrWhiteSpace(value)) return String.Empty;

            return value.Length <= length ? value : value.Substring(value.Length - length);
        }

        /// <summary>
        /// Allows for using strings in null coalescing operations
        /// </summary>
        /// <param name="value">The string value to check</param>
        /// <returns>Null if <paramref name="value"/> is empty or the original value of <paramref name="value"/>.</returns>
        public static string NullIfEmpty(this string value)
        {
            if (value == string.Empty)
                return null;

            return value;
        }

        /// <summary>
        /// Slugifies a string
        /// </summary>
        /// <param name="value">The string value to slugify</param>
        /// <param name="maxLength">An optional maximum length of the generated slug</param>
        /// <returns>A URL safe slug representation of the input <paramref name="value"/>.</returns>
        public static string ToSlug(this string value, int? maxLength = null)
        {
            if (null == value)
                throw new ArgumentNullException(nameof(value));

            // if it's already a valid slug, return it
            if (RegexUtils.SlugRegex.IsMatch(value))
                return value;

            return GenerateSlug(value, maxLength);
        }

        /// <summary>
        /// Converts a string into a slug that allows segments e.g. <example>.blog/2012/07/01/title</example>.
        /// Normally used to validate user entered slugs.
        /// </summary>
        /// <param name="value">The string value to slugify</param>
        /// <returns>A URL safe slug with segments.</returns>
        public static string ToSlugWithSegments(this string value)
        {
            if (null == value)
                throw new ArgumentNullException(nameof(value));

            var segments = value.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var result = segments.Aggregate(string.Empty, (slug, segment) => slug += "/" + segment.ToSlug());
            return result.Trim('/');
        }

        /// <summary>
        /// Separates a PascalCase string
        /// </summary>
        /// <example>
        /// "ThisIsPascalCase".SeparatePascalCase(); // returns "This Is Pascal Case"
        /// </example>
        /// <param name="value">The value to split</param>
        /// <returns>The original string separated on each uppercase character.</returns>
        public static string SeparatePascalCase(this string value)
        {
            if (null == value)
                throw new ArgumentNullException(nameof(value));
            return Regex.Replace(value, "([A-Z])", " $1").Trim();
        }

        /// <summary>
        /// Credit for this method goes to http://stackoverflow.com/questions/2920744/url-slugify-alrogithm-in-cs
        /// </summary>
        private static string GenerateSlug(string value, int? maxLength = null)
        {
            // prepare string, remove accents, lower case and convert hyphens to whitespace
            var result = RemoveAccent(value).Replace("-", " ").ToLowerInvariant();

            result = Regex.Replace(result, @"[^a-z0-9\s-]", string.Empty); // remove invalid characters
            result = Regex.Replace(result, @"\s+", " ").Trim(); // convert multiple spaces into one space

            if (maxLength.HasValue) // cut and trim
                result = result.Substring(0, result.Length <= maxLength ? result.Length : maxLength.Value).Trim();

            return Regex.Replace(result, @"\s", "-"); // replace all spaces with hyphens
        }

        /// <summary>
        /// Returns a string array containing the trimmed substrings in this <paramref name="value"/>
        /// that are delimited by the provided <paramref name="separators"/>.
        /// </summary>
        public static IEnumerable<string> SplitAndTrim(this string value, params char[] separators)
        {
            if (null == value)
                throw new ArgumentNullException(nameof(value));
            return value.Trim().Split(separators, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
        }

        /// <summary>
        /// Checks if the <paramref name="source"/> contains the <paramref name="input"/> based on the provided <paramref name="comparison"/> rules.
        /// </summary>
        public static bool Contains(this string source, string input, StringComparison comparison)
        {
            return source.IndexOf(input, comparison) >= 0;
        }

        /// <summary>
        /// Limits the length of the <paramref name="source"/> to the specified <paramref name="maxLength"/>.
        /// </summary>
        public static string Limit(this string source, int maxLength, string suffix = null)
        {
            if (suffix.IsNotNullOrEmpty())
            {
                maxLength = maxLength - suffix.Length;
            }

            if (source.Length <= maxLength)
            {
                return source;
            }

            return string.Concat(source.Substring(0, maxLength).Trim(), suffix ?? string.Empty);
        }

        private static string RemoveAccent(string value)
        {
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            return Encoding.ASCII.GetString(bytes);
        }

        public static string Titleize(this string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToSentenceCase());
        }

        public static string ToSentenceCase(this string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        }

        #region Validation Methods
        public static bool IsValidInt(this string integer)
        {
            if (integer != null)
                integer = integer.Trim();

            if (string.IsNullOrEmpty(integer))
                return false;
            else
            {
                Regex numericPattern = new Regex(@"^[-+]?\d*$");
                return numericPattern.IsMatch(integer);
            }
        }

        public static bool IsValidDateTime(this string dateTime)
        {
            DateTime date;
            if (DateTime.TryParse(dateTime, out date))
                return true;

            return false;
        }

        public static bool IsValidTime(this string time)
        {
            DateTime newDateTime = DateTime.Now;
            string dateTime = $"{newDateTime.ToShortDateString()} {time}";
            if (!DateTime.TryParse(dateTime, out newDateTime))
                return false;
            else
                return true;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            string val = (value == null) ? string.Empty : value.Trim();
            return string.IsNullOrEmpty(val);
        }

        /// <summary>
        /// Determines whether the specified input is empty.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// 	<c>true</c> if the specified input is empty; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"/>
        public static bool IsEmpty(this string input)
        {
            if (input == null) throw new ArgumentNullException();

            bool res = false;

            if (input.Length == 0)
            {
                res = true;
            }
            return res;
        }

        /// <summary>
        /// true, if the string contains only digits or float-point.
        /// Spaces are not considred.
        /// </summary>
        /// <param name="s">input string</param>
        /// <param name="floatpoint">true, if float-point is considered</param>
        /// <returns>true, if the string contains only digits or float-point</returns>
        public static bool IsNumberOnly(this string s, bool floatpoint)
        {
            s = s.Trim();
            if (s.Length == 0)
                return false;
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                {
                    if (floatpoint && (c == '.' || c == ','))
                        continue;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines whether entire string is UPPER case. 
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>
        /// 	<c>true</c> if [is case upper] [the specified input]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"/>
        public static bool IsCaseUpper(this string input)
        {
            if (input == null) throw new ArgumentNullException();

            if (input.IsEmpty())
            {
            }
            else
            {
                return String.Compare(input, input.ToUpper(), false) == 0;
            }
            return false;
        }

        /// <summary>
        /// Determines whether the string consists of just one char.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>
        /// 	<c>true</c> if [is repeated char] [the specified input]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"/>
        public static bool IsRepeatedChar(this string input)
        {
            if (input == null) throw new ArgumentNullException();

            //??? what about a string with only 1 char???
            if (input.IsEmpty()) return false;
            return input.Replace(input.Substring(0, 1), "").Length == 0;
        }

        /// <summary>
        /// Determines whether the string is pure whitespace.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// 	<c>true</c> if the specified input is whitespace; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"/>
        public static bool IsWhitespace(this string input)
        {
            if (input == null) throw new ArgumentNullException();

            if (input.IsEmpty()) return false;
            return input.Replace(" ", "").Length == 0;
        }

        public static bool IsRegExMatch(this string value, string regularExpression)
        {
            return new Regex(regularExpression).IsMatch(value);
        }
        #endregion

        public static bool IsEmail(this string email)
        {
            int indexOfAt = email.IndexOf('@');
            int indexOfDot = email.IndexOf('.', indexOfAt + 1);

            if (indexOfAt > indexOfDot || indexOfAt <= 0 || indexOfDot >= email.Length - 1)
                return false;

            return true;
        }

        #region Conversion Methods
        public static int ToInt(this string value, int defaultValue = 0)
        {
            int result;
            if (int.TryParse(value, out result))
                return result;

            return defaultValue;
        }

        public static long ToLong(this string value)
        {
            if (value.IsValidInt())
                return Convert.ToInt64(value);
            else
                return 0;
        }

        public static bool TryParseBool(this string text, out bool value)
        {
            string boolValue = (text ?? "").ToLower();
            if (boolValue == "1" || boolValue == "true" || boolValue == "t" || boolValue == "y" || boolValue == "yes" || boolValue == "on" || boolValue == "checked" || boolValue == "selected")
            {
                value = true;
                return true;
            }
            else if (boolValue == "0" || boolValue == "false" || boolValue == "f" || boolValue == "n" || boolValue == "no" || boolValue == "off" || boolValue == "unchecked" || boolValue == "unselected")
            {
                value = false;
                return true;
            }

            value = false;
            return false;
        }

        public static bool ToBool(this string value)
        {
            bool result;
            if (TryParseBool(value, out result))
                return result;

            return false;
        }

        public static List<string> ToStringList(this string value)
        {
            return GetFromStringList<string>(value, ',');
        }

        public static List<string> ToStringList(this string value, char separatingCharacter)
        {
            return GetFromStringList<string>(value, separatingCharacter);
        }

        /// <summary>
        /// Example: List<string> filterOptions = Utilities.String.GetStringList("All, A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z", Char.Parse(","));
        /// Works for strings, integers, bool, and enum lists right now - JHE
        /// </summary>
        /// <param name="commaSeparatedValues"></param>
        /// <param name="separatingCharacter"></param>
        /// <returns></returns>
        /// Test this - JHE
        public static List<T> GetFromStringList<T>(string commaSeparatedValues, char separatingCharacter)
        {
            Type objectType = typeof(T);
            T result = default(T);

            List<T> returnValue = new List<T>();
            char[] sep = { separatingCharacter };
            Array array = commaSeparatedValues.Split(sep);

            for (int i = 0; i < array.Length; i++)
            {
                string objectStringValue = array.GetValue(i).ToString().Trim();
                object obj = (T)result;
                if (obj is Enum)
                    result = (T)Enum.Parse(objectType, objectStringValue, true);
                if (obj is Boolean)
                {
                    string boolValue = objectStringValue.ToLower();
                    if (boolValue.ToBool())
                        result = (T)Convert.ChangeType(true, objectType, null);
                    else
                        result = (T)Convert.ChangeType(false, objectType, null);
                }
                else
                    result = (T)Convert.ChangeType(objectStringValue, objectType, null);

                returnValue.Add(result);
            }

            return returnValue;
        }

        public static string ToCleanString(this string value)
        {
            return value?.Trim() ?? string.Empty;
        }
        #endregion

        #region Remove/Replace Methods
        public static string ReplaceIgnoreCase(this string value, string oldValue, string newValue)
        {
            return Regex.Replace(value, oldValue, newValue, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        public static string RemoveIgnoreCase(this string value, string oldValue)
        {
            return Regex.Replace(value, oldValue, string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        public static string RemoveSpaces(this string value)
        {
            if (value != null)
                return value.Replace(" ", string.Empty);
            else
                return value;
        }

        /// <summary>
        /// Remove accent from strings 
        /// </summary>
        /// <example>
        ///  input:  "Příliš žluťoučký kůň úpěl ďábelské ódy."
        ///  result: "Prilis zlutoucky kun upel dabelske ody."
        /// </example>
        /// <param name="s"></param>
        /// <remarks>founded at http://stackoverflow.com/questions/249087/
        /// how-do-i-remove-diacritics-accents-from-a-string-in-net</remarks>
        /// <returns>string without accents</returns>
        public static string RemoveDiacritics(this string s)
        {
            if (s.IsNullOrEmpty())
                return string.Empty;

            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        /// <summary>
        /// Removes all extra white space, including leading and trailing whitespace.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <exception cref="T:System.ArgumentNullException"/>
        public static string RemoveExtraWhiteSpace(this string input)
        {
            if (input == null) throw new ArgumentNullException();

            // trim leading spaces
            input = input.Trim();
            return ReplaceWithMultipleSweeps(input, "  ", " ");
        }

        /// <summary>
        /// Replaces the all instances of stringFind with replaceWith. 
        /// Does multiple sweeps until there are no more matches, unlike String.Replace() which only does 1 sweep.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="stringToFind">The string to match.</param>
        /// <param name="replaceWith">The string to replace with.</param>
        /// <exception cref="T:System.ArgumentNullException"/>
        public static string ReplaceWithMultipleSweeps(this string input, string stringToFind, string replaceWith)
        {
            if (input == null) throw new ArgumentNullException();
            if (stringToFind == null) throw new ArgumentNullException();
            if (replaceWith == null) throw new ArgumentNullException();

            while (input.Contains(stringToFind))
            {
                input = input.Replace(stringToFind, replaceWith);
            }
            return input;
        }
        #endregion

        public static string CamelCase(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            if (text.Length <= 1)
                return text.ToUpper();

            text = text[0].ToString().ToUpper() + text.Substring(1, text.Length - 1).ToLower();
            if (!text.Contains(" "))
                return text;

            StringBuilder result = new StringBuilder(text);
            for (int i = 0; i < result.Length - 1; ++i)
            {
                if (result[i] == ' ')
                    result[i + 1] = result[i + 1].ToString().ToUpper()[0];
            }

            return result.ToString();
        }

        #region Caseing Methods
        public static string ProperCase(this string text)
        {
            try
            {
                text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
                text = text.Replace(" And ", " and ");
                text = text.Replace(" The ", " the ");
                text = text.Replace(" On ", " on ");
                text = text.Replace(" In ", " in ");
                text = text.Replace(" Llc", " LLC");
                text = text.Replace(" XX", " XX");
                text = text.Replace(" XXX", " XXX");
                return text;
            }
            catch { return ""; }
        }
        public static string ProperCaseIfUpper(this string value)
        {
            return (value != value.ToUpper()) ? value : value.ProperCase();
        }

        /// <summary>
        /// Takes a Pascal CASED string and inserts spaces:
        /// Example: "PascalCaseString" becomes "Pascal Case String" - JHE
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string PascalToSpaced(this string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            Regex regex = new Regex("(?<=[a-z])(?<x>[A-Z])|(?<=.)(?<x>[A-Z])(?=[a-z])");
            name = regex.Replace(name, " ${x}");
            // get rid of any underscores or dashes
            name = name.Replace("_", string.Empty);
            return name.Replace("-", string.Empty);
        }

        public static string ToTitleCase(this string text)
        {
            try
            {
                if (text.IsNullOrEmpty())
                    return string.Empty;

                string returnString = string.Empty;
                string[] arrayOfText = text.Split(' ');

                foreach (string item in arrayOfText)
                {
                    string s = item.ToLower();
                    string d = item[0].ToString().ToUpper();

                    for (int i = 1; i < s.Length; i++)
                        if (s[i - 1] == ' ')
                            d += s[i].ToString().ToUpper();
                        else
                            d += s[i].ToString();

                    returnString += " " + d;
                }

                return returnString.Trim();
            }
            catch { return text ?? string.Empty; }
        }
        #endregion

        public static string GetMd5Sum(this string str)
        {
            Encoder enc = Encoding.Unicode.GetEncoder();

            byte[] unicodeText = new byte[str.Length * 2];
            enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(unicodeText);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
                sb.Append(result[i].ToString("X2"));

            return sb.ToString();
        }

        public static bool ContainsIgnoreCase(this string value, string stringValue)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(stringValue))
                return false;
            return value.ToUpper().Contains(stringValue.ToUpper());
        }

        /// <summary>
        /// Reverse the string
        /// from http://en.wikipedia.org/wiki/Extension_method
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Reverse(this string input)
        {
            char[] chars = input.ToCharArray();
            Array.Reverse(chars);
            return new String(chars);
        }

        public static string ToFullMask(this string value)
        {
            string returnValue = string.Empty;
            for (int i = 0; i < value.Length; i++)
            {
                returnValue += "*";
            }

            return returnValue;
        }

        public static bool IsMatch(this string input, string pattern)
        {
            return new Regex(pattern).IsMatch(input);
        }

        public enum PhoneFormats
        {
            OnlyNumbers /*1231231234*/,
            Dashes /*123-123-1234*/,
            ParenthesisAndDash/*(123) 123-1234*/
        }

        public static string PhoneFormat(this string phone, PhoneFormats format)
        {
            if (!string.IsNullOrEmpty(phone))
            {
                StringBuilder allDigits = new StringBuilder();
                foreach (char c in phone)
                {
                    if (Char.IsDigit(c))
                        allDigits.Append(c);
                }
                string phoneDigits = allDigits.ToString();
                if (phoneDigits.Length == 10)
                {
                    switch (format)
                    {
                        case PhoneFormats.OnlyNumbers:
                            return allDigits.ToString();
                        case PhoneFormats.Dashes:
                            return phoneDigits.Substring(0, 3) + '-' + phoneDigits.Substring(3, 3) + '-' + phoneDigits.Substring(6);
                        case PhoneFormats.ParenthesisAndDash:
                            return '(' + phoneDigits.Substring(0, 3) + ") " + phoneDigits.Substring(3, 3) + '-' + phoneDigits.Substring(6);
                    }
                }
            }
            return "";
        }

        public static bool StartsWithAny(this string text, string[] values)
        {
            return values.Any(text.StartsWith);
        }

        public static string AppendBackSlash(this string path)
        {
            return path.EndsWith("\\") ? path : path + "\\";
        }

        public static string AppendForwardSlash(this string path)
        {
            return path.EndsWith("/") ? path : path + "/";
        }

        /// <summary>
        /// Returns the number of characters you specify starting from the left.
        /// By Kris Komar
        /// </summary>
        /// <param name="s">This string.</param>
        /// <param name="count">How many characters to take.</param>
        /// <returns>The number of characters you specify starting from the left</returns>
        public static string Left(this string s, int count)
        {
            return String.IsNullOrEmpty(s) ? String.Empty : s.Substring(0, Math.Min(s.Length, count));
        }

        /// <summary>
        /// Returns the number of characters you specify starting from the specified index.
        /// By Kris Komar
        /// </summary>
        /// <param name="s">This string.</param>
        /// <param name="index">Which index to start at.</param>
        /// <param name="count">How many characters to take.</param>
        /// <returns>The number of characters you specify starting from the specified index.</returns>
        public static string Mid(this string s, int index, int count)
        {
            if (String.IsNullOrEmpty(s)) { return String.Empty; }
            var mincount = Math.Min(s.Length, count);
            var minindex = Math.Min(s.Length, index);
            return s.Substring(minindex, mincount);
        }

        /// <summary>
        /// Returns true if the string is a valid Int32.
        /// By Kris Komar
        /// </summary>
        /// <param name="s">This string.</param>
        /// <returns>True if the string is a valid Int32.</returns>
        public static bool IsInteger(this string s)
        {
            if (String.IsNullOrEmpty(s)) { return false; }
            var regularExpression = new Regex("^-[0-9]+$|^[0-9]+$");
            return regularExpression.Match(s).Success;
        }

        /// <summary>
        /// Parses the string at spaces and returns the word at the specified word index.
        /// By Kris Komar
        /// </summary>
        /// <param name="s">This string.</param>
        /// <param name="index">The index of the word you wish to retrieve.</param>
        /// <returns>The word at the specified word index.</returns>
        public static string GetWordAtIndex(this string s, int index)
        {
            if (String.IsNullOrEmpty(s)) { return String.Empty; }
            try
            {
                var name = s.Trim();
                var nameWords = name.Split(' ');
                return nameWords[index];
            }
            catch (Exception ex)
            {
                throw new Exception("GetWordAtIndex() caused an exception:", ex); // Will most likely be index out of range
            }
        }

        /// <summary>
        /// Gets the first word in the string.
        /// By Kris Komar
        /// </summary>
        /// <param name="s">This string.</param>
        /// <returns>The first word in the string.</returns>
        public static string FirstWordOnly(this string s)
        {
            return String.IsNullOrEmpty(s) ? String.Empty : GetWordAtIndex(s, 0);
        }

        /// <summary>
        /// Removes any line feed characters in the string and replaces them with a space.
        /// By Kris Komar
        /// </summary>
        /// <param name="s">This string.</param>
        /// <returns>The string without linefeed characters (replaced with spaces).</returns>
        public static string RemoveLineFeeds(this string s)
        {
            return String.IsNullOrEmpty(s) ? String.Empty : Regex.Replace(s, @"[\n\r\t]", " ");
        }

        /// <summary>
        /// Removes all whitespace characters from a string.
        /// By Kris Komar
        /// </summary>
        /// <param name="s">This string.</param>
        /// <returns>The string without whitespace characters.</returns>
        public static string RemoveWhiteSpace(this string s)
        {
            return String.IsNullOrEmpty(s) ? String.Empty : Regex.Replace(s, @"\s", "");
        }
    }
}