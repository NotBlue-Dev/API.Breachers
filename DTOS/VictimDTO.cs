using System.Text.Json.Serialization;
using Breachers;

namespace BreachersAPI.DTOS {
    public class VictimDTO {
        public string Name { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        public Teams Team { get; set; }
        public bool IsHeadshot { get; set; }
        
        public bool IsWallbang { get; set; }
        
        public bool IsCollat { get; set; }
        
        public bool IsSmokeBang { get; set; }
    }
}
