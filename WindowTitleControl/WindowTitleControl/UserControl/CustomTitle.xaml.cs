﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindowTitleControl
{
    /// <summary>
    /// CustomTitle.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomTitle : UserControl
    {
        public static readonly DependencyProperty MeProperty =
            DependencyProperty.Register("Me",
                                        typeof(string),
                                        typeof(MainWindow),
                                        new PropertyMetadata("", new PropertyChangedCallback(MeChanged)),
                                        ValidateMe);
        public string Me
        {
            get { return (string)GetValue(MeProperty); }
            set { SetValue(MeProperty, value); }
        }
        static void MeChanged(DependencyObject property, DependencyPropertyChangedEventArgs args)
        {
            MainWindow main = property as MainWindow;

            string value1 = args.OldValue as string;
            string value2 = args.NewValue as string;
        }
        static bool ValidateMe(object obj)
        {
            string str = obj as string;
            return str != null;
        }


        public CustomTitleViewModel ViewModel
        {
            get
            {
                if (this.DataContext is null)
                {
                    this.DataContext = new CustomTitleViewModel();
                    return this.DataContext as CustomTitleViewModel;
                }
                else
                    return (this.DataContext as CustomTitleViewModel);
            }
            set
            {
                this.DataContext = value;
            }
        }

        public CustomTitle()
        {
            InitializeComponent();
            SubscribeToWindowState();
        }



        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Colled by Parent Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            Window targetWindow = Window.GetWindow(this);

            _ = targetWindow.WindowState == WindowState.Normal ?
                targetWindow.WindowState = WindowState.Maximized :
                targetWindow.WindowState = WindowState.Normal;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void ParentWindow_StateChanged(object sender, EventArgs e)
        {

            Window hostWindow = Window.GetWindow(this);
            // Window-State Depend Changing Style
            if (hostWindow.WindowState != WindowState.Maximized)
            {
                ViewModel.MaximizeButton_Height = 21;

                Style Maximize_Path_Style = (Style)FindResource("Maximize_Path_Style");
                foreach (Setter setters in Maximize_Path_Style.Setters)
                {
                    if (setters.Property.Name == "Height")
                        setters.Value = 21;
                    else if (setters.Property.Name == "Margin")
                        setters.Value = "1 0 2 0";
                }
                MaximizeButtonPath.Style = (Style)FindResource("Maximize_Path_Style");
            }
            else
            {
                ViewModel.MaximizeButton_Height = 29;

                //Style Maximize_Path_Style = (Style)FindResource("Maximize_Path_Style");
                //foreach (Setter setters in Maximize_Path_Style.Setters)
                //{


                //    if (setters.Property.Name == "Height")
                //        setters.Value = 29;
                //    else if (setters.Property.Name == "Margin")
                //        setters.Value = "1 0 0 0";
                //}
                MaximizeButtonPath.Style = (Style)FindResource("Maximized_Path_Style");
            }
        }

        private async void SubscribeToWindowState()
        {
            Window hostWindow = Window.GetWindow(this);

            while (hostWindow == null)
            {
                hostWindow = Window.GetWindow(this);
                await System.Threading.Tasks.Task.Delay(50);
            }

            hostWindow.StateChanged += new System.EventHandler(this.ParentWindow_StateChanged);

            //hostWindow.GetObservable(Window.WindowStateProperty).Subscribe(s =>
            //{
            //    if (s != WindowState.Maximized)
            //    {
            //        maximizeIcon.Data = Avalonia.Media.Geometry.Parse("M2048 2048v-2048h-2048v2048h2048zM1843 1843h-1638v-1638h1638v1638z");
            //        hostWindow.Padding = new Thickness(0, 0, 0, 0);
            //        maximizeToolTip.Content = "Maximize";
            //    }
            //    if (s == WindowState.Maximized)
            //    {
            //        maximizeIcon.Data = Avalonia.Media.Geometry.Parse("M2048 1638h-410v410h-1638v-1638h410v-410h1638v1638zm-614-1024h-1229v1229h1229v-1229zm409-409h-1229v205h1024v1024h205v-1229z");
            //        hostWindow.Padding = new Thickness(7, 7, 7, 7);
            //        maximizeToolTip.Content = "Restore Down";

            //        // This should be a more universal approach in both cases, but I found it to be less reliable, when for example double-clicking the title bar.
            //        /*hostWindow.Padding = new Thickness(
            //                hostWindow.OffScreenMargin.Left,
            //                hostWindow.OffScreenMargin.Top,
            //                hostWindow.OffScreenMargin.Right,
            //                hostWindow.OffScreenMargin.Bottom);*/
            //    }
            //});
        }


    }

}
