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
using VacationsManager.Shared.Enums; 

namespace VacationManager.Tests.Services
{
    public class RoleServiceTests
    {
        private readonly Mock<IRoleRepository> _roleRepositoryMock = new Mock<IRoleRepository>();
        private readonly IRoleService _service;

        public RoleServiceTests()
        {
            _service = new RoleService(_roleRepositoryMock.Object);
        }

        [Test]
        public async Task WhenCreateAsync_WithValidData_ThenSaveAsync()
        {
            // Arrange
            var roleDto = new RoleDto()
            {
                Name = "Jake",
                RoleType = RoleType.CEO, 
                Users = new List<UserDto> 
                {
                    new UserDto { Id = 1, Username = "Gabe" },
                    new UserDto { Id = 2, Username = "Gabe" }
                }
            };

            // Act
            await _service.SaveAsync(roleDto);

            // Assert
            _roleRepositoryMock.Verify(x => x.SaveAsync(roleDto), Times.Once());
        }

        [Test]
        public async Task WhenSaveAsync_WithNull_ThenThrowArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.SaveAsync(null));
            _roleRepositoryMock.Verify(x => x.SaveAsync(null), Times.Never());
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
            _roleRepositoryMock.Verify(x => x.DeleteAsync(It.Is<int>(i => i.Equals(id))), Times.Once);
        }

        [Theory]
        [TestCase(1)]
        [TestCase(22)]
        [TestCase(131)]
        public async Task WhenGetByIdAsync_WithValidRoleId_ThenReturnRole(int roleId)
        {
            // Arrange
            var roleDto = new RoleDto()
            {
                Name = "Jake",
                RoleType = RoleType.Developer, 
                Users = new List<UserDto> 
                {
                    new UserDto { Id = 1, Username = "Gabe" },
                    new UserDto { Id = 2, Username = "Gabe" }
                }
            };
            _roleRepositoryMock.Setup(s => s.GetByIdAsync(It.Is<int>(x => x.Equals(roleId))))
                .ReturnsAsync(roleDto);

            // Act
            var roleResult = await _service.GetByIdIfExistsAsync(roleId);

            // Assert
            _roleRepositoryMock.Verify(x => x.GetByIdAsync(roleId), Times.Once);
            Assert.That(roleResult == roleDto);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(102021)]
        public async Task WhenGetByIdAsync_WithInvalidRoleId_ThenReturnDefault(int roleId)
        {
            // Arrange
            var role = (RoleDto)default;
            _roleRepositoryMock.Setup(s => s.GetByIdAsync(It.Is<int>(x => x.Equals(roleId))))
                .ReturnsAsync(role);

            // Act
            var roleResult = await _service.GetByIdIfExistsAsync(roleId);

            // Assert
            _roleRepositoryMock.Verify(x => x.GetByIdAsync(roleId), Times.Once);
            Assert.That(roleResult == role);
        }

        [Test]
        public async Task WhenUpdateAsync_WithValidData_ThenSaveAsync()
        {
            // Arrange
            var roleDto = new RoleDto
            {
                Name = "Jake",
                RoleType = RoleType.TeamLead,
                Users = new List<UserDto> 
                {
                    new UserDto { Id = 1, Username = "Gabe" },
                    new UserDto { Id = 2, Username = "Gabe" }
                }
            };
            _roleRepositoryMock.Setup(s => s.SaveAsync(It.Is<RoleDto>(x => x.Equals(roleDto))))
               .Verifiable();

            // Act
            await _service.SaveAsync(roleDto);

            // Assert
            _roleRepositoryMock.Verify(x => x.SaveAsync(roleDto), Times.Once);
        }
    }
}
