using AutoFixture;
using FCG.API.Application.Services;
using FCG.API.Domain.DTO.UserDTO;
using FCG.API.Domain.Interfaces.Repositories;
using FCG.API.Domain.Interfaces.Services;
using FCG.Domain.Entities;
using MongoDB.Bson;
using Moq;

namespace FGC.Tests.ServiceTests
{
    public class BaseUserTests : BaseTests
    {
        private ObjectId _stubId;
        private User? _stubEntity;
        private List<User>? _stubList;
        private Mock<IUserRepository>? _mockRepo;
        private IUserService? _service;

        protected override void InitStubs()
        {
            _stubId = ObjectId.GenerateNewId();

            _stubEntity = _fixture.Build<User>()
                                  .With(e => e._id, _stubId)
                                  .Create();

            _stubList = _fixture.Build<User>()
                                .With(e => e._id, ObjectId.GenerateNewId())
                                .CreateMany(2)
                                .ToList();

            _stubList.Insert(0, _stubEntity);
        }

        protected override void MockDependencies()
        {
            _mockRepo = new Mock<IUserRepository>(MockBehavior.Strict);

            _mockRepo.Setup(r => r.GetAllAsync<ProjectUserDTO>())
                .ReturnsAsync(_stubList!.Select(x => new ProjectUserDTO(x)).ToList());

            _mockRepo.Setup(r => r.GetByIdAsync<ProjectUserDTO>(_stubId))
                .ReturnsAsync(new ProjectUserDTO(_stubEntity!));

            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<CreateUserDTO>()))
                .Callback<CreateUserDTO>(dto =>
                {
                    var entity = dto.ToEntity();
                    entity._id = ObjectId.GenerateNewId();
                    _stubList!.Add(entity);
                });

            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<ObjectId>(), It.IsAny<UpdateUserDTO>()))
                .Returns(Task.CompletedTask);

            _mockRepo.Setup(r => r.DeleteAsync(It.IsAny<ObjectId>()))
                .Callback<ObjectId>(id =>
                {
                    var index = _stubList!.FindIndex(x => x._id == id);
                    if (index >= 0) _stubList!.RemoveAt(index);
                })
                .Returns(Task.CompletedTask);

            _service = new UserService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEntities()
        {
            var result = await _service!.GetAllAsync();

            Assert.NotNull(result);
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
            var dto = _fixture.Build<CreateUserDTO>().Create();

            await _service!.CreateAsync(dto);

            _mockRepo!.Verify(r => r.CreateAsync(dto), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_CallsRepository()
        {
            var updateDto = _fixture.Build<UpdateUserDTO>().Create();

            await _service!.UpdateAsync(_stubId, updateDto);

            _mockRepo!.Verify(r => r.UpdateAsync(_stubId, updateDto), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepository()
        {
            await _service!.DeleteAsync(_stubId);

            _mockRepo!.Verify(r => r.DeleteAsync(_stubId), Times.Once);
        }
    }
}
