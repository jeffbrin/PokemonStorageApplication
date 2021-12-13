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
using PokemonBox.Models;
namespace PokemonBox
{
    /// <summary>
    /// Interaction logic for PokemonAdditionWindow.xaml
    /// </summary>
    public partial class PokemonAdditionWindow : Window
    {

        private Pokemon tempPokemon;
        private PCBox localPcBox;

        public PokemonAdditionWindow(PCBox pcBox)
        {
            InitializeComponent();
            InitialOptionsSetup();
            localPcBox = pcBox; // Change the scope to access it in the button function
        }

        private void cmbSpeciesPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetFields(cmbSpeciesPicker.SelectedIndex);
            tempPokemon = new Pokemon(cmbSpeciesPicker.SelectedItem as Pokemon); // Get the selected pokemon and copy it to the temp pokemon
            spSelectedSpecies.DataContext = tempPokemon; // resync the DataContext
        }
        /// <summary>
        /// Ability to move window around screen
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
        /// <summary>
        /// Close the window using custom 'X' Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Minimize Window Using custon '_' button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgMinus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void rdbGender_Checked(object sender, RoutedEventArgs e)
        {
            if (rdbMale.IsChecked == true)
                tempPokemon.Sex = 'M';
            else
                tempPokemon.Sex = 'F';
        }

        // Sets up the combo boxes and temp pokemon for creation
        private void InitialOptionsSetup()
        {
            cmbSpeciesPicker.ItemsSource = PCBox.PokemonOptions; // Bind the combo box with the pokemon options
            cmbSpeciesPicker.SelectedItem = PCBox.PokemonOptions[0]; // Set the default selection for the species picker
            tempPokemon = new Pokemon(cmbSpeciesPicker.SelectedItem as Pokemon); // Set the default pokemon for the selection
            spSelectedSpecies.DataContext = tempPokemon; // sync the selected species stack grid with the temp pokemon
            cmbMoveSelection1.ItemsSource = PCBox.AttackOptions; // Bind the attack selection combo boxes with the attack options
            cmbMoveSelection2.ItemsSource = PCBox.AttackOptions;
            cmbMoveSelection3.ItemsSource = PCBox.AttackOptions;
            cmbMoveSelection4.ItemsSource = PCBox.AttackOptions;
            cmbAbilitySelection.ItemsSource = PCBox.AbilityOptions; // Bind the ability selection combo box with the ability options
        }

        // Makes sure the pokemon created would be valid.
        // Displays an error if the addition is invalid
        private bool ValidatePokemonSelections()
        {
            StringBuilder sb = new StringBuilder();
            bool valid = true;

            // Check species
            if (cmbSpeciesPicker.SelectedItem == null)
            {
                sb.AppendLine("No species selected.");
                valid = false;
            }

            // Check Sex
            if (rdbMale.IsChecked == false && rdbFemale.IsChecked == false)
            {
                sb.AppendLine("No sex selected.");
                valid = false;
            }

            // Check Attacks
            if (Array.TrueForAll<Attack>(tempPokemon.Attacks, element => element == null)) /// Only one attack is needed
            {
                sb.AppendLine("No attack selected.");
                valid = false;
            }

            // Check Ability
            if (tempPokemon.Ability == null)
            {
                sb.AppendLine("No Ability selected.");
                valid = false;
            }

            // Show an error if the selection wasn't valid
            if (!valid)
                MessageBox.Show(sb.ToString(), "Invalid Pokemon", MessageBoxButton.OK, MessageBoxImage.Error);

            return valid;

        }

        // Adds the created pokemon to the box if it's valid
        private void btnCreatePokemon_Click(object sender, RoutedEventArgs e)
        {
            if (ValidatePokemonSelections())
            {
                tempPokemon.Sex = rdbMale.IsChecked == true ? 'M' : 'F'; // Set the pokemon's sex
                localPcBox.AddPokemon(tempPokemon); // Add the new pokemon
                CreatedPokemon = true; // Indicate that the pokemon was added successfully
                this.Close();
            }
        }

        private void btnResetPokemon_Click(object sender, RoutedEventArgs e)
        {
            ResetFields();
        }

        private void btnExitPokemonCreation_Click(object sender, RoutedEventArgs e)
        {
            CreatedPokemon = false;
            this.Close();
        }

        // Resets all the fields, sets the selected species back to the default or to whatever is passed as an argument
        private void ResetFields(int newSpeciesIndex = 0)
        {
            cmbSpeciesPicker.SelectedIndex = newSpeciesIndex; // Reset the selected species
            chkShiny.IsChecked = false; // Reset the shiny checkbox
            rdbFemale.IsChecked = false; // Reset male checkbox
            rdbMale.IsChecked = false; // Reset female checkbox
            txtNickname.Text = string.Empty; // Reset nickname

            // Reset move selections
            cmbMoveSelection1.SelectedItem = null;
            cmbMoveSelection2.SelectedItem = null;
            cmbMoveSelection3.SelectedItem = null;
            cmbMoveSelection4.SelectedItem = null;

            // Reset ability
            cmbAbilitySelection.SelectedItem = null;
        }

        public bool CreatedPokemon { get; set; }

    }
}
