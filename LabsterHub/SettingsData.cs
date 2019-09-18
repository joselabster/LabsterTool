using System;
using System.IO;

namespace LabsterHub
{
    class SettingsData
    {

        public enum DataId
        {
            RepositoryPath ,            // Path to the directory path
            PlatformPath,               // Path to the platform folder
            BuilderPath,                // Path to builder settings
            SimulationPath,             // Path to the Simulation folder
            QuizPath,                   // Path to the quizblock folder
            AutoSwitchSimulationEnabled,// To Remeber the last setting for this
        }

        private static readonly object padlock = new object();
        private static SettingsData instance = null;
        public static SettingsData Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SettingsData();
                    }
                    return instance;
                }
            }
        }

        SettingsData()
        {

        }

        public void Reset()
        {
            Properties.Settings.Default.Reset();
        }

        public string Load(DataId id)
        {
            return Properties.Settings.Default[id.ToString()].ToString(); 
        }

        public void Save(DataId id, string value)
        {
            Properties.Settings.Default[id.ToString()] = value;
            Properties.Settings.Default.Save();
        }

    }
}