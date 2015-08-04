using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ALF.UI
{
    /// <summary>
    /// TitleText.xaml 的交互逻辑
    /// </summary>
    public partial class TitleTextBox
    {
        #region Title Setting

        /// <summary>
        /// 构造函数
        /// </summary>
        public TitleTextBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return TitleText.Text; }
            set { TitleText.Text = value; }
        }

        /// <summary>
        /// 标题区域宽度
        /// </summary>
        public double TitleWidth
        {
            get
            {
                return MainGrid.ColumnDefinitions[0].Width.Value;
            }

            set
            {
                if ((int)value == -1)
                {
                    MainGrid.ColumnDefinitions[0].Width = GridLength.Auto;
                    return;
                }
                MainGrid.ColumnDefinitions[0].Width = new GridLength(value);
            }
        }

        /// <summary>
        /// 值区域宽度
        /// </summary>
        public double ValueWidth
        {
            get
            {
                return MainGrid.ColumnDefinitions[1].Width.Value;
            }

            set
            {
                if ((int)value == -1)
                {
                    MainGrid.ColumnDefinitions[1].Width = GridLength.Auto;
                    return;
                }
                MainGrid.ColumnDefinitions[1].Width = new GridLength(value);
            }
        }

        /// <summary>
        /// 标题颜色
        /// </summary>
        public Brush TitleColor
        {
            get { return TitleText.Foreground; }
            set { TitleText.Foreground = value; }
        }

        /// <summary>
        /// 标题大小
        /// </summary>
        public double TitleSize
        {
            get { return TitleText.FontSize; }
            set { TitleText.FontSize = value; }
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

            ValueText.SetBinding(TextBox.TextProperty, myBinding);

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
                    MainGrid.Height = 200;
                    ValueText.AcceptsReturn = true;
                    ValueText.VerticalContentAlignment = VerticalAlignment.Top;
                    ValueText.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                    ValueText.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                    return;
                }
                MainGrid.Height = 50;
                ValueText.AcceptsReturn = false;
                ValueText.VerticalContentAlignment = VerticalAlignment.Stretch;
            }
        }

        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get
            {
                return ValueText.Text;
            }
            set
            {
                ValueText.Text = value;
            }
        }
        
        /// <summary>
        /// 值背景颜色
        /// </summary>
        public Brush ValueBackground
        {
            get { return ValueText.Background; }
            set { ValueText.Background = value; }
        }

        
        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadonly
        {
            get { return ValueText.IsReadOnly; }
            set { ValueText.IsReadOnly = value; }
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
                    ValueText.BorderBrush = new SolidColorBrush(Colors.Transparent);
                }
                else
                {
                    ValueText.BorderThickness = new Thickness(1);
                    ValueText.BorderBrush = new SolidColorBrush(Colors.Black);
                }
            }
        }

    }
}
