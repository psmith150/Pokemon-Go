using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
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
    /// Interaction logic for NullableComboBox.xaml
    /// </summary>
    public partial class NullableComboBox : UserControl
    {
        public NullableComboBox()
        {
            InitializeComponent();
        }

        #region Public Properties
        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable)GetValue(ItemsSourceProperty);
            }
            set
            {
                IEnumerable<object> collection = value.Cast<object>();
                List<object> listWithNull = new List<object>();
                listWithNull.Add(new EmptyItem());
                foreach (var element in value)
                {
                    listWithNull.Add(element);
                }
                combo.ItemsSource = listWithNull;
                SelectedItem = null;
            }
        }

        public object SelectedItem
        {
            get
            {
                return (object)GetValue(SelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemProperty, value);
                if (value is null)
                {
                    combo.SelectedItem = new EmptyItem();
                }
                else
                {
                    combo.SelectedItem = value;
                }
            }
        }

        public string PlaceholderText
        {
            get
            {
                return (string)GetValue(PlaceholderTextProperty);
            }
            set
            {
                SetValue(PlaceholderTextProperty, value);
            }
        }
        #endregion
        #region Dependency Properties
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(NullableComboBox), new FrameworkPropertyMetadata(null, OnDependencyPropertyChanged));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(NullableComboBox));

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(NullableComboBox));
        #endregion

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ( combo.SelectedItem is EmptyItem)
                this.SelectedItem = null;
            else
                SelectedItem = combo.SelectedItem;
        }

        private static void OnDependencyPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            dynamic self = (NullableComboBox)obj;
            if (object.ReferenceEquals(args.Property, SelectedItemProperty))
            {
                self.SelectedItem = args.NewValue;
            }
            else if (object.ReferenceEquals(args.Property, ItemsSourceProperty))
            {
                self.ItemsSource = (IEnumerable)args.NewValue;
            }
            else if (object.ReferenceEquals(args.Property, PlaceholderTextProperty))
            {
                //self.placeholder.DisplayString = args.NewValue.ToString();
            }
        }

        private class EmptyItem : DynamicObject
        {
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                // just set the result to null and return true 
                result = null;
                return true;
            }
            public override string ToString()
            {
                return "None";
            }
        }
    }
}
