#region Info

// Tsukat tool - by Horlov Andrii (andreygorlovv@gmail.com)
// Tsukat -> https://tsukat.com/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TsukatTool.Editor.CustomSceneManager
{
    [Serializable]
    internal class SelectedScene
    {
        public SceneData SceneData { get; set; }
        public bool IsSelected { get; set; }
    }
    
    public class PrepareBuildWindow : EditorWindow
    {
        private const string ButtonName = "Open Build Settings";
        private const string Header = "Here you can switch between platforms, and make final adjustment before go to the Build Settings";

        private BuildTargetGroup _lastSelectedGroup;
        private List<SelectedScene> _selectedScenes;

        private void OnGUI()
        {
            ScenesSettings scenesSettings = FileManager.LoadScenesSettings();
            if (scenesSettings == null)
            {
                Debug.Log($"Scenes settings is null");
                return;
            }
            
            EditorGUILayout.LabelField(Header, EditorStyles.wordWrappedLabel);
            BuildTargetGroup some = EditorGUILayout.BeginBuildTargetSelectionGrouping();
            if (!some.Equals(_lastSelectedGroup))
            {
                _selectedScenes = new List<SelectedScene>();
            }
            
            foreach (SceneData sceneData in scenesSettings.ScenesData)
            {
                foreach (CustomBuildTarget buildTarget in sceneData.TargetPlatformSettings.BuildTargets)
                {
                    BuildTargetGroup targetGroup = BuildPipeline.GetBuildTargetGroup(Enum.Parse<BuildTarget>(buildTarget.Name));
                    if (!some.Equals(targetGroup) || !buildTarget.IsSelected)
                    {
                        continue;
                    }
                    
                    if (!some.Equals(_lastSelectedGroup))
                    {
                        SelectedScene selectedScene = new SelectedScene {SceneData = sceneData, IsSelected = true};
                        _selectedScenes.Add(selectedScene);
                    }
                    
                    // Debug.Log($"0_selected : {_selectedScenes}");
                    // Debug.Log($"1_selected : {_selectedScenes.Count}");
                    // Debug.Log($"21_key : {sceneData}");
                    // Debug.Log($"2_selected : {_selectedScenes[sceneData]}");

                    SelectedScene currentSceneData = _selectedScenes.FirstOrDefault(t => t.SceneData.Path.Equals(sceneData.Path));
                    if (currentSceneData == null)
                    {
                        continue;
                    }
                    
                    EditorGUILayout.BeginVertical();
                    currentSceneData.IsSelected = EditorGUILayout.ToggleLeft(sceneData.Name, currentSceneData.IsSelected);
                     //EditorGUILayout.LabelField(sceneData.Name);
                    EditorGUILayout.EndVertical();
                }
            }

            EditorGUILayout.EndBuildTargetSelectionGrouping();
            _lastSelectedGroup = some;

            if (GUILayout.Button(ButtonName))
            {
                List<EditorBuildSettingsScene> settingsScenes = new List<EditorBuildSettingsScene>(); 
                foreach (SelectedScene sceneData in _selectedScenes)
                {
                    if (sceneData.IsSelected)
                    {
                        settingsScenes.Add(new EditorBuildSettingsScene(sceneData.SceneData.Path, true));   
                    }
                }

                EditorBuildSettings.scenes = settingsScenes.ToArray();
                GetWindow(typeof(BuildPlayerWindow));
            }
        }

        private void Init()
        {
            
        }
    }
}