using System;
using System.Collections.Generic;
using System.Text;

namespace API.DTOS
{
    public class VictimDTO
    {
        public string Name { get; set; }
        public string Weapon { get; set; }
        public string Team { get; set; }
        public bool IsBot { get; set; }
        public bool IsControllingBot { get; set; }
    }
}
