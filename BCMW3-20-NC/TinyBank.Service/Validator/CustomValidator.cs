using TinyBank.Service.Attributes;

namespace TinyBank.Service.Validator
{
    public static class CustomValidator
    {
        /// <summary>
        /// CustomRequired validator
        /// </summary>
        public static List<string> ValidateRequired(object obj)
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
                bool isRequired = prop.IsDefined(typeof(CustomRequired), true);
                if (!isRequired) continue;

                var value = prop.GetValue(obj);

                if (value == null)
                {
                    errors.Add($"{prop.Name} is required.");
                    continue;
                }

                // String
                if (prop.PropertyType == typeof(string))
                {
                    if (string.IsNullOrWhiteSpace((string)value))
                        errors.Add($"{prop.Name} cannot be empty.");
                    continue;
                }

                // Enum
                if (prop.PropertyType.IsEnum)
                {
                    // check default (0)
                    var defaultEnum = Activator.CreateInstance(prop.PropertyType);

                    if (value.Equals(defaultEnum))
                        errors.Add($"{prop.Name} must be a valid enum value.");
                    continue;
                }

                // Value types (int, decimal, etc.)
                if (prop.PropertyType.IsValueType)
                {
                    var defaultValue = Activator.CreateInstance(prop.PropertyType);
                    if (value.Equals(defaultValue))
                        errors.Add($"{prop.Name} cannot be default value.");
                }
            }

            return errors;
        }


    }
}
