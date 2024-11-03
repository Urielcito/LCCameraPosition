using GameNetcodeStuff;
using HarmonyLib;
using BepInEx.Logging;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace LCCameraPosition.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    public class CameraPatch : MonoBehaviour
    {
        private PlayerControllerB localPlayer;
        private LocalArmsUpdater localArmsUpdater;
        private UnityEngine.Vector3 cameraOffset = UnityEngine.Vector3.zero;

        internal ManualLogSource mls;

        static float originalFov = 0f;
        public static float zoomFov = 0f;

        static UnityEngine.Vector3 originalScale;
        public static UnityEngine.Vector3 colliderScale;

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void PostFix(StartOfRound __instance)
        {
            ((UnityEngine.Component)__instance).gameObject.AddComponent<CameraPatch>();
        }

        private void Start()
        {
            mls = BepInEx.Logging.Logger.CreateLogSource("CameraPatch.kuroshiromugen");
            if ((Object)(object)localArmsUpdater == (Object)null)
            {
                localArmsUpdater = ((Component)this).gameObject.AddComponent<LocalArmsUpdater>();
            }
        }
        private void Update()
        {
            if((Object)(object)localPlayer == (Object)null)
            {
                localPlayer = StartOfRound.Instance.localPlayerController;

                originalFov = 68f;
                zoomFov = originalFov;

                originalScale = localPlayer.thisPlayerBody.transform.localScale;
                colliderScale = originalScale;

                localArmsUpdater.plrCntrB = localPlayer;
            }

            var k = Keyboard.current;
            if (k.upArrowKey.isPressed)
            {
                cameraOffset.y += 0.01f;

                colliderScale.x += 0.01f;
                colliderScale.y += 0.01f;
                colliderScale.z += 0.01f;
                
                zoomFov += 0.1f;
            }
            if (k.downArrowKey.isPressed)
            {
                cameraOffset.y -= 0.01f;

                colliderScale.x -= 0.01f;
                colliderScale.y -= 0.01f;
                colliderScale.z -= 0.01f;

                zoomFov -= 0.1f;
            }
            if (k.leftShiftKey.isPressed && k.numpad0Key.isPressed)
            {
                cameraOffset = UnityEngine.Vector3.zero;
                zoomFov = originalFov;
                colliderScale = originalScale;
            }

            if (!localPlayer.isPlayerDead && !localPlayer.inTerminalMenu)
            {
                localPlayer.gameplayCamera.transform.localPosition = cameraOffset;
                if (localPlayer.gameplayCamera.fieldOfView != zoomFov)
                localPlayer.gameplayCamera.fieldOfView = zoomFov;
                localPlayer.thisPlayerBody.transform.localScale = colliderScale;
            }
            else
            {
                localPlayer.gameplayCamera.transform.localPosition = UnityEngine.Vector3.zero;
                localPlayer.gameplayCamera.fieldOfView = originalFov;
                localPlayer.thisPlayerBody.transform.localScale = originalScale;
            }
        }

        public UnityEngine.Vector3 getCameraOffset()
        {
            return this.cameraOffset;
        }
    }
}
