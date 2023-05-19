using System;
using System.Collections.Generic;
using System.Text;

namespace API.DTOS
{
    public class LastKillDTO
    {
        public KillerDTO Killer { get; set; }
        public VictimDTO Victim { get; set; }
    }
}
