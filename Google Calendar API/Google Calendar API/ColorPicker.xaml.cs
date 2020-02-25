using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Google_Calendar_API
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : Window
    {
        public ColorPicker()
        {
            InitializeComponent();

            //der er data binding på Dansk color picker
            DanishBrushColor = (Color)ColorConverter.ConvertFromString(Properties.Resources.DanishColor);
        }
        Color danishBrushColor;
        Color englishBrushColor;
        Color mathBrushColor;
        Color physicsBrushColor;

        public  Color DanishBrushColor { get => danishBrushColor; set => danishBrushColor = value; }
        public Color EnglishBrushColor { get => englishBrushColor; set => englishBrushColor = value; }
        public Color MathBrushColor { get => mathBrushColor; set => mathBrushColor = value; }
        public Color PhysicsBrushColor { get => physicsBrushColor; set => physicsBrushColor = value; }

        private void cp_SelectedColorChanged_1(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            DanishBrushColor = ColorPickDanish.SelectedColor.Value;
            
            
            
        }

        private void cp_SelectedColorChanged_2(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            EnglishBrushColor = ColorPickEnglish.SelectedColor.Value;
        }

        private void cp_SelectedColorChanged_3(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            MathBrushColor = ColorPickMath.SelectedColor.Value;
        }

        private void cp_SelectedColorChanged_4(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            PhysicsBrushColor = ColorPickPhysics.SelectedColor.Value;
        }
    }
}
