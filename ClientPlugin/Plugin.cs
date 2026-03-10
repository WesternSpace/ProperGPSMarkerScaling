using System;
using System.IO;
using System.Reflection;
using ClientPlugin.Config;
using ClientPlugin.GUI;
using HarmonyLib;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using VRage.FileSystem;
using VRage.Input;
using VRage.Plugins;
using VRageMath;

namespace ClientPlugin
{
    // ReSharper disable once UnusedType.Global
    public class Plugin : IPlugin, IDisposable
    {
        public const string Name = "GpsMarkerScaling";
        public static Plugin Instance { get; private set; }

        public IPluginConfig Config => config?.Data;
        private PersistentConfig<PluginConfig> config;
        private static readonly string ConfigFileName = $"{Name}.cfg";

        private const float SENSITIVITY = 0.001f;


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public void Init(object gameInstance)
        {
            Instance = this;

            if (!Directory.Exists(Path.Combine(MyFileSystem.UserDataPath, "Storage\\PluginData")))
            {
                Directory.CreateDirectory(Path.Combine(MyFileSystem.UserDataPath, "Storage\\PluginData"));
            }

            var configPath = Path.Combine(MyFileSystem.UserDataPath, "Storage\\PluginData", ConfigFileName);
            config = PersistentConfig<PluginConfig>.Load(configPath);

            Harmony harmony = new Harmony(Name);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public void Dispose()
        {
            Instance = null;
        }

        public void Update()
        {
            if (!IsInputEnabled())
                return;

            if (!MyInput.Static.IsKeyPress(MyKeys.Alt) || !MyInput.Static.IsKeyPress(MyKeys.Shift))
            {
                return;
            }

            float currentScale = Config.Scale;

            MyAPIGateway.Utilities.ShowNotification("GPS Scale: " + Math.Round(currentScale, 2), 10);

            float targetScale = default;
            float delta = MyInput.Static.DeltaMouseScrollWheelValue();

            if (delta == 0)
                return;

            targetScale = MathHelper.Clamp(Config.Scale + (delta * SENSITIVITY), 0.2f, 2.5f);
            if (Math.Round(targetScale, 2) != Math.Round(currentScale, 2))
            {
                Config.Scale = (float)MathHelper.Lerp(currentScale, targetScale, .15f);
            }
        }

        private bool IsInputEnabled()
        {
            if (MySession.Static == null || MyInput.Static == null)
            {
                return false;
            }

            if (MyCubeBuilder.Static?.IsActivated ?? false)
            {
                return false;
            }

            if (!MySession.Static.LocalCharacter?.IsInFirstPersonView ?? false)
            {
                return false;
            }

            return true;
        }

        public void OpenConfigDialog()
        {
            MyGuiSandbox.AddScreen(new MyPluginConfigDialog());
        }
    }
}
