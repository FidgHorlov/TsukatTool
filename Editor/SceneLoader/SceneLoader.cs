#region Info

// Tsukat tool - by Horlov Andrii (andreygorlovv@gmail.com)
// Tsukat -> https://tsukat.com/

#endregion

using System.Linq;
using TsukatTool.Editor.CustomSceneManager;
using UnityEditor;
using UnityEngine;

namespace TsukatTool.Editor.SceneLoader
{
    public class SceneLoader
    { 
        private const string MenuItemsPlaceTemplate = "// Place for Menu Items";
        
        public static void AddAllScenesToMenuBar(SceneData[] sceneDataList)
        {
            if (!FileManager.CheckIfTemplatesExist())
            {
                return;
            }

            string allMenuItems = "";
            foreach (SceneData scene in sceneDataList)
            {
                if (!scene.IsCustomSceneLoader) continue;

                string menuItemTemplate = FileManager.GetMenuItemTemplate();
                allMenuItems += EditTemplateName(menuItemTemplate, scene.Name);
                allMenuItems = EditPath(allMenuItems, scene.Path);
            }
            
            if (string.IsNullOrEmpty(allMenuItems))
            {
                return;
            }
            
            string mainTemplate = FileManager.GetSceneLoaderTemplate();
            allMenuItems = EditMainTemplateFile(mainTemplate, allMenuItems);
            FileManager.ReWriteSceneLoader(allMenuItems);
            AssetDatabase.Refresh();
            Debug.Log($"Everything done!");
        }
        
        public static bool IsSceneInMenu(string sceneName)
        {
            string sceneLoader = FileManager.LoadSceneLoader();
            if (string.IsNullOrEmpty(sceneLoader) || string.IsNullOrEmpty(sceneName))
            {
                return false;
            }

            sceneName = string.Concat(sceneName.Where(char.IsLetterOrDigit));
            return sceneLoader.Contains(sceneName);
        }

        private static string EditPath(string templateString, string scenePath)
        {
            return templateString.Replace("{1}", scenePath);
        }

        private static string EditTemplateName(string templateString, string sceneName)
        {
            sceneName = string.Concat(sceneName.Where(char.IsLetterOrDigit));
            return templateString.Replace("{0}", sceneName);
        }

        private static string EditMainTemplateFile(string templateString, string targetText)
        {
            return templateString.Replace(MenuItemsPlaceTemplate, targetText);
        }
    }
}