#region Info

// Tsukat tool - by Horlov Andrii (andreygorlovv@gmail.com)
// Tsukat -> https://tsukat.com/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TsukatTool.Editor.CustomSceneManager
{
    public class SceneManagerWindow : EditorWindow
    {
        private const string ScenesHeader = "Here you can work with scenes";

        private const string ApplySettingsButtonName = "Apply";
        private const string OpenSettingsButtonName = "Scene manager settings";
        private const string PrepareBuildButtonName = "Prepare build";

        private const string TargetPlatformHeader = "Target Platform";
        private const string AddToMenuLabel = "Add to menu?";
        private const string AddToBuildLabel = "Add to build";

        private const string AddBuildFromSettingsLabel = "Add build targets from settings";

        private TargetPlatformSettings _targetPlatformSettings;
        private ScenesSettings _scenesSettings;
        private Vector2 _scrollPosition = new Vector2();

        private bool _wasDictionaryInit;
        private bool _isSceneThings;
        private bool _isMoreDeepSceneEdit;

        private void OnGUI()
        {
            SceneUpdate();
            OpenPrepareBuild();
        }

        private void OnFocus()
        {
            _wasDictionaryInit = false;
        }

        private void CreateButton(string buttonName, Action callback)
        {
            if (GUILayout.Button(buttonName))
            {
                callback?.Invoke();
            }
        }

        private void SceneUpdate()
        {
            EditorGUILayout.LabelField(ScenesHeader);
            if (!_wasDictionaryInit)
            {
                InitSettings();
                _wasDictionaryInit = true;
            }

            ScrollWithAllScenes();

            EditorGUILayout.BeginHorizontal(EditorStyles.inspectorFullWidthMargins);
            CreateButton(ApplySettingsButtonName, AddAllScenesToBuildWindow);
            CreateButton(OpenSettingsButtonName, CustomSceneWindowsManager.OpenSettingsWindow);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
        }

        private void ScrollWithAllScenes()
        {
            EditorGUILayout.BeginVertical(
                EditorStyles.helpBox,
                GUILayout.ExpandHeight(false),
                GUILayout.ExpandWidth(true));

            _scrollPosition =
                GUILayout.BeginScrollView(_scrollPosition,
                    false,
                    false,
                    GUILayout.ExpandHeight(false));

            foreach (SceneData scene in _scenesSettings.ScenesData)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField(scene.Name, EditorStyles.boldLabel);
                scene.IsBuildAdded = EditorGUILayout.ToggleLeft(AddToBuildLabel, scene.IsBuildAdded);
                scene.IsCustomSceneLoader = EditorGUILayout.ToggleLeft(AddToMenuLabel, scene.IsCustomSceneLoader);
                if (_targetPlatformSettings?.BuildTargets == null || _targetPlatformSettings.BuildTargets.Length < 1)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField(AddBuildFromSettingsLabel);
                    EditorGUILayout.EndVertical();
                    continue;
                }

                if (!scene.IsBuildAdded)
                {
                    continue;
                }

                scene.IsBuildTargetSelected = EditorGUILayout.BeginFoldoutHeaderGroup(scene.IsBuildTargetSelected, TargetPlatformHeader);
                if (scene.IsBuildTargetSelected)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    foreach (CustomBuildTarget buildTarget in scene.TargetPlatformSettings.BuildTargets)
                    {
                        if (buildTarget.Name.Equals(BuildTarget.NoTarget.ToString()))
                        {
                            EditorGUILayout.LabelField(AddBuildFromSettingsLabel);
                            continue;
                        }

                        buildTarget.IsSelected = EditorGUILayout.ToggleLeft(buildTarget.Name, buildTarget.IsSelected);
                    }

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            GUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void InitSettings()
        {
            _targetPlatformSettings = FileManager.LoadTargetPlatforms();
            _scenesSettings = FileManager.LoadScenesSettings();

            if (_targetPlatformSettings == null)
            {
                _targetPlatformSettings = new TargetPlatformSettings {BuildTargets = new CustomBuildTarget[1]};
                CustomBuildTarget customBuildTarget = new CustomBuildTarget {Name = BuildTarget.NoTarget.ToString()};
                _targetPlatformSettings.BuildTargets = new[] {customBuildTarget};
            }

            List<SceneData> sceneDataList = new List<SceneData>();
            foreach (Scene scene in ScenesGetter.OpenSceneOneByOne())
            {
                SceneData sceneData = null;
                if (_scenesSettings != null)
                {
                    sceneData = GetSceneDataIfExist(scene);
                }

                if (sceneData == null)
                {
                    sceneData = new SceneData();
                    sceneData.Scene = scene;
                    sceneData.Name = scene.name;
                    sceneData.Path = scene.path;
                    sceneData.TargetPlatformSettings = new TargetPlatformSettings();

                    if (_targetPlatformSettings != null)
                    {
                        sceneData.TargetPlatformSettings.BuildTargets = new CustomBuildTarget[_targetPlatformSettings.BuildTargets.Length];
                        for (int index = 0; index < sceneData.TargetPlatformSettings.BuildTargets.Length; index++)
                        {
                            CustomBuildTarget buildTarget = new CustomBuildTarget
                            {
                                Name = _targetPlatformSettings.BuildTargets[index].Name,
                                IsSelected = false
                            };
                            sceneData.TargetPlatformSettings.BuildTargets[index] = buildTarget;
                        }
                    }
                }

                sceneData.IsBuildAdded = SceneUtility.GetBuildIndexByScenePath(scene.path) > -1;
                sceneData.IsCustomSceneLoader = SceneLoader.SceneLoader.IsSceneInMenu(scene.name);
                sceneDataList.Add(sceneData);
            }

            _scenesSettings = new ScenesSettings {ScenesData = sceneDataList.ToArray()};
        }

        private SceneData GetSceneDataIfExist(Scene scene)
        {
            return _scenesSettings.ScenesData.FirstOrDefault(sceneData => sceneData.Path.Equals(scene.path));
        }

        private void OpenPrepareBuild()
        {
            EditorGUILayout.BeginVertical();
            CreateButton(PrepareBuildButtonName, CustomSceneWindowsManager.OpenPrepareBuildWindow);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Separator();
        }

        private void AddAllScenesToBuildWindow()
        {
            List<EditorBuildSettingsScene> settingsScenes = new List<EditorBuildSettingsScene>();
            foreach (SceneData scene in _scenesSettings.ScenesData)
            {
                if (scene.IsBuildAdded)
                {
                    settingsScenes.Add(new EditorBuildSettingsScene(scene.Path, true));
                }
            }

            EditorBuildSettings.scenes = settingsScenes.ToArray();
            SceneLoader.SceneLoader.AddAllScenesToMenuBar(_scenesSettings.ScenesData);
            FileManager.ReWriteScenesSettings(_scenesSettings);
        }
    }
}