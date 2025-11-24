using System.Reflection;
using System.Text;
using TinyBank.Repository.Attributes;

namespace TinyBank.Service.Helpers
{
    public static class DtoGenerator
    {
        public async static Task GenerateDtosInFolder(Assembly assembly, string outputFolder)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            var entityTypes = assembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    t.IsPublic &&
                    !t.IsAbstract &&
                    t.GetCustomAttributes(typeof(DtoTransformable), inherit: false).Length != 0
                )
                .ToList();

            entityTypes.ForEach(async entity => await GenerateDtosForEntities(entity, outputFolder));
        }

        private static async Task GenerateDtosForEntities(Type entity, string outputFolder)
        {
            var props = entity.GetProperties();
            var idProp = props.FirstOrDefault(p => p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));
            var nonIdProps = props.Where(p => p != idProp).ToList();

            //DTO კლასების დასახელებები
            string createDtoName = $"{entity.Name}ForCreatingDto";
            string getDtoName = $"{entity.Name}ForGettingDto";
            string updateDtoName = $"{entity.Name}ForUpdatingDto";

            //კოდის გენერაცია
            string createCode = GenerateClassCode(createDtoName, nonIdProps);
            string getCode = GenerateClassCode(getDtoName, props);
            string updateCode = GenerateClassCode(updateDtoName, props);

            //შექმნილი dto კლასების ფაილში დამახსოვრება
            await File.WriteAllTextAsync(Path.Combine(outputFolder, createDtoName + ".cs"), createCode);
            await File.WriteAllTextAsync(Path.Combine(outputFolder, getDtoName + ".cs"), getCode);
            await File.WriteAllTextAsync(Path.Combine(outputFolder, updateDtoName + ".cs"), updateCode);
        }

        private static string GenerateClassCode(string dtoName, IEnumerable<PropertyInfo> props)
        {
            StringBuilder sb = new();
            sb.AppendLine("using TinyBank.Repository.Models.Enums;");
            sb.AppendLine();
            sb.AppendLine("namespace TinyBank.Service.Dtos;");
            sb.AppendLine();
            sb.AppendLine($"public class {dtoName}");
            sb.AppendLine("{");
            foreach (var prop in props)
                sb.AppendLine($"\t public {prop.PropertyType.Name} {prop.Name} {{ get; set; }}");
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
