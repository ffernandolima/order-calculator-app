using System;
using System.ComponentModel;
using System.Configuration;

namespace OrderCalculator.App.Configuration
{
	internal class Settings
	{
		// Static holder for instance, need to use lambda to construct since constructor private
		private static readonly Lazy<Settings> _instance = new Lazy<Settings>(() => new Settings(), isThreadSafe: true);

		// Private to prevent direct instantiation
		private Settings()
		{
			this.AppSettingsToObject();
		}

		// Accessor for instance
		public static Settings Instance => _instance.Value;

		public decimal ValueAddedTax { get; private set; }

		private void AppSettingsToObject()
		{
			foreach (var property in this.GetType().GetProperties())
			{
				var value = ConfigurationManager.AppSettings[property.Name];

				if (!string.IsNullOrEmpty(value))
				{
					var propertyType = property.PropertyType;

					var converter = TypeDescriptor.GetConverter(propertyType);

					if (converter.CanConvertFrom(typeof(string)))
					{
						property.SetValue(this, converter.ConvertFrom(value), null);
					}
				}
			}
		}
	}
}
