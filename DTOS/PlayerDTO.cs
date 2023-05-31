using System.Collections.Generic;

namespace BreachersAPI.DTOS {
    public class PlayerDTO {
        public string Name { get; set; }
        public int Score { get; set; }
        public string IdTag { get; set; }
        public string Clan { get; set; }
        public int PingNumber { get; set; }
        public bool IsDead { get; set; }
        public bool IsBot { get; set; }
        public bool IsControllingBot { get; set; }
        public int Credits { get; set; }
        public int Health { get; set; } = 100;
        public float CrouchPercent { get; set; }
        public bool IsFlashed { get; set; }
        public bool IsDroning { get; set; }
        public bool IsOutside { get; set; }
        public bool IsSprinting { get; set; }
        public bool IsCloacked { get; set; }
        public string MainWeapon { get; set; }
        public string SecondaryWeapon { get; set; }
        public List<string> Equipments { get; set; }
        public int FootStepCounter { get; set; }
        public bool UsingVirtualStock { get; set; }
        public StatsDTO Stats { get; set; }
        public List<int> Velocity { get; set; }
        public HeadDTO Head { get; set; }
        public BodyDTO Body { get; set; }
        public RhandDTO Rhand { get; set; }
        public LhandDTO Lhand { get; set; }
    }
}
