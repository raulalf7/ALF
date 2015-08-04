using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace ALF.UI
{
    /// <summary>
    /// TitleCombo.xaml 的交互逻辑
    /// </summary>
    public partial class TitleCombo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TitleCombo()
        {
            InitializeComponent();
            ValueCombo.ItemsSource = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        }

        #region TitleArea

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

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable
        {
            get { return ValueCombo.IsEnabled; }
            set { ValueCombo.IsEnabled = value; }
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

            ValueCombo.SetBinding(Selector.SelectedItemProperty, myBinding);
        }

        #endregion


        /// <summary>
        /// 所选对象
        /// </summary>
        public object SelectedItem
        {
            get
            {
                return ValueCombo.SelectedItem;
            }
            set
            {
                ValueCombo.SelectedItem = value;
            }
        }

        /// <summary>
        /// 所选序号
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return ValueCombo.SelectedIndex;
            }
            set
            {
                ValueCombo.SelectedIndex = value;
            }
        }

        /// <summary>
        /// 绑定数据源
        /// </summary>
        public IEnumerable<object> ItemsSource
        {
            set
            {
                ValueCombo.ItemsSource = null;
                ValueCombo.ItemsSource = value;
            }
        }

        /// <summary>
        /// 数据源字符串
        /// </summary>
        public string Items
        {
            set
            {
                var itemsList = value.Split(',');
                ItemsSource = itemsList;
            }
        }

        /// <summary>
        /// 显示属性名称
        /// </summary>
        public string DisplayMemberPath
        {
            set
            {
                ValueCombo.DisplayMemberPath = value;
            }
            get
            {
                return ValueCombo.DisplayMemberPath;
            }
        }

        /// <summary>
        /// 选择改变事件处理
        /// </summary>
        public event EventHandler SelectionChanged;

        private void ValueCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(sender, e);
            }
        }

    }
}
