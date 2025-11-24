namespace TinyBank.Service.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomMinLength : Attribute
    {
        public int Length { get; }

        public CustomMinLength(int length)
        {
            Length = length;
        }
    }
}
