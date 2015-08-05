using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using MahApps.Metro.Controls;

namespace ALF.METROUI.TitleControl
{
    /// <summary>
    /// TitleText.xaml 的交互逻辑
    /// </summary>
    public partial class Text
    {
        public Text()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return titleText.Text; }
            set { titleText.Text = value; }
        }

        public string Value
        {
            get
            {
                return valueText.Text;
            }
            set
            {
                valueText.Text = value;
            }
        }

        public string Watermark
        {
            set { TextBoxHelper.SetWatermark(titleText, value); }
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
                mainGrid.ColumnDefinitions[0].Width = new GridLength(value);
            }
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


        private bool _isLong = false;
        public bool IsLong
        {
            get { return _isLong; }
            set
            {
                _isLong = value;
                if (value)
                {
                    mainGrid.Height = 200;
                    valueText.AcceptsReturn = true;
                    valueText.VerticalContentAlignment = VerticalAlignment.Top;
                    return;
                }
                mainGrid.Height = 50;
                valueText.AcceptsReturn = false;
                valueText.VerticalContentAlignment = VerticalAlignment.Stretch;
            }
        }


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

        public bool IsReadonly
        {
            get { return valueText.IsReadOnly; }
            set { valueText.IsReadOnly = value; }
        }

        public bool VanishBorder
        {
            set
            {
                if (value)
                {
                    valueText.BorderBrush = new SolidColorBrush(Colors.Transparent);
                }
                else
                {
                    valueText.BorderThickness = new Thickness(1);
                    valueText.BorderBrush = new SolidColorBrush(Colors.Black);
                }
            }
        }

        public Brush ValueBackground
        {
            get { return valueText.Background; }
            set { valueText.Background = value; }
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

            valueText.SetBinding(TextBox.TextProperty, myBinding);

        }

    }
}
