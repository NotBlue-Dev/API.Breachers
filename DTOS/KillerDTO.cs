using Breachers;
using BreachersAPI.DTOS.Utils;

namespace BreachersAPI.DTOS {
    public class KillerDTO {
        public string Name { get; set; }
        
        public ItemId Weapon { get; set; }
        public string Team { get; set; }
    }
}
