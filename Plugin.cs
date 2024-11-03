using BreachersAPI.DTOS;
using BreachersAPI.Networking;
using BreachersAPI.ObjectData;
using BreachersAPI.Patches;
using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using Breachers;
using Breachers.GameModes;
using MethodBase = System.Reflection.MethodBase;

namespace BreachersAPI {
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin {
        internal static RootDTO RootDTO;
        internal static ObjManager objManager;
        internal new static ManualLogSource Log;
        internal static PlayerMatchData[] PrevRoundData;
        private readonly Server server;

        public Plugin() {
            server = new Server();
            objManager = new ObjManager();
            PrevRoundData = null;
            RootDTO = new RootDTO {
                Teams = new List<TeamDTO>() {
                    new TeamDTO() {
                        Team = Teams.Enforcers,
                        Stats = new StatsDTO(),
                        Players = new List<PlayerDTO>(),
                    },
                    new TeamDTO() {
                        Team = Teams.Revolters,
                        Stats = new StatsDTO(),
                        Players = new List<PlayerDTO>()
                    }
                },
                LastKill = new LastKillDTO() {
                    Killer = new KillerDTO(),
                    Victim = new VictimDTO()
                },
                Spectator = new SpectatorDTO() {
                    Position = new List<float>(),
                    Forward = new List<float>(),
                    Right = new List<float>(),
                    Up = new List<float>(),
                    Rotation = new List<float>()
                }
            };
        }
        
        /// <summary>
        /// Chargement du plugin dans le jeu
        /// </summary>
        public override void Load() {
            Log = base.Log;
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded successfully!");

            server.CreateAndStartServer();

            Harmony.CreateAndPatchAll(typeof(GamePatch));
            Harmony.CreateAndPatchAll(typeof(PlayerPatch));

            // Affichage de toutes les méthodes qu'on a patché
            foreach (MethodBase method in Harmony.GetAllPatchedMethods()) {
                Log.LogInfo($"Hook: {method.Name}");
            }
        }

        /// <summary>
        /// Déchargement du plugin dans le jeu
        /// </summary>
        public override bool Unload() {
            server.CloseServer();
            return false;
        }
    }
}
