using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
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
	public class testUploadResult
	{
		public UploadResult testTheUploadResult;
		public MainWindow testTheMainWindow;
		public String TestDlls = "C:\\Users\\Kent\Documents\\My Dropbox\\BAMSAK\\bamsak\\bin\\Debug\\Plugins";

		[TestInitialize]
		public void InitUpload()
		{
			testTheMainWindow = new MainWindow();
			testTheMainWindow.TestScreenShot();
			testTheUploadResult = new UploadResult(testTheMainWindow.EditScreen.OriginalScreenshot);
		}

		[TestMethod]
		public void TestInitUpload()
		{
			Assert.AreEqual(testTheUploadResult.Image, testTheMainWindow.EditScreen.OriginalScreenshot);
			Assert.IsFalse(testTheUploadResult.Worker.IsBusy);
		}

		/*[TestMethod]
		public void TestUploadImgur()
		{
			testTheUploadResult.TestImgurUpload();
			Assert.IsFalse(String.IsNullOrEmpty(testTheUploadResult.lastLink));
		}*/

	}
}
