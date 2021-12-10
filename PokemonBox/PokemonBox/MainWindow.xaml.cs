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
using PokemonBox.Models;
using System.Drawing;

namespace PokemonBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        PCBox pcBox;

        public MainWindow()
        {
            InitializeComponent();
            // Instantiate the pc box
            pcBox = new PCBox();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            PokemonAdditionWindow pokemonAdderWindow = new PokemonAdditionWindow(pcBox.StoredPokemon);
            pokemonAdderWindow.ShowDialog();
        }

        /// <summary>
        /// Allows the user to drag the window around the screen if mouse is pressed on the header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void imgClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void imgMinus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }

}

