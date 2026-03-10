using System;
using Sandbox;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Utils;
using VRageMath;

namespace ClientPlugin.GUI
{

    public class MyPluginConfigDialog : MyGuiScreenBase
    {
        private const string Caption = "GPS Marker Scaling Configuration";
        public override string GetFriendlyName() => "MyPluginConfigDialog";

        private MyLayoutTable layoutTable;

        private MyGuiControlLabel scaleLabel;
        private MyGuiControlSlider scaleSlider;

        private MyGuiControlButton closeButton;

        public MyPluginConfigDialog() : base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.5f, 0.3f), false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
        {
            EnabledBackgroundFade = true;
            m_closeOnEsc = true;
            m_drawEvenWithoutFocus = true;
            CanHideOthers = true;
            CanBeHidden = true;
            CloseButtonEnabled = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            RecreateControls(true);
        }

        public override void RecreateControls(bool constructor)
        {
            base.RecreateControls(constructor);

            CreateControls();
            LayoutControls();
        }

        private void CreateControls()
        {
            AddCaption(Caption);

            var config = Plugin.Instance.Config;
            CreateSlider(out scaleLabel, out scaleSlider, config.Scale, value => config.Scale = value, "Marker Scale", "Changes the marker scale.");

            closeButton = new MyGuiControlButton(position: new Vector2(0f, 0.1f),originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: OnOk);
            Controls.Add(closeButton);
        }

        private void OnOk(MyGuiControlButton _) => CloseScreen();

        private void CreateSlider(out MyGuiControlLabel labelControl, out MyGuiControlSlider checkboxControl, float value, Action<float> store, string label, string tooltip)
        {
            labelControl = new MyGuiControlLabel
            {
                Text = label,
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP
            };

            checkboxControl = new MyGuiControlSlider(minValue: 0.2f, maxValue: 2.5f, toolTip: tooltip)
            {
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
                Enabled = true,
                Value = value
            };
            checkboxControl.ValueChanged += cb => store(cb.Value);
        }

        private void LayoutControls()
        {
            var size = Size ?? Vector2.One;
            layoutTable = new MyLayoutTable(this, new Vector2(-0.4f, -0.3f) * size, 0.6f * size);
            layoutTable.SetColumnWidths(200f, 100f);
            // TODO: Add more row heights here as needed
            layoutTable.SetRowHeights(90f);

            var row = 0;

            layoutTable.Add(scaleLabel, MyAlignH.Left, MyAlignV.Center, row, 0);
            layoutTable.Add(scaleSlider, MyAlignH.Left, MyAlignV.Center, row, 1);
        }

        public override bool Draw()
        {
            base.Draw();

            MyGuiManager.DrawString("White", $"Scale: {Math.Round(Plugin.Instance.Config.Scale, 2)}", new Vector2(0.5f), 1f, new Color(1f, 1f, 1f, m_transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);

            return true;
        }
    }
}