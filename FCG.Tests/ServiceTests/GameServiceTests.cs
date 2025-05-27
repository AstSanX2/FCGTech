using AutoFixture;
using FCG.API.Application.Services;
using FCG.API.Domain.DTO.GameDTO;
using FCG.API.Domain.Interfaces.Repositories;
using FCG.API.Domain.Interfaces.Services;
using FCG.Domain.Entities;
using MongoDB.Bson;
using Moq;

namespace FGC.Tests.ServiceTests
{
    public class GameServiceTests : BaseTests
    {
        private List<Game> _stubList;
        private Mock<IGameRepository> _mockRepo;
        private IGameService _service;

        public GameServiceTests()
        {
            _stubList = [];
        }

        protected override void InitStubs()
        {
            _stubList = _fixture.Build<Game>()
                                 .With(e => e._id, ObjectId.GenerateNewId())
                                 .CreateMany(2)
                                 .ToList();
        }

        protected override void MockDependencies()
        {
            _mockRepo = new Mock<IGameRepository>(MockBehavior.Strict);

            _mockRepo.Setup(r => r.GetAllAsync<ProjectGameDTO>())
                .ReturnsAsync(_stubList!.Select(x => new ProjectGameDTO(x)).ToList());

            _mockRepo.Setup(r => r.GetByIdAsync<ProjectGameDTO>(It.IsAny<ObjectId>()))
                .ReturnsAsync((ObjectId id) =>
                {
                    var game = _stubList?.FirstOrDefault(x => x._id == id);
                    if (game == null)
                        return null;

                    return new ProjectGameDTO(game);
                });

            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<CreateGameDTO>()))
                .ReturnsAsync((CreateGameDTO dto) =>
                {
                    var entity = dto.ToEntity();
                    entity._id = ObjectId.GenerateNewId();
                    _stubList!.Add(entity);
                    return entity;
                });

            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<ObjectId>(), It.IsAny<UpdateGameDTO>()))
                .Returns(Task.CompletedTask);

            _mockRepo.Setup(r => r.DeleteAsync(It.IsAny<ObjectId>()))
                .Callback<ObjectId>(id =>
                {
                    var index = _stubList!.FindIndex(x => x._id == id);
                    if (index >= 0) _stubList!.RemoveAt(index);
                })
                .Returns(Task.CompletedTask);

            _service = new GameService(_mockRepo.Object);
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
            var dto = new CreateGameDTO
            {
                Name = "Test",
                Description = "Description test",
                Category = "FPS",
                ReleaseDate = DateTime.Now.AddMonths(-1),
                LastUpdateDate = DateTime.Now,
                Price = 59.99m
            };

            var response = await _service.CreateAsync(dto);

            Assert.False(response.HasError);
            Assert.Equal(200, response.StatusCode);
            Assert.NotNull(response.Data);

            _mockRepo.Verify(r => r.CreateAsync(dto), Times.Once);
            _mockRepo.Verify(r => r.GetByIdAsync<ProjectGameDTO>(response.Data._id), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_CallsRepository()
        {
            var updateDto = _fixture.Build<UpdateGameDTO>().Create();

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