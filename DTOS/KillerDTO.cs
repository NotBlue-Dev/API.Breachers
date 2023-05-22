using Breachers;
using BreachersAPI.DTOS.Utils;

namespace BreachersAPI.DTOS {
    public class KillerDTO {
        public string Name { get; set; }
        public ItemId Weapon { get; set; }
        public string Team { get; set; }
        public bool IsHeadshot { get; set; }
        public bool IsWallbang { get; set; }
        public bool IsCollat { get; set; }
        public bool IsSmokeBang { get; set; }
    }
}
