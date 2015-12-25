using ColossalFramework;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Hotkeys
{
    public class Util
    {
        public static void PanelPrint(params string[] args)
        {
            DateTime now = System.DateTime.Now;
            long millis = now.Ticks / 10000;
            string s = String.Format("[Hotkeys] {0, -42} {1, 22} {2, 2}  {3}.{4}", String.Join(" ", args), now, Thread.CurrentThread.ManagedThreadId, millis / 1000, millis % 1000);
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, s);
        }

        public static void DebugPrint(params string[] args)
        {
            DateTime now = System.DateTime.Now;
            long millis = now.Ticks / 10000;
            string s = String.Format("[Hotkeys] {0, -42} {1, 22} {2, 2}  {3}.{4}", String.Join(" ", args), now, Thread.CurrentThread.ManagedThreadId, millis / 1000, millis % 1000);
            Debug.Log(s);
        }

        public static void EnableLogChannel()
        {
            CODebugBase<LogChannel>.EnableChannels(LogChannel.All);
            CODebugBase<LogChannel>.verbose = true;
        }

        public static void EnableInternalLogChannel()
        {
            CODebugBase<InternalLogChannel>.EnableChannels(InternalLogChannel.All);
            CODebugBase<InternalLogChannel>.verbose = true;
        }
    }
}
