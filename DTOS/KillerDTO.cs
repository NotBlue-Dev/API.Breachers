using System.Text.Json.Serialization;
using Breachers;
using BreachersAPI.DTOS.Utils;

namespace BreachersAPI.DTOS {
    public class KillerDTO {
        public string Name { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        public ItemId Weapon { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))] 
        public Teams Team { get; set; }
    }
}
