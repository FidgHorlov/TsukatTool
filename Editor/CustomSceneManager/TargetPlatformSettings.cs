#region Info

// Tsukat tool - by Horlov Andrii (andreygorlovv@gmail.com)
// Tsukat -> https://tsukat.com/

#endregion

using System;

namespace TsukatTool.Editor.CustomSceneManager
{
    [Serializable]
    public class TargetPlatformSettings
    {
        public CustomBuildTarget[] BuildTargets;
    }

    [Serializable]
    public class CustomBuildTarget
    {
        public string Name;
        public bool IsSelected;
    }
}