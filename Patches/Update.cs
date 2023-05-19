using System;
using API.DTOS;
using API.DTOS.Utils;
using Breachers;
using Breachers.Networking;
using HarmonyLib;
using Breachers.FX;
using Breachers.GameModes;
using Fusion;

namespace API.Patches
{

    //TO DO EVENT AU LIEU DE MODIFIER DANS LE PATCH
    internal class Update
    {
        internal static NetGameData _gameData;
        internal static NetGameData _lastGameData;

        /*
        [HarmonyPatch(typeof(NetCharacter), "Update")]
        [HarmonyPostfix]
        static void Postfix_NetCharacter(NetCharacter __instance)
        {
            RootDTO dto = Plugin.rootDTO;
            try
            {

            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogWarning($"Exception in patch of static void Breachers.NetCharacter::Update():\n{ex}");
            }
        }
        */

        //GameData (server)
        // [HarmonyPatch(typeof(NetGameData), "Spawned")]
        // [HarmonyPostfix]
        // static void PostFix_GameData(NetGameData __instance)
        // {
        //     RootDTO dto = Plugin.RootDTO;
        //     
        //     try
        //     {
        //         _gameData = __instance;
        //         dto.ServerName = _gameData.NET_ServerName.ToString();
        //         dto.ServerOwner = _gameData.NET_ServerOwner.ToString();
        //         dto.SpectatorCount = _gameData.NET_SpectatorCount;
        //         dto.Map = _gameData.NET_ActiveSceneName.ToString();
        //         dto.PrivateServer = _gameData.NET_HasPassword;
        //         dto.ServerIsLocked = _gameData.NET_IsLocked;
        //         
        //     }
        //     catch (System.Exception ex)
        //     {
        //         Plugin.Log.LogWarning($"Exception in patch of static void Breachers.NetGameData::Spawned():\n{ex}");
        //     }
        // }

        [HarmonyPatch(typeof(SpectatorActivePlayerStatus), "OnSyncedDataChanged")]
        [HarmonyPostfix]
        static void PostFix_SpectatorActivePlayerStatus()
        {
            RootDTO dto = Plugin.RootDTO;
        
            try
            {
                dto.ServerName = _gameData.NET_ServerName.ToString();
                dto.ServerOwner = _gameData.NET_ServerOwner.ToString();
                dto.SpectatorCount = _gameData.NET_SpectatorCount;
                dto.Map = _gameData.NET_ActiveSceneName.ToString();
                dto.PrivateServer = _gameData.NET_HasPassword;
                dto.ServerIsLocked = _gameData.NET_IsLocked;
                
                foreach (var player in _gameData.SyncedPlayerGameData)
                {
                    TeamDTO team = dto.Teams.Find(x => x.Team == player.Team.ToString());
                    if (team != null)
                    {
                        PlayerDTO p = team.Players.Find(x => x.Name == player.Name.ToString());
                        if (p != null)
                        {
                            p.SetInfos(player);
                        }
                        else
                        {
                            PlayerDTO newPlayer = new PlayerDTO();
                            newPlayer.SetInfos(player);
                            team.Players.Add(newPlayer);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogWarning($"Exception in patch of static void Breachers.SpectatorActivePlayerStatus::OnSyncedDataChanged():\n{ex}");
            }
        }

        /*
        [HarmonyPatch(typeof(Match), "OnPlayerLeft")]
        [HarmonyPostfix]
        static void PostFix_Match(object[] __args)
        {
            RootDTO dto = Plugin.rootDTO;
            try
            {
                Plugin.Log.LogInfo(__args[0]);
            }
            catch (System.Exception ex)
            {
                Plugin.Log.LogWarning($"Exception in patch of static void Breachers.NetVoiceHandler::Spawned():\n{ex}");
            }
        }
        */

        //Expose match type
        // [HarmonyPatch(typeof(NetVoiceHandler), "Spawned")]
        // [HarmonyPostfix]
        // static void PostFix_NetVoiceHandler(NetVoiceHandler __instance)
        // {
        //     RootDTO dto = Plugin.RootDTO;
        //     try
        //     {
        //         dto.MatchType = __instance.NET_MatchType.ToString();
        //     }
        //     catch (System.Exception ex)
        //     {
        //         Plugin.Log.LogWarning($"Exception in patch of static void Breachers.NetVoiceHandler::Spawned():\n{ex}");
        //     }
        // }

        // [HarmonyPatch(typeof(SimpleClock), "SetTime")]
        // [HarmonyPostfix]
        // static void PostFix_SimpleClock(SimpleClock __instance)
        // {
        //     RootDTO dto = Plugin.RootDTO;
        //     try
        //     {
        //         dto.RoundTimer = __instance._lastTimeInSeconds;
        //     }
        //     catch (System.Exception ex)
        //     {
        //         Plugin.Log.LogWarning($"Exception in patch of static void Breachers.SimpleClock::SetTime():\n{ex}");
        //     }
        // }

        // [HarmonyPatch(typeof(ClientConnectionInfo), "FixedUpdateNetwork")]
        // [HarmonyPostfix]
        // static void PostFix_ClientConnectionInfo(ClientConnectionInfo __instance)
        // {
        //     RootDTO dto = Plugin.RootDTO;
        //     try
        //     {
        //         dto.PacketLoss = __instance._packetLossMeasureDelay;       
        //     }
        //     catch (System.Exception ex)
        //     {
        //         Plugin.Log.LogWarning($"Exception in patch of static void Breachers.ClientConnectionInfo::FixedUpdateNetwork():\n{ex}");
        //     }
        // }

        
        // [HarmonyPatch(typeof(ServerPing), "Refresh")]
        // [HarmonyPostfix]
        // static void PostFix_ServerPing(ServerPing __instance)
        // {
        //     RootDTO dto = Plugin.RootDTO;
        //     try
        //     {
        //         dto.Ping = __instance._ping.time;
        //         dto.ServerIP = __instance._ipAddress;
        //     }
        //     catch (System.Exception ex)
        //     {
        //         Plugin.Log.LogWarning($"Exception in patch of static void Breachers.ServerPing::Refresh():\n{ex}");
        //     }
        // }

        // [HarmonyPatch(typeof(NetCharacter), "OnHealthChanged")]
        // [HarmonyPostfix]
        // public static void OnPlayerLeft([HarmonyArgument(index:0)] Changed<NetCharacter> arg) {
        //     try {
        //         Plugin.Log.LogInfo("\n");
        //         Plugin.Log.LogInfo($"===== TO : {arg.Behaviour.NET_Health} =====");
        //         Plugin.Log.LogInfo($"===== PlayerName : {arg.Behaviour.NET_Nickname} =====");
        //         Plugin.Log.LogInfo($"===== Team : {arg.Behaviour.NET_CurrentTeam} =====");
        //         Plugin.Log.LogInfo("\n");
        //     } catch (Exception ex) {
        //         Plugin.Log.LogWarning($"Exception in patch of static void Breachers.Match::OnPlayerLeft():\n{ex}");
        //     }
        // }
    }
}