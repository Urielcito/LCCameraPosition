using GameNetcodeStuff;
using HarmonyLib;
using BepInEx.Logging;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LCCameraPosition.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    public class CameraPatch : MonoBehaviour
    {
        private PlayerControllerB localPlayer;
        private UnityEngine.Vector3 cameraOffset = UnityEngine.Vector3.zero;

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void PostFix(StartOfRound __instance)
        {
            ((UnityEngine.Component)__instance).gameObject.AddComponent<CameraPatch>();
        }

        private void Update()
        {
            localPlayer = StartOfRound.Instance.localPlayerController;
            var k = Keyboard.current;
            if (k.upArrowKey.isPressed)
            {
                cameraOffset.y += 0.01f;
            }
            if (k.downArrowKey.isPressed)
            {
                cameraOffset.y -= 0.01f;
            }
            if (k.rightArrowKey.isPressed)
            {
                cameraOffset.x += 0.01f;
            }
            if (k.leftArrowKey.isPressed)
            {
                cameraOffset.x -= 0.01f;
            }

            if (!localPlayer.isPlayerDead && !localPlayer.inSpecialInteractAnimation && !localPlayer.inTerminalMenu)
            {
                ((Component)localPlayer.gameplayCamera).transform.localPosition = cameraOffset;
            }
            else
            {
                ((Component)localPlayer.gameplayCamera).transform.localPosition = UnityEngine.Vector3.zero;
            }
        }
    }
}
