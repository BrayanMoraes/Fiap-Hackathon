using HealthMed.Domain.DTO;

namespace HealthMed.Tests.DTO
{
    public class GerenciarAgendaDTOTests
    {
        [Fact]
        public void IsHorarioValido_ShouldReturnTrue_WhenHorarioIsWithinValidRange()
        {
            // Arrange
            var dto = new GerenciarAgendaDTO
            {
                Horario = TimeSpan.FromHours(10) // Horário válido
            };

            // Act
            var result = dto.IsHorarioValido();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsHorarioValido_ShouldReturnFalse_WhenHorarioIsOutsideValidRange()
        {
            // Arrange
            var dto = new GerenciarAgendaDTO
            {
                Horario = TimeSpan.FromHours(20) // Horário inválido
            };

            // Act
            var result = dto.IsHorarioValido();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDataValida_ShouldReturnTrue_WhenDataIsTodayOrFuture()
        {
            // Arrange
            var dto = new GerenciarAgendaDTO
            {
                Data = DateTime.Now.AddDays(1) // Data válida (futuro)
            };

            // Act
            var result = dto.IsDataValida();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDataValida_ShouldReturnFalse_WhenDataIsInThePast()
        {
            // Arrange
            var dto = new GerenciarAgendaDTO
            {
                Data = DateTime.Now.AddDays(-1) // Data inválida (passado)
            };

            // Act
            var result = dto.IsDataValida();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValorConsultaValido_ShouldReturnTrue_WhenValorConsultaIsGreaterThanZero()
        {
            // Arrange
            var dto = new GerenciarAgendaDTO
            {
                ValorConsulta = 150.00m // Valor válido
            };

            // Act
            var result = dto.IsValorConsultaValido();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValorConsultaValido_ShouldReturnFalse_WhenValorConsultaIsZeroOrNegative()
        {
            // Arrange
            var dto = new GerenciarAgendaDTO
            {
                ValorConsulta = 0.00m // Valor inválido
            };

            // Act
            var result = dto.IsValorConsultaValido();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsAgendaValida_ShouldReturnTrue_WhenAllValidationsPass()
        {
            // Arrange
            var dto = new GerenciarAgendaDTO
            {
                Horario = TimeSpan.FromHours(10), // Horário válido
                Data = DateTime.Now.AddDays(1),  // Data válida
                ValorConsulta = 150.00m         // Valor válido
            };

            // Act
            var result = dto.IsAgendaValida();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAgendaValida_ShouldReturnFalse_WhenAnyValidationFails()
        {
            // Arrange
            var dto = new GerenciarAgendaDTO
            {
                Horario = TimeSpan.FromHours(20), // Horário inválido
                Data = DateTime.Now.AddDays(1),  // Data válida
                ValorConsulta = 150.00m         // Valor válido
            };

            // Act
            var result = dto.IsAgendaValida();

            // Assert
            Assert.False(result);
        }
    }
}
