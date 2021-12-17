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
        public enum ErrorMessageResult
        {
            //
            // Summary:
            //     The message box returns no result.
            None = 0,
            //
            // Summary:
            //     The result value of the message box is OK.
            OK = 1,
            //
            // Summary:
            //     The result value of the message box is Cancel.
            Cancel = 2,
            //
            // Summary:
            //     The result value of the message box is Yes.
            Yes = 6,
            //
            // Summary:
            //     The result value of the message box is No.
            No = 7
        }
        public ErrorMessageResult ChooseErrorMessageresult
        {
            get
            {
                if (btnError1.IsPressed)
                {
                    return ErrorMessageResult.Yes;
                }
                if (btnError2.IsPressed)
                {
                    return ErrorMessageResult.No;
                }
                return ErrorMessageResult.Cancel;
            }
        }

        public ErrorMessage(string errorMessage)
        {
            InitializeComponent();
            txtError.Text = errorMessage;
        }
        private void grdHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private bool MouseOnMaximizeOrMinimize()
        {
            return this.imgMinus.IsMouseOver;
        }
        private void imgClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        private void btnErrorClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
