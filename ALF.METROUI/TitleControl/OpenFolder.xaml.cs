using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;
using MahApps.Metro.Controls;
using Binding = System.Windows.Data.Binding;
using TextBox = System.Windows.Controls.TextBox;

namespace ALF.METROUI.TitleControl
{
    /// <summary>
    /// TitleOpenFolder.xaml 的交互逻辑
    /// </summary>
    public partial class OpenFolder
    {
        public OpenFolder()
        {
            InitializeComponent();
        }


        #region Title Area

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


        #region Value Area

        public string Watermark
        {
            set { TextBoxHelper.SetWatermark(this, value); }
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

        public Brush ValueBackground
        {
            get { return valueText.Background; }
            set { valueText.Background = value; }
        }

        #endregion 


        #region Priavte Fields

        private bool _isLong;

        private string _fileFilter = "All Files|*.*";

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
        

        public string FileFilter
        {
            get { return _fileFilter; }
            set { _fileFilter = value; }
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

        private void ImageButton_OnClick(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            Value = dialog.SelectedPath;
        }
    }
}
