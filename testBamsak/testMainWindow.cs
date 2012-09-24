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
    public class testMainWindow
    {
        public MainWindow testTheMainWindow;

        [TestInitialize]
        public void InitTestWindow()
        {
            testTheMainWindow = new MainWindow();
        }

        [TestCleanup]
        public void RemoveTestWindow()
        {
            testTheMainWindow = null;
        }

        [TestMethod]
        public void testScreenShot()
        {
            testTheMainWindow.TestScreenShot();
            Assert.IsNotNull(testTheMainWindow.screenShot);
            Assert.IsFalse(testTheMainWindow.IsActive);
            Assert.AreEqual((int)testTheMainWindow.EditScreen.bitmapImage.Height, testTheMainWindow.screenShot.Height);
            Assert.AreEqual((int)testTheMainWindow.EditScreen.bitmapImage.Width, testTheMainWindow.screenShot.Width);
        }

        [TestMethod]
        public void testSetSaveLocation()
        {
            testTheMainWindow.TestSetSave();
            Assert.AreNotEqual(String.Empty, testTheMainWindow.saveLocation);
            Assert.IsTrue(System.IO.Directory.Exists(testTheMainWindow.defaultDirectory + "\\Screenshots"));
        }

        [TestMethod]
        public void testInitWindow()
        {
            Assert.AreEqual(testTheMainWindow.Suffix, 0);
            Assert.AreEqual(testTheMainWindow.defaultDirectory, "C:");
        }
    }
}
