using System.Collections.Generic;

namespace BreachersAPI.DTOS {
    public class TeamDTO {
        public string Team { get; set; }
        public StatsDTO Stats { get; set; }
        public List<PlayerDTO> Players { get; set; }
    }
}
