using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

using HarmonyLib;
using Lib;
using System.Drawing.Imaging;
using Solaris.Properties;

namespace Solaris
{
    public class SolarisTheme : AuroraPatch.Patch
    {
        override public string Description => "Solaris Theme";
        override public IEnumerable<string> Dependencies => new[] { "ThemeCreator", "Lib" };

        // Fonts
        static readonly private FontFamily fontFamily = new FontFamily("Tahoma");
        static readonly private Font mainFont = new Font(fontFamily, 8, FontStyle.Regular);
        static readonly private Font buttonFont = new Font(fontFamily, 7, FontStyle.Bold);

        // Colors
        static readonly private Color mainBackgroundColor = Color.FromArgb(12, 12, 12);
        static readonly private Color mainTextColor = Color.FromArgb(223, 223, 223);
        static readonly private Color buttonBackgroundColor = Color.FromArgb(23, 26, 39);
        static readonly private Color orbitColor = Color.FromArgb(64, 64, 64);
        static readonly private Color planetColor = Color.FromArgb(128, 128, 128);

        override protected void Loaded(Harmony harmony)
        {
            ThemeCreator.ThemeCreator.AddColorChange(Color.FromArgb(0, 0, 64), mainBackgroundColor);
            ThemeCreator.ThemeCreator.AddColorChange(Color.FromArgb(255, 255, 192), mainTextColor);

            ThemeCreator.ThemeCreator.AddColorChange(
                typeof(Button),
                new ThemeCreator.ColorChange { BackgroundColor = buttonBackgroundColor }
            );

            ThemeCreator.ThemeCreator.AddFontChange(mainFont);
            ThemeCreator.ThemeCreator.AddFontChange(typeof(Button), buttonFont);

            ThemeCreator.ThemeCreator.SetCometTailColor(orbitColor);
            ThemeCreator.ThemeCreator.SetPlanetColor(planetColor);

            ThemeCreator.ThemeCreator.DrawEllipsePrefixAction((graphics, pen) =>
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // LimeGreen circles is used to mark orbits and colonies
                if (pen.Color == Color.LimeGreen)
                {
                    pen.Color = orbitColor;
                }
            });

            ThemeCreator.ThemeCreator.FillEllipsePrefixAction((graphics, brush) =>
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
            });

            ThemeCreator.ThemeCreator.DrawLinePrefixAction((graphics, pen) =>
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Movement tails
                // TODO: Hostiles
                if (pen.Color == Color.FromArgb(0, 206, 209) || pen.Color == Color.FromArgb(255, 255, 192))
                {
                    pen.Color = ControlPaint.Dark(pen.Color, 0.5f);
                }
                // Comet path (distance ruler also uses LimeGreen but has pen.Width > 1)
                else if (pen.Color == Color.LimeGreen && pen.Width == 1)
                {
                    pen.Color = orbitColor;
                }
            });

            // Button images
            ChangeButtonImage(AuroraButton.ZoomIn, Resources.Icon_ZoomIn, mainTextColor);
            ChangeButtonImage(AuroraButton.ZoomOut, Resources.Icon_ZoomOut, mainTextColor);
            ChangeButtonImage(AuroraButton.Up, Resources.Icon_Up, mainTextColor);
            ChangeButtonImage(AuroraButton.Down, Resources.Icon_Down, mainTextColor);
            ChangeButtonImage(AuroraButton.Left, Resources.Icon_Left, mainTextColor);
            ChangeButtonImage(AuroraButton.Right, Resources.Icon_Right, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarColony, Resources.Icon_Colony, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarIndustry, Resources.Icon_Industry, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarMining, Resources.Icon_Mining, Color.FromArgb(243, 174, 129));
            ChangeButtonImage(AuroraButton.ToolbarResearch, Resources.Icon_Research, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarWealth, Resources.Icon_Wealth, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarClass, Resources.Icon_Class, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarProject, Resources.Icon_Project, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarFleet, Resources.Icon_Fleet, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarMissileDesign, Resources.Icon_MissileDesign, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarTurrent, Resources.Icon_Turrent, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarGroundForces, Resources.Icon_GroundForces, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarCommanders, Resources.Icon_Commanders, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarMedals, Resources.Icon_Medals, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarRace, Resources.Icon_Race, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarSystem, Resources.Icon_System, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarGalactic, Resources.Icon_Galactic, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarHabitable, Resources.Icon_Galactic, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarComparison, Resources.Icon_Comparison, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarIntelligence, Resources.Icon_Intelligence, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarTechnology, Resources.Icon_Technology, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarSurvey, Resources.Icon_Survey, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarSector, Resources.Icon_Sector, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarEvents, Resources.Icon_Events, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarRefreshTactical, Resources.Icon_Refresh, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarRefreshGalactic, Resources.Icon_Refresh, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarSave, Resources.Icon_Save, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarGame, Resources.Icon_Game, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarGrid, Resources.Icon_Grid, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarUndo, Resources.Icon_Undo, mainTextColor);
            ChangeButtonImage(AuroraButton.ToolbarSavePositions, Resources.Icon_SavePositions, mainTextColor);
        }

        static private void ChangeButtonImage(AuroraButton button, Bitmap image, Color color)
        {
            ThemeCreator.ThemeCreator.AddImageChange(
                button,
                ColorizeImage(image, color)
            );
        }

        static private Bitmap ColorizeImage(Bitmap image, Color color)
        {
            var imageAttributes = new ImageAttributes();

            float r = color.R / 255f;
            float g = color.G / 255f;
            float b = color.B / 255f;

            float[][] colorMatrixElements = {
               new float[] {0, 0, 0, 0, 0},
               new float[] {0, 0, 0, 0, 0},
               new float[] {0, 0, 0, 0, 0},
               new float[] {0, 0, 0, 1, 0},
               new float[] {r, g, b, 0, 1}
            };

            var colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix);

            var colorizedImage = new Bitmap(image.Width, image.Height);
            var graphics = Graphics.FromImage(colorizedImage);
            var rect = new Rectangle(0, 0, image.Width, image.Height);

            graphics.DrawImage(image, rect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);

            return colorizedImage;
        }
    }
}
