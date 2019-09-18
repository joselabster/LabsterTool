using Hardcodet.Wpf.TaskbarNotification;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace LabsterHub
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool m_isExplicitClose = false;// Indicate if it is an explicit form close request from the user.

        public MainWindow()
        {

            // Properties.Settings.Default.Reset();

            InitializeComponent();

            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            ListViewMenu.SelectedIndex = 0;
            ListViewMenu_SelectionChanged(null, null);

        }

        private void ButtonPopUpLogout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;

            switch (index)
            {
                case 0: // Main
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new UserControlInit());
                    MoveCurosrMenu(index);
                    break;
                case 1: // Github
                    Process.Start("https://github.com/Livit");
                    break;
                default:
                    GridPrincipal.Children.Clear();
                    break;

            }
        }

        private void MoveCurosrMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, (150 + (60 * index)), 0 ,0);
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            //Set this to unclock the Minimize on close 
            m_isExplicitClose = true;
            this.Close();
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Visible;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {

            base.OnClosing(e);

            if (m_isExplicitClose == false)//NOT a user close request? ... then hide
            {
                e.Cancel = true;
                this.Visibility = Visibility.Collapsed;
            }

        }

    }
}
