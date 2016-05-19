using System;
using System.IO;
using System.Linq;
using System.Windows;
using WindowsDesktop;
using System.Runtime.InteropServices;

namespace VDWallpaperSwitcher
{
    public partial class MainWindow : Window
    {
        private const int SPI_SETDESKWALLPAPER = 20,
                          SPIF_UPDATEINIFILE = 0x01,
                          SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public MainWindow()
        {
            VirtualDesktop.CurrentChanged += (sender, e) =>
            {
                var desktops = VirtualDesktop.GetDesktops();
                var index = Array.IndexOf(desktops, desktops.First(i => i.Id == VirtualDesktop.Current.Id));

                if (index > -1)
                {
                    var files = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\img", "*",
                        SearchOption.AllDirectories);
                    SystemParametersInfo(SPI_SETDESKWALLPAPER, 0,
                        files[index % files.Length], SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
                }
            };

            this.InitializeComponent();
        }
    }
}
