using System.Text.Json.Serialization;

namespace TVMaze.Data.Seeding
{
    public class EmbeddedDataSeedDto
    {
        [JsonPropertyName("cast")]
        public CastSeedDto[] Casts { get; set; }
    }
}
