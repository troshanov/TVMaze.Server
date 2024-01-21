﻿namespace TVMaze.Data.Models
{
    public class Person
    {
        public Person()
        {
            this.Casts = new HashSet<ShowCast>();
        }
        public long Id { get; set; }
        
        public string Name { get; set; }

        public ICollection<ShowCast> Casts { get; set; }
    }
}
