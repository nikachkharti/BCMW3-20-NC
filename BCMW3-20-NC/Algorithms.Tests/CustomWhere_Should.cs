namespace Algorithms.Tests
{
    public class CustomWhere_Should
    {
        [Fact]
        public void FindAllElements()
        {
            //Arrange - მომზადება
            List<int> data = new()
            { 11, 12, 7, 33, 21, -88, 351, 12131, 345, 22, 22 };

            var expected = new List<int>() { 7, 21, 12131 };

            //Act - მოქმედება
            var actual = data.CustomWhere(x => x % 7 == 0);

            //Assert - მტკიცება
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ValidateSource()
        {
            List<int> source = null;
            Assert.Throws<ArgumentNullException>(
                () => source.CustomWhere(x => x % 7 == 0)
            );
        }

        [Fact]
        public void ValidatePredicate()
        {
            List<int> source = new() { 1, 2, 3 };
            Func<int, bool> predicate = null;

            Assert.Throws<ArgumentNullException>(
                () => source.CustomWhere(predicate)
            );
        }

    }
}
