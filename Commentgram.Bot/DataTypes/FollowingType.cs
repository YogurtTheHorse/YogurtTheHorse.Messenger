using System;
using System.ComponentModel;
using System.Globalization;

using YogurtTheHorse.Messenger.Localizations;

namespace Commentgram.Bot {
	public enum FollowingType {
		Everything = 0,
		Nothing = 1
	}

	public class FollowingTypeConverter : EnumConverter {
		public FollowingTypeConverter() : base(typeof(FollowingType)) { }

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
			if (value != null && value is string stringValue) {
				foreach (FollowingType followingType in Enum.GetValues(typeof(FollowingType))) {
					foreach (string localeName in LocaleManager.GetLocalesNames()) {
						if (LocaleManager.GetString($"FollowingType.{followingType}", localeName) == stringValue) {
							return followingType;
						}
					}
				}
			}

			return base.ConvertFrom(context, culture, value);
		}
	}
}