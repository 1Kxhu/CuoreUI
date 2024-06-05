using Svg;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CuoreUI.Controls
{
    public partial class cuiSvgBox : Control
    {
        public cuiSvgBox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
        }

        string GetSVGLocation()
        {
            string current = Environment.CurrentDirectory;
            string foldername = "\\cuore";
            string combinedFolders = current + foldername;
            if (Directory.Exists(combinedFolders) == false)
            {
                Directory.CreateDirectory(combinedFolders);
            }

            string filename = "\\" + Name + "_cuore.svg";
            string combinedFile = combinedFolders + filename;

            return combinedFile;
        }

        private string[] privateSvgCode = new string[] { "" };
        public string[] SvgCode
        {
            get
            {
                return privateSvgCode;
            }
            set
            {
                privateSvgCode = value;
                if (OverrideColor == Color.Empty)
                {
                    SaveSVG(false);
                }
                else
                {
                    SaveSVG(true);
                }
            }
        }

        public string[] RunningSvgCode
        {
            get;
            private set;
        } = new string[] { "" };

        private async void SaveSVG(bool alternative)
        {
            string path = GetSVGLocation();
            string[] tempSVGCODE = SvgCode;

            if (alternative && OverrideColor != Color.Empty)
            {
                string yourColor = ColorToHexString(OverrideColor);

                string pattern1 = @"stroke=""([^""]+)""";
                string pattern2 = @"fill=""([^""]+)""";


                for (int i = 0; i < tempSVGCODE.Length; i++)
                {
                    tempSVGCODE[i] = Regex.Replace(tempSVGCODE[i], pattern1, $"stroke=\"{yourColor}\"");
                    tempSVGCODE[i] = Regex.Replace(tempSVGCODE[i], pattern2, $"fill=\"{yourColor}\"");
                }
            }

            RunningSvgCode = tempSVGCODE;

            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                foreach (var line in tempSVGCODE)
                {
                    await writer.WriteLineAsync(line);
                }
            }

            Invalidate();
        }

        string ColorToHexString(Color color)
        {
            string r = color.R.ToString("x2");
            string g = color.G.ToString("x2");
            string b = color.B.ToString("x2");

            return "#" + r + g + b;
        }

        private Color privateOverrideColor = Color.Empty;
        public Color OverrideColor
        {
            get
            {
                return privateOverrideColor;
            }
            set
            {
                privateOverrideColor = value;
                SaveSVG(true);
            }

        }

        Bitmap bitmap;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (SvgCode.Length > 0)
            {
                string result = string.Join("\n", SvgCode);
                var byteArray = Encoding.ASCII.GetBytes(result.ToCharArray());

                using (var stream = new MemoryStream(byteArray))
                {
                    SvgDocument svgDocument = SvgDocument.Open(GetSVGLocation());

                    svgDocument.Width = ClientRectangle.Width;
                    svgDocument.Height = ClientRectangle.Height;

                    bitmap = svgDocument.Draw();
                    e.Graphics.DrawImage(bitmap, Point.Empty);
                }
            }
        }
    }
}
