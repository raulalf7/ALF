using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ALF.UI.EduUI
{
    /// <summary>
    /// RegionTreeControl.xaml 的交互逻辑
    /// </summary>
    public partial class RegionTreeControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RegionTreeControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataBaseEngineType">数据库引擎类型</param>
        /// <param name="recordYear">统计年份</param>
        /// <param name="showLogo">是否显示区划树上图标</param>
        public RegionTreeControl(MSSQL.DataModel.DataBaseEngineType dataBaseEngineType, int recordYear, bool showLogo =false)
        {
            InitializeComponent();
            _recordYear = recordYear;
            _showLogo = showLogo;
            MSSQL.Tools.DataBaseType = dataBaseEngineType;
            try
            {
                AnalysisTypeComboBox.ItemsSource = new List<string>
                    {
                        "统计",
                        "采集",
                        "区划"
                    };
                Load(1, false);
                AnalysisTypeComboBox.SelectedIndex = 0;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        /// 加载区划树
        /// </summary>
        /// <param name="type">加载类型</param>
        /// <param name="isLock">是否锁定</param>
        public void Load(int type, bool isLock)
        {
            if (isLock)
            {
                AnalysisTypeComboBox.SelectedItem = "采集";
                AnalysisTypeComboBox.IsEnabled = false;
            }
            else
            {
                AnalysisTypeComboBox.IsEnabled = true;
            }
            AppType = type;
            TypeTreeView.Items.Clear();
            var list = EduTools.DataDB.code_getRegionTreeNodeList(AppType, "", "", "", "", 0, _recordYear).ToList();
            foreach (var current in list)
            {
                var newItem = new TreeViewItem
                {
                    Header = current,
                    Tag = current,
                    HeaderTemplate = TypeTreeView.ItemTemplate
                };
                TypeTreeView.Items.Add(newItem);
            }
        }


        #region Private Fields

        private readonly bool _showLogo;
        private readonly int _recordYear;

        #endregion


        #region Public Properties

        /// <summary>
        /// 选择改变事件
        /// </summary>
        public event EventHandler SelectChange;

        /// <summary>
        /// 选择类型
        /// </summary>
        public int AppType { get; set; }

        /// <summary>
        /// 选择节点
        /// </summary>
        public return_getRegionTreeNodeList SelectItem
        {
            get
            {
                var treeViewItem = TypeTreeView.SelectedItem as TreeViewItem;
                return_getRegionTreeNodeList result;
                if (treeViewItem == null)
                {
                    result = null;
                }
                else
                {
                    var returnGetRegionTreeNodeList = treeViewItem.Tag as return_getRegionTreeNodeList;
                    result = returnGetRegionTreeNodeList;
                }
                return result;
            }
        }

        /// <summary>
        /// 选择路径
        /// </summary>
        public string SelectedPath
        {
            get
            {
                string result;
                if (SelectItem == null)
                {
                    result = null;
                }
                else
                {
                    result = SelectItem.node1 + SelectItem.node2 + SelectItem.node3 + SelectItem.node4;
                }
                return result;
            }
        }

        #endregion


        #region Private Methods

        private void OnSelectChange()
        {
            if (SelectChange != null)
            {
                SelectChange(this, EventArgs.Empty);
            }
        }

        private void typeTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            OnSelectChange();
            var treeViewItem = TypeTreeView.SelectedItem as TreeViewItem;
            if (treeViewItem == null) return;
            if (treeViewItem.Items.Count != 0) return;
            var returnGetRegionTreeNodeList = treeViewItem.Tag as return_getRegionTreeNodeList;
            if (returnGetRegionTreeNodeList == null) return;
            if (returnGetRegionTreeNodeList.nodeLevel == 5) return;
            var singleResult =
                EduTools.DataDB.code_getRegionTreeNodeList(AppType,
                    returnGetRegionTreeNodeList.node1,
                    returnGetRegionTreeNodeList.node2,
                    returnGetRegionTreeNodeList.node3,
                    returnGetRegionTreeNodeList.node4, 0, _recordYear
                    );
            foreach (var current in singleResult)
            {
                var newItem = new TreeViewItem
                {
                    Header = current,
                    Tag = current,
                    HeaderTemplate = TypeTreeView.ItemTemplate
                };
                treeViewItem.Items.Add(newItem);
            }
        }

        private void RectLoaded(object sender, RoutedEventArgs e)
        {
            var rect = sender as Rectangle;
            if (rect == null)
            {
                return;
            }

            if (!_showLogo)
            {
                rect.Height = 0;
                rect.Width = 0;
                return;
            }
            var visual = rect.Fill as VisualBrush;
            if (visual == null)
            {
                return;
            }
            Console.WriteLine("");
            if (rect.Tag.ToString() == "采集")
            {
                visual.Visual = Application.Current.Resources["appbar_home_question"] as Visual;
            }
            if (rect.Tag.ToString() == "统计")
            {
                visual.Visual = Application.Current.Resources["appbar_home"] as Visual;
            }
            if (rect.Tag.ToString() == "代管")
            {
                visual.Visual = Application.Current.Resources["appbar_home_question"] as Visual;
            }
            rect.Fill = visual;
        }

        #endregion
    }
}
