using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaSpritesheetRenderer
{
    public partial class MainWindow
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
    }
}
