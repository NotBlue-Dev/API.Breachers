using System.Collections.Generic;
using System.Linq;
using Breachers.Networking;
using BreachersAPI.DTOS;
using BreachersAPI.DTOS.Utils;
using Fusion;

namespace BreachersAPI.Utils; 

public static class Extensions {
    public static bool HasValue(this string str) {
        return !string.IsNullOrEmpty(str);
    }
    
    public static bool HasValue(this object obj) {
        return obj != null;
    }

    public static List<PlayerDTO> ResetAndRefill(NetworkArray<PlayerSyncData> players)
    {
        var tempDTO = new List<PlayerDTO>();
        foreach (PlayerSyncData player in players)
        {
            TeamDTO team = Plugin.RootDTO.Teams.FirstOrDefault(t => t.Team == player.Team);
            if (team.HasValue())
            {
                PlayerDTO newPlayer = new PlayerDTO();
                newPlayer.Head = new HeadDTO();
                newPlayer.Equipments = new List<string>();
                newPlayer.SetInfos(player);
                tempDTO.Add(newPlayer);
            }
        }

        return tempDTO;
    }
}