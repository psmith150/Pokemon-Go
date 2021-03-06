﻿using Pokemon_Go_Database.Model;
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
    /// Interaction logic for PokemonFilterViewer.xaml
    /// </summary>
    public partial class PokedexFilterViewer : UserControl
    {
        public PokedexFilterViewer()
        {
            InitializeComponent();
        }
        #region Public Properties
        public PokedexFilterElement Filter
        {
            get { return (PokedexFilterElement)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }
        public Array FilterTypes
        {
            get
            {
                return Enum.GetValues(typeof(PokedexFilterType));
            }
        }
        public Array ComparisonTypes
        {
            get
            {
                return Enum.GetValues(typeof(FilterComparisonType));
            }
        }
        #endregion
        #region Dependency Properties
        // Using a DependencyProperty as the backing store for ValuesDataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register("Filter", typeof(PokedexFilterElement), typeof(PokedexFilterViewer));
        #endregion
    }
}
