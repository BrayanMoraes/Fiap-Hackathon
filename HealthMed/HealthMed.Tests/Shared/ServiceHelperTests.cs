using HealthMed.Domain.Enum;
using HealthMed.Domain.Shared;

namespace HealthMed.Tests.Shared
{
    public class ServiceHelperTests
    {
        [Fact]
        public void HandleException_ShouldReturnOperationResultWithErrorStatus()
        {
            // Arrange
            var exception = new Exception("Test exception");

            // Act
            var result = ServiceHelper.HandleException<string>(exception);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(TypeReturnStatus.Error, result.Status);
            Assert.Equal("Erro: Test exception", result.Message);
            Assert.Null(result.ResultObject);
        }

        [Fact]
        public void HandleException_ShouldIncludeCustomMessage_WhenProvided()
        {
            // Arrange
            var exception = new Exception("Test exception");
            var customMessage = "Custom error message";

            // Act
            var result = ServiceHelper.HandleException<string>(exception, customMessage);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(TypeReturnStatus.Error, result.Status);
            Assert.Equal(customMessage, result.Message);
            Assert.Null(result.ResultObject);
        }

        [Fact]
        public void HandleException_ShouldHandleNullException()
        {
            // Arrange
            Exception exception = null;

            // Act
            var result = ServiceHelper.HandleException<string>(exception, "Custom error message");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(TypeReturnStatus.Error, result.Status);
            Assert.Equal("Custom error message", result.Message);
            Assert.Null(result.ResultObject);
        }
    }
}
