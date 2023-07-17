using System.Collections.Generic;
using UnityEngine;

namespace BreachersAPI.DTOS {
    public class HeadDTO {
        public List<float> Position { get; set; }
        public List<float> Forward { get; set; }
        public List<float> Right { get; set; }
        public List<float> Up { get; set; }
        public List<float> Rotation { get; set; }
    }
}
