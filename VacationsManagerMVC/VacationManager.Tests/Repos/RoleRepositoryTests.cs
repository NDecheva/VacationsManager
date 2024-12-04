using NUnit.Framework;
using VacationsManager.Data;
using VacationsManager.Data.Entities;
using VacationsManager.Data.Repos;
using VacationsManager.Shared.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace VacationManager.Tests.Repos
{
    [TestFixture]
    public class RoleRepositoryTests : BaseRepositoryTests<RoleRepository, Role, RoleDto>
    {
        private VacationsManagerDbContext _context;
        private RoleRepository _repository;
        private IMapper _mapper;


        [SetUp]
        public void Setup()
        {
            // Конфигуриране на In-Memory база данни
            var options = new DbContextOptionsBuilder<VacationsManagerDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;


            _context = new VacationsManagerDbContext(options);


            // Конфигуриране на Mapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Role, RoleDto>();
                cfg.CreateMap<RoleDto, Role>();
            });
            _mapper = config.CreateMapper();


            // Инициализация на RoleRepository
            _repository = new RoleRepository(_context, _mapper);
        }


        [Test]
        public async Task GetByNameIfExistsAsync_ReturnsRole_WhenRoleExists()
        {
            // Arrange
            var role = new Role
            {
                Id = 1,
                Name = "Admin"
            };
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();


            // Act
            var result = await _repository.GetByNameIfExistsAsync("Admin");


            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Admin"));
        }


        [Test]
        public async Task GetByNameIfExistsAsync_ReturnsNull_WhenRoleDoesNotExist()
        {
            // Act
            var result = await _repository.GetByNameIfExistsAsync("NonExistentRole");


            // Assert
            Assert.That(result, Is.Null);
        }


        [Test]
        public async Task GetByNameIfExistsAsync_ReturnsNull_WhenDatabaseIsEmpty()
        {
            // Act
            var result = await _repository.GetByNameIfExistsAsync("AnyRole");


            // Assert
            Assert.That(result, Is.Null);
        }


        [TearDown]
        public void TearDown()
        {
            // Изчистване на базата след всеки тест
            _context.Database.EnsureDeleted();

            // Dispose the repository if necessary
            _repository?.Dispose();

            // Dispose the context
            _context.Dispose();

            // Set repository to null (optional)
            _repository = null;
        }
    }
}