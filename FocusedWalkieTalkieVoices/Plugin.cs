using BepInEx.Configuration;
using BepInEx;
using HarmonyLib;
using LethalConfig.ConfigItems.Options;
using LethalConfig.ConfigItems;
using LethalConfig;
using FocusedWalkieTalkieVoices.Patches;

namespace FocusedWalkieTalkieVoices
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("ainavt.lc.lethalconfig")]
    public class Plugin : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);

        public static Plugin instance;

        public ConfigEntry<float> focusedWalkieTalkieVolumeMultiplier;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            InitializeConfigValues();

            harmony.PatchAll(typeof(Plugin));
            harmony.PatchAll(typeof(StartOfRoundPatch));

            this.Logger.LogInfo(PluginInfo.PLUGIN_NAME + " loaded");
        }

        private void InitializeConfigValues()
        {
            LethalConfigManager.SetModDescription(PluginInfo.PLUGIN_NAME);

            focusedWalkieTalkieVolumeMultiplier = ((BaseUnityPlugin)instance).Config.Bind(
                "FocusedWalkieTalkieVoices",
                "Volume Multiplier",
                0.2f,
                "How loud other players are compared to players talking through your walkie-talkie. From 0.0 to 1.0."
            );

            FloatSliderOptions volumePercentageSlider = new FloatSliderOptions
            {
                RequiresRestart = false,
                Min = 0f,
                Max = 1f
            };
            FloatSliderConfigItem configItem = new FloatSliderConfigItem(focusedWalkieTalkieVolumeMultiplier, volumePercentageSlider);
            LethalConfigManager.AddConfigItem(configItem);

            focusedWalkieTalkieVolumeMultiplier.SettingChanged += delegate
            {
                StartOfRound.Instance.UpdatePlayerVoiceEffects();
            };
        }

        public static void Log(string message)
        {
            instance.Logger.LogInfo($"{message}");
        }

        public static void LogError(string message)
        {
            instance.Logger.LogError(message);
        }
    }
}
