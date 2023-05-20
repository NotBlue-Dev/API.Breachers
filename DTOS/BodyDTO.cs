using System.Collections.Generic;

namespace BreachersAPI.DTOS {
    public class BodyDTO {
        public List<double> Position { get; set; }
        public List<double> Forward { get; set; }
        public List<double> Left { get; set; }
        public List<double> Up { get; set; }
    }
}
