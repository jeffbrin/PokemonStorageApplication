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

namespace PokemonBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        // TODO: DELETE
        private void btnTesting_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, Dictionary<string, string[]>> attackeMatchups = DataReader.GetTypesInformation("PokemonData/attackMatchups.csv");
            Dictionary<string, Dictionary<string, string[]>> defenceMatchups = DataReader.GetTypesInformation("PokemonData/defenceMatchups.csv");

            string matchup = "Fire is weak against: ";

            foreach (string type in defenceMatchups["Normal"]["Weak"])
                matchup += $"{type} ";
            txtTesting.Text = matchup;
        }
    }
}
