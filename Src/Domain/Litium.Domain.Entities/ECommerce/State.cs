using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Litium.Resources;

namespace Litium.Domain.Entities.Shop
{
    [TypeConverter(typeof(EnumToLocalizedName))]
    public enum State
    {
        [Display(Description = "Canceled", ResourceType = typeof(WebStroreResource))]
        Canceled = 0,

        [Display(Description = "Created", ResourceType = typeof(WebStroreResource))]
        Created = 1,

        [Display(Description = "Paid", ResourceType = typeof(WebStroreResource))]
        Payed = 2,

        [Display(Description = "Delivered", ResourceType = typeof(WebStroreResource))]
        Delivered = 3
    }

    public class EnumToLocalizedName : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType.Equals(typeof(Enum)));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return (destinationType.Equals(typeof(String)));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (!(value is Enum))
            {
                throw new ArgumentException(@"Can only convert an instance of enum.", "value");
            }

            return ((Enum)value).GetLocalizedName();
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (!destinationType.Equals(typeof(String)))
            {
                throw new ArgumentException(@"Can only convert to string.", "destinationType");
            }

            if (!(value is Enum))
            {
                throw new ArgumentException(@"Can only convert an instance of enum.", "value");
            }

            return ((Enum)value).GetLocalizedName();
        }
    }

    public static class StateEnumExtension
    {
        public static string GetLocalizedName(this Enum @enum)
        {
            if (@enum == null)
                return null;

            string description = @enum.ToString();
            FieldInfo fieldInfo = @enum.GetType().GetField(description);
            var attributes = (DisplayAttribute[])fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
            
            if (attributes.Any())
            {
                description = attributes[0].GetDescription();
            }
            return description;
        }
    }
}