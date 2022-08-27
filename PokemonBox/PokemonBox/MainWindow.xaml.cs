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
        private MediaPlayer soundsPlayer;
        private bool musicMuted, soundsMuted;

        // The box's save state
        private bool Saved
        {
            get { return saved; }
            set
            {
                saved = value;
                SyncSaveStatus();
            }
        }

        // Constructor
        public MainWindow()
        {
            InitializeComponent();
            // Instantiate the pc box
            pcBox = new PCBox(MaxCapacity);

            // Link the save status to the display
            SyncSaveStatus();

            // Ready music player, music one needs to be done in xaml so it can be repeated
            meMusic.Source = new Uri("Audio/Music/116 Pokemon Center (Daytime).mp3", UriKind.Relative);
            meMusic.LoadedBehavior = MediaState.Play; // Play the music when the media player is loaded


            // Instantiate sounds player
            soundsPlayer = new MediaPlayer();

        }

        // Loop the music
        private void meMusic_MediaEnded(object sender, RoutedEventArgs e)
        {
            meMusic.Position = new TimeSpan(0, 0, 0); // Reset to 0,0,0 (start)
        }


        // Toggles the background music and changes the appearance of the button
        private void ToggleBackgroundMusic()
        {
            // Toggle the mute
            musicMuted = !musicMuted;
            meMusic.IsMuted = musicMuted;

            // Change the text decorations
            if (musicMuted)
            {
                txtMusicMuted.TextDecorations = TextDecorations.Strikethrough;
                txtMusicMuted.FontWeight = FontWeights.Normal;
            }
            else
            {
                txtMusicMuted.TextDecorations = null;
                txtMusicMuted.FontWeight = FontWeights.Bold;
            }

        }

        // Toggle whether the sound effects are muted and change the appearance of the button
        private void ToggleSoundEffects()
        {
            // Toggle the mute
            soundsMuted = !soundsMuted;

            // Setup the dtext ecorations
            if (soundsMuted)
            {
                txtSoundsMuted.TextDecorations = TextDecorations.Strikethrough;
                txtSoundsMuted.FontWeight = FontWeights.Normal;
            }
            else
            {
                txtSoundsMuted.TextDecorations = null;
                txtSoundsMuted.FontWeight = FontWeights.Bold;
            }
        }

        // Play the cry of the selected pokemon
        private void PlaySelectedPokemonCry()
        {
            // Play the pokemon's cry if the selected pokemon isn't null and it isn't muted
            if (selectedPokemon != null && !soundsMuted) 
            {
                soundsPlayer.Open(new Uri(selectedPokemon.CrySoundPath, UriKind.Relative));
                soundsPlayer.Play();
            }
        }

        // Toggle sound effects when the mute button is clicked
        private void btnEffectsMute_Click(object sender, RoutedEventArgs e)
        {
            ToggleSoundEffects();
        }
        // Toggle music when the mute button is clicked
        private void btnMusicMute_Click(object sender, RoutedEventArgs e)
        {
            ToggleBackgroundMusic();
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
        // Subtracted by one because the background image is a border
        private int MaxCapacity
        {
            get { return grdBoxedPokemon.Children.OfType<Border>().ToArray().Length - 1; }
        }

        // Opens the add pokemon window and adds a pokemon to the box if one was created
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
                    // Add the new pokemon to the display
                    DisplayNewPokemon();
                    // No longer saved
                    Saved = false;
                }

            }
            else
            {
                // Show an error message indicating that the box is full
                ErrorMessage em = new ErrorMessage("The box is full! Remove some pokemon before adding more", "Full Box", "Ok", MessageBoxResult.OK);

                Grid.SetColumnSpan(em.btnError2, 2);
                em.grdButtons.Children.Remove((UIElement)em.FindName("btnError3"));
                em.Show();
                //MessageBox.Show("The box is full! Remove some pokemon before adding more.", "Full Box", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Sets the Data context of a new pokemon cell
        private void DisplayNewPokemon()
        {
            int storedPokemonQuantity = pcBox.StoredPokemon.Count; // get the number of pokemon

            // Get all the children of type stack panel and convert the Ienumerable returned to an array.
            // Then set the data context of the first unfilled cell to the newly created pokemon
            grdBoxedPokemon.Children.OfType<Border>().ToArray()[storedPokemonQuantity].DataContext = pcBox.StoredPokemon[storedPokemonQuantity - 1]; // Set the data context of the next slot to the new pokemon
        }

        /// <summary>
        /// Allows the user to drag the window around the screen if mouse is pressed on the header
        /// </summary>
        private void grdHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FixMargins();
            if (e.LeftButton == MouseButtonState.Pressed && !MouseOnMaximizeOrMinimize())
            {
                WindowState = WindowState.Normal;
                DragMove();
            }
        }
        private bool MouseOnMaximizeOrMinimize()
        {
            return this.imgMax.IsMouseOver || this.imgMinus.IsMouseOver;
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
                selectedPokemonUiParent = senderButton; // set the new pokemon image parent

                // Set the background of the new parent
                selectedPokemonUiParent.Background = new SolidColorBrush(Colors.White);
                selectedPokemonUiParent.Opacity = 0.5;

                // Display the new selected pokemon in the focus section
                grdPokemonDisplayAndAdd.DataContext = selectedPokemon;

                // Play the pokemon's cry
                PlaySelectedPokemonCry();
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
            // Check that there is a selected pokemon parent
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
                // Display the warning
                string warning = $"You are about to release your {shiny}{selectedPokemon.Species}{nickname}. This can not be reversed, are you sure you want to release {pronouns}?";

                // If the user confirms the delete
                ErrorMessage em = new ErrorMessage(warning, "Warning!", "Yes", MessageBoxResult.Yes, "No", MessageBoxResult.No);
                if (em.ShowBox() == MessageBoxResult.Yes)
                {
                    // Remove the pokemon
                    RemoveSelectedPokemon();
                    // No longer saved
                    Saved = false;
                }

                // OLD
                //if (MessageBox.Show(warning, "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                //{
                //    // Remove the pokemon
                //    RemoveSelectedPokemon();
                //    // No longer saved
                //    Saved = false;
                //}
            }
        }

        // Remove a selected pokemon
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
                    // Index is i+1 since the first cell is used for the background
                    cells[i+1].DataContext = pcBox.StoredPokemon[i];
                // Or bind to null if no pokemon exists there
                else
                    cells[i+1].DataContext = null;
            }

            // Display the correct pokemon in the main display
            grdPokemonDisplayAndAdd.DataContext = selectedPokemon;
        }

        // Save when the save button is pressed
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveLogic();
        }

        // Load from a file when the load button is pressed
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            // Check if the user has saved or wants to save
            if (CheckBeforeOpening())
            {
                // Get the desired file to open
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "CSV Files|*.csv";

                // If the user selected a file to open
                if (open.ShowDialog() == true)
                {
                    // Store the old save location and save state in case the file reading goes wrong
                    string oldSaveLocation = saveLocation;
                    bool oldSaved = saved;
                    saveLocation = open.FileName;
                    Saved = true;

                    // Load from the file and make sure it was successful
                    if (!pcBox.LoadFromFile(saveLocation))
                    {
                        // Show error and revert to old save location and state
                        ErrorMessage em = new ErrorMessage("Error loading data from file.", "Failed to load", "Ok", MessageBoxResult.OK);
                        em.ShowBox();
                        //MessageBox.Show("Error loading data from file.", "Failed to load", MessageBoxButton.OK, MessageBoxImage.Error);
                        saveLocation = oldSaveLocation;
                        Saved = oldSaved;
                    }
                    else
                    {
                        // If the box was saved correctly, fix the bindings
                        selectedPokemon = null;
                        grdPokemonDisplayAndAdd.DataContext = selectedPokemon;
                        // Place pokemon in correct cells
                        ReplacePokemonInCorrectCells();
                        // Clear any selection effects
                        ClearSelectedBoxEffects();
                    }

                }
            }

        }

        /// <summary>
        /// Checks that the user has already saved, or asks the user if they want to save and gives them the chance to
        /// </summary>
        /// <returns></returns>
        private bool CheckBeforeOpening()
        {

            // If the file is already saved return
            if (saved)
                return true;

            // Check if the user wants to save

            //ErrorMessage errorMessage = new ErrorMessage("Do you want to save changes?");
            //errorMessage.btnError1.Content = "Yes";
            //errorMessage.btnError2.Content = "No";
            //errorMessage.btnError3.Content = "Cancel";
            //errorMessage.Show();
            //ErrorMessage.ErrorMessageResult result = errorMessage.ChooseErrorMessageresult;

            //if (result == ErrorMessage.ErrorMessageResult.No)
            //    return true;

            ////if Cancel the loading
            //if (result == ErrorMessage.ErrorMessageResult.Cancel)
            //    return false;

            //if (result == ErrorMessage.ErrorMessageResult.Yes)
            //    SaveLogic();

            //return saved;

            ErrorMessage em = new ErrorMessage("Do you want to save changes?", "Unsaved data", "Yes", MessageBoxResult.Yes, "No", MessageBoxResult.No, "Cancel", MessageBoxResult.Cancel);

            MessageBoxResult result =
            em.ShowBox();
            //If the user doesn't want to save 
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
            if (pcBox.SaveToFile(saveLocation))
                Saved = true;
            else
            {
                //MessageBox.Show("Your box could not be saved. Try again.", "Error saving.", MessageBoxButton.OK, MessageBoxImage.Error);
                ErrorMessage em = new ErrorMessage("Your box could not be saved. Try again.", "Error saving.", "Ok", MessageBoxResult.OK);
                em.ShowBox();

                Saved = false;
            }

        }

        // Open the pokemon stats window to show details
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

        // Make sure the user saved or doesn't want to save before closing
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Check if the user has saved, if not ask them if they want to then exit
            e.Cancel = !CheckBeforeOpening();

        }

        // Open the edit pokemon window
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Make sure there is a selected pokemon
            if (selectedPokemon != null)
            {
                // If there is a pokemon selected, open the edit window with that pokemon
                EditPokemonWindow editWindow = new EditPokemonWindow(selectedPokemon);
                editWindow.ShowDialog();

                // If the user saved the changes
                if (editWindow.ChangesSaved)
                {
                    // Replace the edited pokemon with the new version
                    ReplaceSelectedPokemon(editWindow.editingPokemon);
                    Saved = false;
                }

            }
        }

        private void imgMax_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
                ResizeMode = ResizeMode.NoResize;
                Thickness margin = grdMain.Margin;
                margin.Left = 8;
                margin.Right = 8;
                margin.Top = 8;
                margin.Bottom = 8;
                grdMain.Margin = margin;
            }
            else
            {
                this.WindowState = WindowState.Normal;
                ResizeMode = ResizeMode.CanResizeWithGrip;
                Thickness margin = grdMain.Margin;
                margin.Left = 0;
                margin.Right = 0;
                margin.Top = 0;
                margin.Bottom = 0;
                grdMain.Margin = margin;
            }

        }
        private void FixMargins()
        {
            if (WindowState == WindowState.Maximized)
            {
                Thickness margin = grdMain.Margin;
                margin.Left = 8;
                margin.Right = 8;
                margin.Top = 8;
                margin.Bottom = 8;
                grdMain.Margin = margin;
            }
            else
            {
                ResizeMode = ResizeMode.CanResizeWithGrip;
                Thickness margin = grdMain.Margin;
                margin.Left = 0;
                margin.Right = 0;
                margin.Top = 0;
                margin.Bottom = 0;
                grdMain.Margin = margin;
            }
        }

        // Replace a selected pokemon
        private void ReplaceSelectedPokemon(Pokemon newPokemon)
        {
            // Replace the pokemon in the pcBox
            pcBox.StoredPokemon[pcBox.StoredPokemon.IndexOf(selectedPokemon)] = newPokemon; 
            // Set it to the new selected pokemon since it had to have been selected to be edited
            // This needs to be done since newPokemon is a difference object than the old selected pokemon
            selectedPokemon = newPokemon;
            // Set the data context of the selected pokemon cell and replace all the pokemon in their cells to ensure consistency
            (selectedPokemonUiParent.Parent as Border).DataContext = selectedPokemon;
            ReplacePokemonInCorrectCells(); // Sync the display
        }


    }

}

