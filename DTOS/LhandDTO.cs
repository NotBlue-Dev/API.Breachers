using System;
using System.Collections.Generic;
using System.Text;

namespace API.DTOS
{
    public class LhandDTO
    {
        public List<double> Pos { get; set; }
        public List<double> Forward { get; set; }
        public List<double> Left { get; set; }
        public List<double> Up { get; set; }
    }
}
