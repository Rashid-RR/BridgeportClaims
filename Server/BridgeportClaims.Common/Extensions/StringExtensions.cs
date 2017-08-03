using System;
using System.Collections.Generic;

namespace BridgeportClaims.Common.Extensions
{
	public static class StringExtensions
	{
		public static string Left(this string value, int maxLength)
		{
			if (value.IsNullOrWhiteSpace()) return value;
			maxLength = Math.Abs(maxLength);

			return value.Length <= maxLength
				? value
				: value.Substring(0, maxLength);
		}

		public static string Right(this string value, int maxLength)
		{
			if (value.IsNullOrWhiteSpace()) return value;
			maxLength = Math.Abs(maxLength);

			return value.Length <= maxLength
				? value
				: value.Substring(value.Length - maxLength, maxLength);
		}

		public static bool IsNotNullOrEmpty(this string _this) => !string.IsNullOrEmpty(_this);

		public static bool IsNotNullOrWhiteSpace(this string _this) => !string.IsNullOrWhiteSpace(_this);

		public static bool IsNullOrWhiteSpace(this string _this) => string.IsNullOrWhiteSpace(_this);

		public static string Repeat(this string _this, int count) => Repeat(_this, count, string.Empty);

		public static string Repeat(this string _this, int count, string separator)
		{
			if (count <= 0 || string.IsNullOrEmpty(_this))
			{
				return string.Empty;
			}
			var list = new List<string>();
			for (var i = 0; i < count; i++)
				list.Add(_this);
			return string.Join(separator, list);
		}


		public static bool ContainsAll(this string _this, params string[] values)
		{
			if (null == _this)
				throw new NullReferenceException("String.ContainsAll() can not be called on a null value.");
			for (int i = 0, l = values.Length; i < l; i++)
			{
				if (!_this.Contains(values[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static bool ContainsAny(this string _this, params string[] values)
		{
			if (_this == null)
			{
				throw new NullReferenceException("String.ContainsAny() can not be called on a null value.");
			}
			for (int i = 0, l = values.Length; i < l; i++)
			{
				if (_this.Contains(values[i]))
				{
					return true;
				}
			}
			return false;
		}
	}
}