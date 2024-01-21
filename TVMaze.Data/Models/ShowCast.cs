namespace TVMaze.Data.Models
{
    public class ShowCast
    {
        public long ShowId { get; set; }

        public long PersonId { get; set; }

        public Show Show { get; set; }

        public Person Person { get; set; }
    }
}
