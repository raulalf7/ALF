using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ALF.SILVERLIGHT.TitleControl
{
    /// <summary>
    /// TitleText.xaml 的交互逻辑
    /// </summary>
    public partial class Text
    {
        #region Title Setting

        /// <summary>
        /// 构造函数
        /// </summary>
        public Text()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return titleText.Text; }
            set { titleText.Text = value; }
        }

        /// <summary>
        /// 标题区域宽度
        /// </summary>
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

        /// <summary>
        /// 值区域宽度
        /// </summary>
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

        /// <summary>
        /// 标题颜色
        /// </summary>
        public Brush TitleColor
        {
            get { return titleText.Foreground; }
            set { titleText.Foreground = value; }
        }

        /// <summary>
        /// 标题大小
        /// </summary>
        public double TitleSize
        {
            get { return titleText.FontSize; }
            set { titleText.FontSize = value; }
        }

        #endregion


        #region Binding

        private string _bindingString;

        /// <summary>
        /// 绑定属性名称
        /// </summary>
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

        /// <summary>
        /// 设置绑定
        /// </summary>
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



        private bool _isLong;

        /// <summary>
        /// 是否长字符串
        /// </summary>
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
                    valueText.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                    valueText.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                    return;
                }
                mainGrid.Height = 50;
                valueText.AcceptsReturn = false;
                valueText.VerticalContentAlignment = VerticalAlignment.Stretch;
            }
        }

        /// <summary>
        /// 值
        /// </summary>
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
        
        /// <summary>
        /// 值背景颜色
        /// </summary>
        public Brush ValueBackground
        {
            get { return valueText.Background; }
            set { valueText.Background = value; }
        }

        
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadonly
        {
            get { return valueText.IsReadOnly; }
            set { valueText.IsReadOnly = value; }
        }

        /// <summary>
        /// 是否显示边框
        /// </summary>
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

    }
}
