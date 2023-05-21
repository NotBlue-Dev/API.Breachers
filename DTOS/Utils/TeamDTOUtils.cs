using System.Linq;

namespace BreachersAPI.DTOS.Utils;

public static class TeamDTOUtils
{
    public static void SetTeamStats(this TeamDTO dto)
    {
        dto.Stats.Kills = dto.Players.Sum(player => player.Stats.Kills);
        dto.Stats.Deaths = dto.Players.Sum(player => player.Stats.Deaths);
        dto.Stats.Assists = dto.Players.Sum(player => player.Stats.Assists);
        dto.Stats.Defuses = dto.Players.Sum(player => player.Stats.Defuses);
        dto.Stats.Plants = dto.Players.Sum(player => player.Stats.Plants);
    }
}