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

/*
 * Welcome to the Tibia Spritesheet Renderer 
 * Its just a simple program with a GUI that allows you to output sprite sheets of any specified size (multiples of 32px). 
 * 
 * NOTE: If you want this to work for EARLIER version, you'll need to ReadUInt16 instead of ReadUInt32 for the count. 
 */


namespace TibiaSpritesheetRenderer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //Everything you're looking for are in the appropiate files
        // Core functionality (rendering) can be found in MainWindow.Functions
        // The properties/fields can be found in MainWindow.Properties
        // Button triggers are found in MainWindow.Buttons

        public MainWindow()
        {
            InitializeComponent();

            outputWidth = 320;
            outputHeight = 320;
        }

    }
}
