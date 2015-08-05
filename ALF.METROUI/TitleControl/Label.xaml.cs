using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using MahApps.Metro.Controls;

namespace ALF.METROUI.TitleControl
{
    /// <summary>
    /// TitleLabel.xaml 的交互逻辑
    /// </summary>
    public partial class Label
    {
        public Label()
        {
            InitializeComponent();
        }


        #region TitleArea

        public string Title
        {
            get { return titleText.Text; }
            set { titleText.Text = value; }
        }

        public double TitleWidth
        {
            get
            {
                return mainGrid.ColumnDefinitions[0].Width.Value;
            }

            set
            {
                if ((int)value == -1)
                {
                    mainGrid.ColumnDefinitions[0].Width = GridLength.Auto;
                    return;
                }
                mainGrid.ColumnDefinitions[0].Width = new GridLength(value);
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

        #endregion


        #region Binding

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

        #endregion


        #region Value Area

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

        public double ValueWidth
        {
            get
            {
                return mainGrid.ColumnDefinitions[1].Width.Value;
            }

            set
            {
                if ((int)value == -1)
                {
                    mainGrid.ColumnDefinitions[1].Width = GridLength.Auto;
                    return;
                }
                mainGrid.ColumnDefinitions[1].Width = new GridLength(value);
            }
        }

        public string Watermark
        {
            set { TextBoxHelper.SetWatermark(this, value); }
        }

        public Brush ValueBackground
        {
            get { return valueText.Background; }
            set { valueText.Background = value; }
        }

        #endregion

    }
}
