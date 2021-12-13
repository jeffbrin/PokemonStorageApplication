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
using Microsoft.Win32;
using System.IO;

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
        private string saveLocation;
        private bool saved = true; // Saved is true if there is an empty box since there are no unsaved changes

        public MainWindow()
        {
            InitializeComponent();
            // Instantiate the pc box
            pcBox = new PCBox(MaxCapacity);

            // Link the save status to the display
            SyncSaveStatus();
        }

        // Sets the saveStatus backing field to match the saved bool
        private void SyncSaveStatus()
        {
            // New file
            if (string.IsNullOrEmpty(saveLocation) && saved)
                txtStatus.Text = "New box.";
            // Not saved
            else if (!saved || string.IsNullOrEmpty(saveLocation))
                txtStatus.Text = "Unsaved changed";
            // Saved
            else
                txtStatus.Text = $"Saved to: {saveLocation}";
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
                {
                    DisplayNewPokemon();
                    // No longer saved
                    saved = false;
                    SyncSaveStatus();
                }
                
            }
            else
            {
                // Show an error message indicating that the box is full
                MessageBox.Show("The box is full! Remove some pokemon before adding more.", "Full Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Sets the Data context of a new pokemon cell
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
                ClearSelectedBoxEffects();

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
            // If the user clicks on an empty cell, clear the selection
            else if (((sender as Button).Parent as Border).DataContext == null)
            {
                // If there was already a selected pokemon, clear its background
                ClearSelectedBoxEffects();

                // Make the selected pokemon null and clear the selected pokemon display
                selectedPokemon = null;
                grdPokemonDisplayAndAdd.DataContext = selectedPokemon;
            }
        }

        // Clears the effects shown on the selected pokemon box
        private void ClearSelectedBoxEffects()
        {
            if (selectedPokemonUiParent != null)
            {
                selectedPokemonUiParent.Background = new SolidColorBrush(Colors.Transparent); // Reset the background
                selectedPokemonUiParent.Opacity = 1; // Reset the opacity
            }
        }

        // Releases a pokemon from the box
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
                    // No longer saved
                    saved = false;
                    SyncSaveStatus();
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

        // Resets all the data contexts for the cells in the box and for the selected pokemon section
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

            // Display the correct pokemon in the main display
            grdPokemonDisplayAndAdd.DataContext = selectedPokemon;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveLogic();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            // Get desired file
            if (CheckBeforeOpening())
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "CSV Files|*.csv";
                

                if (open.ShowDialog() == true)
                {
                    // Store the old save location and save state in case the file reading goes wrong
                    string oldSaveLocation = saveLocation;
                    bool oldSaved = saved;
                    saveLocation = open.FileName;
                    saved = true;

                    // Make sure the file was loaded correctly
                    if (!pcBox.LoadFromFile(saveLocation))
                    {
                        // Show error and revert to old save location and state
                        MessageBox.Show("Error loading data from file.", "Failed to load", MessageBoxButton.OK, MessageBoxImage.Error);
                        saveLocation = oldSaveLocation;
                        saved = oldSaved;
                    }
                    else
                    {
                        // If the box was saved correctly, fix the bindings
                        selectedPokemon = null;
                        grdPokemonDisplayAndAdd.DataContext = selectedPokemon;
                        ReplacePokemonInCorrectCells();
                        ClearSelectedBoxEffects();
                    }

                    // Change the save status 
                    SyncSaveStatus();
                }
            }
            
        }

        private bool CheckBeforeOpening()
        {
            //// Check if the save location exists already
            //if (string.IsNullOrEmpty(saveLocation))
            //    return true;

            // If the file is already saved return
            if (saved)
                return true;

            // Check if the user wants to save
            MessageBoxResult result =
            MessageBox.Show("Do you want to save changes?", "Unsaved data", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

            // If the user doesn't want to save 
            if (result == MessageBoxResult.No)
                return true;
            
            // If the user cancels the loading
            if (result == MessageBoxResult.Cancel)
                return false;


            // User wants to save
            if (result == MessageBoxResult.Yes)
                SaveLogic();

            return saved;

        }

        /// <summary>
        /// Check if the user has already saved, if not find a save location and try to save
        /// </summary>
        private void SaveLogic()
        {
            // Check if isn't there is already a save location
            if (string.IsNullOrEmpty(saveLocation))
            {
                // Get the user to pick a save location
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "CSV Files|*.csv"; //"Description|*.ext|XYZ File|*.xyz|All Files|*.*";

                // Show the save window
                if (save.ShowDialog() == true)
                {
                    // If they saved, store the file name
                    saveLocation = save.FileName;

                }
                else // Leave if they don't wanna save
                    return;
            }

            // Save the data to the save location
            if (DataReaderWriter.SaveBox(saveLocation, pcBox.StoredPokemon.ToArray()))
                saved = true;
            else
                MessageBox.Show("Your box could not be saved. Try again.", "Error saving.", MessageBoxButton.OK, MessageBoxImage.Error);

            // Sync the save status
            SyncSaveStatus();
        }

        private void btnShowStats_Click(object sender, RoutedEventArgs e)
        {
            // If there is a selected pokemon
            if (selectedPokemon != null)
            {
                // Open the stats window with the selected pokemon
                ViewStatsWindow statsWindow = new ViewStatsWindow(selectedPokemon);
                statsWindow.ShowDialog();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Check if the user has saved, if not ask them if they want to then exit
            e.Cancel = !CheckBeforeOpening();

            //e.Cancel;
        }

    }

}

