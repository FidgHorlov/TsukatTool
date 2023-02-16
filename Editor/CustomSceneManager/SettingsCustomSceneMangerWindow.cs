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
    public class SettingsCustomSceneMangerWindow : EditorWindow, IHasCustomMenu
    {
        private const string ButtonName = "Apply settings";
        private const string Header = "Select all build platform you will use";
        private const string HeaderNotSupportedPlatforms = "NOT SUPPORTED PLATFORMS:";

        private const float ScrollOffsetPositionX = 15f;
        private const float ScrollScenesMaxHeight = 400f;

        private TargetPlatformSettings _selectedBuildTargets;
        private List<CustomBuildTarget> _buildTargets;
        private List<CustomBuildTarget> _buildTargetsNotSupported;

        private Vector2 _scrollPosition;
        private bool _wasInit;
        private bool _lockNotSupportedPlatforms = true;

        private void OnGUI()
        {
            EditorGUILayout.LabelField(Header);
            if (!_wasInit)
            {
                InitBuildDictionary();
                _wasInit = true;
            }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, false, GUILayout.Width(position.width - ScrollOffsetPositionX),
                GUILayout.Height(ScrollScenesMaxHeight), GUILayout.MinHeight(100));
            
            foreach (CustomBuildTarget buildTarget in _buildTargets)
            {
                buildTarget.IsSelected = EditorGUILayout.ToggleLeft(buildTarget.Name, buildTarget.IsSelected);
            }

            if (!_lockNotSupportedPlatforms)
            {
                EditorGUILayout.LabelField(HeaderNotSupportedPlatforms);
                foreach (CustomBuildTarget buildTarget in _buildTargetsNotSupported)
                {
                    buildTarget.IsSelected = EditorGUILayout.ToggleLeft(buildTarget.Name, buildTarget.IsSelected);
                }
            }

            GUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            if (GUILayout.Button(ButtonName))
            {
                _selectedBuildTargets = new TargetPlatformSettings();
                _selectedBuildTargets.BuildTargets = _buildTargets.FindAll(t => t.IsSelected).ToArray();

                FileManager.ReWriteTargetPlatforms(_selectedBuildTargets);
                Close();
            }
        }
        
        void IHasCustomMenu.AddItemsToMenu(GenericMenu menu)
        {
            string nameOfContextMenu = _lockNotSupportedPlatforms ? "Unlock" : "Lock";
            nameOfContextMenu += " Un Supported Platforms";

            GUIContent content = new GUIContent(nameOfContextMenu);
            menu.AddItem(content, false, LockUnlock);
        }

        private void InitBuildDictionary()
        {
            _buildTargets = new List<CustomBuildTarget>();
            _buildTargetsNotSupported = new List<CustomBuildTarget>();

            foreach (BuildTarget build in Enum.GetValues(typeof(BuildTarget)))
            {
                CustomBuildTarget customBuildTarget = new CustomBuildTarget {Name = build.ToString(), IsSelected = false};
                if (IsBuildTargetSupported(build))
                {
                    _buildTargets.Add(customBuildTarget);
                }
                else
                {
                    _buildTargetsNotSupported.Add(customBuildTarget);
                }
            }

            _selectedBuildTargets = FileManager.LoadTargetPlatforms();
            if (_selectedBuildTargets == null)
            {
                return;
            }

            foreach (CustomBuildTarget allBuildTarget in _buildTargets)
            {
                foreach (CustomBuildTarget buildTarget in _selectedBuildTargets.BuildTargets)
                {
                    if (allBuildTarget.Name.Equals(buildTarget.Name))
                    {
                        allBuildTarget.IsSelected = true;
                    }
                }
            }

            _buildTargets = new List<CustomBuildTarget>(_buildTargets.OrderByDescending(buildTarget => buildTarget.IsSelected).ToArray());
        }

        private bool IsBuildTargetSupported(BuildTarget buildTarget)
        {
            return BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Unknown, buildTarget);
        }

        private void LockUnlock()
        {
            _lockNotSupportedPlatforms = !_lockNotSupportedPlatforms;
        }
    }
}