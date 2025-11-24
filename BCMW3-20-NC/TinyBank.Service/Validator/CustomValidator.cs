using System.Reflection;
using TinyBank.Service.Attributes;

namespace TinyBank.Service.Validator
{
    public static class CustomValidator
    {
        public static List<string> Validate(object obj)
        {
            var errors = new List<string>();

            if (obj == null)
            {
                errors.Add("Object cannot be null.");
                return errors;
            }

            var type = obj.GetType();
            var properties = type.GetProperties();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj);

                // ---------------------------
                //  REQUIRED
                // ---------------------------
                if (prop.IsDefined(typeof(CustomRequired), true))
                {
                    if (value == null)
                    {
                        errors.Add($"{prop.Name} is required.");
                        continue;
                    }

                    if (prop.PropertyType == typeof(string) &&
                        string.IsNullOrWhiteSpace((string)value))
                    {
                        errors.Add($"{prop.Name} cannot be empty.");
                        continue;
                    }

                    if (prop.PropertyType.IsEnum)
                    {
                        var defaultEnum = Activator.CreateInstance(prop.PropertyType);

                        if (value.Equals(defaultEnum))
                            errors.Add($"{prop.Name} must be a valid enum value.");
                    }

                    if (prop.PropertyType.IsValueType)
                    {
                        var defaultValue = Activator.CreateInstance(prop.PropertyType);
                        if (value.Equals(defaultValue))
                            errors.Add($"{prop.Name} cannot be default value.");
                    }
                }

                // ---------------------------
                //  MIN LENGTH
                // ---------------------------
                var minLengthAttr = prop.GetCustomAttribute<CustomMinLength>(true);
                if (minLengthAttr != null && value != null)
                {
                    if (prop.PropertyType == typeof(string))
                    {
                        if (((string)value).Length < minLengthAttr.Length)
                            errors.Add($"{prop.Name} must be at least {minLengthAttr.Length} characters.");
                    }
                }

                // ---------------------------
                //  MAX LENGTH
                // ---------------------------
                var maxLengthAttr = prop.GetCustomAttribute<CustomMaxLength>(true);
                if (maxLengthAttr != null && value != null)
                {
                    if (prop.PropertyType == typeof(string))
                    {
                        if (((string)value).Length > maxLengthAttr.Length)
                            errors.Add($"{prop.Name} must be no longer than {maxLengthAttr.Length} characters.");
                    }
                }
            }

            return errors;
        }
    }
}
