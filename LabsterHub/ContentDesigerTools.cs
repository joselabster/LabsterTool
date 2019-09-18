using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LabsterHub
{
    public sealed class ContentDesigerTools
    {

        private static readonly object padlock = new object();
        private static ContentDesigerTools instance = null;
        public static ContentDesigerTools Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ContentDesigerTools();
                    }
                    return instance;
                }
            }
        }

        private FileSystemWatcher watcher;

        // private static string RepositoryPath = @"C:\Users\Labster\Labster\";
        // private static string PlatformPath = DirectoryPath + @"Platform\";
        // private static string SimulationPath = DirectoryPath + @"Simulations\Simulations\";
        // private static string QuizPath = SimulationPath + @"QuizBlocks\";

        ContentDesigerTools()
        {
            SettingsData settings = SettingsData.Instance;
            this.watcher = new FileSystemWatcher();
        }

        ///// Simulation Switch
        public void EnableAutoSimulationSwitcher()
        {

            var path = SettingsData.Instance.Load(SettingsData.DataId.SimulationPath);

            if (Directory.Exists(path) == false)
            {
                throw new Exception("DirectoryPath doesn't exist");
            }

            this.watcher.Path = path;

            // Watch for changes in LastAccess and LastWrite times, and the renaming of files or directories.
            this.watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            // Only watch the engine file.
            this.watcher.Filter = "Engine_*.xml";

            // Add event handlers.
            this.watcher.Changed += new FileSystemEventHandler(OnChanged);
            this.watcher.Created += new FileSystemEventHandler(OnChanged);
            this.watcher.Deleted += new FileSystemEventHandler(OnChanged);

            watcher.EnableRaisingEvents = true;
        }

        public void DisableAutoSimulationSwitcher()
        {
            watcher.EnableRaisingEvents = false;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {

            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);

            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Deleted:
                    {
                        // If file is deleted, means we have change simulation. Wait until a new engine file is created
                    }
                    break;
                case WatcherChangeTypes.Created:
                    {
                        // New engine file created, let's update settings
                        UpdateSettings();
                    }
                    break;

            }

        }

        public void UpdateSettings()
        {
            string platformSimSetting = GetCurrentSimulationInPlatformSettings();
            string builderSimSetting = GetCurrentSimulationInBuilderSettings();

            string simSimulation = GetCurrentSimulationInSimulationFolder();

            if (platformSimSetting != null && simSimulation != null)
            {
                string PlatformPath = SettingsData.Instance.Load(SettingsData.DataId.PlatformPath);
                UpdatePlaformSettingEngine(PlatformPath);
            }

            if (builderSimSetting != null && simSimulation != null)
            {
                string BuilderPath = SettingsData.Instance.Load(SettingsData.DataId.BuilderPath);
                UpdatePlaformSettingEngine(BuilderPath);
            }
        }

        private void UpdatePlaformSettingEngine(string path)
        {

            System.Threading.Thread.Sleep(250);

            string settingPathFile = path + "Settings.xml";

            string engineFile = GetCurrentSimulationInSimulationFolder();
            string quizFile = GetCurrentQuizSimulationInSimulationFolder();

            XmlDocument docPlatform = new XmlDocument();
            docPlatform.Load(settingPathFile);
            XmlNode root = docPlatform.DocumentElement;
            root.Attributes["EngineXML"].Value = engineFile;

            // Update Quiz
            {
                XmlNodeList xnList = root.SelectNodes("Server/ServerAPI[@Id='QuizBlock']");

                if (xnList.Count == 0 || xnList.Count > 1)
                {
                    throw new Exception("Found is wrong. Found many quizblocks attributes ):");
                }

                foreach (XmlNode xn in xnList)
                {
                    xn.Attributes["Path"].Value = ReplaceQuiz(xn.Attributes["Path"].Value, quizFile);
                }

            }

            docPlatform.Save(settingPathFile);

        }

        private string ReplaceQuiz(string original, string newQuiz)
        {

            original = original.Replace('/', '\\');

            if (original.StartsWith(@"file:\\\") == true)
            {
                original = original.Replace(@"file:\\\", @"file:///");
            }

            string[] words = original.Split('\\');

            words[words.Length - 1] = newQuiz;

            StringBuilder builder = new StringBuilder();
            foreach (string value in words)
            {
                builder.Append(value);
                builder.Append('\\');
            }

            builder.Remove(builder.Length - 1, 1);

            return builder.ToString();

        }

        private string GetCurrentSimulationInPlatformSettings()
        {
            string PlatformPath = SettingsData.Instance.Load(SettingsData.DataId.PlatformPath);

            if (PlatformPath.Length == 0)
                return null;

            XmlDocument docPlatform = new XmlDocument();
            docPlatform.Load(PlatformPath + "Settings.xml");
            XmlNode root = docPlatform.DocumentElement;
            return root.Attributes["EngineXML"].Value;
        }

        private string GetCurrentSimulationInBuilderSettings()
        {
            string BuilderPath = SettingsData.Instance.Load(SettingsData.DataId.BuilderPath);

            if (BuilderPath.Length == 0)
                return null;

            XmlDocument docPlatform = new XmlDocument();
            docPlatform.Load(BuilderPath + "Settings.xml");
            XmlNode root = docPlatform.DocumentElement;
            return root.Attributes["EngineXML"].Value;
        }

        private string GetCurrentSimulationInSimulationFolder()
        {
            var SimulationPath = SettingsData.Instance.Load(SettingsData.DataId.SimulationPath);

            if (SimulationPath.Length == 0)
                return null;

            DirectoryInfo d = new DirectoryInfo(SimulationPath);
            FileInfo[] Files = d.GetFiles("*.xml");
            foreach (FileInfo file in Files)
            {
                if (file.Name.StartsWith("Engine_"))
                {
                    return file.Name;
                }
            }
            return "";
        }

        private string GetCurrentQuizSimulationInSimulationFolder()
        {
            var QuizPath = SettingsData.Instance.Load(SettingsData.DataId.QuizPath);
            DirectoryInfo d = new DirectoryInfo(QuizPath);
            FileInfo[] Files = d.GetFiles("*.xml");
            foreach (FileInfo file in Files)
            {
                if (file.Name.StartsWith("QuizBlocks_"))
                {
                    return file.Name;
                }
            }
            return "";
        }

    }
}
