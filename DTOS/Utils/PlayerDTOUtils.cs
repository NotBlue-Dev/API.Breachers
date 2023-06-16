using Breachers.Networking;

namespace BreachersAPI.DTOS.Utils; 

public static class PlayerDTOUtils {
    
    /// <summary>
    /// Remplit un PlayerDTO avec les infos d'un joueur
    /// </summary>
    /// <param name="dto">DTO à modifier</param>
    /// <param name="player">Infos</param>
    public static void SetInfos(this PlayerDTO dto, PlayerSyncData player) {
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

    public static void changeFlag(this PlayerDTO dto, bool flag)
    {
        dto.IsConnectedToTheGame = flag;
    }
}