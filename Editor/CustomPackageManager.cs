#region Info

// Tsukat tool - by Horlov Andrii (andreygorlovv@gmail.com)
// Tsukat -> https://tsukat.com/

#endregion

using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace TsukatTool.Editor
{
    public static class CustomPackageManager
    {
        private const string AssetUsageDetectorPath = "https://github.com/yasirkula/UnityAssetUsageDetector.git";

        private const string AssetUsageDetectorUnityPath = "Tsukat/Add.../Asset Usage Detector";

        private const string AssetUsageDetectorName = "com.yasirkula.assetusagedetector";
        private const string TsukatToolName = "com.tsukat.tool";

        private const string PackageInstalledMsg = "Package installed!";

        private static ListRequest _request;
        private static string _currentPackageName;

        [MenuItem(AssetUsageDetectorUnityPath, false, 0)]
        private static void AddUsageDetector()
        {
            _request = Client.List(offlineMode: true);
            EditorApplication.update += Progress;
            _currentPackageName = AssetUsageDetectorName;
        }

        private static void Progress()
        {
            if (!_request.IsCompleted)
            {
                return;
            }

            switch (_request.Status)
            {
                case StatusCode.Success:
                    if (IsPackageAlreadyInstalled())
                    {
                        return;
                    }

                    EditorApplication.update += AddPackageProgress;
                    break;
                case StatusCode.Failure:
                    Debug.LogError(_request.Error.message);
                    break;
            }

            EditorApplication.update -= Progress;
        }

        private static bool IsPackageAlreadyInstalled()
        {
            foreach (PackageInfo packageInfo in _request.Result)
            {
                if (!packageInfo.name.Equals(_currentPackageName))
                {
                    continue;
                }

                Debug.Log($"<b>{_currentPackageName}</b> is already Installed!");
                EditorApplication.update -= Progress;
                return true;
            }

            return false;
        }

        private static void AddPackageProgress()
        {
            if (!_request.IsCompleted)
            {
                return;
            }

            switch (_request.Status)
            {
                case StatusCode.Success:
                    Debug.Log(PackageInstalledMsg);
                    break;
                case StatusCode.Failure:
                    Debug.LogError(_request.Error.message);
                    break;
            }

            EditorApplication.update -= AddPackageProgress;
        }
    }
}