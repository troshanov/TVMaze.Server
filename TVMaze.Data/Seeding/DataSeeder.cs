using TVMaze.Data.Models;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using TVMaze.Data.Interfaces;

namespace TVMaze.Data.Seeding
{
    public static class DataSeeder
    {
        public static async Task Seed(
            IRepository<Person> personsRepo,
            IRepository<Show> showsRepo,
            IHttpClientFactory httpFactory,
            int records = 1000)
        {

            if (showsRepo.AllAsNoTracking().Any())
            {
                return; // DB has been seeded
            }

            var id = 1;
            var client = httpFactory.CreateClient("TVMaze");

            while (true)
            {
                var response = await client.GetAsync($"/shows/{id++}?embed=cast");

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        // Some show ids are missing and throw 404
                        continue;
                    }

                    Console.WriteLine($"Call to {client.BaseAddress} failed with status code {response.StatusCode}");
                    continue;
                }

                var result = await response.Content.ReadFromJsonAsync<ShowsSeedDto>();
                var show = new Show
                {
                    Name = result.Name
                };

                foreach (var cast in result.EmbeddedData.Casts)
                {
                    //Check if actor is already persisted
                    var person = await personsRepo
                        .AllAsNoTracking()
                        .FirstOrDefaultAsync(a => a.Name == cast.Person.Name);

                    ShowCast entity;

                    //Avoid duplicating actors
                    if (person == null)
                    {
                        person = new Person
                        {
                            Name = cast.Person.Name
                        };

                        entity = new ShowCast
                        {
                            Person = person,
                            Show = show
                        };
                    }
                    else
                    {
                        entity = new ShowCast
                        {
                            PersonId = person.Id,
                            Show = show
                        };
                    }

                    // Some shows have bad cast data (duplicates)
                    if (!show.Casts.Any(c => c.Person == person
                        || c.PersonId != 0 && c.PersonId == person.Id))
                    {
                        show.Casts.Add(entity);
                    }
                }

                await showsRepo.AddAsync(show);
                await showsRepo.SaveChangesAsync();

                // Set records to a desired number of scraped records
                if (id == records)
                {
                    break;
                }
            };
        }
    }
}
