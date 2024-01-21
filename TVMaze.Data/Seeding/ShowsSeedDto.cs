using System.Text.Json.Serialization;

namespace TVMaze.Data.Seeding
{
    public class ShowsSeedDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        [JsonPropertyName("_embedded")]
        public EmbeddedDataSeedDto EmbeddedData { get; set; }
    }
}
