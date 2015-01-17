using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TibiaSpritesheetRenderer
{
    public partial class MainWindow
    {

        private void load_sprite_button_Click(object sender, RoutedEventArgs e)
        {
            DisplayLoadDialog();
            CheckSpritesFile();
        }

        private void render_button_Click(object sender, RoutedEventArgs e)
        {
            DisplaySaveDialog();
            RenderSpritesheets();
        }

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
