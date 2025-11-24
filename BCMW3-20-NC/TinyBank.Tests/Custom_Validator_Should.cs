using TinyBank.Service.Attributes;
using TinyBank.Service.Validator;

namespace TinyBank.Tests
{
    public enum TestEnum
    {
        None = 0,
        Value1 = 1
    }
    public class TestDto
    {
        [CustomRequired]
        public string RequiredString { get; set; }

        [CustomRequired]
        public int RequiredInt { get; set; }

        [CustomRequired]
        public TestEnum RequiredEnum { get; set; }

        [CustomMinLength(3)]
        public string MinLengthString { get; set; }

        [CustomMaxLength(5)]
        public string MaxLengthString { get; set; }
    }

    public class Custom_Validator_Should
    {

        #region Required Attribute Tests

        [Fact]
        public void ReturnError_WhenRequiredStringIsNull()
        {
            // Arrange
            var dto = new TestDto
            {
                RequiredString = null
            };

            // Act
            var result = CustomValidator.Validate(dto);

            // Assert
            Assert.Contains("RequiredString is required.", result);
        }


        [Fact]
        public void ReturnError_WhenRequiredStringIsEmpty()
        {
            // Arrange
            var dto = new TestDto
            {
                RequiredString = ""
            };

            // Act
            var result = CustomValidator.Validate(dto);

            // Assert
            Assert.Contains("RequiredString cannot be empty.", result);
        }


        [Fact]
        public void ReturnError_WhenRequiredIntIsDefault()
        {
            // Arrange
            var dto = new TestDto
            {
                RequiredInt = 0
            };

            // Act
            var result = CustomValidator.Validate(dto);

            // Assert
            Assert.Contains("RequiredInt cannot be default value.", result);
        }


        [Fact]
        public void ReturnError_WhenRequiredEnumIsDefault()
        {
            // Arrange
            var dto = new TestDto
            {
                RequiredEnum = TestEnum.None
            };

            // Act
            var result = CustomValidator.Validate(dto);

            // Assert
            Assert.Contains("RequiredEnum must be a valid enum value.", result);
        }


        [Fact]
        public void NotReturnErrors_WhenRequiredFieldsAreValid()
        {
            // Arrange
            var dto = new TestDto
            {
                RequiredString = "Valid",
                RequiredInt = 10,
                RequiredEnum = TestEnum.Value1
            };

            // Act
            var result = CustomValidator.Validate(dto);

            // Assert
            Assert.Empty(result);
        }

        #endregion


        #region Min Length Tests
        [Fact]
        public void ReturnError_WhenStringIsShorterThanMinLength()
        {
            // Arrange
            var dto = new TestDto
            {
                MinLengthString = "ab" // < 3
            };

            // Act
            var result = CustomValidator.Validate(dto);

            // Assert
            Assert.Contains("MinLengthString must be at least 3 characters.", result);
        }

        [Fact]
        public void NotReturnError_WhenStringMeetsMinLength()
        {
            // Arrange
            var dto = new TestDto
            {
                MinLengthString = "abc"
            };

            // Act
            var result = CustomValidator.Validate(dto);

            // Assert
            Assert.DoesNotContain(result, x => x.Contains("MinLengthString"));
        }
        #endregion


        #region Max Length Tests
        [Fact]
        public void ReturnError_WhenStringExceedsMaxLength()
        {
            // Arrange
            var dto = new TestDto
            {
                MaxLengthString = "123456" // > 5
            };

            // Act
            var result = CustomValidator.Validate(dto);

            // Assert
            Assert.Contains("MaxLengthString must be no longer than 5 characters.", result);
        }

        [Fact]
        public void NotReturnError_WhenStringWithinMaxLength()
        {
            // Arrange
            var dto = new TestDto
            {
                MaxLengthString = "12345"
            };

            // Act
            var result = CustomValidator.Validate(dto);

            // Assert
            Assert.DoesNotContain(result, x => x.Contains("MaxLengthString"));
        }
        #endregion


        #region Null Object Tests
        [Fact]
        public void ReturnError_WhenObjectIsNull()
        {
            // Act
            var result = CustomValidator.Validate(null);

            // Assert
            Assert.Contains("Object cannot be null.", result);
        }
        #endregion

    }
}
