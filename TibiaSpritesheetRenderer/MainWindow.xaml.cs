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

    public class TibiaObject
    {
        public int id;
        public int width;
        public int height;
        public List<int> spriteIds;

        public TibiaObject(int id, int width, int height, List<int> spriteIds)
        {
            this.id = id;
            this.width = width;
            this.height = height;
            this.spriteIds = spriteIds;
        }
    }


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int objectId;
        int spriteId;

        string spritePath = "";
        string dataPath = "";

        Dictionary<int, Bitmap> sprites = new Dictionary<int, Bitmap>();
        Dictionary<int, TibiaObject> objects = new Dictionary<int, TibiaObject>();

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
                LoadAllSprites();
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
            Bitmap bmp;

            sprites.TryGetValue(spriteId, out bmp);

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

        private void LoadAllSprites()
        {
            sprites = new Dictionary<int, Bitmap>();

            BinaryReader reader = new BinaryReader(File.OpenRead(spritePath));

            UInt32 spriteVersion = reader.ReadUInt32();
            UInt32 spriteCount = reader.ReadUInt32();

            for(int i = 0; i < spriteCount; i++)
            {
                Bitmap bitmap = new Bitmap(32, 32);
                int currentPixel = 0;

                reader.BaseStream.Seek(8 + (i + 1) * 4, SeekOrigin.Begin);
                reader.BaseStream.Seek(reader.ReadUInt32() + 3, SeekOrigin.Begin);

                var offset = reader.BaseStream.Position + reader.ReadUInt16();

                while(reader.BaseStream.Position < offset)
                {
                    var transparentPixels = reader.ReadUInt16();
                    var coloredPixels = reader.ReadUInt16();

                    if (transparentPixels > 1024 || coloredPixels > 1024)
                        break;

                    currentPixel += transparentPixels;

                    for(int j = 0; j < coloredPixels; j++)
                    {
                        var red = reader.ReadByte();
                        var green = reader.ReadByte();
                        var blue = reader.ReadByte();

                        int x = currentPixel % 32;
                        int y = currentPixel / 32;

                        bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(red, green, blue));

                        currentPixel++;
                    }
                }

                sprites.Add(i+1, bitmap);
            }

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
                    byte optbyte;
                    do
                    {
                        optbyte = reader.ReadByte();
                        switch (optbyte)
                        {
                            case 0x00:
                                {
                                    reader.ReadUInt16();
                                    break;
                                }
                            case 0x08:
                                {
                                    reader.ReadUInt16();
                                    break;
                                }
                            case 0x09:
                                {
                                    reader.ReadUInt16();
                                    break;
                                }
                            case 0x16:
                                {
                                    reader.ReadUInt16();
                                    reader.ReadUInt16();
                                    break;
                                }
                            case 0x19:
                                {
                                    reader.BaseStream.Seek(4, SeekOrigin.Current);
                                    break;
                                }
                            case 0x1A:
                                {
                                    reader.ReadUInt16();
                                    break;
                                }
                            case 0x1D:
                                {
                                    reader.ReadUInt16();
                                    break;
                                }
                            case 0x1E:
                                {
                                    reader.ReadUInt16();
                                    break;
                                }
                            case 0x21:
                                {
                                    reader.ReadUInt16();
                                    break;
                                }
                            case 0x22:
                                {
                                    reader.ReadUInt16();
                                    reader.ReadUInt16();
                                    reader.ReadUInt16();
                                    var size = reader.ReadUInt16();
                                    var blah = reader.ReadChars(size);
                                    reader.ReadUInt16();
                                    reader.ReadUInt16();
                                    break;
                                }
                            case 0x23:
                                {
                                    reader.ReadUInt16();
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                                
                        }
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

                    /*
                    if (animationLength > 1)
                    {
                        reader.ReadByte();
                        reader.ReadInt32();
                        reader.ReadByte();
                        for (int i = 0; i < animationLength; i++)
                        {
                            reader.ReadUInt32();
                            reader.ReadUInt32();
                        }
                    }
                     */

                    var numSprites =
                    (UInt32)width * (UInt32)height *
                    (UInt32)frames *
                    (UInt32)xdiv * (UInt32)ydiv * (UInt32)zdiv *
                    (UInt32)animationLength;

                    var spriteList = new List<int>();

                    // Read the sprite ids
                    for (UInt32 i = 0; i < numSprites; ++i)
                    {
                        var spriteId = reader.ReadUInt32();
                        spriteList.Add((int)spriteId);
                    }

                    if (spriteList.Count > 0)
                        objects.Add(id, new TibiaObject(id, width, height, spriteList));

                    ++id;
                }
            }
        }

        private void next_button_Click(object sender, RoutedEventArgs e)
        {
         
        }

        private void find_obj_Click(object sender, RoutedEventArgs e)
        {
            var objs = objects.Where(o => o.Value.width == 2 && o.Value.height == 2 && o.Value.spriteIds.Count == 4);
            Random r = new Random();
            int randomId = r.Next(0, objs.Count());
            objectId = objs.ElementAt(randomId).Key;

            Debug.WriteLine("Random object id found: " + objectId);

            TibiaObject target;
            objects.TryGetValue(objectId, out target);
           
            if(target != null)
            {
                foreach (int id in target.spriteIds)
                {
                    Debug.WriteLine("id: " + id);
                    Bitmap bmp;
                    sprites.TryGetValue(id-1, out bmp);
                    if(bmp != null)
                    {
                        string fileName = "C:\\dump\\" + id + ".png";
                        bmp.Save(fileName);
                    }
                }
            }
        }






    }
}
