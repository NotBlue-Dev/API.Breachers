using System;
using System.Collections.Generic;

namespace BreachersAPI.DTOS {
    public class RootDTO {
        public string ServerName { get; set; }
        public string ServerOwner { get; set; }
        public string ServerIP { get; set; }
        public bool ServerIsLocked { get; set; }
        public bool PrivateServer { get; set; }
        public string Map { get; set; }
        public string MatchType { get; set; }
        public int SpectatorCount { get; set; }
        public int PlayerCount { get; set; }
        public string GameStatus { get; set; }
        public float RoundTimer { get; set; }
        public string RoundTimer_Display => TimeSpan.FromSeconds(RoundTimer).ToString(@"mm\:ss");
        public string EMPTimer { get; set; }
        public int TotalRoundCount { get; set; }
        public int BlueRoundWon { get; set; }
        public int OrangeRoundWon { get; set; }
        public int PacketLoss { get; set; }
        public int Ping { get; set; }
        
        public LastKillDTO LastKill { get; set; }
        public List<TeamDTO> Teams { get; set; }
    }
}
