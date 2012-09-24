using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
    //[TestClass]
    //public class testLocalSave
    //{
    //    public LocalSavePlugin testPlugin;
    //    public MainWindow TestMainWindow;
    //    public String testLocation;

    //    [TestInitialize]
    //    public void InitPlugin()
    //    {
    //        testPlugin = new LocalSavePlugin();
    //        TestMainWindow = new MainWindow();
    //        testLocation = "C:\\testthis.jpg";
    //    }

    //    [TestMethod]
    //    public void TestUpload()
    //    {
    //        TestMainWindow.TestScreenShot();
    //        testPlugin.setSaveLocation(testLocation);
    //        testPlugin.upload(TestMainWindow.EditScreen.OriginalScreenshot);
    //        Assert.IsFalse(String.IsNullOrEmpty(testPlugin.Link));
    //        File.Delete(testLocation);
    //    }

    //    [TestMethod]
    //    [ExpectedException(typeof(ArgumentNullException),"Could not save to location")]
    //    public void NullTestUpload()
    //    {
    //        TestMainWindow.TestScreenShot();
    //        //testPlugin.setSaveLocation(testLocation);
    //        testPlugin.upload(TestMainWindow.EditScreen.OriginalScreenshot);
    //        Assert.IsFalse(String.IsNullOrEmpty(testPlugin.Link));
    //        File.Delete(testLocation);
    //    }

    //    [TestMethod]
    //    public void TestOnInit()
    //    {
    //        Assert.AreEqual(String.Empty, testPlugin.Link);
    //    }
    //}
}
