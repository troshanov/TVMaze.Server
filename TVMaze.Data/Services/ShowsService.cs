using TVMaze.Data.DTOs;
using TVMaze.Data.Models;
using TVMaze.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TVMaze.Data.Services
{
    public class ShowsService : IShowsService
    {
        private readonly IRepository<Show> showsRepo;
        private readonly IRepository<ShowCast> showCastsRepo;

        public ShowsService(
            IRepository<Show> showsRepo,
            IRepository<ShowCast> showCastsRepo)
        {
            this.showsRepo = showsRepo;
            this.showCastsRepo = showCastsRepo;
        }

        public async Task<SearchResultDto> GetShowsAsync(int size, int page = 1)
        {
            var result = new SearchResultDto();

            result.TotalCount = await this.showsRepo
                .AllAsNoTracking()
                .CountAsync();

           result.Shows = await this.showsRepo
                .AllAsNoTracking()
                .Skip((page - 1) * size)
                .Take(size)
                .Select(show => new ShowDto
                {
                    Id = show.Id,
                    Name = show.Name
                })
                .ToListAsync();

            return result;
        }
        public async Task<SearchResultDto> GetShowsByTitleAsync(string title, int size, int page = 1)
        {
            var result = new SearchResultDto();

            result.TotalCount = await this.showsRepo
                .AllAsNoTracking()
                .Where(s => s.Name.Contains(title))
                .CountAsync();

            result.Shows = await this.showsRepo
                .AllAsNoTracking()
                .Where(s => s.Name.Contains(title))
                .Skip((page - 1) * size)
                .Take(size)
                .Select(show => new ShowDto
                {
                    Id = show.Id,
                    Name = show.Name
                })
                .ToListAsync();

            return result;
        }

        public List<PersonDto> GetCast(int id)
        {
            var result = this.showCastsRepo
                .AllAsNoTracking()
                .Where(sc => sc.ShowId == id)
                .Select(sc => new PersonDto
                {
                    Id = sc.PersonId,
                    Name = sc.Person.Name
                })
                .ToList();

            return result;
        }
    }
}
