using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ALF.METROUI
{
    /// <summary>
    /// ImageButton.xaml 的交互逻辑
    /// </summary>
    public partial class ImageButton
    {
        public ImageButton()
        {
            InitializeComponent();
        }

        public event EventHandler Click;
        
        public Orientation ImageOrientation
        {
            set { panel.Orientation = value; }
            get { return panel.Orientation; }
        }

        public Visibility ImageTextVisibility
        {

            set { textBlock.Visibility = value; }
            get { return textBlock.Visibility; }
        }

        public double ImageSize
        {
            set
            {
                rect.Width = value;
                rect.Height = value;
            }
            get { return rect.Width; }
        }

        public double ButtonSize
        {
            set
            {
                buttonBase.Width = value;
                buttonBase.Height = value;
            }
            get { return rect.Width; }
        }

        public double ButtonWidth
        {
            get
            {
                return buttonBase.Width;
            }

            set
            {
                buttonBase.Width = value;
            }
        }

        public Stretch ImageStretch
        {
            
            set
            {
                if (rect == null)
                {
                    return;
                }
                var visual = rect.Fill as VisualBrush;
                if (visual == null)
                {
                    return;
                }
                visual.Stretch = value;
            }
        }

        public string ImageName
        {
            set
            {
                if (rect == null)
                {
                    return;
                }
                var visual = rect.Fill as VisualBrush;
                if (visual == null)
                {
                    return;
                }
                visual.Visual = Application.Current.Resources[value] as Visual;
                rect.Fill = visual;
            }
        }

        public string Text
        {
            get
            {
                return textBlock.Text;
            }
            set
            {
                textBlock.Text = value;
            }
        }

        public string ButtonStyle
        {
            set { buttonBase.Style = Application.Current.Resources[value] as Style; }
            get { return buttonBase.Style.ToString(); }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (Click != null)
            {
                Click(sender, e);
            }
        }
    }
}
