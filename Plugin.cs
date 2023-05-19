
using API.DTOS;
using API.Networking;
using API.ObjectData;
using API.patches;
using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;

namespace API
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        internal static RootDTO rootDTO;
        internal static ObjManager objManager;
        internal static new ManualLogSource Log;
        private readonly Server server;

        public Plugin()
        {
            rootDTO = new RootDTO();
            server = new Server();
            objManager = new ObjManager();
            rootDTO.Teams = new List<TeamDTO>();
            rootDTO.LastKill = new LastKillDTO();

            TeamDTO Enforcers = new()
            {
                Team = "Enforcers",
                Stats = new StatsDTO(),
                Players = new List<PlayerDTO>()
            };
            rootDTO.Teams.Add(Enforcers);

            TeamDTO Revolters = new()
            {
                Team = "Revolters",
                Stats = new StatsDTO(),
                Players = new List<PlayerDTO>()
            };

            rootDTO.Teams.Add(Revolters);

        }
        public override void Load()
        {
           
  
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            server.CreateAndStartServer();

            Harmony.CreateAndPatchAll(typeof(Update));

            var originalMethods = Harmony.GetAllPatchedMethods();
            foreach (var method in originalMethods) { Log.LogInfo("TOUT LES HOOK : " + method);  }
        }

        public override bool Unload()
        {
            server.CloseServer();
            return false;
        }

    }
}
