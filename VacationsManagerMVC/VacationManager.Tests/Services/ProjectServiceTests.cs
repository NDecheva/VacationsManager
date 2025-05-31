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
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _projectRepositoryMock = new Mock<IProjectRepository>();
        private readonly Mock<ITeamService> _teamServiceMock = new Mock<ITeamService>();
        private readonly Mock<ITeamRepository> _teamRepositoryMock = new Mock<ITeamRepository>();  
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private readonly IProjectService _service;

        public ProjectServiceTests()
        {
            _service = new ProjectService(
                _projectRepositoryMock.Object,
                _teamServiceMock.Object,
                _userServiceMock.Object,         
                _teamRepositoryMock.Object);
        }

        [Test]
        public async Task WhenCreateAsync_WithValidData_ThenSaveAsync()
        {
            // Arrange
            var projectDto = new ProjectDto
            {
                Name = "Test Project",
                Description = "This is a test project",
                Teams = new List<TeamDto>
                {
                    new TeamDto { Name = "Team A" },
                    new TeamDto { Name = "Team B" }
                }
            };

            // Act
            await _service.SaveAsync(projectDto);

            // Assert
            _projectRepositoryMock.Verify(x => x.SaveAsync(projectDto), Times.Once());
        }

        [Test]
        public async Task WhenSaveAsync_WithNull_ThenThrowArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.SaveAsync(null));

            _projectRepositoryMock.Verify(x => x.SaveAsync(null), Times.Never);
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
            _projectRepositoryMock.Verify(x => x.DeleteAsync(It.Is<int>(i => i.Equals(id))), Times.Once);
        }

        [Theory]
        [TestCase(1)]
        [TestCase(22)]
        [TestCase(131)]
        public async Task WhenGetByIdAsync_WithValidProjectId_ThenReturnProject(int projectId)
        {
            // Arrange
            var projectDto = new ProjectDto
            {
                Name = "Test Project",
                Description = "This is a test project",
                Teams = new List<TeamDto>
                {
                    new TeamDto { Name = "Team A" }
                }
            };

            _projectRepositoryMock.Setup(x => x.GetByIdAsync(It.Is<int>(x => x.Equals(projectId))))
                .ReturnsAsync(projectDto);

            // Act
            var projectResult = await _service.GetByIdIfExistsAsync(projectId);

            // Assert
            _projectRepositoryMock.Verify(x => x.GetByIdAsync(projectId), Times.Once());
            Assert.That(projectResult == projectDto);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(102021)]
        public async Task WhenGetByIdAsync_WithInvalidProjectId_ThenReturnDefault(int projectId)
        {
            // Arrange
            var project = (ProjectDto)default;

            _projectRepositoryMock.Setup(s => s.GetByIdAsync(It.Is<int>(x => x.Equals(projectId))))
                .ReturnsAsync(project);

            // Act
            var projectResult = await _service.GetByIdIfExistsAsync(projectId);

            // Assert
            _projectRepositoryMock.Verify(x => x.GetByIdAsync(projectId), Times.Once());
            Assert.That(projectResult == project);
        }

        [Test]
        public async Task WhenUpdateAsync_WithValidData_ThenSaveAsync()
        {
            // Arrange
            var projectDto = new ProjectDto
            {
                Name = "Updated Test Project",
                Description = "Updated description",
                Teams = new List<TeamDto>
                {
                    new TeamDto { Name = "Updated Team A" }
                }
            };

            _projectRepositoryMock.Setup(s => s.SaveAsync(It.Is<ProjectDto>(x => x.Equals(projectDto))))
                .Verifiable();

            // Act
            await _service.SaveAsync(projectDto);

            // Assert
            _projectRepositoryMock.Verify(x => x.SaveAsync(projectDto), Times.Once());
        }
    }
}