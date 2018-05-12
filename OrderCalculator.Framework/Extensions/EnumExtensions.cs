using System;
using System.ComponentModel;

namespace OrderCalculator.Framework.Extensions
{
	public static class EnumExtensions
	{
		public static TEnum GetValueFromDescription<TEnum>(this string description) where TEnum : struct
		{
			var defaultValue = default(TEnum);

			if (string.IsNullOrWhiteSpace(description))
			{
				return defaultValue;
			}

			var type = typeof(TEnum);

			if (type.IsEnum)
			{
				var fields = type.GetFields();

				foreach (var field in fields)
				{
					var attribute = (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute);

					if (attribute != null)
					{
						if (attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
						{
							var value = (TEnum)field.GetValue(null);

							return value;
						}
					}
					else
					{
						if (field.Name.Equals(description, StringComparison.OrdinalIgnoreCase))
						{
							var value = (TEnum)field.GetValue(null);

							return value;
						}
					}
				}
			}

			return defaultValue;
		}
	}
}
