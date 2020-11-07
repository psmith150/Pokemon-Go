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
            emptyItem = new EmptyItem()
            {
                DisplayString = this.PlaceholderText
            };
        }

        #region Private Fields
        EmptyItem emptyItem;
        #endregion

        #region Public Properties
        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
                List<object> listWithNull = new List<object>();
                listWithNull.Add(emptyItem);
                if (value != null)
                {
                    foreach (var element in value)
                    {
                        listWithNull.Add(element);
                    }
                }
                else
                {
                    BindingOperations.ClearAllBindings(combo);
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
                    combo.SelectedItem = emptyItem;
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
                emptyItem.DisplayString = value;
            }
        }

        public int AlternationCount
        {
            get
            {
                return (int)GetValue(AlternationCountProperty);
            }
            set
            {
                SetValue(AlternationCountProperty, value);
            }
        }
        public string DisplayMemberPath
        {
            get
            {
                return (string)GetValue(DisplayMemberPathProperty);
            }
            set
            {
                combo.DisplayMemberPath = value;
                SetValue(DisplayMemberPathProperty, value);
            }
        }
        public bool IsDropDownOpen
        {
            get
            {
                return (bool)GetValue(IsDropDownOpenProperty);
            }
            set
            {
                SetValue(IsDropDownOpenProperty, value);
            }
        }
        public bool HasItems
        {
            get
            {
                return combo.HasItems;
            }
        }
        public bool IsEditable
        {
            get
            {
                return (bool)GetValue(IsEditableProperty);
            }
            set
            {
                SetValue(IsEditableProperty, value);
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return (bool)GetValue(IsReadOnlyProperty);
            }
            set
            {
                SetValue(IsReadOnlyProperty, value);
            }
        }
        public bool? IsSynchronizedWithCurrentItem
        {
            get
            {
                return (bool)GetValue(IsSynchronizedWithCurrentItemProperty);
            }
            set
            {
                SetValue(IsSynchronizedWithCurrentItemProperty, value);
            }
        }
        public string ItemStringFormat
        {
            get
            {
                return (string)GetValue(ItemStringFormatProperty);
            }
            set
            {
                SetValue(ItemStringFormatProperty, value);
            }
        }
        public int SelectedIndex
        {
            get
            {
                return (int)GetValue(SelectedIndexProperty);
            }
            set
            {
                SetValue(SelectedIndexProperty, value);
            }
        }
        public object SelectedValue
        {
            get
            {
                return (object)GetValue(SelectedValueProperty);
            }
            set
            {
                SetValue(SelectedValueProperty, value);
            }
        }
        public string SelectedValuePath
        {
            get
            {
                return (string)GetValue(SelectedValuePathProperty);
            }
            set
            {
                SetValue(SelectedValuePathProperty, value);
            }
        }
        public object SelectionBoxItem
        {
            get
            {
                return combo.SelectionBoxItem;
            }
        }
        public bool StaysOpenOnEdit
        {
            get
            {
                return (bool)GetValue(StaysOpenOnEditProperty);
            }
            set
            {
                SetValue(StaysOpenOnEditProperty, value);
            }
        }
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }
        #endregion
        #region Dependency Properties
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(NullableComboBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDependencyPropertyChanged));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(NullableComboBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDependencyPropertyChanged));

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(NullableComboBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDependencyPropertyChanged));

        public static readonly DependencyProperty AlternationCountProperty =
            DependencyProperty.Register("AlternationCount", typeof(int), typeof(NullableComboBox), new FrameworkPropertyMetadata(ComboBox.AlternationCountProperty.DefaultMetadata.DefaultValue));

        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(NullableComboBox), new FrameworkPropertyMetadata(ComboBox.DisplayMemberPathProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDependencyPropertyChanged));

        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(NullableComboBox), new FrameworkPropertyMetadata(ComboBox.IsDropDownOpenProperty.DefaultMetadata.DefaultValue));

        public static readonly DependencyProperty HasItemsProperty =
            DependencyProperty.Register("HasItems", typeof(bool), typeof(NullableComboBox), new FrameworkPropertyMetadata(ComboBox.HasItemsProperty.DefaultMetadata.DefaultValue));

        public static readonly DependencyProperty IsEditableProperty =
            DependencyProperty.Register("IsEditable", typeof(bool), typeof(NullableComboBox), new FrameworkPropertyMetadata(ComboBox.IsEditableProperty.DefaultMetadata.DefaultValue));

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(NullableComboBox), new FrameworkPropertyMetadata(ComboBox.IsReadOnlyProperty.DefaultMetadata.DefaultValue));

        public static readonly DependencyProperty IsSynchronizedWithCurrentItemProperty =
            DependencyProperty.Register("IsSynchronizedWithCurrentItem", typeof(bool?), typeof(NullableComboBox), new FrameworkPropertyMetadata(ComboBox.IsSynchronizedWithCurrentItemProperty.DefaultMetadata.DefaultValue));

        public static readonly DependencyProperty ItemStringFormatProperty =
            DependencyProperty.Register("ItemStringFormat", typeof(string), typeof(NullableComboBox), new FrameworkPropertyMetadata(ComboBox.ItemStringFormatProperty.DefaultMetadata.DefaultValue));

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(NullableComboBox), new FrameworkPropertyMetadata(ComboBox.SelectedIndexProperty.DefaultMetadata.DefaultValue));

        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(object), typeof(NullableComboBox), new FrameworkPropertyMetadata(ComboBox.SelectedValueProperty.DefaultMetadata.DefaultValue));

        public static readonly DependencyProperty SelectedValuePathProperty =
            DependencyProperty.Register("SelectedValuePath", typeof(string), typeof(NullableComboBox), new FrameworkPropertyMetadata(ComboBox.SelectedValuePathProperty.DefaultMetadata.DefaultValue));

        public static readonly DependencyProperty SelectionBoxItemProperty =
            DependencyProperty.Register("SelectionBoxItem", typeof(object), typeof(NullableComboBox), new FrameworkPropertyMetadata(ComboBox.SelectionBoxItemProperty.DefaultMetadata.DefaultValue));

        public static readonly DependencyProperty StaysOpenOnEditProperty =
            DependencyProperty.Register("StaysOpenOnEdit", typeof(bool), typeof(NullableComboBox), new FrameworkPropertyMetadata(ComboBox.StaysOpenOnEditProperty.DefaultMetadata.DefaultValue));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(NullableComboBox), new FrameworkPropertyMetadata(ComboBox.TextProperty.DefaultMetadata.DefaultValue));
        #endregion

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combo.SelectedItem is EmptyItem)
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
                self.emptyItem.DisplayString = args.NewValue.ToString();
            }
            else if (object.ReferenceEquals(args.Property, DisplayMemberPathProperty))
            {
                self.DisplayMemberPath = args.NewValue.ToString();
            }
        }


    }
    public class EmptyItem : DynamicObject
    {
        public string DisplayString { get; set; }
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            // Return the instance, so that paths with nested properties will keep returning the instance until the last property is reached, which will call ToString()
            result = this;
            return true;
        }
        public override string ToString()
        {
            return DisplayString;
        }
    }
}
