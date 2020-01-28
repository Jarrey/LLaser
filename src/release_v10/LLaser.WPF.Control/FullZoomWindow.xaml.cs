//*********************************************************
// LLaser.WPF project - FullZoomWindow.xaml.cs
// Created at 2013-6-16
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code.
//
//*********************************************************

using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LLaser.WPF.Controls
{
    using System.Windows.Media;

    using LLaser.Common;

    /// <summary>
    /// Interaction logic for FullZoomWindow.xaml
    /// </summary>
    public partial class FullZoomWindow : Window
    {
        public FullZoomWindow()
        {
            Info = new ImageInfo();
            InitializeComponent();
        }

        #region dependency properties

        public BitmapSource Bitmap
        {
            get { return (BitmapSource)GetValue(BitmapProperty); }
            set { SetValue(BitmapProperty, value); }
        }
        public static readonly DependencyProperty BitmapProperty =
            DependencyProperty.Register("Bitmap", typeof(BitmapSource), typeof(FullZoomWindow), new PropertyMetadata(null));

        #endregion

        #region perproties
        public ImageInfo Info { get; set; }
        #endregion

        #region event hanlders

        private void btnClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (Bitmap != null)
                CommonFunctions.SaveImage(Bitmap, "BMP (*bmp)|*.bmp");
        }

        private void CommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OnCopyImage(object sender, ExecutedRoutedEventArgs e)
        {
            if (Bitmap != null)
                CommonFunctions.CopyImage(Bitmap);
        }

        private void FullZoomWindow_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            ctlZoomBox.Scale += ctlZoomBox.Scale > 2 ? 0.5 : 0.1;
        }

        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            ctlZoomBox.Scale -= ctlZoomBox.Scale > 2 ? 0.5 : 0.1;
        }
        #endregion

        private void Image_OnMouseMove(object sender, MouseEventArgs e)
        {
            Info.Position = e.GetPosition(imgBitmap);
            var bitmap = Bitmap as WriteableBitmap;
            if (bitmap != null)
            {
                Info.PointColor = bitmap.GetPixel((int)Info.Position.X, (int)Info.Position.Y);
            }
        }

        #region nested class

        public class ImageInfo : BindableBase
        {
            private Point _position;
            public Point Position
            {
                get { return _position; }
                set { SetProperty(ref _position, value); }
            }

            private Color _pointColor;
            public Color PointColor
            {
                get { return _pointColor; }
                set { SetProperty(ref _pointColor, value); }
            }
        }

        #endregion
    }
}
