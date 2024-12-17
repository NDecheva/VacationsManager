using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationsManager.Services;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Enums;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;

namespace VacationManager.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
        private readonly IUserService _service;

        public UserServiceTests()
        {
            _service = new UserService(_userRepositoryMock.Object);
        }

        [Test]
        public async Task WhenCreateAsync_WithValidData_ThenSaveAsync()
        {
            // Arrange
            var userDto = new UserDto
            {
                Username = "john_doe",
                FirstName = "John",
                LastName = "Doe",
                Role = new RoleDto { Name = "Developer", RoleType = RoleType.Developer },
                Team = new TeamDto { Name = "Development Team", ProjectId = 1, TeamLeaderId = 1 },
                VacationRequests = new List<VacationRequestDto>
                {
                    new VacationRequestDto { Id = 1, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(5) }
                }
            };

            // Act
            await _service.SaveAsync(userDto);

            // Assert
            _userRepositoryMock.Verify(x => x.SaveAsync(userDto), Times.Once());
        }

        [Test]
        public async Task WhenSaveAsync_WithNull_ThenThrowArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.SaveAsync(null));

            _userRepositoryMock.Verify(x => x.SaveAsync(null), Times.Never());
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
            _userRepositoryMock.Verify(x => x.DeleteAsync(It.Is<int>(i => i.Equals(id))), Times.Once);
        }

        [Theory]
        [TestCase(1)]
        [TestCase(22)]
        [TestCase(131)]
        public async Task WhenGetByIdAsync_WithValidUserId_ThenReturnUser(int userId)
        {
            // Arrange
            var userDto = new UserDto
            {
                Username = "john_doe",
                FirstName = "John",
                LastName = "Doe",
                Role = new RoleDto { Name = "Developer", RoleType = RoleType.Developer },
                Team = new TeamDto { Name = "Development Team", ProjectId = 1, TeamLeaderId = 1 },
                VacationRequests = new List<VacationRequestDto>
                {
                    new VacationRequestDto { Id = 1, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(5) }
                }
            };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(It.Is<int>(x => x.Equals(userId))))
                .ReturnsAsync(userDto);

            // Act
            var userResult = await _service.GetByIdIfExistsAsync(userId);

            // Assert
            _userRepositoryMock.Verify(x => x.GetByIdAsync(userId), Times.Once());
            Assert.That(userResult == userDto);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(102021)]
        public async Task WhenGetByIdAsync_WithInvalidUserId_ThenReturnDefault(int userId)
        {
            // Arrange
            var user = (UserDto)default;

            _userRepositoryMock.Setup(s => s.GetByIdAsync(It.Is<int>(x => x.Equals(userId))))
                .ReturnsAsync(user);

            // Act
            var userResult = await _service.GetByIdIfExistsAsync(userId);

            // Assert
            _userRepositoryMock.Verify(x => x.GetByIdAsync(userId), Times.Once());
            Assert.That(userResult == user);
        }

        [Test]
        public async Task WhenUpdateAsync_WithValidData_ThenSaveAsync()
        {
            // Arrange
            var userDto = new UserDto
            {
                Username = "john_doe_updated",
                FirstName = "John",
                LastName = "Doe",
                Role = new RoleDto { Name = "TeamLead", RoleType = RoleType.TeamLead },
                Team = new TeamDto { Name = "Updated Team", ProjectId = 2, TeamLeaderId = 2 },
                VacationRequests = new List<VacationRequestDto>
                {
                    new VacationRequestDto { Id = 1, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(5) }
                }
            };

            _userRepositoryMock.Setup(s => s.SaveAsync(It.Is<UserDto>(x => x.Equals(userDto))))
               .Verifiable();

            // Act
            await _service.SaveAsync(userDto);

            // Assert
            _userRepositoryMock.Verify(x => x.SaveAsync(userDto), Times.Once());
        }
    }
}
