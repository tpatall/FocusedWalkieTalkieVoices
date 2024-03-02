using GameNetcodeStuff;
using HarmonyLib;

namespace FocusedWalkieTalkieVoices.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("UpdatePlayerVoiceEffects")]
        static void ChangeVolumeWhileWalkieTalkie(PlayerControllerB[] ___allPlayerScripts)
        {
            if (GameNetworkManager.Instance == null || GameNetworkManager.Instance.localPlayerController == null)
            {
                return;
            }

            PlayerControllerB playerControllerB = ((!GameNetworkManager.Instance.localPlayerController.isPlayerDead || !(GameNetworkManager.Instance.localPlayerController.spectatedPlayerScript != null)) ? GameNetworkManager.Instance.localPlayerController : GameNetworkManager.Instance.localPlayerController.spectatedPlayerScript);
            for (int i = 0; i < ___allPlayerScripts.Length; i++)
            {
                PlayerControllerB playerControllerB2 = ___allPlayerScripts[i];
                if ((!playerControllerB2.isPlayerControlled && !playerControllerB2.isPlayerDead) || playerControllerB2 == GameNetworkManager.Instance.localPlayerController)
                {
                    continue;
                }

                bool flag = playerControllerB2.speakingToWalkieTalkie && playerControllerB.holdingWalkieTalkie && playerControllerB2 != playerControllerB;
                if (!GameNetworkManager.Instance.localPlayerController.isPlayerDead)
                {
                    if (flag)
                    {
                        playerControllerB2.voicePlayerState.Volume = 1.0f;

                        for (int j = 0; j < ___allPlayerScripts.Length; j++)
                        {
                            PlayerControllerB playerControllerB3 = ___allPlayerScripts[j];
                            if ((!playerControllerB3.isPlayerControlled && !playerControllerB3.isPlayerDead) || playerControllerB3 == GameNetworkManager.Instance.localPlayerController 
                                || playerControllerB3.speakingToWalkieTalkie)
                            {
                                continue;
                            }

                            playerControllerB3.voicePlayerState.Volume = Plugin.instance.focusedWalkieTalkieVolumeMultiplier.Value;
                        }
                    }
                }
            }
        }
    }
}
