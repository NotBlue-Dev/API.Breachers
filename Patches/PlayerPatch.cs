using System;
using System.Collections.Generic;
using System.Linq;
using API.DTOS;
using API.Utils;
using Breachers;
using Fusion;
using HarmonyLib;

namespace API.Patches; 

public class PlayerPatch {
    
    // ////// //
    // HEALTH //
    // ////// //
    [HarmonyPatch(typeof(NetCharacter), nameof(NetCharacter.OnHealthChanged))]
    [HarmonyPostfix]
    public static void NetCharacter_OnHealthChanged([HarmonyArgument(index:0)] Changed<NetCharacter> arg) {
        try {
            NetCharacter player = arg.Behaviour;
            string playerName = player.NET_Nickname;

            PlayerDTO existingPlayer = Plugin.RootDTO.Teams.SelectMany(team => team.Players).FirstOrDefault(p => p.Name == playerName);
            // Si on a bien le joueur dans notre liste des joueurs, on met à jour sa vie
            if (existingPlayer != null) {
                existingPlayer.Health = player.NET_Health;
            }
        } catch (Exception ex) {
            Plugin.Log.LogError($"[BreachersAPI] - PlayerPatch@NetCharacter_OnHealthChanged");
            Plugin.Log.LogError($"[BreachersAPI] - Error while getting data: {ex}");
        }
    }
    
    // ////// //
    // STATUS //
    // ////// //
    [HarmonyPatch(typeof(NetCharacter), nameof(NetCharacter.OnPlayerStateChanged))]
    [HarmonyPostfix]
    public static void NetCharacter_OnPlayerStateChanged([HarmonyArgument(index:0)] Changed<NetCharacter> arg) {
        try {
            NetCharacter player = arg.Behaviour;
            
            List<PlayerDTO> players = Plugin.RootDTO.Teams.SelectMany(team => team.Players).ToList();
            PlayerDTO existingPlayer = players.FirstOrDefault(p => p.Name == player.NET_Nickname);
            
            // Si on a bien le joueur dans notre liste des joueurs, on met à jour sa vie
            if (existingPlayer != null) {
                existingPlayer.IsDead = player.NET_PlayerState != NetCharacter.PlayerState.Alive;
                Plugin.Log.LogInfo($"[BreachersAPI] - {player.NET_Nickname} is {(player.NET_PlayerState == NetCharacter.PlayerState.Alive ? "Alive" : "Dead")}");
            }
        } catch (Exception ex) {
            Plugin.Log.LogError($"[BreachersAPI] - PlayerPatch@NetCharacter_OnPlayerStateChanged");
            Plugin.Log.LogError($"[BreachersAPI] - Error while getting data: {ex}");
        }
    }
    
    // //////////// //
    // TEAM CHANGED //
    // //////////// //
    [HarmonyPatch(typeof(NetCharacter), nameof(NetCharacter.OnCurrentTeamChanged))]
    [HarmonyPostfix]
    public static void NetCharacter_OnCurrentTeamChanged([HarmonyArgument(index:0)] Changed<NetCharacter> arg) {
        try {
            string playerName = arg.Behaviour.NET_Nickname;
            Teams playerTeam = arg.Behaviour.NET_CurrentTeam;

            PlayerDTO existingPlayer = Plugin.RootDTO.Teams.SelectMany(team => team.Players).FirstOrDefault(p => p.Name == playerName);
            // Si on a bien le joueur dans notre liste des joueurs
            if (existingPlayer != null) {
                // On récupère la nouvelle team du joueur
                TeamDTO newTeam = Plugin.RootDTO.Teams.FirstOrDefault(t => t.Team == playerTeam.ToString());
                if (newTeam.HasValue()) {
                    // Si le joueur n'est pas dans la team, on le rajoute et on le supprime de l'ancienne team
                    if (!newTeam.Players.FirstOrDefault(p => p.Name == playerName).HasValue()) {
                        TeamDTO oldTeam = Plugin.RootDTO.Teams.FirstOrDefault(t => t.Players.Any(p => p.Name == existingPlayer.Name));
                        
                        newTeam.Players.Add(existingPlayer);
                        oldTeam?.Players.Remove(existingPlayer);
                    }
                }
            }
        } catch (Exception ex) {
            Plugin.Log.LogError($"[BreachersAPI] - PlayerPatch@NetCharacter_OnCurrentTeamChanged");
            Plugin.Log.LogError($"[BreachersAPI] - Error while getting data: {ex}");
        }
    }
}