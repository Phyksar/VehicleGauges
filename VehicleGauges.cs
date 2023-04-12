using System.Drawing;
using System.Windows.Forms;

namespace GTA
{
    public class VehicleGauges : Script
    {
        private const float MpsToKmph = 3.6f;
        private const float MetricToImperial = 0.621371f;
        private const string MetricUnits = "km/H";
        private const string ImperialUnits = "mpH";

        private bool useMetricUnits;
        private bool useForPassenger;

        private Font speedGaugeFont;
        private Color speedGaugeColor;
        private RectangleF speedGaugeRectangle;

        public VehicleGauges()
        {
            speedGaugeFont = new Font(0.024f, FontScaling.ScreenUnits, true, false);
            speedGaugeFont.Effect = FontEffect.Edge;
            speedGaugeFont.EffectSize = 2;
            speedGaugeColor = Color.FromArgb(200, Color.White);
            speedGaugeRectangle = new RectangleF(0.145f, 0.9225f, 0.06f, 0.03f);
            useMetricUnits = Settings.GetValueBool("UseMetricUnits", "Configuration", true);
            useForPassenger = Settings.GetValueBool("UseForPassenger", "Configuration", true);
            PerFrameDrawing += new GraphicsEventHandler(DrawOverlay);
        }

        private void DrawOverlay(object sender, GraphicsEventArgs e)
        {
            if (!ShouldDraw()) {
                return;
            }
            e.Graphics.Scaling = FontScaling.ScreenUnits;
            e.Graphics.DrawText(
                GetSpeedGaugeText(),
                speedGaugeRectangle,
                TextAlignment.Right | TextAlignment.VerticalCenter,
                speedGaugeColor,
                speedGaugeFont);
        }

        private string GetSpeedGaugeText()
        {
            float speed = Player.Character.CurrentVehicle.Speed * MpsToKmph;
            if (!useMetricUnits) {
                speed *= MetricToImperial;
            }
            return string.Format("{0:n0} {1}", speed, useMetricUnits ? MetricUnits : ImperialUnits);
        }

        private bool ShouldDraw()
        {
            return Player.CanControlCharacter
                && Player.Character.CurrentVehicle != null
                && Player.Character.isInVehicle()
                && Player.Character.isSittingInVehicle()
                && (useForPassenger || Player.Character.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == Player);
        }
    }
}
