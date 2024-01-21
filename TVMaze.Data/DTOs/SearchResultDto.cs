namespace TVMaze.Data.DTOs
{
    public class SearchResultDto
    {
        public List<ShowDto> Shows { get; set; }

        public int TotalCount { get; set; }
    }
}
