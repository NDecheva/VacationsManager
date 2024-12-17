using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationsManager.Services;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;

namespace VacationManager.Tests.Services
{
    public class TeamServiceTests
    {
        private readonly Mock<ITeamRepository> _teamRepositoryMock = new Mock<ITeamRepository>();
        private readonly ITeamService _service;

        public TeamServiceTests()
        {
            _service = new TeamService(_teamRepositoryMock.Object);
        }

        [Test]
        public async Task WhenCreateAsync_WithValidData_ThenSaveAsync()
        {
            // Arrange
            var teamDto = new TeamDto
            {
                Name = "Development Team",
                ProjectId = 1,
                TeamLeaderId = 1,
                Project = new ProjectDto { Id = 1, Name = "Project A" },
                TeamLeader = new UserDto { Id = 1, Username = "John Doe" },
                Developers = new List<UserDto>
                {
                    new UserDto { Id = 2, Username = "Alice" },
                    new UserDto { Id = 3, Username = "Bob" }
                }
            };

            // Act
            await _service.SaveAsync(teamDto);

            // Assert
            _teamRepositoryMock.Verify(x => x.SaveAsync(teamDto), Times.Once());
        }

        [Test]
        public async Task WhenSaveAsync_WithNull_ThenThrowArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.SaveAsync(null));
            _teamRepositoryMock.Verify(x => x.SaveAsync(null), Times.Never());
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
            _teamRepositoryMock.Verify(x => x.DeleteAsync(It.Is<int>(i => i.Equals(id))), Times.Once);
        }

        [Theory]
        [TestCase(1)]
        [TestCase(22)]
        [TestCase(131)]
        public async Task WhenGetByIdAsync_WithValidTeamId_ThenReturnTeam(int teamId)
        {
            // Arrange
            var teamDto = new TeamDto
            {
                Name = "Development Team",
                ProjectId = 1,
                TeamLeaderId = 1,
                Project = new ProjectDto { Id = 1, Name = "Project A" },
                TeamLeader = new UserDto { Id = 1, Username = "John Doe" },
                Developers = new List<UserDto>
                {
                    new UserDto { Id = 2, Username = "Alice" },
                    new UserDto { Id = 3, Username = "Bob" }
                }
            };

            _teamRepositoryMock.Setup(x => x.GetByIdAsync(It.Is<int>(x => x.Equals(teamId))))
                .ReturnsAsync(teamDto);

            // Act
            var teamResult = await _service.GetByIdIfExistsAsync(teamId);

            // Assert
            _teamRepositoryMock.Verify(x => x.GetByIdAsync(teamId), Times.Once());
            Assert.That(teamResult == teamDto);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(102021)]
        public async Task WhenGetByIdAsync_WithInvalidTeamId_ThenReturnDefault(int teamId)
        {
            // Arrange
            var team = (TeamDto)default;

            _teamRepositoryMock.Setup(s => s.GetByIdAsync(It.Is<int>(x => x.Equals(teamId))))
                .ReturnsAsync(team);

            // Act
            var teamResult = await _service.GetByIdIfExistsAsync(teamId);

            // Assert
            _teamRepositoryMock.Verify(x => x.GetByIdAsync(teamId), Times.Once());
            Assert.That(teamResult == team);
        }

        [Test]
        public async Task WhenUpdateAsync_WithValidData_ThenSaveAsync()
        {
            // Arrange
            var teamDto = new TeamDto
            {
                Name = "Updated Development Team",
                ProjectId = 1,
                TeamLeaderId = 1,
                Project = new ProjectDto { Id = 1, Name = "Project A" },
                TeamLeader = new UserDto { Id = 1, Username = "John Doe" },
                Developers = new List<UserDto>
                {
                    new UserDto { Id = 2, Username = "Alice" },
                    new UserDto { Id = 3, Username = "Bob" }
                }
            };

            _teamRepositoryMock.Setup(s => s.SaveAsync(It.Is<TeamDto>(x => x.Equals(teamDto))))
               .Verifiable();

            // Act
            await _service.SaveAsync(teamDto);

            // Assert
            _teamRepositoryMock.Verify(x => x.SaveAsync(teamDto), Times.Once);
        }
    }
}
