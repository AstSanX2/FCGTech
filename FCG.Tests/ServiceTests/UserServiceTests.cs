using AutoFixture;
using FCG.API.Application.Services;
using FCG.API.Domain.DTO.UsersDTO;
using FCG.API.Domain.Interfaces.Repositories;
using FCG.API.Domain.Interfaces.Services;
using FCG.Domain.Entities;
using MongoDB.Bson;
using Moq;

namespace FGC.Tests.ServiceTests
{
    public class UserServiceTests : BaseTests
    {
        private List<User> _stubList;
        private Mock<IUserRepository> _mockRepo;
        private IUserService _service;

        public UserServiceTests()
        {
            _stubList = [];
        }

        protected override void InitStubs()
        {
            _stubList = _fixture.Build<User>()
                                .With(e => e._id, ObjectId.GenerateNewId())
                                .CreateMany(2)
                                .ToList();
        }

        protected override void MockDependencies()
        {
            _mockRepo = new Mock<IUserRepository>(MockBehavior.Strict);

            _mockRepo.Setup(r => r.GetAllAsync<ProjectUserDTO>())
                .ReturnsAsync(_stubList!.Select(x => new ProjectUserDTO(x)).ToList());

            _mockRepo.Setup(r => r.GetByIdAsync<ProjectUserDTO>(It.IsAny<ObjectId>()))
                .ReturnsAsync((ObjectId id) =>
                {
                    var user = _stubList?.FirstOrDefault(x => x._id == id);
                    if (user == null)
                        return null;

                    return new ProjectUserDTO(user);
                });

            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<CreateUserDTO>()))
                .ReturnsAsync((CreateUserDTO dto) =>
                {
                    var entity = dto.ToEntity();
                    entity._id = ObjectId.GenerateNewId();
                    _stubList!.Add(entity);
                    return entity;
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
            Assert.Contains(result, e => e._id == ObjectId.Empty);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsEntity()
        {
            var result = await _service!.GetByIdAsync(ObjectId.Empty);

            Assert.NotNull(result);
            Assert.Equal(ObjectId.Empty, result!._id);
        }

        [Fact]
        public async Task CreateAsync_CallsRepository_AndReturnsExpectedResult()
        {
            // Arrange
            var dto = new CreateUserDTO
            {
                Name = "Usuário Teste",
                Email = "teste@email.com",
                Password = "Senha@123"
            };

            // Act
            var response = await _service.CreateAsync(dto);

            // Assert
            Assert.False(response.HasError);
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(response.Data);

            _mockRepo.Verify(r => r.CreateAsync(dto), Times.Once);
            _mockRepo.Verify(r => r.GetByIdAsync<ProjectUserDTO>(response.Data._id), Times.Once);
        }


        [Fact]
        public async Task UpdateAsync_CallsRepository()
        {
            var updateDto = _fixture.Build<UpdateUserDTO>().Create();

            await _service!.UpdateAsync(ObjectId.Empty, updateDto);

            _mockRepo!.Verify(r => r.UpdateAsync(ObjectId.Empty, updateDto), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepository()
        {
            await _service!.DeleteAsync(ObjectId.Empty);

            _mockRepo!.Verify(r => r.DeleteAsync(ObjectId.Empty), Times.Once);
        }
    }
}
