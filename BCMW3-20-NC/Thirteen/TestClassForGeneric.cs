namespace Thirteen
{
    public class TestClassForGeneric<T> /*where T : IEnumerable*/
    {
        public T? Identifier { get; set; }
        public string? Name { get; set; }
    }

    public interface MyInterface<T>
    {
        public void Test(T value);
    }

    class MyClass : MyInterface<string>
    {
        public void Test(string value)
        {
            throw new NotImplementedException();
        }
    }
}
