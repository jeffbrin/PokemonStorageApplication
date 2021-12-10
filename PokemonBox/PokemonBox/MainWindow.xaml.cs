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
using System.Drawing;

namespace PokemonBox
{
    /// <summary>MaxCapacity;
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private PCBox pcBox;
        private Pokemon selectedPokemon;
        private Button selectedPokemonUiParent;

        public MainWindow()
        {
            InitializeComponent();
            // Instantiate the pc box
            pcBox = new PCBox(MaxCapacity);

        }

        // The maximum number of pokemon that can be stored
        private int MaxCapacity
        {
            get { return grdBoxedPokemon.Children.OfType<Border>().ToArray().Length; }
        }

        // Adds a pokemon to the box
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Check if the box is full
            if (!pcBox.IsFull)
            {
                // Open the pokemon addition window
                PokemonAdditionWindow pokemonAdderWindow = new PokemonAdditionWindow(pcBox);
                pokemonAdderWindow.ShowDialog(); // Show the pokemon creation window
                if (pokemonAdderWindow.CreatedPokemon) //If a pokemon was created, bind it to the next available grid cell
                    DisplayNewPokemon();
            }
            else
            {
                // Show an error message indicating that the box is full
                MessageBox.Show("The box is full! Remove some pokemon before adding more.", "Full Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayNewPokemon()
        {
            int storedPokemonQuantity = pcBox.StoredPokemon.Count; // get the number of pokemon
           
            // Get all the children of type stack panel and convert the Ienumerable returned to an array.
            // Then set the data context of the first unfilled cell to the newly created pokemon
            grdBoxedPokemon.Children.OfType<Border>().ToArray()[storedPokemonQuantity - 1].DataContext = pcBox.StoredPokemon[storedPokemonQuantity - 1]; // Set the data context of the next slot to the new pokemon
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

        // Selects a pokemon from the grid
        private void btnSelectPokemon_Click(object sender, RoutedEventArgs e)
        {
            // Make sure the button's cell has a pokemon assigned to it
            if (((sender as Button).Parent as Border).DataContext is Pokemon)
            {
                // If there was already a selected pokemon, clear its background
                if (selectedPokemonUiParent != null) {
                    selectedPokemonUiParent.Background = new SolidColorBrush(Colors.Transparent); // Reset the background
                    selectedPokemonUiParent.Opacity = 1; // Reset the opacity
                }

                // Get the new pokemon and parent
                Button senderButton = sender as Button;
                selectedPokemon = (senderButton.Parent as Border).DataContext as Pokemon; // get the pokemon stored in the same cell as the button (sender)
                selectedPokemonUiParent = senderButton;

                // Set the background of the new parent
                selectedPokemonUiParent.Background = new SolidColorBrush(Colors.White);
                selectedPokemonUiParent.Opacity = 0.5;

                // Display the new selected pokemon
                grdPokemonDisplayAndAdd.DataContext = selectedPokemon;
            }
        }

        private void btnReleasePokemon_Click(object sender, RoutedEventArgs e)
        {
            // Make sure there's a selected pokemon
            if (selectedPokemon != null)
            {
                // Warn the user that they will be deleting their pokemon
                string pronouns = selectedPokemon.Sex == 'M' ? "him" : "her";
                string shiny = selectedPokemon.IsShiny ? "shiny " : string.Empty;
                string nickname = string.IsNullOrWhiteSpace(selectedPokemon.Nickname) ? "" : $" {selectedPokemon.Nickname}";
                string warning = $"You are about to release your {shiny}{selectedPokemon.Species}{nickname}. This can not be reversed, are you sure you want to release {pronouns}?";

                // If the user confirms the delete
                if(MessageBox.Show(warning, "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    RemoveSelectedPokemon();
                }
            }
        }
        
        private void RemoveSelectedPokemon()
        {
            selectedPokemonUiParent.Background = new SolidColorBrush(Colors.Transparent); // Reset the background to transparent
            selectedPokemonUiParent.Opacity = 1; // Reset the backgrouns opacity
            pcBox.RemovePokemon(selectedPokemon); // Remove the selected pokemon
            selectedPokemon = null; // Reset the selected pokemon to null
            grdPokemonDisplayAndAdd.DataContext = selectedPokemon; // Sync the display with the new selected pokemon (null)
            ReplacePokemonInCorrectCells(); // Replace all the pokemon now that a hole may have been made
        }

        private void ReplacePokemonInCorrectCells()
        {
            // Get all the pokemon cells
            Border[] cells = grdBoxedPokemon.Children.OfType<Border>().ToArray();

            // Loop through each pokemon
            for (int i = 0; i < MaxCapacity; i++)
            {
                // Bind the pokemon to their new spot
                if (i < pcBox.StoredPokemon.Count)
                    cells[i].DataContext = pcBox.StoredPokemon[i];
                // Or bind to null if no pokemon exists there
                else
                    cells[i].DataContext = null;
            }
        }
    }

}

