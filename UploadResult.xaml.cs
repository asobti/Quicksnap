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
using System.Windows.Shapes;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Configuration;


namespace bamsak
{
	/// <summary>
	/// Interaction logic for UploadResult.xaml
	/// </summary>
	public partial class UploadResult : Window
	{
		private CroppedBitmap image;
		private readonly BackgroundWorker worker;
		public String lastLink;
		Dictionary<String, Reflection> dlls;


		public UploadResult(CroppedBitmap image)
		{
			this.image = image;
			lastLink = String.Empty;
			//Topmost = true;
			InitializeComponent();

			String dir = ConfigurationManager.AppSettings["dllsDirectory"];
			List<String> files = getFilesFromDirectory(dir);
			loadDLLs(files);

			List<Button> buttons = new List<Button>() { btn1, btn2 };

			int i = 0;
			foreach (KeyValuePair<String, Reflection> kvp in dlls)
			{
				buttons[i].Content = kvp.Key;
				i++;
			}


			while (i < 2)
			{
				buttons[i].Visibility = Visibility.Hidden;
				i++;
			}

			worker = new BackgroundWorker();
			worker.DoWork += worker_DoWork;
			worker.RunWorkerCompleted += worker_RunWorkerCompleted;

			linkTxt.IsReadOnly = true;
			actionProgress.Visibility = System.Windows.Visibility.Hidden;
		}


		public List<String> getFilesFromDirectory(String directory)
		{
			List<String> files = new List<String>();
			if (directory == null)
				throw new ArgumentNullException("Configuration file did not specify plugins directory");
			string[] files1 = Directory.GetFiles(directory);
			foreach (string file in files1)
			{
				files.Add(file);
			}
			return files;
		}

		public void loadDLLs(List<String> files)
		{
			if (dlls == null)
				dlls = new Dictionary<String, Reflection>();
			foreach (String file in files)
			{
				Reflection r = new Reflection();
				r.loadDLL(file);
				dlls.Add(r.getServiceName(), r);
			}
		}

		private void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			// retrieve arguments
			UploadArgs args = (UploadArgs)e.Argument;
			String link = String.Empty;

			Reflection plugin = new Reflection();
			plugin.loadDLL(args.dll);
			if (args.location != null)
			{
				plugin.setSaveLocation(args.location);
			}


			if (plugin != null)
			{
				plugin.upload(image);
				link = plugin.getUrl();                 
			}

			lastLink = link;
			e.Result = link;
		}

		private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			linkTxt.Text = e.Result.ToString();
			actionProgress.Visibility = System.Windows.Visibility.Hidden;
			btnClipboard.IsEnabled = true;
		}

		private void btn1_Click(object sender, RoutedEventArgs e)
		{
			int i = 0;
			UploadArgs args = new UploadArgs();
			args.dll = this.dlls.Values.ElementAt(i);
			if (this.dlls.Values.ElementAt(i).isLocal())
			{
				Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
				dialog.Filter = "Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
				dialog.Title = "Choose a location to save your screenshot";
				dialog.ShowDialog();
				if (!String.IsNullOrEmpty(dialog.FileName))
				{
					args.location = dialog.FileName;
				}
			}
			prepareForUpload(args);
		}

		private void btn2_Click(object sender, RoutedEventArgs e)
		{
			int i = 1;
			UploadArgs args = new UploadArgs();
			args.dll = this.dlls.Values.ElementAt(i);
			if (this.dlls.Values.ElementAt(i).isLocal())
			{
				Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
				dialog.Filter = "Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
				dialog.Title = "Choose a location to save your screenshot";
				dialog.ShowDialog();
				if (!String.IsNullOrEmpty(dialog.FileName))
				{
					args.location = dialog.FileName;
				}
			}
			prepareForUpload(args);
		}

		private void btn3_Click(object sender, RoutedEventArgs e)
		{
			int i = 2;
			UploadArgs args = new UploadArgs();
			args.dll = this.dlls.Values.ElementAt(i);
			if (this.dlls.Values.ElementAt(i).isLocal())
			{
				Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
				dialog.Filter = "Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
				dialog.Title = "Choose a location to save your screenshot";
				dialog.ShowDialog();
				if (!String.IsNullOrEmpty(dialog.FileName))
				{
					args.location = dialog.FileName;
				}
			}
			prepareForUpload(args);
		}

		private void prepareForUpload(UploadArgs args)
		{
			// freeze the image so the background thread can access it
			image.Freeze();
			
			// show progress bar
			actionProgress.Visibility = System.Windows.Visibility.Visible;
			worker.RunWorkerAsync(args);            
		}    
   
//Test Stuff
//-=-=-=-=-=

		public CroppedBitmap Image
		{
			get
			{
				return image;
			}
		}

		public BackgroundWorker Worker
		{
			get
			{
				return worker;
			}
		}

		private void btnClipboard_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(this.lastLink))
			{
				Clipboard.SetText(this.lastLink);
			}
		}

	}   
}
