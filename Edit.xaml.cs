using System;
using System.Collections.Generic;
using System.IO;
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
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Drawing.Imaging;

namespace bamsak
{
    /// <summary>
    /// Interaction logic for Edit.xaml
    /// </summary>
    public partial class Edit : Window
    {
        private CroppedBitmap originalScreenshot;
        private CroppedBitmap bmpScreenshot;
        private Point Highlight_PointDown;
        private Point Highlight_PointUp;
        private Point Crop_PointDown;
        private Point Crop_PointUp;
        private Point AddText_PointDown;
        private Point AddText_PointUp;
        private Stack<UIElement> elements;
        private Stack<UIElement> redos;
        private Color highlightColor;
        private Color? textboxBackgroundColor;
        private double opacity;
        bool methodUnlocked = true;

        public BitmapImage bitmapImage;
        //private CroppedBitmap croppedImage;

        double topLeftX = 0;
        double topLeftY = 0;
        bool setRect = false;

        private double originalHeight;
        private double originalWidth;


        public Edit(System.Drawing.Bitmap bmp)
        {
            // Convert to bitmapimage            
            using (MemoryStream memory = new MemoryStream())
            {
                bmp.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }

            
            elements = new Stack<UIElement>();
            redos = new Stack<UIElement>();
            this.originalScreenshot = new CroppedBitmap(bitmapImage, new Int32Rect(0, 0, bitmapImage.PixelWidth, bitmapImage.PixelHeight));
            this.bmpScreenshot = originalScreenshot;
            InitializeComponent();
            imgViewer.Background = new ImageBrush(this.bmpScreenshot);
            
            textBox_Width.Text = bmpScreenshot.Width.ToString();
            textBox_Height.Text = bmpScreenshot.Height.ToString();
            WindowStyle = WindowStyle.SingleBorderWindow;
            //Topmost = true;
            WindowState = WindowState.Maximized;
            btn_Redo.IsEnabled = false;
            btn_Undo.IsEnabled = false;
            highlightColor = new Color();

            this.originalHeight = imgViewer.Height;
            this.originalWidth = imgViewer.Width;

            initialize();
        }

        private void Crop(Point mouseDown, Point mouseUp)
        {
            double mdx = mouseDown.X;
            double mdy = mouseDown.Y;
            double mux = mouseUp.X;
            double muy = mouseUp.Y;

            double leftx;
            double rightx;
            double topy;
            double bottomy;

            if (mdx < mux)
            {
                leftx = mdx;
                rightx = mux;
            }
            else
            {
                leftx = mux;
                rightx = mdx;
            }

            if (mdy < muy)
            {
                bottomy = muy;
                topy = mdy;
            }
            else
            {
                bottomy = mdy;
                topy = muy;
            }
            Point topleft = new Point(leftx, topy);
            Size size = new Size(Math.Abs(rightx - leftx), Math.Abs(topy - bottomy));

            ROI.Width = size.Width;
            ROI.Height = size.Height;
            //Canvas.SetLeft(ROI, topleft.X);
            //Canvas.SetTop(ROI, topleft.Y);
            setRect = false;


            // calculate the crop rectangle's bounds
            int x = (int)topleft.X;
            int y = (int)topleft.Y;
            int width = (int)size.Width;
            int height = (int)size.Height;
            Int32Rect cropRect = new Int32Rect(x, y, width, height);

            //crop
            this.bmpScreenshot = new CroppedBitmap(this.bmpScreenshot, cropRect);

            // move cropped image to it's previous location in the whole image
            //imgViewer.RenderTransform = new TranslateTransform(x, y);
                        
            imgViewer.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            imgViewer.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            
            imgViewer.Background = new ImageBrush(bmpScreenshot);
            imgViewer.Height = height; imgViewer.Width = width;


            // reset rectangle
            ROI.Height = 0;
            ROI.Width = 0;
        }       

        private void AddText(Point mouseDown, Point mouseUp)
        {
            double mdx = mouseDown.X;
            double mdy = mouseDown.Y;
            double mux = mouseUp.X;
            double muy = mouseUp.Y;

            double leftx;
            double rightx;
            double topy;
            double bottomy;

            if (mdx < mux)
            {
                leftx = mdx;
                rightx = mux;
            }
            else
            {
                leftx = mux;
                rightx = mdx;
            }

            if (mdy < muy)
            {
                bottomy = muy;
                topy = mdy;
            }
            else
            {
                bottomy = mdy;
                topy = muy;
            }

            Point topleft = new Point(leftx, topy);
            Size size = new Size(Math.Abs(rightx - leftx), Math.Abs(topy - bottomy));

            TextBox tb = new TextBox();
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Width = size.Width;
            tb.Height = size.Height;
            tb.BorderBrush = new DrawingBrush();

            if (textboxBackgroundColor == null)
            {
                tb.Background = null;
            }
            else
            {
                tb.Background = new SolidColorBrush((Color)textboxBackgroundColor);
            }

            double left = topleft.X;
            double right = 0;
            double top = topleft.Y;
            double bottom = 0;

            Console.WriteLine("Top: {0} Left: {1} ", top, left);

            tb.Margin = new Thickness(left, top, right, bottom);
            addElement(tb);
            tb.Focus();
        }

        private void Highlight(Point mouseDown, Point mouseUp)
        {
            double mdx = mouseDown.X;
            double mdy = mouseDown.Y;
            double mux = mouseUp.X;
            double muy = mouseUp.Y;

            double leftx;
            double rightx;
            double topy;
            double bottomy;

            if (mdx < mux)
            {
                leftx = mdx;
                rightx = mux;
            }
            else
            {
                leftx = mux;
                rightx = mdx;
            }

            if (mdy < muy)
            {
                bottomy = muy;
                topy = mdy;
            }
            else
            {
                bottomy = mdy;
                topy = muy;
            }


            Point topleft = new Point(leftx, topy);
            Size size = new Size(Math.Abs(rightx - leftx), Math.Abs(topy - bottomy));
            Rectangle rect = new Rectangle();
            InkCanvas.SetLeft(rect, topleft.X);
            InkCanvas.SetTop(rect, topleft.Y);
            rect.Width = size.Width;
            rect.Height = size.Height;
            rect.Fill = new SolidColorBrush() { Color = highlightColor, Opacity = opacity };
            addElement(rect);
        }

        private void addElement(UIElement element)
        {
            elements.Push(element);
            imgViewer.Children.Add(element);
            if (elements.Count > 0)
            {
                btn_Undo.IsEnabled = true;
            }
        }

        private void initialize()
        {
            combo_HighlightColor.Items.Clear();
            combo_HighlightColor.Items.Add(new ComboboxItem() { Text = "Yellow", Value = Color.FromRgb(255, 255, 0) });
            combo_HighlightColor.Items.Add(new ComboboxItem() { Text = "Cyan", Value = Color.FromRgb(0, 255, 255) });
            combo_HighlightColor.Items.Add(new ComboboxItem() { Text = "Green", Value = Color.FromRgb(0, 255, 0) });
            combo_HighlightColor.Items.Add(new ComboboxItem() { Text = "Pink", Value = Color.FromRgb(255, 20, 147) });
            combo_HighlightColor.SelectedIndex = 0;

            highlightColor = Colors.Yellow;

            combo_TextBoxBackground.Items.Clear();
            combo_TextBoxBackground.Items.Add(new ComboboxItem() { Text = "White", Value = Colors.White });
            combo_TextBoxBackground.Items.Add(new ComboboxItem() { Text = "Clear", Value = null });
            combo_TextBoxBackground.Items.Add(new ComboboxItem() { Text = "Yellow", Value = Colors.Yellow });
            combo_TextBoxBackground.Items.Add(new ComboboxItem() { Text = "Red", Value = Colors.Red });
            combo_TextBoxBackground.Items.Add(new ComboboxItem() { Text = "Blue", Value = Colors.Blue });
            combo_TextBoxBackground.Items.Add(new ComboboxItem() { Text = "Green", Value = Colors.Green });
            combo_TextBoxBackground.SelectedIndex = 0;

            textboxBackgroundColor = Colors.White;

            opacity = .40;
            opacitySlider.Value = opacity;
        }

        private void btn_Highlight_Click(object sender, RoutedEventArgs e)
        {
            if (methodUnlocked)
            {
                Console.WriteLine("Highlight");
                imgViewer.MouseDown += Highlight_MouseDownEvent;
                imgViewer.MouseUp += Highlight_MouseUpEvent;
                methodUnlocked = false;
            }
        }
        private CroppedBitmap flattenCanvas()
        {
            CroppedBitmap image = null;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            RenderTargetBitmap renderer = new RenderTargetBitmap((int)imgViewer.ActualWidth, (int)imgViewer.ActualHeight, 96d, 96d, PixelFormats.Default);
            renderer.Render(imgViewer);
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderer));
            encoder.Save(ms);

            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = ms;
            img.EndInit();

            image = new CroppedBitmap(img, new Int32Rect(0, 0, img.PixelWidth, img.PixelHeight));

            return image;
        }

        private void Highlight_MouseDownEvent(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Mouse Down Event [" + e.GetPosition(imgViewer) + "]");
            Highlight_PointDown = e.GetPosition(imgViewer);
            imgViewer.MouseDown -= Highlight_MouseDownEvent;
            imgViewer.MouseMove += Highlight_MouseMoveEvent;
            StartROI(Highlight_PointDown.X, Highlight_PointDown.Y);
        }

        private void Highlight_MouseMoveEvent(object sender, MouseEventArgs e)
        {
            System.Windows.Point pt = e.MouseDevice.GetPosition(sender as Canvas);
            UpdateROI(System.Math.Abs((int)(pt.X - Highlight_PointDown.X)), System.Math.Abs((int)(pt.Y - Highlight_PointDown.Y)));
        }

        private void Highlight_MouseUpEvent(object sender, MouseEventArgs e)
        {
            imgViewer.MouseMove -= Highlight_MouseMoveEvent;
            EndROI();
            Console.WriteLine("Mouse Up Event [" + e.GetPosition(imgViewer) + "]");
            Highlight_PointUp = e.GetPosition(imgViewer);
            imgViewer.MouseUp -= Highlight_MouseUpEvent;
            Highlight(Highlight_PointDown, Highlight_PointUp);
            methodUnlocked = true;
        }


        private void AddText_MouseDownEvent(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Mouse Down Event [" + e.GetPosition(imgViewer) + "]");
            AddText_PointDown = e.GetPosition(imgViewer);
            imgViewer.MouseDown -= AddText_MouseDownEvent;
            imgViewer.MouseMove += AddText_MouseMoveEvent;
            StartROI(AddText_PointDown.X, AddText_PointDown.Y);
        }

        private void AddText_MouseMoveEvent(object sender, MouseEventArgs e)
        {
            System.Windows.Point pt = e.MouseDevice.GetPosition(sender as Canvas);
            UpdateROI(System.Math.Abs((int)(pt.X - AddText_PointDown.X)), System.Math.Abs((int)(pt.Y - AddText_PointDown.Y)));
        }

        private void AddText_MouseUpEvent(object sender, MouseEventArgs e)
        {
            imgViewer.MouseMove -= AddText_MouseMoveEvent;
            EndROI();
            Console.WriteLine("Mouse Up Event [" + e.GetPosition(imgViewer) + "]");
            AddText_PointUp = e.GetPosition(imgViewer);
            imgViewer.MouseUp -= AddText_MouseUpEvent;
            AddText(AddText_PointDown, AddText_PointUp);
            methodUnlocked = true;
        }

        private void btn_Undo_Click(object sender, RoutedEventArgs e)
        {
            UIElement element = elements.Pop();
            imgViewer.Children.Remove(element);
            redos.Push(element);
            if (elements.Count == 0)
            {
                btn_Undo.IsEnabled = false;
            }
            btn_Redo.IsEnabled = true;
        }

        private void btn_Redo_Click(object sender, RoutedEventArgs e)
        {
            UIElement element = redos.Pop();
            imgViewer.Children.Add(element);
            elements.Push(element);
            if (redos.Count == 0)
            {
                btn_Redo.IsEnabled = false;
            }
            btn_Undo.IsEnabled = true;
        }

        private void btn_AddText_Click(object sender, RoutedEventArgs e)
        {
            if (methodUnlocked)
            {
                Console.WriteLine("AddText");
                imgViewer.MouseDown += AddText_MouseDownEvent;
                imgViewer.MouseUp += AddText_MouseUpEvent;
                methodUnlocked = false;
            }
        }

        private void combo_HighlightColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            highlightColor = (Color)(((bamsak.ComboboxItem)combo_HighlightColor.SelectedItem).Value);
        }

        private void opacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            opacity = e.NewValue;
            opacityLabel.Content = opacity * 100 + "%";
            Console.WriteLine("New Opacity {0}", opacity);
        }


        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            elements.Clear();
            redos.Clear();
            imgViewer.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            imgViewer.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            imgViewer.Width = originalWidth;
            imgViewer.Height = originalHeight;
            
            imgViewer.Children.Clear();
            btn_Redo.IsEnabled = false;
            btn_Undo.IsEnabled = false;
            imgViewer.Background = new ImageBrush(this.originalScreenshot);
            btn_Crop.IsEnabled = true;
        }

        private void btn_Done_Click(object sender, RoutedEventArgs e)
        {
            CroppedBitmap image = flattenCanvas();
            UploadResult upload = new UploadResult(image);

            this.Hide();
            upload.Show();
        }

        private void combo_TextBoxBackground_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bamsak.ComboboxItem selectedItem = (bamsak.ComboboxItem)combo_TextBoxBackground.SelectedItem;
            if (selectedItem.Value != null)
            {
                textboxBackgroundColor = (Color)(selectedItem.Value);
            }
            else
            {
                textboxBackgroundColor = null;
            }
        }

        private void btn_Resize_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            // exit the app
            Application.Current.Shutdown();
        }

        private void uploadBtn_Click(object sender, RoutedEventArgs e)
        {
            UploadResult uploadResult = new UploadResult(this.bmpScreenshot);
            this.Hide();
            uploadResult.Show();
        }

        private void btn_Crop_Click(object sender, RoutedEventArgs e)
        {
            if (methodUnlocked)
            {
                Console.WriteLine("Crop");
                imgViewer.MouseDown += Crop_MouseDownEvent;
                imgViewer.MouseUp += Crop_MouseUpEvent;
                methodUnlocked = false;
            }
        }
        private void Crop_MouseDownEvent(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Mouse Down Event [" + e.GetPosition(imgViewer) + "]");
            Crop_PointDown = e.GetPosition(imgViewer);
            imgViewer.MouseDown -= Crop_MouseDownEvent;
            imgViewer.MouseMove += canvas_MouseMove;
            topLeftY = topLeftX = 0;
            setRect = true;
            topLeftX = Crop_PointDown.X; topLeftY = Crop_PointDown.Y;
            StartROI(Crop_PointDown.X, Crop_PointDown.Y);
        }

        private void Crop_MouseUpEvent(object sender, MouseEventArgs e)
        {            
            EndROI();
            Console.WriteLine("Mouse Up Event [" + e.GetPosition(imgViewer) + "]");
            Crop_PointUp = e.GetPosition(imgViewer);
            imgViewer.MouseUp -= Crop_MouseUpEvent;
            Crop(Crop_PointDown, Crop_PointUp);          
            
            methodUnlocked = true;
            imgViewer.MouseMove -= canvas_MouseMove;            
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (setRect == true)
            {
                //get mouse location relative to the canvas
                System.Windows.Point pt = e.MouseDevice.GetPosition(sender as Canvas);
                UpdateROI(System.Math.Abs((int)(pt.X - topLeftX)), System.Math.Abs((int)(pt.Y - topLeftY)));                
            }
        }

        //Test Stuff

        public CroppedBitmap OriginalScreenshot
        {
            get
            {
                return originalScreenshot;
            }
        }

        private void StartROI(double x, double y)
        {
            ROI.Margin = new Thickness(x, y, 0, 0);
            ROI.Width = 0d;
            ROI.Height = 0d;
        }

        private void UpdateROI(double width, double height)
        {
            ROI.Width = width;
            ROI.Height = height;
        }

        private void GetROI(out double top, out double left, out double height, out double width)
        {
            top = ROI.Margin.Top;
            left = ROI.Margin.Left;
            height = ROI.Height;
            width = ROI.Width;
        }

        private void EndROI()
        {
            ROI.Margin = new Thickness(0, 0, 0, 0);
            ROI.Height = 0d;
            ROI.Width = 0d;
        }        
    }
        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
    }
