namespace BreachersAPI.DTOS {
    public class KillerDTO {
        public string Name { get; set; }
        public string Weapon { get; set; }
        public string Team { get; set; }
        public string Assist { get; set; }
        public bool Headshot { get; set; }
        public bool Wallbang { get; set; }
        public int Distance { get; set; }
        public bool IsBot { get; set; }
        public bool IsControllingBot { get; set; }
    }
}
