using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BridgeportClaims.Common.Extensions
{
	/// <summary>
	/// Enum Extentions - JHE
	/// </summary>
	public static class EnumExtensions
	{
		#region Conversion Methods
		/// <summary>
		/// To return the integer value of the Enum as a string.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToIntString(this Enum value)
		{
			return Convert.ToInt32(value).ToString();
		}

	    /// <summary>
		/// To return the integer value of an Enum. - JHE
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static int ToInt(this Enum value)
		{
			return Convert.ToInt32(value);
		}
		#endregion

		/// <summary>
		/// Takes a Pascal CASED string and inserts spaces:
		/// Example: "PascalCaseString" becomes "Pascal Case String"
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string PascalToSpaced(this Enum value)
		{
			return value.ToString().PascalToSpaced();
		}

		public static IList<T> GetValues<T>(this Enum enumeration)
		{
			return GetValues<T>();
		}
		public static IList<T> GetValues<T>()
		{
			var enumType = typeof(T);
			if (!enumType.IsEnum)
				throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");

			IList<T> values = new List<T>();

			var fields = from field in enumType.GetFields()
						 where field.IsLiteral
						 select field;

			foreach (var field in fields)
			{
				var value = field.GetValue(enumType);
				values.Add((T)value);
			}

			return values;
		}

		public static IEnumerable<T> GetValues<T>(this Type enumType)
		{
			if (!enumType.IsEnum)
				throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
			return Enum.GetNames(enumType).Select(t => (T)Enum.Parse(enumType, t));
		}

		public static IEnumerable<T> GetValues<T>(this T e)
		{
			var enumType = typeof(T);
			if (!enumType.IsEnum)
				throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
			return Enum.GetNames(enumType).Select(t => (T)Enum.Parse(enumType, t));
		}

		public static IList<EnumNameValue> GetValuesList<T>(this Enum enumeration)
		{
			return GetValuesList<EnumNameValue>();
		}
		public static IList<EnumNameValue> GetValuesList<T>()
		{
			var enumType = typeof(T);
			if (!enumType.IsEnum)
				throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");

			IList<EnumNameValue> values = new List<EnumNameValue>();

			var fields = from field in enumType.GetFields()
						 where field.IsLiteral
						 select field;

			foreach (var field in fields)
			{
				var value = field.GetValue(enumType);
				values.Add(new EnumNameValue() { Name = ((T)value).ToString().PascalToSpaced(), Value = ((T)value as Enum).ToInt() });
			}

			return values;
		}
	}

	public class EnumNameValue
	{
		public string Name { get; set; }
		public int Value { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
