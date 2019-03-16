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

namespace Pokemon_Go_Database.CustomControls
{
    /// <summary>
    /// Interaction logic for PokemonSimulationViewer.xaml
    /// </summary>
    public partial class PokemonSimulationViewer : UserControl
    {
        public PokemonSimulationViewer()
        {
            InitializeComponent();
        }
        #region Public Properties
        public Pokemon Pokemon
        {
            get { return (Pokemon)GetValue(PokemonProperty); }
            set { SetValue(PokemonProperty, value); }
        }
        public ListCollectionView SpeciesView
        {
            get { return (ListCollectionView)GetValue(SpeciesViewProperty); }
            set { SetValue(SpeciesViewProperty, value); }
        }
        #endregion
        #region Dependency Properties
        // Using a DependencyProperty as the backing store for ValuesDataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpeciesViewProperty =
            DependencyProperty.Register("SpeciesView", typeof(ListCollectionView), typeof(PokemonSimulationViewer));
        public static readonly DependencyProperty PokemonProperty =
            DependencyProperty.Register("Pokemon", typeof(Pokemon), typeof(PokemonSimulationViewer));
        #endregion
    }
}
