namespace Algorithms.Tests
{
    public class CustomFirstOrDefault_Should
    {
        [Fact]
        public void ReturnFirstElement()
        {
            var numbers = new[] { 1, 2, 3 };
            var expected = 1;

            var actual = numbers.CustomFirstOrDefault(x => x > 0);

            Assert.Equal(expected, actual);
        }

    }
}
