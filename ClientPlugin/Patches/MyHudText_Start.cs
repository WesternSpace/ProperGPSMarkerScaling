using HarmonyLib;
using Sandbox.Game.Gui;
using Sandbox.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientPlugin.Patches
{
    [HarmonyPatch(typeof(MyHudText), "Start")]
    internal static class MyHudText_Start
    {
        private static bool Prefix(ref float scale)
        {
            scale *= Plugin.Instance.Config.Scale;

            return true;
        }
    }
}
