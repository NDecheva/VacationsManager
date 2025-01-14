using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Services;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;

namespace VacationManager.Tests.Services
{
    public class NotificationServiceTests
    {
        private readonly Mock<INotificationRepository> _notificationRepositoryMock = new Mock<INotificationRepository>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>(); 
        private readonly INotificationService _service;

        public NotificationServiceTests()
        {
            _service = new NotificationService(_notificationRepositoryMock.Object, _mapperMock.Object); 
        }

        [Test]
        public async Task WhenCreateAsync_WithValidData_ThenSaveAsync()
        {
            // Arrange
            var notificationDto = new NotificationDto
            {
                RecipientId = 1,
                Recipient = new UserDto { Id = 1, Username = "John Doe" },
                Message = "Test Message",
                DateSent = DateTime.UtcNow,
                IsRead = false
            };

            // Act
            await _service.SaveAsync(notificationDto);

            // Assert
            _notificationRepositoryMock.Verify(x => x.SaveAsync(notificationDto), Times.Once());
        }

        [Test]
        public async Task WhenSaveAsync_WithNull_ThenThrowArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.SaveAsync(null));

            _notificationRepositoryMock.Verify(x => x.SaveAsync(null), Times.Never);
        }

        [Theory]
        [TestCase(1)]
        [TestCase(22)]
        [TestCase(131)]
        public async Task WhenDeleteAsync_WithCorrectId_ThenCallDeleteAsyncInRepository(int id)
        {
            // Arrange
            // Act
            await _service.DeleteAsync(id);

            // Assert
            _notificationRepositoryMock.Verify(x => x.DeleteAsync(It.Is<int>(i => i.Equals(id))), Times.Once);
        }

        [Theory]
        [TestCase(1)]
        [TestCase(22)]
        [TestCase(131)]
        public async Task WhenGetByIdAsync_WithValidNotificationId_ThenReturnNotification(int notificationId)
        {
            // Arrange
            var notificationDto = new NotificationDto
            {
                RecipientId = 1,
                Recipient = new UserDto { Id = 1, Username = "John Doe" },
                Message = "Test Message",
                DateSent = DateTime.UtcNow,
                IsRead = false
            };

            _notificationRepositoryMock.Setup(x => x.GetByIdAsync(It.Is<int>(x => x.Equals(notificationId))))
                .ReturnsAsync(notificationDto);

            // Act
            var notificationResult = await _service.GetByIdIfExistsAsync(notificationId);

            // Assert
            _notificationRepositoryMock.Verify(x => x.GetByIdAsync(notificationId), Times.Once());
            Assert.That(notificationResult == notificationDto);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(102021)]
        public async Task WhenGetByIdAsync_WithInvalidNotificationId_ThenReturnDefault(int notificationId)
        {
            // Arrange
            var notification = (NotificationDto)default;

            _notificationRepositoryMock.Setup(s => s.GetByIdAsync(It.Is<int>(x => x.Equals(notificationId))))
                .ReturnsAsync(notification);

            // Act
            var notificationResult = await _service.GetByIdIfExistsAsync(notificationId);

            // Assert
            _notificationRepositoryMock.Verify(x => x.GetByIdAsync(notificationId), Times.Once());
            Assert.That(notificationResult == notification);
        }

        [Test]
        public async Task WhenUpdateAsync_WithValidData_ThenSaveAsync()
        {
            // Arrange
            var notificationDto = new NotificationDto
            {
                RecipientId = 1,
                Recipient = new UserDto { Id = 1, Username = "John Doe" },
                Message = "Updated Test Message",
                DateSent = DateTime.UtcNow,
                IsRead = true
            };

            _notificationRepositoryMock.Setup(s => s.SaveAsync(It.Is<NotificationDto>(x => x.Equals(notificationDto))))
                .Verifiable();

            // Act
            await _service.SaveAsync(notificationDto);

            // Assert
            _notificationRepositoryMock.Verify(x => x.SaveAsync(notificationDto), Times.Once());
        }
    }
}
