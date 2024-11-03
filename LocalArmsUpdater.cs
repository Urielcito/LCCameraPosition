using BepInEx.Logging;
using GameNetcodeStuff;
using LCCameraPosition.Patches;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LCCameraPosition
{
    [DefaultExecutionOrder(-1)]
    internal class LocalArmsUpdater : MonoBehaviour
    {
        internal PlayerControllerB plrCntrB;

        private GameObject offsetBuild;

        private Vector3 org;
        private Vector3 val;

        internal ManualLogSource mls;

        private void Start()
        {
            mls = BepInEx.Logging.Logger.CreateLogSource("LocalArmsUpdater.kuroshiromugen");
            val = Vector3.zero;
        }

        private void LateUpdate()
        {
            if (plrCntrB == null)
            {
                return;
            }
            if (offsetBuild == null || offsetBuild.transform.parent == null)
            {
                if (offsetBuild == null)
                {
                    offsetBuild = new GameObject("LocalOffset");
                }
                offsetBuild.transform.parent = plrCntrB.localArmsTransform.parent;
                org = offsetBuild.transform.parent.localPosition;
                val = org;
                return;
            }
            if(!plrCntrB.inTerminalMenu && !plrCntrB.inSpecialInteractAnimation)
            {
                var k = Keyboard.current;
                if (k.upArrowKey.isPressed)
                {
                    val.y += 0.01f;
                }
                if (k.downArrowKey.isPressed)
                {
                    val.y -= 0.01f;
                }
                if (k.rightArrowKey.isPressed)
                {
                    val.x += 0.01f;
                }
                if (k.leftArrowKey.isPressed)
                {
                    val.x -= 0.01f;
                }
                if (k.leftShiftKey.isPressed && k.numpad0Key.isPressed)
                {
                    val = org;
                }
            }
            offsetBuild.transform.parent.localPosition = val;
        }
    }
}
