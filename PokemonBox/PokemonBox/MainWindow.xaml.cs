﻿using System;
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

        PCBox pcBox;

        public MainWindow()
        {
            InitializeComponent();
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Instantiate the pc box
            pcBox = new PCBox();
<<<<<<< HEAD
            cmbSpeciesOptions.ItemsSource = pcBox.GetAttackOptions();
=======
            //cmbSpeciesOptions.ItemsSource = pcBox.GetPokemonOptions();
>>>>>>> bd475a3c05a58665ce01afefd13dea1671fb58ba
        }

        //private void cmbAttackTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    // Set the bindings for the pokemon selection grid
        //    grdSelectedSpecies.DataContext = cmbSpeciesOptions.SelectedItem;
        //}
    }
}
