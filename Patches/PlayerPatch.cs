using System;
using System.Linq;
using BreachersAPI.DTOS;
using BreachersAPI.Utils;
using Breachers;
using Breachers.Networking;
using BreachersAPI.DTOS.Utils;
using Fusion;
using HarmonyLib;

namespace BreachersAPI.Patches; 

public class PlayerPatch {
    
    //TO DO, recup les valeurs des netcharacter on spawn (la t'arrive dans une game avec un mec a 50 de vie
    //ca affiche 100 tant qu'il est pas update
    
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
            Plugin.Log.LogError($"PlayerPatch@NetCharacter_OnHealthChanged");
            Plugin.Log.LogError($"Error while getting data: {ex}");
        }
    }
    
    // /////// //
    // DESPAWN //
    // /////// //
    // Quand on quitte une game on vide le DTO players et TEAM
    // Quand un player leave ca despawn pas son netcharacter est remplacer par un bot
    [HarmonyPatch(typeof(NetCharacter), nameof(NetCharacter.Despawned))]
    [HarmonyPostfix]
    public static void NetCharacter_Despawned(NetCharacter __instance) {
        try {
            string playerName = __instance.NET_Nickname;
            Teams playerTeam = __instance.NET_CurrentTeam;
            
            PlayerDTO existingPlayer = Plugin.RootDTO.Teams.SelectMany(team => team.Players)
                .FirstOrDefault(p => p.Name == playerName);
            TeamDTO currentTeam = Plugin.RootDTO.Teams.FirstOrDefault(t => t.Team == playerTeam);

            if (existingPlayer != null) {
                currentTeam?.Players.Remove(existingPlayer);
            }

            if (currentTeam?.Players.Count == 0)
            {
                currentTeam.Stats = new StatsDTO();
            }
        } catch (Exception ex) {
            Plugin.Log.LogError($"PlayerPatch@NetCharacter_Despawned");
            Plugin.Log.LogError($"Error while getting data: {ex}");
        }
    }
    
    // ////// // //
    // FOOT STEP //
    // ////// // //
    [HarmonyPatch(typeof(NetCharacter), nameof(NetCharacter.OnFootStepCounterChanged))]
    [HarmonyPostfix]
    public static void NetCharacter_OnFootStepCounterChanged([HarmonyArgument(index:0)] Changed<NetCharacter> arg) {
        try {
            NetCharacter player = arg.Behaviour;
            string playerName = player.NET_Nickname;
            
            PlayerDTO existingPlayer = Plugin.RootDTO.Teams.SelectMany(team => team.Players)
                .FirstOrDefault(p => p.Name == playerName);
            Plugin.Log.LogInfo(existingPlayer + playerName);
            if (existingPlayer != null)
            {
                existingPlayer.FootStepCounter = player.NET_FootStepCounter;
            }
        } catch (Exception ex) {
            Plugin.Log.LogError($"PlayerPatch@NetCharacter_OnFootStepCounterChanged");
            Plugin.Log.LogError($"Error while getting data: {ex}");
        }
    }
    
    // ////// // //
    //  OUTSIDE  //
    // ////// // //
    // a tester
    [HarmonyPatch(typeof(NetCharacter), nameof(NetCharacter.OnIsOutsideChanged), typeof(Changed<NetCharacter>))]
    [HarmonyPostfix]
    //methode avec deux fois le meme nom mais une des deux a un arg donc on le passe en params
    public static void NetCharacter_OnIsOutsideChanged([HarmonyArgument(index:0)] Changed<NetCharacter> arg) {
        try {
            NetCharacter player = arg.Behaviour;
            string playerName = player.NET_Nickname;

            PlayerDTO existingPlayer = Plugin.RootDTO.Teams.SelectMany(team => team.Players)
                .FirstOrDefault(p => p.Name == playerName);
            if (existingPlayer != null)
            {
                existingPlayer.IsOutside = player.NET_IsOutside;
            }
        } catch (Exception ex) {
            Plugin.Log.LogError($"PlayerPatch@NetCharacter_OnIsOutsideChanged");
            Plugin.Log.LogError($"Error while getting data: {ex}");
        }
    }
    
    // ////// ///// ///
    // VIRTUAL STOCK //
    // ////// ///// ///
    // a tester
    [HarmonyPatch(typeof(NetCharacter), nameof(NetCharacter.OnVirtualStockChanged))]
    [HarmonyPostfix]
    public static void NetCharacter_OnVirtualStockChanged([HarmonyArgument(index:0)] bool arg, NetCharacter __instance) {
        try {
            string playerName = __instance.NET_Nickname;

            PlayerDTO existingPlayer = Plugin.RootDTO.Teams.SelectMany(team => team.Players)
                .FirstOrDefault(p => p.Name == playerName);
            if (existingPlayer != null)
            {
                existingPlayer.UsingVirtualStock = arg;
            }
        } catch (Exception ex) {
            Plugin.Log.LogError($"PlayerPatch@NetCharacter_OnVirtualStockChanged");
            Plugin.Log.LogError($"Error while getting data: {ex}");
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
                TeamDTO newTeam = Plugin.RootDTO.Teams.FirstOrDefault(t => t.Team == playerTeam);
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
            Plugin.Log.LogError($"PlayerPatch@NetCharacter_OnCurrentTeamChanged");
            Plugin.Log.LogError($"Error while getting data: {ex}");
        }
    }
    
    // ///////////// //
    // GLOBAL UPDATE //
    // ///////////// //
    [HarmonyPatch(typeof(SpectatorActivePlayerStatus), nameof(SpectatorActivePlayerStatus.OnSyncedDataChanged))]
    [HarmonyPostfix]
    public static void SpectatorActivePlayerStatus_OnSyncedDataChanged(NetGameData netGameData) {
        // Déclenché lors d'un Player Leave/Join ou Mort (entre autres, aussi Credit change, Grappin?, Drone?, etc.)
        // Kills/Assists/Deaths 
        try {
            Plugin.RootDTO.ServerName = netGameData.NET_ServerName.ToString();
            Plugin.RootDTO.ServerOwner = netGameData.NET_ServerOwner.ToString();
            Plugin.RootDTO.SpectatorCount = netGameData.NET_SpectatorCount;
            Plugin.RootDTO.Map = netGameData.NET_ActiveSceneName.ToString();
            Plugin.RootDTO.PrivateServer = netGameData.NET_HasPassword;
            Plugin.RootDTO.ServerIsLocked = netGameData.NET_IsLocked;
            Plugin.RootDTO.PlayerCount = netGameData.SyncedPlayerGameData.ToArray().Count(p => !p.IsBot);

            foreach (PlayerSyncData playerData in netGameData.SyncedPlayerGameData) {
                TeamDTO team = Plugin.RootDTO.Teams.FirstOrDefault(t => t.Team == playerData.Team);
                if (team.HasValue()) {
                    PlayerDTO player = team.Players.FirstOrDefault(p => p.Name == playerData.Name.ToString());
                    if (player.HasValue()) {
                        player.SetInfos(playerData);
                    } else {
                        PlayerDTO newPlayer = new PlayerDTO();
                        newPlayer.SetInfos(playerData);
                        team.Players.Add(newPlayer);
                    }
                    team.SetTeamStats();
                }
            }
        } catch (Exception ex) {
            Plugin.Log.LogError($"PlayerPatch@SpectatorActivePlayerStatus_OnSyncedDataChanged");
            Plugin.Log.LogError($"Error while getting data: {ex}");
        }
    }
}