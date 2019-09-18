using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace LabsterHub
{
    /// <summary>
    /// Interaction logic for UserControlInit.xaml
    /// </summary>
    public partial class UserControlInit : UserControl
    {
        public UserControlInit()
        {
            InitializeComponent();

            LoadSettings();
        }

        private void LoadSettings()
        {

            {
                DirectoryPathtextBox.Text = SettingsData.Instance.Load(SettingsData.DataId.RepositoryPath);
            }
            {
                bool result = false;
                bool.TryParse(SettingsData.Instance.Load(SettingsData.DataId.AutoSwitchSimulationEnabled), out result);
                AutoSwitchSimulation.IsChecked = result;
            }

            // All settings loaded into control, let's switch to the simulation so we the platform file is updated
            Button_Switch_Simulation_Click(null, null);
            // Also let's start to watch the engine so we can update
            AutoSwitchSimulation_Click(null, null);
        }

        private void Button_Switch_Simulation_Click(object sender, RoutedEventArgs e)
        {
            ContentDesigerTools.Instance.UpdateSettings();
        }

        private void DirectoryPathtextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var path = DirectoryPathtextBox.Text;
            if (Directory.Exists(path) == true)
            {

                string DirectoryPath = path;
                string PlatformPath = DirectoryPath + @"\Platform\";
                string SimulationPath = DirectoryPath + @"\Simulations\Simulations\";
                string QuizPath = SimulationPath + @"\QuizBlocks\";
                string BuilderPath = PlatformPath + @"\Livit.Learn.BuilderUI\";

                SettingsData.Instance.Save(SettingsData.DataId.RepositoryPath, DirectoryPath);
                SettingsData.Instance.Save(SettingsData.DataId.PlatformPath, PlatformPath);
                SettingsData.Instance.Save(SettingsData.DataId.SimulationPath, SimulationPath);
                SettingsData.Instance.Save(SettingsData.DataId.QuizPath, QuizPath);
                SettingsData.Instance.Save(SettingsData.DataId.BuilderPath, BuilderPath);

            }
        }

        private void AutoSwitchSimulation_Click(object sender, RoutedEventArgs e)
        {
            if (AutoSwitchSimulation.IsChecked.Value == true)
            {
                ContentDesigerTools.Instance.EnableAutoSimulationSwitcher();
            }
            else
            {
                ContentDesigerTools.Instance.DisableAutoSimulationSwitcher();

            }
            SettingsData.Instance.Save(SettingsData.DataId.AutoSwitchSimulationEnabled, AutoSwitchSimulation.IsChecked.Value.ToString());
        }

        private void Button_Reload_Simulation_Click(object sender, RoutedEventArgs e)
        {
            ContentDesigerTools.Instance.UpdateSettings();
        }
    }
}
