using System;
using UnityEngine;

namespace Hotkeys
{
    /// <summary>
    /// This is the only class that cannot be replaced by recompiling while in game. Unity wants
    /// to use the original version in any case. Therefore, the class is small and simple so that
    /// there is no need to modify it while in game.
    /// </summary>
    public class HotkeysScript : MonoBehaviour
    {
        public Action updateAction = Empty;
        public readonly Action emptyAction = Empty;

        public void Awake()
        {
            DontDestroyOnLoad(this);
            Util.DebugPrint("HotkeysScript.Awake");
        }

        public void Start()
        {
            Util.DebugPrint("HotkeysScript.Start: script id =", GetInstanceID().ToString());
        }

        public void Update()
        {
            if (Input.anyKeyDown)
                updateAction(); // goes to Keys.Update()
        }

        public void OnDestroy()
        {
            Util.DebugPrint("HotkeysScript.OnDestroy: script id =", GetInstanceID().ToString());
        }

        public static void Empty()
        {
        }
    }
}
