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
    /// Interaction logic for EditPokemonWindow.xaml
    /// </summary>
    public partial class EditPokemonWindow : Window
    {
        public bool ChangesSaved { get; set; }
        public Pokemon editingPokemon { get; set; } // Needed to access the pokemon in the other window
        private Pokemon localTargetPokemon;

        public EditPokemonWindow(Pokemon targetPokemon)
        {
            InitializeComponent();

            // Copy the pokemon
            localTargetPokemon = targetPokemon;
            editingPokemon = GetClonePokemon();

            // Setup the options for the comboboxes
            InitialOptionsSetup();
        }

        private void InitialOptionsSetup()
        {
            this.DataContext = editingPokemon;

            // Setup the option
            cmbSpeciesPicker.ItemsSource = PCBox.PokemonOptions;
            cmbAbilitySelection.ItemsSource = PCBox.AbilityOptions;
            cmbMoveSelection1.ItemsSource = PCBox.AttackOptions;
            cmbMoveSelection2.ItemsSource = PCBox.AttackOptions;
            cmbMoveSelection3.ItemsSource = PCBox.AttackOptions;
            cmbMoveSelection4.ItemsSource = PCBox.AttackOptions;

            cmbSpeciesPicker.SelectedIndex = editingPokemon.PokedexNumber - 1;
        }

        /// <summary>
        /// Ability to move window around screen
        /// </summary>
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
        private void imgClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Minimize Window Using custon '_' button
        /// </summary>
        private void imgMinus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnExitPokemonEditing_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnResetPokemon_Click(object sender, RoutedEventArgs e)
        {
            editingPokemon = GetClonePokemon();
            this.DataContext = editingPokemon; // Needed since it's a new object
            cmbSpeciesPicker.SelectedIndex = editingPokemon.PokedexNumber - 1;
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            ChangesSaved = true;
            this.Close();
        }

        private void cmbSpeciesPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            editingPokemon = new Pokemon(cmbSpeciesPicker.SelectedItem as Pokemon, editingPokemon.IsShiny, editingPokemon.Sex, editingPokemon.Ability, editingPokemon.Nickname, editingPokemon.Attacks);
            this.DataContext = editingPokemon; // Needed since it's a new object
        }

        // Returns a clone of the target pokemon
        private Pokemon GetClonePokemon()
        {
            Pokemon clonePokemon = new Pokemon(localTargetPokemon, localTargetPokemon.IsShiny, localTargetPokemon.Sex, localTargetPokemon.Ability, localTargetPokemon.Nickname);
            // Attacks need a copy as well since they're objects and we don't want to edit the original attacks if the copy changes them
            clonePokemon.Attacks = new Attack[] {
                PCBox.AttackOptions[clonePokemon.Attacks[0].AttackId],
                PCBox.AttackOptions[clonePokemon.Attacks[1].AttackId],
                PCBox.AttackOptions[clonePokemon.Attacks[2].AttackId],
                PCBox.AttackOptions[clonePokemon.Attacks[3].AttackId]
            };
            // Same reason as the attacks
            clonePokemon.Ability = PCBox.AbilityOptions[clonePokemon.Ability.AbilityId];

            return clonePokemon;
        }
    }
}
