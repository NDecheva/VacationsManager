using Moq;
using System;
using System.Threading.Tasks;
using NUnit.Framework;
using VacationsManager.Services;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;
using VacationsManager.Shared.Enums;

namespace VacationManager.Tests.Services
{
    public class VacationRequestServiceTests
    {
        private readonly Mock<IVacationRequestRepository> _vacationRequestRepositoryMock = new Mock<IVacationRequestRepository>();
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private readonly Mock<INotificationService> _notificationServiceMock = new Mock<INotificationService>();
        private readonly IVacationRequestService _service;

        public VacationRequestServiceTests()
        {
            _service = new VacationRequestService(
                _vacationRequestRepositoryMock.Object,
                _userServiceMock.Object,
                _notificationServiceMock.Object);
        }

        [Test]
        public async Task WhenCreateAsync_WithValidData_ThenSaveAsync()
        {
            // Arrange
            var vacationRequestDto = new VacationRequestDto
            {
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(5),
                IsHalfDay = false,
                IsApproved = false,
                Requester = new UserDto { Id = 1, Username = "Jane Doe" },
                Attachment = "vacation_attachment.pdf",
                VacationType = VacationType.PaidLeave
            };

            // Act
            await _service.SaveAsync(vacationRequestDto);

            // Assert
            _vacationRequestRepositoryMock.Verify(x => x.SaveAsync(vacationRequestDto), Times.Once());
        }

        [Test]
        public async Task WhenSaveAsync_WithNull_ThenThrowArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.SaveAsync(null));

            _vacationRequestRepositoryMock.Verify(x => x.SaveAsync(null), Times.Never);
        }

        [Theory]
        [TestCase(1)]
        [TestCase(22)]
        [TestCase(131)]
        public async Task WhenDeleteAsync_WithCorrectId_ThenCallDeleteAsyncInRepository(int id)
        {
            // Act
            await _service.DeleteAsync(id);

            // Assert
            _vacationRequestRepositoryMock.Verify(x => x.DeleteAsync(It.Is<int>(i => i.Equals(id))), Times.Once);
        }

        [Theory]
        [TestCase(1)]
        [TestCase(22)]
        [TestCase(131)]
        public async Task WhenGetByIdAsync_WithValidRequestId_ThenReturnVacationRequest(int requestId)
        {
            // Arrange
            var vacationRequestDto = new VacationRequestDto
            {
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(5),
                IsHalfDay = false,
                IsApproved = false,
                Requester = new UserDto { Id = 1, Username = "Jane Doe" },
                Attachment = "vacation_attachment.pdf",
                VacationType = VacationType.PaidLeave
            };

            _vacationRequestRepositoryMock.Setup(x => x.GetByIdAsync(It.Is<int>(x => x.Equals(requestId))))
                .ReturnsAsync(vacationRequestDto);

            // Act
            var vacationRequestResult = await _service.GetByIdIfExistsAsync(requestId);

            // Assert
            _vacationRequestRepositoryMock.Verify(x => x.GetByIdAsync(requestId), Times.Once());
            Assert.That(vacationRequestResult == vacationRequestDto);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(102021)]
        public async Task WhenGetByIdAsync_WithInvalidRequestId_ThenReturnDefault(int requestId)
        {
            // Arrange
            var vacationRequest = (VacationRequestDto)default;

            _vacationRequestRepositoryMock.Setup(s => s.GetByIdAsync(It.Is<int>(x => x.Equals(requestId))))
                .ReturnsAsync(vacationRequest);

            // Act
            var vacationRequestResult = await _service.GetByIdIfExistsAsync(requestId);

            // Assert
            _vacationRequestRepositoryMock.Verify(x => x.GetByIdAsync(requestId), Times.Once());
            Assert.That(vacationRequestResult == vacationRequest);
        }

        [Test]
        public async Task WhenUpdateAsync_WithValidData_ThenSaveAsync()
        {
            // Arrange
            var vacationRequestDto = new VacationRequestDto
            {
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(5),
                IsHalfDay = false,
                IsApproved = true,
                Requester = new UserDto { Id = 1, Username = "Jane Doe" },
                Attachment = "updated_vacation_attachment.pdf",
                VacationType = VacationType.UnpaidLeave
            };

            _vacationRequestRepositoryMock.Setup(s => s.SaveAsync(It.Is<VacationRequestDto>(x => x.Equals(vacationRequestDto))))
                .Verifiable();

            // Act
            await _service.SaveAsync(vacationRequestDto);

            // Assert
            _vacationRequestRepositoryMock.Verify(x => x.SaveAsync(vacationRequestDto), Times.Once());
        }
    }
}
