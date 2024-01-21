using TVMaze.Data.DTOs;

namespace TVMaze.Data.Interfaces
{
    public interface IShowsService
    {
        Task<SearchResultDto> GetShowsAsync(int size, int page = 1);

        Task<SearchResultDto> GetShowsByTitleAsync(string title, int size, int page = 1);

        List<PersonDto> GetCast(int id);
    }
}
