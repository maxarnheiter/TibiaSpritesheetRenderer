using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace TibiaSpritesheetRenderer
{
    public partial class MainWindow
    {

        private void DisplayLoadDialog()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Tibia";
            dialog.DefaultExt = ".spr";
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                spritesPath = dialog.FileName;
            }
        }

        private void DisplaySaveDialog()
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
                haveSavePath = true;
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

        private void RenderSpritesheets()
        {
            //Check if we can actually save
            if (!haveSavePath)
                return;

            BinaryReader reader = new BinaryReader(File.OpenRead(spritesPath));

            reader.ReadUInt32(); //Skip over version info
            int count = (int)reader.ReadUInt32();

            string finalSavePath = savePath;
            string finalSaveName = saveName;

            int sheetNumber = 0;
            int row = 0;
            int column = 0;

            int finalPXWidth = outputWidth;
            int finalPXHeight = outputHeight;

            int maxColumnSize = (finalPXWidth / 32)- 1;
            int maxRowSize = (finalPXHeight / 32) - 1;

            Bitmap bitmap = new Bitmap(finalPXWidth, finalPXHeight);

            for (int i = 0; i < count; i++)
            {
                int currentPixel = 0;

                reader.BaseStream.Seek(8 + (i + 1) * 4, SeekOrigin.Begin);
                reader.BaseStream.Seek(reader.ReadUInt32() + 3, SeekOrigin.Begin);

                var offset = reader.BaseStream.Position + reader.ReadUInt16();

                while (reader.BaseStream.Position < offset)
                {
                    var transparentPixels = reader.ReadUInt16();
                    var coloredPixels = reader.ReadUInt16();

                    if (transparentPixels > 1024 || coloredPixels > 1024)
                        break;

                    currentPixel += transparentPixels;

                    for (int j = 0; j < coloredPixels; j++)
                    {
                        var red = reader.ReadByte();
                        var green = reader.ReadByte();
                        var blue = reader.ReadByte();

                        int x = currentPixel % 32 + (column * 32);
                        int y = currentPixel / 32 + (row * 32);

                        bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(red, green, blue));

                        currentPixel++;
                    }
                }
                column++;

                //Check if column exceeds maximum (Return)
                if(column == maxColumnSize)
                {
                    row++;
                    column = 0;
                }

                //Check if row exceeds maximum or if we're done looping through sprites (Save Page)
                if (row == maxRowSize || i == count-1)
                {
                    SavePage(bitmap, finalSavePath + finalSaveName + "_" + sheetNumber);
                    row = 0;
                    column = 0;
                    sheetNumber++;
                    bitmap = new Bitmap(finalPXWidth, finalPXHeight);
                }

               
            }
        }

        void SavePage(Bitmap image, string fullPath)
        {
            image.Save(fullPath + ".png");
        }

    }
}
