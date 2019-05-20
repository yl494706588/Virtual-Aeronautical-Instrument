using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Text;
using System.Data;

namespace AvionicsInstrumentControlDemo
{
    public class AttitudeIndicatorInstrumentControl : InstrumentControl
    {
        #region Fields

        // Parameters
        double PitchAngle = 0; // Phi
		double RollAngle = 0; // Theta
        int Heading;
        // Images
        Bitmap bmpCadran = new Bitmap(AvionicsInstrumentControlDemo.AvionicsInstrumentsControls.AvionicsInstrumentsControlsRessources.Horizon_Background);
        Bitmap bmpBoule = new Bitmap(AvionicsInstrumentControlDemo.AvionicsInstrumentsControls.AvionicsInstrumentsControlsRessources.Horizon_GroundSky);
        Bitmap bmpAvion = new Bitmap(AvionicsInstrumentControlDemo.AvionicsInstrumentsControls.AvionicsInstrumentsControlsRessources.Maquette_Avion);
        Bitmap bmpHedingWeel = new Bitmap(AvionicsInstrumentControlDemo.AvionicsInstrumentsControls.AvionicsInstrumentsControlsRessources.HeadingWeel);
        #endregion

        #region Contructor

		private System.ComponentModel.Container components = null;

        public AttitudeIndicatorInstrumentControl()
		{
			// Double bufferisation
			SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint |
				ControlStyles.AllPaintingInWmPaint, true);
        }

        #endregion

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        #region Paint

        protected override void OnPaint(PaintEventArgs pe)
        {
            // Calling the base class OnPaint
            base.OnPaint(pe);

            // Pre Display computings

            Point ptBoule = new Point(-75, - 629);
            Point ptRotation = new Point(174, 174);
            Point ptImgHeadingWeel = new Point(0, 0);

            double alphaHeadingWeel = InterpolPhyToAngle(Heading, 0, 360, 360, 0);
            float scale = (float)this.Width / bmpCadran.Width;//0.923F;//
            // Affichages - - - - - - - - - - - - - - - - - - - - - - 

            bmpCadran.MakeTransparent(Color.Yellow);
            bmpAvion.MakeTransparent(Color.Yellow);
            bmpHedingWeel.MakeTransparent(Color.Yellow);


            


            // display Horizon
            RotateAndTranslate(pe, bmpBoule, RollAngle, 0, ptBoule, (int)(5.715*PitchAngle), ptRotation, scale);

            // diplay mask
            Pen maskPen = new Pen(this.BackColor,0*scale);
            pe.Graphics.DrawRectangle(maskPen, 0, 0, bmpCadran.Width * scale, bmpCadran.Height * scale);

            // display cadran
            pe.Graphics.DrawImage(bmpCadran, 0, 0, (float)(bmpCadran.Width * scale), (float)(bmpCadran.Height * scale));

            // display aircraft symbol
            pe.Graphics.DrawImage(bmpAvion, (float)((0.5 * bmpCadran.Width - 0.5 * bmpAvion.Width) * scale), (float)((0.5 * bmpCadran.Height - 0.5 * bmpAvion.Height) * scale), (float)(bmpAvion.Width * scale), (float)(bmpAvion.Height * scale));

            //display hangxiang
            RotateImage(pe, bmpHedingWeel, alphaHeadingWeel, ptImgHeadingWeel, ptRotation, scale);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Define the physical value to be displayed on the indicator
        /// </summary>
        /// <param name="aircraftPitchAngle">The aircraft pitch angle in °deg</param>
        /// <param name="aircraftRollAngle">The aircraft roll angle in °deg</param
        public void SetAttitudeIndicatorParameters(double aircraftPitchAngle, double aircraftRollAngle, int aircraftHeading)
        {
            PitchAngle = aircraftPitchAngle;
            RollAngle = -aircraftRollAngle * Math.PI / 180;
            Heading = aircraftHeading;
            Heading = (360 - Heading) % 360;
            //if (Heading < 0) Heading = (180 + Heading) + 180;
            
            this.Refresh();
        }

        #endregion

    }
}
