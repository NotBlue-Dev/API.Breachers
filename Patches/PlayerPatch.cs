using System;
using System.Collections.Generic;
using System.Linq;
using BreachersAPI.DTOS;
using BreachersAPI.Utils;
using Breachers;
using Breachers.GameModes;
using Breachers.Networking;
using BreachersAPI.DTOS.Utils;
using Fusion;
using HarmonyLib;
using UnityEngine;

namespace BreachersAPI.Patches; 

public class PlayerPatch {
    
    // ////// //
    // HEALTH //
    // ////// //
    [HarmonyPatch(typeof(NetCharacter), nameof(NetCharacter.OnHealthChanged))]
    [HarmonyPostfix]
    public static void NetCharacter_OnHealthChanged(Changed<NetCharacter> changed) {
        try {
            NetCharacter player = changed.Behaviour;
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
    //  SPAWN  //
    // /////// //
    // Quand on spawn on met à jour la vie du joueur (car pas update pas SyncedPlayerGameData)
    [HarmonyPatch(typeof(NetCharacter), nameof(NetCharacter.Spawned))]
    [HarmonyPostfix]
    public static void NetCharacter_Spawned(NetCharacter __instance) {
        try {
            string playerName = __instance.NET_Nickname;

            PlayerDTO existingPlayer = Plugin.RootDTO.Teams.SelectMany(team => team.Players).FirstOrDefault(p => p.Name == playerName);
            if (existingPlayer != null) {
                existingPlayer.Health = __instance.NET_Health;
            }
        } catch (Exception ex) {
            Plugin.Log.LogError($"PlayerPatch@NetCharacter_Spawned");
            Plugin.Log.LogError($"Error while getting data: {ex}");
        }
    }
    
    // /////// //
    // DESPAWN //
    // /////// //
    [HarmonyPatch(typeof(NetCharacter), nameof(NetCharacter.Despawned))]
    [HarmonyPostfix]
    public static void NetCharacter_Despawned(NetCharacter __instance) {
        try
        {
            string playerName = __instance.NET_Nickname;
            Teams playerTeam = __instance.NET_CurrentTeam;

            PlayerDTO existingPlayer = Plugin.RootDTO.Teams.SelectMany(team => team.Players)
                .FirstOrDefault(p => p.Name == playerName);
            TeamDTO currentTeam = Plugin.RootDTO.Teams.FirstOrDefault(t => t.Team == playerTeam);

            if (existingPlayer != null)
            {
                currentTeam?.Players.Remove(existingPlayer);
            }
            
            if (currentTeam?.Players.Count == 0)
            {
                currentTeam.Stats = new StatsDTO();
                currentTeam.Players.Clear();
            }
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"PlayerPatch@NetCharacter_Despawned");
            Plugin.Log.LogError($"Error while getting data: {ex}");
        }
    }
    
    // ////// // //OnPlayerStateChanged
    // FOOT STEP //
    // ////// // //
    [HarmonyPatch(typeof(NetCharacter), nameof(NetCharacter.OnFootStepCounterChanged))]
    [HarmonyPostfix]
    public static void NetCharacter_OnFootStepCounterChanged(Changed<NetCharacter> changed) {
        try {
            NetCharacter player = changed.Behaviour;
            string playerName = player.NET_Nickname;
            
            PlayerDTO existingPlayer = Plugin.RootDTO.Teams.SelectMany(team => team.Players)
                .FirstOrDefault(p => p.Name == playerName);
            if (existingPlayer != null)
            {
                existingPlayer.FootStepCounter = player.NET_FootStepCounter;
            }
        } catch (Exception ex) {
            Plugin.Log.LogError($"PlayerPatch@NetCharacter_OnFootStepCounterChanged");
            Plugin.Log.LogError($"Error while getting data: {ex}");
        }
    }
    
    // ////// // //////
    // PlAYER UPDATE //
    // ////// // //////
    [HarmonyPatch(typeof(NetCharacter), nameof(NetCharacter.FixedUpdateNetwork))]
    [HarmonyPostfix]
    public static void NetCharacter_FixedUpdateNetwork(NetCharacter __instance)
    {
        try
        {
            string playerName = __instance.NET_Nickname;

            PlayerDTO existingPlayer = Plugin.RootDTO.Teams.SelectMany(team => team.Players)
                .FirstOrDefault(p => p.Name == playerName);
            if (existingPlayer != null)
            {
                Vector3 pos = __instance.Head.position;
                existingPlayer.Head.X = pos.x;
                existingPlayer.Head.Y = pos.y;
                existingPlayer.Head.Z = pos.z;
                existingPlayer.CrouchPercent = __instance.NET_CrouchPercent;
                existingPlayer.IsDroning = __instance.NET_IsDroning;
                existingPlayer.IsSprinting = __instance.NET_IsSprinting;
                existingPlayer.IsCloacked = __instance.NET_IsCloaked;
            }
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"PlayerPatch@NetCharacter_FixedUpdateNetwork");
            Plugin.Log.LogError($"Error while getting data: {ex}");
        }
    }
    
    // ////////////////// //
    // NETHAND EQUIPMENTS //
    // ////////////////// //
    [HarmonyPatch(typeof(NetHand), nameof(NetHand.OnGrabbedObjectChanged))]
    [HarmonyPostfix]
    public static void NetHand_OnGrabbedObjectChanged(Changed<NetHand> changed) {
        try
        {
            NetHand hand = changed.Behaviour;
            string playerName = hand._netCharacter.NET_Nickname;

            PlayerDTO existingPlayer = Plugin.RootDTO.Teams.SelectMany(team => team.Players)
                .FirstOrDefault(p => p.Name == playerName);
            if (existingPlayer != null)
            {
                existingPlayer.Equipments.Clear();
                foreach (var equipment in hand._netCharacter._equipmentBelt._allSlots)
                {
                    if (equipment.NET_EquippedObject) {
                        existingPlayer.Equipments.Add(equipment.NET_EquippedObject.ItemId.ToString());
                    }
                }
            }
            
            
        } catch (Exception ex) {
            Plugin.Log.LogError($"NetHand@OnGrabbedObjectChanged");
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
    public static void NetCharacter_OnIsOutsideChanged(Changed<NetCharacter> changed) {
        try {
            NetCharacter player = changed.Behaviour;
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
    
    
    // //////////////////
    // BELT EQUIPMENTS //
    // //////////////////
    [HarmonyPatch(typeof(EquipmentSlot), nameof(EquipmentSlot.EquipmentInSlotChanged))]
    [HarmonyPostfix]
    public static void EquipmentSlot_EquipmentInSlotChanged(EquipmentSlot __instance) {
        try
        {
            string playerName = __instance._netCharacter.NET_Nickname;

            PlayerDTO existingPlayer = Plugin.RootDTO.Teams.SelectMany(team => team.Players)
                .FirstOrDefault(p => p.Name == playerName);
            if (existingPlayer != null)
            {
                existingPlayer.Equipments.Clear();
                foreach (var equipment in __instance._netCharacter._equipmentBelt._allSlots)
                {
                    if (equipment.NET_EquippedObject) {
                        existingPlayer.Equipments.Add(equipment.NET_EquippedObject.ItemId.ToString());
                    }
                }
            }
            
            
        } catch (Exception ex) {
            Plugin.Log.LogError($"EquipmentSlot@EquipmentInSlotChanged");
            Plugin.Log.LogError($"Error while getting data: {ex}");
        }
    }
    
    
    // ////// ///// ///
    // VIRTUAL STOCK //
    // ////// ///// ///
    // a tester
    [HarmonyPatch(typeof(NetCharacter), nameof(NetCharacter.OnVirtualStockChanged))]
    [HarmonyPostfix]
    public static void NetCharacter_OnVirtualStockChanged(NetCharacter __instance) {
        try {
            string playerName = __instance.NET_Nickname;

            PlayerDTO existingPlayer = Plugin.RootDTO.Teams.SelectMany(team => team.Players)
                .FirstOrDefault(p => p.Name == playerName);
            if (existingPlayer != null)
            {
                existingPlayer.UsingVirtualStock = __instance.NET_UsingVirtualStock;
            }
        } catch (Exception ex) {
            Plugin.Log.LogError($"PlayerPatch@NetCharacter_OnVirtualStockChanged");
            Plugin.Log.LogError($"Error while getting data: {ex}");
        }
    }
    
    // //////// ////
    //  LAST KILL //
    // //////// ////
    [HarmonyPatch(typeof(NetMessageSender), nameof(NetMessageSender.RPC_DisplaySpectatorKillConfirmationMessage))]
    [HarmonyPostfix]
    public static void RPC_DisplaySpectatorKillConfirmationMessage(
        PlayerRef spectator,
        string killerColorHex,
        string killerNickname,
        string victimColorHex,
        string victimNickname,
        int usedWeapon,
        bool isHeadshot,
        bool isWallBang,
        bool isSmokeBang,
        bool isCollat)
    {
        try {
            Plugin.RootDTO.LastKill.Killer.Name = killerNickname;
            Plugin.RootDTO.LastKill.Killer.Team = TeamsColor.GetTeamNameFromColor(killerColorHex);
            Plugin.RootDTO.LastKill.Victim.Name = victimNickname;
            Plugin.RootDTO.LastKill.Victim.Team = TeamsColor.GetTeamNameFromColor(victimColorHex);
            Plugin.RootDTO.LastKill.Victim.IsHeadshot = isHeadshot;
            Plugin.RootDTO.LastKill.Victim.IsWallbang = isWallBang;
            Plugin.RootDTO.LastKill.Victim.IsSmokeBang = isSmokeBang;
            Plugin.RootDTO.LastKill.Victim.IsCollat = isCollat;
            Plugin.RootDTO.LastKill.Killer.Weapon = (ItemId)usedWeapon;
        } catch (Exception ex) {
            Plugin.Log.LogError($"PlayerPatch@NetMessageSender_RPC_DisplaySpectatorKillConfirmationMessage");
            Plugin.Log.LogError($"Error while getting data: {ex}");
        }
    }

    // //////////// //
    // TEAM CHANGED //
    // //////////// //
    [HarmonyPatch(typeof(NetCharacter), nameof(NetCharacter.OnCurrentTeamChanged))]
    [HarmonyPostfix]
    public static void NetCharacter_OnCurrentTeamChanged(Changed<NetCharacter> changed) {
        try {
            string playerName = changed.Behaviour.NET_Nickname;
            Teams playerTeam = changed.Behaviour.NET_CurrentTeam;
            
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
            Plugin.RootDTO.SpectatorCount = netGameData.NET_SpectatorCount;
            Plugin.RootDTO.Map = netGameData.NET_ActiveSceneName.ToString();
            Plugin.RootDTO.PlayerCount = netGameData.SyncedPlayerGameData.ToArray().Count(p => !p.IsBot);

            foreach (PlayerSyncData playerData in netGameData.SyncedPlayerGameData) {
                TeamDTO team = Plugin.RootDTO.Teams.FirstOrDefault(t => t.Team == playerData.Team);
                if (team.HasValue()) {
                    PlayerDTO player = team.Players.FirstOrDefault(p => p.Name == playerData.Name.ToString());
                    if (player.HasValue()) {
                        player.SetInfos(playerData);
                    } else {
                        PlayerDTO newPlayer = new PlayerDTO();
                        newPlayer.Head = new HeadDTO();
                        newPlayer.Equipments = new List<string>();
                        newPlayer.SetInfos(playerData);
                        team.Players.Add(newPlayer);
                    }
                    team.SetTeamStats();
                }
            }
            
            if(Plugin.RootDTO.Teams != null) {
                foreach (TeamDTO team in Plugin.RootDTO.Teams) {
                    if (team.Players != null) {
                        foreach (PlayerDTO player in team.Players) {
                            //remove player if not in game anymore SyncedPlayerGameData is a network array
                            if (!netGameData.SyncedPlayerGameData.ToArray().Any(p => p.Name.ToString() == player.Name)) {
                                team.Players.Remove(player);
                            }
                            
                        }
                    }
                }
            }
        } catch (Exception ex) {
            Plugin.Log.LogError($"PlayerPatch@SpectatorActivePlayerStatus_OnSyncedDataChanged");
            Plugin.Log.LogError($"Error while getting data: {ex}");
        }
    }
}