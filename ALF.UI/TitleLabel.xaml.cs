using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ALF.UI
{
    /// <summary>
    /// TitleLabel.xaml 的交互逻辑
    /// </summary>
    public partial class TitleLabel
    {
        #region Title Setting

        /// <summary>
        /// 构造函数
        /// </summary>
        public TitleLabel()
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

            ValueText.SetBinding(TextBlock.TextProperty, myBinding);

        }

        #endregion


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
    }
}
