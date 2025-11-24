namespace TinyBank.Service.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomMaxLength : Attribute
    {
        public int Length { get; }

        public CustomMaxLength(int length)
        {
            Length = length;
        }
    }
}
