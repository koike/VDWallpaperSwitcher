using System;
using System.IO;
using System.Linq;
using System.Windows;
using WindowsDesktop;
using System.Runtime.InteropServices;

namespace VDWallpaperSwitcher
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public MainWindow()
        {
            VirtualDesktop.CurrentChanged += this.CurrentChanged;
            this.InitializeComponent();
        }

        private void CurrentChanged(object sender, VirtualDesktopChangedEventArgs e)
        {
            var index = Array.IndexOf(VirtualDesktop.GetDesktops(),
                VirtualDesktop.GetDesktops().First(i => i.Id == VirtualDesktop.Current.Id));

            if (index > -1)
            {
                var files = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\img", "*", SearchOption.AllDirectories);
                var path = files[index % files.Length];

                if (File.Exists(path))
                {
                    SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
                }
            }
        }
    }
}
