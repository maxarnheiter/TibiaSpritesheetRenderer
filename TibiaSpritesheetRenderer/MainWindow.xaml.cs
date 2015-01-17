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

        string spritesPath = "";
        bool haveSpriteFile;


        string savePath = "";
        string saveName = "";
        bool haveSavePath;

        int _outputWidth;
        int outputWidth
        {
            get { return _outputWidth; }
            set
            {
                _outputWidth = value;
                width_label.Content = _outputWidth + "px  (" + _outputWidth / 32 + ")";
            }
            
        }

        int _outputHeight;
        int outputHeight
        {
            get { return _outputHeight; }
            set
            {
                _outputHeight = value;
                height_label.Content = _outputHeight + "px  (" + _outputHeight / 32 + ")";
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            outputWidth = 320;
            outputHeight = 320;
        }

        private void load_sprite_button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Tibia";
            dialog.DefaultExt = ".spr";
            bool? result = dialog.ShowDialog();

            if(result == true)
            {
                spritesPath = dialog.FileName;
                CheckSpritesFile();
            }
        }


        private void render_button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "spriteSheet";
            dialog.DefaultExt = ".png";
            dialog.Filter = "PNG Image |(.png)";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                saveName = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName);
                savePath = System.IO.Path.GetDirectoryName(dialog.FileName) + "\\";
                
                //TODO
            }
        }

        private void CheckSpritesFile()
        {
            BinaryReader reader = new BinaryReader(File.OpenRead(spritesPath));
            UInt32 version = reader.ReadUInt32();
            UInt32 count = reader.ReadUInt32();

            sprites_path_label.Content = spritesPath;
            sprite_quantity_label.Content = "Sprites Loaded: " + count.ToString();
            sprite_version_label.Content = "Sprite File Version: " + version.ToString();

            haveSpriteFile = true;

            reader.Close();
        }

        /*
        private void LoadAllSprites()
        {
   

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
        */

        private void width_up_button_Click(object sender, RoutedEventArgs e)
        {
            outputWidth += 32;
        }

        private void width_down_button_Click(object sender, RoutedEventArgs e)
        {
            outputWidth -= 32;
        }

        private void height_up_button_Click(object sender, RoutedEventArgs e)
        {
            outputHeight += 32;
        }

        private void height_down_button_Click(object sender, RoutedEventArgs e)
        {
            outputHeight -= 32;
        }


    }
}
