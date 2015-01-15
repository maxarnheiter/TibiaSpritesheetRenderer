using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace TibiaSpritesheetRenderer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void OnLoadSpritesButtonClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Tibia.spr";
            dialog.DefaultExt = ".spr";
            bool? result = dialog.ShowDialog();

            if(result == true)
            {
                System.Drawing.Image image = LoadSprites(dialog.FileName);
            }

        }

        private System.Drawing.Image LoadSprites(string file)
        {
            int spriteId = 2;   //System.IO.EndOfStreamException

            int size = 32;
            Bitmap bitmap = new Bitmap(size, size);

            using (BinaryReader reader = new BinaryReader(File.OpenRead(file)))
            {
                Debug.WriteLine(reader);

                ushort currentPixel = 0;
                long targetOffset;

                reader.BaseStream.Seek(6 + (spriteId - 1) * 4, SeekOrigin.Begin);
                reader.BaseStream.Seek(reader.ReadUInt32() + 3, SeekOrigin.Begin);

                targetOffset = reader.BaseStream.Position + reader.ReadUInt16();

                Debug.WriteLine("here");

                while (reader.BaseStream.Position < targetOffset)
                {
                    ushort transparentPixels = reader.ReadUInt16();
                    ushort coloredPixels = reader.ReadUInt16();
                    currentPixel += transparentPixels;
                    for (int i = 0; i < coloredPixels; i++)
                    {
                        bitmap.SetPixel(
                            currentPixel % size,
                            currentPixel / size,
                            System.Drawing.Color.FromArgb(reader.ReadByte(), reader.ReadByte(), reader.ReadByte())
                        );
                        currentPixel++;
                    }
                }
            }

            return bitmap;
        }

    }
}
