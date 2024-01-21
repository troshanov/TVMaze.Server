using Moq;
using Xunit;
using MockQueryable.Moq;
using TVMaze.Data.Models;
using TVMaze.Data.Services;
using TVMaze.Test.Constants;
using TVMaze.Data.Interfaces;

namespace TVMaze.Test.DataServices
{
    public class ShowsServiceTests
    {
        private Mock<IRepository<Show>> showsRepoMock;
        private Mock<IRepository<ShowCast>> showCastsRepoMock;
        private IShowsService showsService;
        public ShowsServiceTests()
        {
            this.showsRepoMock = new Mock<IRepository<Show>>();
            this.showCastsRepoMock = new Mock<IRepository<ShowCast>>();

            this.showsService = new ShowsService(showsRepoMock.Object, showCastsRepoMock.Object);
        }


        [Fact]
        public async Task GetShowsReturnsCorrectShowsForGivenSize()
        {
            var mock = ShowsConstant.All.BuildMock();
            this.showsRepoMock.Setup(s => s.AllAsNoTracking())
                .Returns(mock);

            var expectedIds = new List<long>{ 1, 2, 3, 4, 5 }; 
            var result = await this.showsService.GetShowsAsync(5);

            foreach (var show in result.Shows)
            {
                Assert.True(expectedIds.Contains(show.Id));
            }
        }

        [Fact]
        public async Task GetShowsReturnsCorrectShowsForGivenPageAndSize()
        {
            var mock = ShowsConstant.All.BuildMock();
            this.showsRepoMock.Setup(s => s.AllAsNoTracking())
                .Returns(mock);

            var expectedIds = new List<long> {  6, 7, 8, 9, 10};
            var result = await this.showsService.GetShowsAsync(5,2);

            

            foreach (var show in result.Shows)
            {
                Assert.True(expectedIds.Contains(show.Id));
            }
        }

        [Fact]
        public async Task GetShowsReturnsCorrectTotalShowsCount()
        {
            var mock = ShowsConstant.All.BuildMock();
            this.showsRepoMock.Setup(s => s.AllAsNoTracking())
                .Returns(mock);

            var expectedIds = new List<long> { 1, 2, 3, 4, 5 };
            var result = await this.showsService.GetShowsAsync(5);

            Assert.True(result.Shows.Count == 5);
        }
    }
}
