using TVMaze.Data.Models;

namespace TVMaze.Test.Constants
{
    public static class ShowsConstant
    {
        private static int id = 1;

        public static List<Show> All = new List<Show>
        {
            new Show{Id = id++, Name = "A" },
            new Show{Id = id++, Name = "B" },
            new Show{Id = id++, Name = "C" },
            new Show{Id = id++, Name = "D" },
            new Show{Id = id++, Name = "E" },
            new Show{Id = id++, Name = "F" },
            new Show{Id = id++, Name = "G" },
            new Show{Id = id++, Name = "H" },
            new Show{Id = id++, Name = "I" },
            new Show{Id = id++, Name = "J" },
        };
    }
}
