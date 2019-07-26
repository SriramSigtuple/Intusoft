using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTUSOFT.Configuration.AdvanceSettings;
using Newtonsoft.Json;

namespace PrintSettings
{
    public class VariousPPSettings
    {
        public List<PostProcessingSettings> postProcessingSettingsList;
        private static VariousPPSettings ppSettings;
        static string ppSettingFileName = @"VariousPPSettings.json";
        public VariousPPSettings()
        {
            
        }

        private static void AddDefaultSettings()
        {
            PostProcessingSettings setting1 = new PostProcessingSettings();
            PostProcessingSettings setting2 = new PostProcessingSettings();
            PostProcessingSettings setting3 = new PostProcessingSettings();
            PostProcessingSettings setting4 = new PostProcessingSettings();
            PostProcessingSettings setting5 = new PostProcessingSettings();
            PostProcessingSettings setting6 = new PostProcessingSettings();
            PostProcessingSettings setting7 = new PostProcessingSettings();
            PostProcessingSettings setting8 = new PostProcessingSettings();
            PostProcessingSettings setting9 = new PostProcessingSettings();
            ppSettings.postProcessingSettingsList = new List<PostProcessingSettings>();

            ppSettings.postProcessingSettingsList.Add(setting1);
            ppSettings.postProcessingSettingsList.Add(setting2);
            ppSettings.postProcessingSettingsList.Add(setting3);
            ppSettings.postProcessingSettingsList.Add(setting4);
            ppSettings.postProcessingSettingsList.Add(setting5);
            ppSettings.postProcessingSettingsList.Add(setting6);
            ppSettings.postProcessingSettingsList.Add(setting7);
            ppSettings.postProcessingSettingsList.Add(setting8);
            ppSettings.postProcessingSettingsList.Add(setting9);
        }

        public static VariousPPSettings GetInstance()
        {
            if (!Deserialize(ppSettingFileName))
            {
                AddDefaultSettings();
            }
            return ppSettings;
        }

        private static bool Deserialize(string ppSettingsFileName)
        {
            bool returnVal = false;
            if (File.Exists(ppSettingFileName))
            {
                string settingsString = File.ReadAllText(ppSettingsFileName);
                ppSettings = (VariousPPSettings)JsonConvert.DeserializeObject(settingsString, typeof(VariousPPSettings));
                if (ppSettings.postProcessingSettingsList.Count == 0)
                {
                    returnVal = false;
                }
                else
                    returnVal = true;
            }
            return returnVal;
        }

        public void Serialize()
        {
            string settingsStr = JsonConvert.SerializeObject(ppSettings);
            File.WriteAllText(ppSettingFileName, settingsStr);
        }
    }
}
