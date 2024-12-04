using NUnit.Framework;
using VacationsManager.Data;
using VacationsManager.Data.Entities;
using VacationsManager.Data.Repos;
using VacationsManager.Shared.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VacationsManager.Shared.Security;


namespace VacationManager.Tests.Repos
{
    [TestFixture]
    public class UserRepositoryTests : BaseRepositoryTests<UserRepository, User, UserDto>
    {
        private VacationsManagerDbContext _context;
        private UserRepository _repository;
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
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<UserDto, User>();
            });
            _mapper = config.CreateMapper();


            // Инициализация на UserRepository
            _repository = new UserRepository(_context, _mapper);
        }


        [Test]
        public async Task CanUserLoginAsync_ReturnsTrue_WhenCredentialsAreCorrect()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Password = PasswordHasher.HashPassword("password"),
                FirstName = "Test",
                LastName = "User"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();


            // Act
            var result = await _repository.CanUserLoginAsync("testuser", "password");


            // Assert
            Assert.That(result, Is.True);
        }


        [Test]
        public async Task CanUserLoginAsync_ReturnsFalse_WhenUserNotFound()
        {
            // Act
            var result = await _repository.CanUserLoginAsync("nonexistentuser", "password");


            // Assert
            Assert.That(result, Is.False);
        }


        [Test]
        public async Task CanUserLoginAsync_ReturnsFalse_WhenPasswordIsIncorrect()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Password = PasswordHasher.HashPassword("password"),
                FirstName = "Test",
                LastName = "User"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();


            // Act
            var result = await _repository.CanUserLoginAsync("testuser", "wrongpassword");


            // Assert
            Assert.That(result, Is.False);
        }


        [Test]
        public async Task GetByUsernameAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "existinguser",
                Password = PasswordHasher.HashPassword("password"),
                FirstName = "John",
                LastName = "Doe"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();


            // Act
            var result = await _repository.GetByUsernameAsync("existinguser");


            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Username, Is.EqualTo("existinguser"));
        }


        [Test]
        public async Task GetByUsernameAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Act
            var result = await _repository.GetByUsernameAsync("nonexistentuser");


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