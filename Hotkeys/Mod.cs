using ICities;
using System;
using UnityEngine;

namespace Hotkeys
{
    public class Mod : IUserMod, ILoadingExtension
    {
        private static int scnt = 0;
        private int cnt = 0;
        private string GAME_OBJECT = "HotkeysObject";
        private Component script;
        private Keys controller;

        public Mod()
        {
            Util.DebugPrint("Mod constructor");
        }

        public string Name
        {
            get
            {
                Util.DebugPrint("Mod.Name");
                return "Hotkeys";
            }
        }

        public string Description
        {
            get { return "Hotkeys Mod"; }
        }

        public void OnEnabled()
        {
            Util.DebugPrint("Mod.OnEnabled");
        }

        public void OnCreated(ILoading loading)
        {
            Util.DebugPrint("Mod.OnCreated", loading.loadingComplete.ToString(), scnt.ToString(), cnt.ToString());
            scnt++;
            cnt++;

            GameObject gameObject = CheckGameObject();
            bool veryFirstTime = gameObject == null;
            CheckController(!veryFirstTime);

            if (veryFirstTime)
            {
                // Ordinary load or new game from the main menu. We are not yet in game.
                gameObject = new GameObject(GAME_OBJECT);
                script = gameObject.AddComponent<HotkeysScript>();
                Util.DebugPrint("GameObject created: id =", gameObject.GetInstanceID().ToString());
            }
            else
            {
                // The mod dll was just recompiled while in game.
                script = gameObject.GetComponent("HotkeysScript");
                SetAction();
            }

            Util.DebugPrint("HotkeysScript id =", script.GetInstanceID().ToString());
        }

        public void OnLevelLoaded(LoadMode mode)
        {
            Util.DebugPrint("Mod.OnLevelLoaded");
            SetAction();
        }

        void SetAction()
        {
            controller.InGame();

            if (script is HotkeysScript)
                ((HotkeysScript) script).updateAction = controller.Update;
            else
            {
                // The recompiled case.
                Type type = script.GetType();
                Action action = controller.Update;
                type.GetField("updateAction").SetValue(script, action);
            }
        }

        void StopAction()
        {
            if (script is HotkeysScript)
                ((HotkeysScript) script).updateAction = HotkeysScript.Empty;
            else
            {
                // The recompiled case.
                Type type = script.GetType();
                type.GetField("updateAction").SetValue(script, type.GetField("emptyAction").GetValue(script));
            }

            controller.Unloading();
        }

        public void OnLevelUnloading()
        {
            Util.DebugPrint("Mod.OnLevelUnloading");
        }

        // Called on exit to main menu / desktop. Not called on in-game load level or dll recompile.
        public void OnReleased()
        {
            Util.DebugPrint("Mod.OnReleased");
            GameObject gameObject = CheckGameObject();

            if (!(gameObject == null))
            {
                Util.DebugPrint("GameObject destroyed: id =", gameObject.GetInstanceID().ToString());
                GameObject.Destroy(gameObject);
            }
        }

        // Called just before the dll is recompiled. Also called when leaving game.
        public void OnDisabled()
        {
            Util.DebugPrint("Mod.OnDisabled");
            StopAction();
        }

        GameObject CheckGameObject()
        {
            GameObject gameObject = GameObject.Find(GAME_OBJECT);

            if (gameObject == null)
                Util.DebugPrint("GameObject does not exist");
            else
                Util.DebugPrint("GameObject exists: id =", gameObject.GetInstanceID().ToString());

            return gameObject;
        }

        void CheckController(bool recompiled)
        {
            // OnCreated was called because some other mod was recompiled?
            if (controller != null)
                StopAction();

            controller = new Keys(recompiled);
        }
    }
}
