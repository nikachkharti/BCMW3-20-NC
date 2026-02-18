using Forum.Application.Contracts.Service;
using Forum.Application.Services;
using MimeKit;
using Moq;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Forum.Tests
{
    public class NotificationServiceShould
    {
        private const string _destinationEmailAddress = "nika.chkhartishvili7@gmail.com";

        [Fact]
        [Trait("Forum", "NotificationService")]
        public async Task Return_Success_When_Email_Sent_Successfully()
        {
            ///1. Arrange

            //1.1 Mock IConfiguration
            var mockConfig = new Mock<IConfiguration>();
            mockConfig
                .Setup(c => c["EmailSettings:Sender"])
                .Returns("nika.chkhartishvili7@gmail.com");
            mockConfig
                .Setup(c => c["EmailSettings:SmtpServer"])
                .Returns("smtp.gmail.com");
            mockConfig
                .Setup(c => c["EmailSettings:Port"])
                .Returns("465");
            mockConfig
                .Setup(c => c["EmailSettings:UseSsl"])
                .Returns("true");
            mockConfig
                .Setup(c => c["EmailSettings:Username"])
                .Returns("nika.chkhartishvili7@gmail.com");
            mockConfig
                .Setup(c => c["EmailSettings:Password"])
                .Returns("Password123!");

            //1.2 Mock ISmtpClient
            var mockSmtp = new Mock<ISmtpClientWrapper>();
            mockSmtp
                .Setup(s => s.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.CompletedTask);
            mockSmtp
                .Setup(s => s.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            mockSmtp
                .Setup(s => s.SendAsync(It.IsAny<MimeMessage>()))
                .Returns(Task.CompletedTask);
            mockSmtp
                .Setup(s => s.DisconnectAsync(It.IsAny<bool>()))
                .Returns(Task.CompletedTask);


            //1.3 Create mocked NotificationService instance
            var service = new NotificationService(mockConfig.Object, mockSmtp.Object);



            ///2. Act
            var result = await service.SendAsync(
                _destinationEmailAddress,
                "Email From Xunit",
                "<p>Hello From Xunit</p>");



            ///3. Assert
            Assert.True(result.success);
            Assert.Contains("Message sent successfully", result.message);

        }


        [Fact]
        [Trait("Forum", "NotificationService")]
        public async Task Send_ShouldReturnFailure_WhenEmailIsInvalid()
        {
            //Arrange
            var mockConfig = new Mock<IConfiguration>();
            var mockSmtp = new Mock<ISmtpClientWrapper>();

            var service = new NotificationService(mockConfig.Object, mockSmtp.Object);

            //Act
            var result = await service.SendAsync("invalid-email", "Test", "Body");

            //Assert
            Assert.False(result.success);
            Assert.Contains("Sending email must be a valid email address", result.message, StringComparison.OrdinalIgnoreCase);
            mockSmtp.Verify(s => s.SendAsync(It.IsAny<MimeMessage>()), Times.Never);
        }



    }
}
