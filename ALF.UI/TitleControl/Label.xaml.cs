using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ALF.UI.TitleControl
{
    /// <summary>
    /// TitleLabel.xaml 的交互逻辑
    /// </summary>
    public partial class Label
    {
        #region Title Setting

        /// <summary>
        /// 构造函数
        /// </summary>
        public Label()
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

            valueText.SetBinding(TextBlock.TextProperty, myBinding);

        }

        #endregion


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
    }
}
