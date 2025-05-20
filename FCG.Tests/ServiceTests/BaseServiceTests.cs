using AutoFixture;
using FCG.Application.Services;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces.Repositories;
using MongoDB.Bson;
using Moq;

namespace FGC.Tests.ServiceTests
{
    public class BaseServiceTests : BaseTests
    {
        private BaseEntity? _stubEntity;
        private ObjectId _stubId;
        private List<BaseEntity>? _stubList;
        private Mock<IBaseRepository<BaseEntity>>? _mockRepo;

        private BaseService<BaseEntity>? _service;

        protected override void InitStubs()
        {
            _stubId = ObjectId.GenerateNewId();

            _stubEntity = _fixture.Build<BaseEntity>()
                                  .With(e => e._id, _stubId)
                                  .Create();

            _stubList = _fixture.Build<BaseEntity>()
                                .With(e => e._id, ObjectId.GenerateNewId())
                                .CreateMany(2)
                                .ToList();

            _stubList.Insert(0, _stubEntity);
        }

        protected override void MockDependencies()
        {
            _mockRepo = new Mock<IBaseRepository<BaseEntity>>(MockBehavior.Strict);
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(_stubList!);
            _mockRepo.Setup(r => r.GetByIdAsync(_stubId)).ReturnsAsync(_stubEntity);
            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<BaseEntity>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.UpdateAsync(_stubId, It.IsAny<BaseEntity>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.DeleteAsync(_stubId)).Returns(Task.CompletedTask);


            _service = new BaseService<BaseEntity>(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEntities()
        {
            var result = await _service!.GetAllAsync();

            Assert.Equal(_stubList!.Count, result.Count);
            Assert.Contains(result, e => e._id == _stubId);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsEntity()
        {
            var result = await _service!.GetByIdAsync(_stubId);

            Assert.NotNull(result);
            Assert.Equal(_stubId, result!._id);
        }

        [Fact]
        public async Task CreateAsync_CallsRepository()
        {
            await _service!.CreateAsync(_stubEntity!);
            _mockRepo!.Verify(r => r.CreateAsync(_stubEntity!), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_CallsRepository()
        {
            await _service!.UpdateAsync(_stubId, _stubEntity!);
            _mockRepo!.Verify(r => r.UpdateAsync(_stubId, _stubEntity!), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepository()
        {
            await _service!.DeleteAsync(_stubId);
            _mockRepo!.Verify(r => r.DeleteAsync(_stubId), Times.Once);
        }
    }
}
