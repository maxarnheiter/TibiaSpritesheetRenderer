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
using System.Threading;

namespace TibiaSpritesheetRenderer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int index = 2;

        string spritePath = "";
        string dataPath = "";

        public MainWindow()
        {
            InitializeComponent();

        }

        private void load_sprite_button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Tibia.spr";
            dialog.DefaultExt = ".spr";
            bool? result = dialog.ShowDialog();

            if(result == true)
            {
                spritePath = dialog.FileName;
            }
        }

        private void load_data_button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Tibia.dat";
            dialog.DefaultExt = ".dat";
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                dataPath = dialog.FileName;
                LoadData();
            }
        }

        private void SetImage()
        {
            System.Drawing.Image image = LoadSprites(spritePath, index);
            Bitmap bmp = image as Bitmap;

            MemoryStream memory = new MemoryStream();

            bmp.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
            memory.Position = 0;
            BitmapImage bitmapimage = new BitmapImage();
            bitmapimage.BeginInit();
            bitmapimage.StreamSource = memory;
            bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapimage.EndInit();

            test_image.Source = bitmapimage;
            base_window.UpdateLayout();
        }

        private System.Drawing.Image LoadSprites(string file, int spriteId)
        {

            int size = 32;
            Bitmap bitmap = new Bitmap(size, size);

            using (BinaryReader reader = new BinaryReader(File.OpenRead(file)))
            {


                ushort currentPixel = 0;
                long targetOffset;

                reader.BaseStream.Seek(6 + (spriteId - 1) * 4, SeekOrigin.Begin);
                reader.BaseStream.Seek(reader.ReadUInt32() + 3, SeekOrigin.Begin);

                targetOffset = reader.BaseStream.Position + reader.ReadUInt16();


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
                Debug.WriteLine("made it here");
            }

            Debug.WriteLine("finished");
            
            return bitmap;
        }

        private void LoadData()
        {
            if (dataPath != null)
            {
                FileStream fileStream = new FileStream(dataPath, FileMode.Open);
                BinaryReader reader = new BinaryReader(fileStream);

                UInt32 datSignature = reader.ReadUInt32();

                UInt16 itemCount = reader.ReadUInt16();
                UInt16 creatureCount = reader.ReadUInt16();
                UInt16 effectCount = reader.ReadUInt16();
                UInt16 distanceCount = reader.ReadUInt16();

                UInt16 minclientID = 100; //items starts at 100
                UInt16 maxclientID = itemCount;

                UInt16 id = minclientID;
                while (id <= maxclientID)
                {
                    Debug.WriteLine("ID: " + id);
                    byte optbyte;
                    do
                    {
                    optbyte = reader.ReadByte();
                    } while (optbyte != 0xFF);

                    var width = reader.ReadByte();
                    var height = reader.ReadByte();
                    if ((width > 1) || (height > 1))
                    {
                        reader.BaseStream.Position++;
                    }
                    var frames = reader.ReadByte();
                    var xdiv = reader.ReadByte();
                    var ydiv = reader.ReadByte();
                    var zdiv = reader.ReadByte();
                    var animationLength = reader.ReadByte();

                    var numSprites =
                    (UInt32)width * (UInt32)height *
                    (UInt32)frames *
                    (UInt32)xdiv * (UInt32)ydiv * zdiv *
                    (UInt32)animationLength;

                    var spriteList = new List<UInt32>();

                    // Read the sprite ids
                    for (UInt32 i = 0; i <numSprites; ++i)
                    {
                        var spriteId = reader.ReadUInt32();
                        Debug.WriteLine(spriteId);
                        spriteList.Add(spriteId);
                    }
                    ++id;
                }
            }
        }

        private void next_button_Click(object sender, RoutedEventArgs e)
        {
            index++;
            SetImage();
           // AlternativeLoadSprite();
        }


        public void AlternativeLoadSprite()
        {
            FileStream fileStream = new FileStream(spritePath, FileMode.Open);
            try
            {
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    UInt32 sprSignature = reader.ReadUInt32();
                    UInt16 totalPics = reader.ReadUInt16();

                    List<UInt32> spriteIndexes = new List<UInt32>();
                    for (uint i = 0; i < totalPics; ++i)
                    {
                        UInt32 index = reader.ReadUInt32();
                        spriteIndexes.Add(index);
                    }

                    Debug.WriteLine("num of sprite indexex: " + spriteIndexes.Count());

                    UInt16 id = 1;
                    foreach (UInt32 element in spriteIndexes)
                    {
                        UInt32 index = element + 3;
                        reader.BaseStream.Seek(index, SeekOrigin.Begin);
                        UInt16 size = reader.ReadUInt16();
                        Debug.WriteLine("size is " + size);

                        var dump = reader.ReadBytes(size);
                        

                    }

                    ++id;
                }
            }
            finally
            {
                fileStream.Close();
            }
        }




    }
}
