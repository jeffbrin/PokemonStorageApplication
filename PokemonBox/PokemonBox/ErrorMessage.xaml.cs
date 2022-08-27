using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PokemonBox
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ErrorMessage : Window
    {
        MessageBoxResult button1Result, button2Result, button3Result;
        private MessageBoxResult ChooseErrorMessageresult { get; set; }

        public ErrorMessage(string errorMessage, string caption, string buttonText1, MessageBoxResult result1, string buttonText2 = "", MessageBoxResult result2 = MessageBoxResult.None, string buttonText3 = "", MessageBoxResult result3 = MessageBoxResult.None)
        {
            InitializeComponent();
            txtError.Text = errorMessage;
            txtCaption.Text = caption;
            btnError1.Content = buttonText1;
            button1Result = result1;

            // Set the text of the second button, remove otherwise
            if (buttonText2 != string.Empty)
            {
                btnError2.Content = buttonText2;
                button2Result = result2;

            }
            else
                this.grdButtons.Children.Remove((UIElement)FindName("btnError2"));

            // Set the text of the third button, remove otherwise
            if (buttonText3 != string.Empty)
            {
                btnError3.Content = buttonText3;
                button3Result = result3;
            }
            else
                this.grdButtons.Children.Remove((UIElement)FindName("btnError3"));

        }

        private void grdHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private bool MouseOnMaximizeOrMinimize()
        {
            return this.imgMinus.IsMouseOver;
        }
        private void btnClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void imgMinus_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void FixMargins()
        {
            if (WindowState == WindowState.Maximized)
            {
                Thickness margin = grdMain.Margin;
                margin.Left = 8;
                margin.Right = 8;
                margin.Top = 8;
                margin.Bottom = 8;
                grdMain.Margin = margin;
            }
            else
            {
                ResizeMode = ResizeMode.CanResizeWithGrip;
                Thickness margin = grdMain.Margin;
                margin.Left = 0;
                margin.Right = 0;
                margin.Top = 0;
                margin.Bottom = 0;
                grdMain.Margin = margin;
            }
        }
        private void imgMax_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
                ResizeMode = ResizeMode.NoResize;
                Thickness margin = grdMain.Margin;
                margin.Left = 8;
                margin.Right = 8;
                margin.Top = 8;
                margin.Bottom = 8;
                grdMain.Margin = margin;
            }
            else
            {
                this.WindowState = WindowState.Normal;
                ResizeMode = ResizeMode.CanResizeWithGrip;
                Thickness margin = grdMain.Margin;
                margin.Left = 0;
                margin.Right = 0;
                margin.Top = 0;
                margin.Bottom = 0;
                grdMain.Margin = margin;
            }

        }

        // Shows the box and returns the result

        public MessageBoxResult ShowBox()
        {
            this.ShowDialog();
            return ChooseErrorMessageresult;
        }

        // These are used to set the result by different buttons, could have been done by changing each button's
        // click event but we don't have a good understanding of how events work and we didn't want to take something straight off stack overflow
        // click event but we don't have a good understanding of how events work and we didn't want to take something straight off stack overflow

        private void btnError1_Click(object sender, RoutedEventArgs e)
        {
            ChooseErrorMessageresult = button1Result;
            this.Close();
        }

        private void btnError2_Click(object sender, RoutedEventArgs e)
        {
            ChooseErrorMessageresult = button2Result;
            this.Close();
        }
        private void btnError3_Click(object sender, RoutedEventArgs e)
        {
            ChooseErrorMessageresult = button3Result;
            this.Close();
        }
        private void XButtonClose(object sender, RoutedEventArgs e)
        {
            ChooseErrorMessageresult = MessageBoxResult.Cancel;
            this.Close();
        }
    }
}
