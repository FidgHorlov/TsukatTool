#region Info

// Tsukat tool - by Horlov Andrii (andreygorlovv@gmail.com)
// Tsukat -> https://tsukat.com/

#endregion

using System.IO;
using TsukatTool.Editor.CustomSceneManager;
using UnityEditor;
using UnityEngine;

namespace TsukatTool.Editor
{
    public class FileManager
    {
        private const string MultiToolSettingsPath = "Assets/TsukatTool/Editor/Settings/TargetPlatformSettings.settings";
        private const string ScenesSettings = "Assets/TsukatTool/Editor/Settings/ScenesSettings.settings";

        private const string SceneLoaderTargetPath = "Assets/TsukatTool/Editor/CustomSceneLoader.cs";
        private const string SceneLoaderTemplatePath = "Assets/TsukatTool/Editor/SceneLoader/SceneLoaderTemplate.template";
        private const string MenuItemTemplatePath = "Assets/TsukatTool/Editor/SceneLoader/MenuItemTemplate.template";
        
        private const string GlobalTemplateMessage = "Can't find Global Template file\r\nPath: {0}";
        private const string MenuItemTemplateMessage = "Can't find MenuItem Template file\r\nPath: {0}";

        public static TargetPlatformSettings LoadTargetPlatforms()
        {
            string json = LoadFromFile(MultiToolSettingsPath);
            return JsonUtility.FromJson<TargetPlatformSettings>(json);
        }

        public static void ReWriteTargetPlatforms(TargetPlatformSettings targetPlatformSettings)
        {
            string jsonData = JsonUtility.ToJson(targetPlatformSettings, true);
            ReWriteFile(MultiToolSettingsPath, jsonData);
        }

        public static ScenesSettings LoadScenesSettings()
        {
            string json = LoadFromFile(ScenesSettings);
            return JsonUtility.FromJson<ScenesSettings>(json);
        }

        public static void ReWriteScenesSettings(ScenesSettings scenesSettings)
        {
            string jsonData = JsonUtility.ToJson(scenesSettings, true);
            ReWriteFile(ScenesSettings, jsonData);
        }

        public static void ReWriteSceneLoader(string data)
        {
            ReWriteFile(SceneLoaderTargetPath, data);
        }

        public static string LoadSceneLoader()
        {
            return LoadFromFile(SceneLoaderTargetPath);
        }

        public static string GetSceneLoaderTemplate()
        {
            return LoadFromFile(SceneLoaderTemplatePath);
        }

        public static string GetMenuItemTemplate()
        {
            return LoadFromFile(MenuItemTemplatePath);
        }
        
        public static bool CheckIfTemplatesExist()
        {
            if (!File.Exists(SceneLoaderTemplatePath))
            {
                Debug.LogError(string.Format(GlobalTemplateMessage, SceneLoaderTemplatePath));
                return false;
            }

            if (File.Exists(MenuItemTemplatePath))
            {
                return true;
            }

            Debug.LogError(string.Format(MenuItemTemplateMessage, MenuItemTemplatePath));
            return false;
        }
        
        private static void ReWriteFile(string pathFile, string jsonData)
        {
            if (!File.Exists(pathFile))
            {
                string folderPath = Path.GetDirectoryName(pathFile);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                
                FileStream fileStream = File.Create(pathFile);
                fileStream.Close();
                fileStream.Dispose();
            }

            File.WriteAllText(pathFile, string.Empty);
            StreamWriter streamWriter = new StreamWriter(pathFile);
            streamWriter.Write(jsonData);
            streamWriter.Close();
            AssetDatabase.Refresh();
        }


        private static string LoadFromFile(string pathFile)
        {
            if (!File.Exists(pathFile))
            {
                return null;
            }
            
            StreamReader streamReader = File.OpenText(pathFile);
            string fileData = streamReader.ReadToEnd();
            streamReader.Close();
            return fileData;
        }
    }
}