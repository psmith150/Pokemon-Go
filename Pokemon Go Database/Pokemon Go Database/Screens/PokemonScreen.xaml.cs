﻿using GalaSoft.MvvmLight.Messaging;
using Pokemon_Go_Database.Model;
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

namespace Pokemon_Go_Database.Screens
{
    /// <summary>
    /// Interaction logic for PokedexScreen.xaml
    /// </summary>
    public partial class PokemonScreen : UserControl
    {
        public PokemonScreen(PokemonViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
            Messenger.Default.Register<PokemonMessage>(this, HandlePokemonMessage);
        }
        private void HandlePokemonMessage(PokemonMessage msg)
        {
            this.PokemonGrid.SelectedItem = msg.Pokemon;
            this.PokemonGrid.ScrollIntoView(msg.Pokemon);
        }
    }
}
