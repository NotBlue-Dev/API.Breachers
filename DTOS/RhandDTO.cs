using System.Collections.Generic;

namespace BreachersAPI.DTOS {
    public class RhandDTO {
        public List<double> Pos { get; set; }
        public List<double> Forward { get; set; }
        public List<double> Left { get; set; }
        public List<double> Up { get; set; }
    }
}
