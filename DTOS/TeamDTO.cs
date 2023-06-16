using System.Collections.Generic;
using System.Text.Json.Serialization;
using Breachers;

namespace BreachersAPI.DTOS {
    public class TeamDTO {
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        public Teams Team { get; set; }
        public StatsDTO Stats { get; set; }
        public List<PlayerDTO> Players { get; set; }
    }
}
