using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationsManager.Data;
using VacationsManager.Data.Entities;
using VacationsManager.Shared.Dtos;
using YourNamespace.Data.Repos;

namespace VacationManager.Tests.Repos
{
    public abstract class BaseRepositoryTests<TRepository, T, TModel>
        where TRepository : BaseRepository<T, TModel>
        where T : class, IBaseEntity
        where TModel : BaseModel
    {
        private Mock<VacationsManagerDbContext> mockContext;
        private Mock<DbSet<T>> mockDbSet;
        private Mock<IMapper> mockMapper;
        private TRepository repository;
        [SetUp]
        public void Setup()
        {
            mockContext = new Mock<VacationsManagerDbContext>();
            mockDbSet = new Mock<DbSet<T>>();
            mockMapper = new Mock<IMapper>();
            repository = new Mock<TRepository>(mockContext.Object, mockMapper.Object)
            { CallBase = true }.Object;
        }
        [Test]
        public void MapToModel_ValidEntity_ReturnsMappedModel()
        {
            //Arrange
            var entity = new Mock<T>();
            var model = new Mock<TModel>();
            mockMapper.Setup(m => m.Map<TModel>(entity.Object)).Returns(model.Object);

            //Act
            var result = repository.MapToModel(entity.Object);

            //Assert
            Assert.That(result, Is.EqualTo(model.Object));
        }
        [Test]
        public void MapToEntity_ValidEntity_ReturnsMapToEntity()
        {
            //Arrange
            var entity = new Mock<T>();
            var model = new Mock<TModel>();
            mockMapper.Setup(m => m.Map<T>(model.Object)).Returns(entity.Object);

            //Act
            var result = repository.MapToEntity(model.Object);

            //Assert
            Assert.That(result, Is.EqualTo((T)entity.Object));
        }
    }
}

