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
            set { Panel.Orientation = value; }
            get { return Panel.Orientation; }
        }

        public Visibility ImageTextVisibility
        {

            set { text.Visibility = value; }
            get { return text.Visibility; }
        }

        public double ImageSize
        {
            set
            {
                Rect.Width = value;
                Rect.Height = value;
            }
            get { return Rect.Width; }
        }

        public double ButtonSize
        {
            set
            {
                ButtonBase.Width = value;
                ButtonBase.Height = value;
            }
            get { return Rect.Width; }
        }

        public double ButtonWidth
        {
            get
            {
                return ButtonBase.Width;
            }

            set
            {
                ButtonBase.Width = value;
            }
        }

        public Stretch ImageStretch
        {
            
            set
            {
                if (Rect == null)
                {
                    return;
                }
                var visual = Rect.Fill as VisualBrush;
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
                if (Rect == null)
                {
                    return;
                }
                var visual = Rect.Fill as VisualBrush;
                if (visual == null)
                {
                    return;
                }
                visual.Visual = Application.Current.Resources[value] as Visual;
                Rect.Fill = visual;
            }
        }

        public string Text
        {
            get
            {
                return text.Text;
            }
            set
            {
                text.Text = value;
            }
        }

        public string ButtonStyle
        {
            set { ButtonBase.Style = Application.Current.Resources[value] as Style; }
            get { return ButtonBase.Style.ToString(); }
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
