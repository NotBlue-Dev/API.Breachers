using System.Collections.Generic;
using Breachers;

namespace BreachersAPI.DTOS {
    public class TeamDTO {
        public Teams Team { get; set; }
        public StatsDTO Stats { get; set; }
        public List<PlayerDTO> Players { get; set; }
    }
}
