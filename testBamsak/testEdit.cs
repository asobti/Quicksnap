using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using bamsak;

namespace testBamsak
{
    [TestClass]
    public class testEdit
    {
        public Edit testTheEdit;
        public Bitmap testBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        [TestInitialize]
        public void InitEdit()
        {
            testTheEdit = new Edit(testBitmap);
        }

        [TestMethod]
        public void TestInitEdit()
        {
            Assert.IsNotNull(testTheEdit.OriginalScreenshot);

        }
    }
}
