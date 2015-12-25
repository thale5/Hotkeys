using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hotkeys
{
    /// <summary>
    /// A keymapping and the sequence of mouse clicks (~macro) associated with it.
    /// </summary>
    public class Hotkey
    {
        readonly KeyCode key = KeyCode.None;
        readonly bool ctrl, alt, shift, cmd;
        readonly Click[] clicks;

        public bool isEmpty
        {
            get
            {
                return key == KeyCode.None || clicks.Length == 0;
            }
        }

        public bool isPressed
        {
            get
            {
                return Input.GetKeyDown(key) && ctrl == Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)
                                             && alt == Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)
                                             && shift == Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)
                                             && cmd == osx && (Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand));
            }
        }

        // Accepted keydown codes.
        static readonly KeyCode[] keyCodes;

        // Prefixes of the keydown codes that are not accepted.
        static string[] ignored = {"None", "Esc", "RightControl", "LeftControl", "RightAlt", "LeftAlt", "RightShift", "LeftShift",
                                   "RightCommand", "LeftCommand", "AltGr", "Mouse0", "Mouse1", "Joystick"};

        static bool osx = Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor;

        public Hotkey(KeyCode key, bool ctrl, bool alt, bool shift, bool cmd, Click[] clicks)
        {
            this.key = key;
            this.ctrl = ctrl;
            this.alt = alt;
            this.shift = shift;
            this.cmd = cmd;
            this.clicks = clicks;
        }

        public Hotkey(Click[] clicks)
        {
            for (int i = 0; i < keyCodes.Length; i++)
                if (Input.GetKeyDown(keyCodes[i]))
                {
                    this.key = keyCodes[i];
                    break;
                }

            this.ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            this.alt = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
            this.shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            this.cmd = osx && (Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand));
            this.clicks = clicks;
        }

        public bool Execute()
        {

            return true;
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder(16);

            if (ctrl)
                b.Append("ctrl-");

            if (alt)
                b.Append("alt-");

            if (shift)
                b.Append("shift-");

            if (cmd)
                b.Append("cmd-");

            return b.Append(key.ToString()).ToString();
        }

        static Hotkey()
        {
            List<KeyCode> codes = new List<KeyCode>(128);

            foreach (KeyCode k in Enum.GetValues(typeof(KeyCode)))
            {
                string s = k.ToString();

                if (!ignored.Any(i => s.StartsWith(i)))
                    codes.Add(k);
            }

            keyCodes = codes.ToArray();
        }
    }
}
