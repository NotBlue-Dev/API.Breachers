using System;
using System.Collections.Generic;
using System.Text;

namespace API.DTOS
{
    public class TeamDTO
    {
        public string Team { get; set; }
        public StatsDTO Stats { get; set; }
        public List<PlayerDTO> Players { get; set; }
    }

}
