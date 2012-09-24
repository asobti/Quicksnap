using bamsak;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Media.Imaging;

namespace testBamsak
{
	
	
	/// <summary>
	///This is a test class for ReflectionTest and is intended
	///to contain all ReflectionTest Unit Tests
	///</summary>
	[TestClass()]
	public class ReflectionTest
	{


		private TestContext testContextInstance;
		public String testDLL = String.Empty;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}


		/// <summary>
		///A test for AnthonyTest
		///</summary>
		/*[TestMethod()]
		public void AnthonyTestTest()
		{
			Reflection target = new Reflection(); // TODO: Initialize to an appropriate value
			string expected = string.Empty; // TODO: Initialize to an appropriate value
			string actual;
			actual = target.AnthonyTest();
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for getBitmap
		///</summary>
		[TestMethod()]
		public void getBitmapTest()
		{
			Reflection target = new Reflection(); // TODO: Initialize to an appropriate value
			CroppedBitmap expected = null; // TODO: Initialize to an appropriate value
			CroppedBitmap actual;
			actual = target.getBitmap();
			Assert.AreEqual(expected, actual);
			//Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for getUrl
		///</summary>
		[TestMethod()]
		public void getUrlTest()
		{
			Reflection target = new Reflection(); // TODO: Initialize to an appropriate value
			string expected = string.Empty; // TODO: Initialize to an appropriate value
			string actual;
			actual = target.getUrl();
			Assert.AreEqual(expected, actual);
			//Assert.Inconclusive("Verify the correctness of this test method.");
		}

		/// <summary>
		///A test for loadDLL
		///</summary>
		[TestMethod()]
		public void loadDLLTest()
		{
			Reflection target = new Reflection();
			string dll =  testDLL;
			target.loadDLL(dll);
			Assert.IsNotNull(target.instance);
			//Assert.Inconclusive("A method that does not return a value cannot be verified.");
		}

		/// <summary>
		///A test for setBitmap
		///</summary>
		[TestMethod()]
		public void setBitmapTest()
		{
			Reflection target = new Reflection(); // TODO: Initialize to an appropriate value
			CroppedBitmap bitmap = null; // TODO: Initialize to an appropriate value
			target.setBitmap(bitmap);
			//Assert.Inconclusive("A method that does not return a value cannot be verified.");
		}

		/// <summary>
		///A test for upload
		///</summary>
		[TestMethod()]
		public void uploadTest()
		{
			Reflection target = new Reflection(); // TODO: Initialize to an appropriate value
			CroppedBitmap image = null; // TODO: Initialize to an appropriate value
			target.upload(image);
			//Assert.Inconclusive("A method that does not return a value cannot be verified.");
		}*/
	}
}
