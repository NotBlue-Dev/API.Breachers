using System.Collections.Generic;
using UnityEngine;

namespace BreachersAPI.DTOS {
    public class HeadDTO {
        public Vector3 Position { get; set; }
        public Vector3 Forward { get; set; }
        public Vector3 Right { get; set; }
        public Vector3 Up { get; set; }
        public Quaternion Rotation { get; set; }
    }
}
