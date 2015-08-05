using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace ALF.METROUI.TitleControl
{
    /// <summary>
    /// TitleCombo.xaml 的交互逻辑
    /// </summary>
    public partial class Combo
    {

        public Combo()
        {
            InitializeComponent();
            valueCombo.ItemsSource = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        }

        public object SelectedItem
        {
            get
            {
                return valueCombo.SelectedItem;
            }
            set
            {
                valueCombo.SelectedItem = value;
            }
        }

        public int SelectedIndex
        {
            get
            {
                return valueCombo.SelectedIndex;
            }
            set
            {
                valueCombo.SelectedIndex = value;
            }
        }

        private string _bindingString;

        public string Binding
        {
            set
            {
                _bindingString = value;
                SetBinding();
            }
            get
            {
                return _bindingString;
            }
        }

        public void SetBinding()
        {
            var myBinding = new Binding(_bindingString)
            {
                Source = DataContext,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            valueCombo.SetBinding(Selector.SelectedItemProperty, myBinding);
        }

        public IEnumerable<object> ItemsSource
        {
            set
            {
                valueCombo.ItemsSource = null;
                valueCombo.ItemsSource = value;
            }
        }

        public string Items
        {
            set
            {
                var itemsList = value.Split(',');
                ItemsSource = itemsList;
            }
            get
            {
                return "";
            }
        }

        public string DisplayMemberPath
        {
            set
            {
                valueCombo.DisplayMemberPath = value;
            }
            get
            {
                return valueCombo.DisplayMemberPath;
            }
        }

        public bool IsEnable
        {
            get { return valueCombo.IsEnabled; }
            set { valueCombo.IsEnabled = value; }
        }

        public string Title
        {
            get { return titleText.Text; }
            set { titleText.Text = value; }
        }

        public double ValueWidth
        {
            get
            {
                return mainGrid.ColumnDefinitions[1].Width.Value;
            }

            set
            {
                if (value == -1)
                {
                    mainGrid.ColumnDefinitions[1].Width = GridLength.Auto;
                    return;
                }
                mainGrid.ColumnDefinitions[1].Width = new GridLength(value);
            }
        }

        public double TitleWidth
        {
            get
            {
                return mainGrid.ColumnDefinitions[0].Width.Value;
            }

            set
            {
                if (value == -1)
                {
                    mainGrid.ColumnDefinitions[0].Width = GridLength.Auto;
                    return;
                }
                mainGrid.ColumnDefinitions[0].Width   = new GridLength(value);
            }
        }

        private void ValueCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(sender, e);
            }
        }

        public event EventHandler SelectionChanged;

        public Brush TitleColor
        {
            get { return titleText.Foreground; }
            set { titleText.Foreground = value; }
        }

        public double TitleSize
        {
            get { return titleText.FontSize; }
            set { titleText.FontSize = value; }
        }

    }
}
