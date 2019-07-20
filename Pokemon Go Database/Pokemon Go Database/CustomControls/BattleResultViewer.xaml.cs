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
    public partial class BattleResultViewer : UserControl
    {
        public BattleResultViewer()
        {
            InitializeComponent();
        }
        #region Public Properties
        public BattleResult Result
        {
            get { return (BattleResult)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }
        #endregion
        #region Dependency Properties
        // Using a DependencyProperty as the backing store for ValuesDataSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register("Result", typeof(BattleResult), typeof(BattleResultViewer));
        #endregion
    }
}
