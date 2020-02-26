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
using System.Resources;
using System.IO;

namespace Google_Calendar_API
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : Window
    {

        
            MainWindow main = new MainWindow(1);

        public ColorPicker()
        {
            InitializeComponent();

            
            

            SelectColorOnPicker();

        }

        private void SelectColorOnPicker()
        {
            ColorPickDanish.SelectedColor = main.DanishBrushColor;
            ColorPickEnglish.SelectedColor = main.EnglishBrushColor;
            ColorPickMath.SelectedColor = main.MathBrushColor;
            ColorPickPhysics.SelectedColor = main.PhysicsBrushColor;
        }
        

        private void ColorPickDanishChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            


        }

        private void cp_SelectedColorChanged_2(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            
        }

        private void cp_SelectedColorChanged_3(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            
        }

        private void cp_SelectedColorChanged_4(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            main.DanishBrushColor = ColorPickDanish.SelectedColor.Value;
            main.EnglishBrushColor = ColorPickEnglish.SelectedColor.Value;
            main.MathBrushColor = ColorPickMath.SelectedColor.Value;
            main.PhysicsBrushColor = ColorPickPhysics.SelectedColor.Value;

            File.WriteAllText(MainWindow.HomeworkColors, $"Danish {main.DanishBrushColor.ToString()}:English {main.EnglishBrushColor.ToString()}:Math {main.MathBrushColor.ToString()}:Physics {main.PhysicsBrushColor.ToString()}:");

        }

        
    }
}
