using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace VCMDL.NET
{
    static class Program
    {
        public static ModelEditor Form_ModelEditor;
        public static SkinEditor Form_SkinEditor;
        public static CommonToolbox Ctrl_CommonToolbox;
        public static bool ClosingFinal = false;

        public static class Resources
        {
			static public Cursor CursorPan = SetupCursor(Properties.Resources.ArrowPan);
			static public Cursor CursorSelect2 = SetupCursor2(Properties.Resources.ArrowSelect);
			static public Cursor CursorRotate = SetupCursor2(Properties.Resources.ArrowRotate);
			static public Cursor CursorScale = SetupCursor2(Properties.Resources.ArrowScale);
			static public Cursor CursorMove = SetupCursor2(Properties.Resources.ArrowMove);
			static public Cursor CursorBuildVert = SetupCursor2(Properties.Resources.ArrowVert);
			static public Cursor CursorBuildFace = SetupCursor2(Properties.Resources.ArrowTri);
			static public Cursor CursorSelect = SetupCursor2(Properties.Resources.ArrowSelect);
			static public Cursor CursorDefault = Cursors.Default;

            public struct IconInfo
            {
                public bool fIcon;
                public int xHotspot;
                public int yHotspot;
                public IntPtr hbmMask;
                public IntPtr hbmColor;
            }

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);
            [DllImport("user32.dll")]
            public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

            public static Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot)
            {
                IntPtr ptr = bmp.GetHicon();
                IconInfo tmp = new IconInfo();
                GetIconInfo(ptr, ref tmp);
                tmp.xHotspot = xHotSpot;
                tmp.yHotspot = yHotSpot;
                tmp.fIcon = false;
                ptr = CreateIconIndirect(ref tmp);
                return new Cursor(ptr);
            }

            static Cursor SetupCursor(Bitmap img)
            {
                return CreateCursor(img, 0, 0);
            }

			static Cursor SetupCursor2(Bitmap AddBmp)
			{
				Bitmap newB = new Bitmap(AddBmp);
				using (Graphics g = Graphics.FromImage(newB))
					Cursors.Arrow.DrawStretched(g, new Rectangle(new Point(-Cursors.Arrow.HotSpot.X, -Cursors.Arrow.HotSpot.Y), Cursors.Arrow.Size));

				return CreateCursor(newB, 0, 0);
			}
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run((Form_ModelEditor = new ModelEditor()));
        }

        public static void InitForms()
        {
            Form_SkinEditor = new SkinEditor();
            Ctrl_CommonToolbox = new CommonToolbox();
        }
    }
}
