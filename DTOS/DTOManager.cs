using Breachers.Networking;
using System;
using System.Collections.Generic;
using System.Text;

namespace API.DTOS
{
    internal class DTOManager
    {
        public static void AddPlayersStats(PlayerDTO dto, PlayerSyncData player)
        {
            dto.Name = player.Name.ToString();
            dto.Clan = player.Clan.ToString();
            dto.IdTag = player.IdTag.ToString();
            dto.IsBot = player.IsBot;
            dto.IsDead = player.IsDead;
            dto.IsControllingBot = player.IsControllingBot;
            dto.PingNumber = player.TeamCharacterID;
            dto.Credits = player.Credits;
            dto.Score = player.Score;
            dto.Stats ??= new StatsDTO();
            dto.Stats.Kills = player.Kills;
            dto.Stats.Deaths = player.Deaths;
            dto.Stats.Assists = player.Assists;
            dto.Stats.Defuses = player.Defuses;
            dto.Stats.Plants = player.Plants;
        }
    }
}
