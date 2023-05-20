using System;
using System.Linq;
using BreachersAPI.DTOS;
using BreachersAPI.DTOS.Utils;
using BreachersAPI.Utils;
using Breachers.FX;
using Breachers.Networking;
using HarmonyLib;

namespace BreachersAPI.Patches; 

public class GamePatch {
    
    // ////////////////// //
    // GENERAL GAME INFOS //
    // ////////////////// //
    [HarmonyPatch(typeof(NetGameData), nameof(NetGameData.Spawned))]
    [HarmonyPostfix]
    public static void NetGameData_Spawned(NetGameData __instance) {
        try {
            Plugin.RootDTO.ServerName = __instance.NET_ServerName.ToString();
            Plugin.RootDTO.ServerOwner = __instance.NET_ServerOwner.ToString();
            Plugin.RootDTO.ServerIsLocked = __instance.NET_IsLocked;
            Plugin.RootDTO.PrivateServer = __instance.NET_HasPassword;
            Plugin.RootDTO.Map = __instance.NET_ActiveSceneName.ToString();
            Plugin.RootDTO.SpectatorCount = __instance.NET_SpectatorCount;
            Plugin.RootDTO.PlayerCount = __instance.SyncedPlayerGameData.ToArray().Count(p => !p.IsBot);

            // Recherche des joueurs
            foreach (PlayerSyncData player in __instance.SyncedPlayerGameData) {
                TeamDTO team = Plugin.RootDTO.Teams.FirstOrDefault(t => t.Team == player.Team);
                if (team.HasValue()) {
                    PlayerDTO playerDTO = team.Players.FirstOrDefault(p => p.Name == player.Name);
                    // Si on a déjà le joueur, on le met à jour, sinon on le créé
                    if (playerDTO.HasValue()) {
                        playerDTO.SetInfos(player);
                    } else {
                        PlayerDTO newPlayer = new PlayerDTO();
                        newPlayer.SetInfos(player);
                        team.Players.Add(newPlayer);
                    }
                }
            }
        } catch (Exception ex) {
            Plugin.Log.LogError($"GamePatch@NetGameData_Spawned");
            Plugin.Log.LogError($"Error while getting data: {ex}");
        }
    }
    
    // ////////// //
    // MATCH TYPE //
    // ////////// //
    [HarmonyPatch(typeof(NetVoiceHandler), nameof(NetVoiceHandler.Spawned))]
    [HarmonyPostfix]
    public static void NetVoiceHandler_Spawned(NetVoiceHandler __instance) {
        try {
            Plugin.RootDTO.MatchType = __instance.NET_MatchType.ToString();
        } catch (Exception ex) {
            Plugin.Log.LogError($"GamePatch@NetVoiceHandler_Spawned");
            Plugin.Log.LogError($"Error while getting data: {ex}");
        }
    }
    
    // /////////// //
    // ROUND TIMER //
    // /////////// //
    [HarmonyPatch(typeof(SimpleClock), nameof(SimpleClock.SetTime))]
    [HarmonyPostfix]
    public static void SimpleClock_SetTime(SimpleClock __instance) {
        try {
            Plugin.RootDTO.RoundTimer = __instance._lastTimeInSeconds;
        } catch (Exception ex) {
            Plugin.Log.LogError($"GamePatch@SimpleClock_SetTime");
            Plugin.Log.LogError($"Error while getting data: {ex}");
        }
    }
    
    // /////////// //
    // PACKET LOSS //
    // /////////// //
    [HarmonyPatch(typeof(ClientConnectionInfo), nameof(ClientConnectionInfo.FixedUpdateNetwork))]
    [HarmonyPostfix]
    public static void ClientConnectionInfo_FixedUpdateNetwork(ClientConnectionInfo __instance) {
        try {
            Plugin.RootDTO.PacketLoss = __instance._packetLossMeasureDelay;       
        } catch (Exception ex) {
            Plugin.Log.LogError($"GamePatch@ClientConnectionInfo_FixedUpdateNetwork");
            Plugin.Log.LogError($"Error while getting data: {ex}");
        }
    }
    
    // //////////////// //
    // PING + SERVER IP //
    // //////////////// //
    [HarmonyPatch(typeof(ServerPing), nameof(ServerPing.Refresh))]
    [HarmonyPostfix]
    public static void ServerPing_Refresh(ServerPing __instance) {
        try {
            Plugin.RootDTO.Ping = __instance._ping.time;
            Plugin.RootDTO.ServerIP = __instance._ipAddress;
        } catch (Exception ex) {
            Plugin.Log.LogError($"GamePatch@ServerPing_Refresh");
            Plugin.Log.LogError($"Error while getting data: {ex}");
        }
    }
}