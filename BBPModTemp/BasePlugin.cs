using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.OptionsAPI;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Studentcator
{
    [BepInPlugin("ganaisthere.plus.studentcator", "Studentcator", "0.1.0.0")]
    [BepInDependency("mtm101.rulerp.bbplus.baldidevapi")]

    public class BasePlugin : BaseUnityPlugin
    {
        public static BasePlugin Instance { get; private set; }
        public static Harmony harmony;
        public static AssetManager AssetMan = new AssetManager();
        public static string ModPath;
        public static bool ready = false;
        public static List<Sprite> packs = new List<Sprite>();
        public static List<string> packNames = new List<string>();
        //--------------------------------------------------------------
        public static bool optionsMenuBuilt = false;
        //--------------------------------------------------------------
        public ConfigEntry<int> ConfigPackIndex;

        internal void Awake()
        {
            Instance = this;
            harmony = new Harmony("ganaisthere.plus.studentcator");
            harmony.PatchAllConditionals();
            ModPath = AssetLoader.GetModPath(this);

            ConfigPackIndex = Config.Bind
            (
                "General",
                "Pack Index",
                0,
                "The index of selected pack."
            );

            LoadAssets();

            CustomOptionsCore.OnMenuInitialize += OnMen;
        }

        private void OnMen(OptionsMenu __instance, CustomOptionsHandler handler)
        {
            handler.AddCategory<PackOptions>("Studentcator\nOptions");
        }

        public void LoadAssets()
        {
            if (!Directory.Exists(ModPath))
            {
                return;
            }
            string[] getFiles = Directory.GetFiles(ModPath, "*.png", SearchOption.TopDirectoryOnly);
            if (getFiles.Length <= 0)
            {
                return;
            }
            packs.Clear();
            packNames.Clear();
            foreach (string file in getFiles)
            {
                Log(file);
                packs.Add(AssetLoader.SpriteFromFile(file, new Vector2(0.5f, 0.5f)));
                packNames.Add(Path.GetFileName(file));
            }

            if (packs.Count - 1 < ConfigPackIndex.Value)
            {
                ConfigPackIndex.Value = packs.Count - 1;
            }

            ready = true;
        }

        internal void Log(object data, int level = 0)
        {
            if (level == 1)
            {
                Logger.LogWarning(data);
            }
            else if (level == 2)
            {
                Logger.LogError(data);
            }
            else
            {
                Logger.LogInfo(data);
            }
        }
    }
}
