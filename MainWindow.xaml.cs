using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace bamsak
{   
    public partial class MainWindow : Window
    {
        private static Bitmap bmpScreenshot;
        private static Graphics gfxScreenshot;
        private string DefaultDirectory = String.Empty;
        private string SaveLocation = String.Empty;
        private int numSuffix;
        public Edit EditScreen;

        public MainWindow()
        {
            InitializeComponent();
            getSettings();
        }

        // loads in settings from the saved config file
        private void getSettings()
        {
            // for now, the settings are just hard coded until we get around to 
            // coding the setttings class
            this.DefaultDirectory = "C:";
            this.numSuffix = 0;
        }

        private void btnScreenshot_Click(object sender, RoutedEventArgs e)
        {
            takeScreenshot();
        }

        /*
         * Grabs a screenshot of the user's desktop
         */
        private void takeScreenshot()
        {
            try
            {
                // hide the current window 
                this.Hide();
                this.SetSaveLocation();
                // give the window some time to sleep
                Thread.Sleep(200);
                bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                bmpScreenshot.Save(SaveLocation, ImageFormat.Png);
                numSuffix++;

                // open image editing window
                EditScreen = new Edit(bmpScreenshot);
                EditScreen.Show();
                this.Close();

            }
            catch 
            {
                // log the error somewhere
                // and inform the user about it
            }            
        }

        private void SetSaveLocation()
        {
            System.IO.Directory.CreateDirectory(DefaultDirectory + "\\Screenshots");
            SaveLocation = DefaultDirectory + "\\Screenshots\\Screenshot_" + numSuffix;
            SaveLocation += ".png";
        }

//Test Stuff
//-=-=-=-=-=

        public Bitmap screenShot
        {
            get
            {
                return bmpScreenshot;
            }
        }

        public String saveLocation
        {
            get
            {
                return SaveLocation;
            }
        }

        public String defaultDirectory
        {
            get
            {
                return DefaultDirectory;
            }
        }

        public int Suffix
        {
            get
            {
                return numSuffix;
            }
        }

//These functions are public sololy for testing
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-==-=

        public void TestScreenShot()
        {
            takeScreenshot();
        }

        public void TestSetSave()
        {
            SetSaveLocation();
        }

    }
}
