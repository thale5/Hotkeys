using ColossalFramework.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Hotkeys
{
    public class Keys
    {
        UILabel label;
        List<Click> clicks = null;
        Hotkey hotkey = null;
        readonly bool recompiled;
        bool inProgress = false;

        public Keys(bool recompiled)
        {
            Util.DebugPrint("Keys constructor");
            this.recompiled = recompiled;
        }

        public void InGame()
        {
            Util.DebugPrint("Keys.InGame");
        }

        public void Unloading()
        {
            Util.DebugPrint("Keys.Unloading");

            if (!(label == null))
                UnityEngine.Object.Destroy(label);

            label = null;
        }

        public void Update()
        {
            // Currently recording?
            if (inProgress)
                Recording();
            else
            {
                if (Input.GetKeyDown(KeyCode.P)) // start recording a hotkey?
                {
                    inProgress = true;
                    clicks = new List<Click>(4);
                }
                else if (hotkey != null && hotkey.isPressed)
                    hotkey.Execute();
                else if (Input.GetMouseButtonDown(0))
                {
                    if (Input.GetKey(KeyCode.LeftAlt))
                        Show(UISupport.FindHovered());
                    else if (Input.GetKey(KeyCode.LeftShift))
                        UIWalk.Walk();
                }
            }
        }

        // Hotkey recording logic.
        void Recording()
        {
            if (Input.GetMouseButton(1)) // cancelled?
            {
                inProgress = false;
                clicks.Clear();
                clicks = null;
            }
            else if (Input.GetMouseButtonDown(0)) // a left mouse button click?
            {
                Click click = UISupport.FindHovered();

                if (click != null)
                    clicks.Add(click);
            }
            else // bind the clicks to the keymapping the user presses right now
            {
                Hotkey h = new Hotkey(clicks.ToArray());

                if (!h.isEmpty)
                {
                    hotkey = h;
                    inProgress = false;
                    clicks.Clear();
                    clicks = null;
                }
            }
        }

        void Show(object o)
        {
            if (label == null)
                CreateLabel();

            if (o != null)
            {
                label.text = o.ToString();
                label.Show();
            }
            else
                label.Hide();
        }

        void CreateLabel()
        {
            UIView view = ColossalFramework.UI.UIView.GetAView();
            label = (UILabel) view.AddUIComponent(typeof(UILabel));
            label.transformPosition = new Vector3(-1.1f, 0.5f);
            label.textColor = new Color32(240, 240, 240, 192);
            label.autoSize = true;
            label.textScale = 1.2f;
            label.dropShadowColor = new Color32(64, 64, 64, 64);
            label.dropShadowOffset = new Vector2(2f, -1.5f);
            label.useDropShadow = true;
        }
    }
}
