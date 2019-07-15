using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace Wpf
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BitmapSource _iconImage;
        public BitmapSource IconImage
        {
            get
            {
                return _iconImage;
            }
            set
            {
                _iconImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IconImage)));
            }
        }

        public ICommand SelectCommand { get; }

        public MainWindowViewModel()
        {
            SelectCommand = new Command(() =>
            {
                var dialog = new OpenFileDialog()
                {
                    AddExtension = true,
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Multiselect = false,
                };

                var selected = dialog.ShowDialog() ?? false;
                if (!selected) return;

                var fileName = dialog.FileName;
                using (var icon = Icon.ExtractAssociatedIcon(fileName))
                using (var bmpIcon = icon.ToBitmap())
                {
                    IconImage = BitmapToBitmapSourceUnManaged(bmpIcon);
                }
            });
        }

        private BitmapSource BitmapToBitmapSourceManaged(Bitmap bmp)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                return BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
        }

        private BitmapSource BitmapToBitmapSourceUnManaged(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    handle, IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(handle);
            }
        }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteObject([In] IntPtr hObject);
    }
}
