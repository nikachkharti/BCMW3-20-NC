using System.Reflection;
using TinyBank.Service.Attributes;

namespace TinyBank.Service.Validators
{
    public static class CustomValidator
    {
        public static List<string> Validate(object obj)
        {
            var erros = new List<string>();

            if (obj == null)
            {
                erros.Add("Object cannot be null.");
                return erros;
            }

            var type = obj.GetType();
            var properties = type.GetProperties();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj);

                //REQUIRED
                if (prop.IsDefined(typeof(CustomRequired), inherit: true))
                {
                    if (value == null)
                    {
                        erros.Add($"{prop.Name} is required.");
                        continue;
                    }

                    if (prop.PropertyType == typeof(string) && string.IsNullOrWhiteSpace((string)value))
                    {
                        erros.Add($"{prop.Name} cannot be empty.");
                        continue;
                    }

                    if (prop.PropertyType.IsValueType)
                    {
                        var defaultValue = Activator.CreateInstance(prop.PropertyType);
                        if (value.Equals(defaultValue))
                            erros.Add($"{prop.Name} cannot be default value.");
                    }

                    if (prop.PropertyType.IsEnum)
                    {
                        var defaultValue = Activator.CreateInstance(prop.PropertyType);
                        if (value.Equals(defaultValue))
                            erros.Add($"{prop.Name} must be a valid enum value.");
                    }
                }

                //MIN LENGTH
                var minLengthAttr = prop.GetCustomAttribute<CustomMinLength>(true);
                if (minLengthAttr != null && value != null)
                {
                    if (prop.PropertyType == typeof(string))
                    {
                        if (((string)value).Length < minLengthAttr.Length)
                            erros.Add($"{prop.Name} must be no longer than {minLengthAttr.Length} characters");
                    }
                }


                //MAX LENGTH
                var maxLengthAttr = prop.GetCustomAttribute<CustomMaxLength>(true);
                if (maxLengthAttr != null && value != null)
                {
                    if (prop.PropertyType == typeof(string))
                    {
                        if (((string)value).Length > maxLengthAttr.Length)
                            erros.Add($"{prop.Name} must be no longer than {maxLengthAttr.Length} characters");
                    }
                }

            }


            return erros;
        }
    }
}
