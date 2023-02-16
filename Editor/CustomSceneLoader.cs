using UnityEditor;
using UnityEditor.SceneManagement;

namespace TsukatTool.Editor
{
    public class CustomSceneLoader
    {
                [MenuItem("Tsukat/Load scene/SCenarer2Android")]
        private static void LoadSceneSCenarer2Android()
        {
            EditorSceneManager.OpenScene("Assets/SCenarer2 - Android.unity");
        }
            [MenuItem("Tsukat/Load scene/SCenarer22Windows")]
        private static void LoadSceneSCenarer22Windows()
        {
            EditorSceneManager.OpenScene("Assets/SCenarer2 2 - Windows.unity");
        }
            [MenuItem("Tsukat/Load scene/SCenarer23Both")]
        private static void LoadSceneSCenarer23Both()
        {
            EditorSceneManager.OpenScene("Assets/SCenarer2 3 - Both.unity");
        }
            [MenuItem("Tsukat/Load scene/SCenarer28")]
        private static void LoadSceneSCenarer28()
        {
            EditorSceneManager.OpenScene("Assets/SCenarer2 8.unity");
        }
            [MenuItem("Tsukat/Load scene/SCenarer29")]
        private static void LoadSceneSCenarer29()
        {
            EditorSceneManager.OpenScene("Assets/SCenarer2 9.unity");
        }
            [MenuItem("Tsukat/Load scene/SCenarer232")]
        private static void LoadSceneSCenarer232()
        {
            EditorSceneManager.OpenScene("Assets/SCenarer2 32.unity");
        }
            [MenuItem("Tsukat/Load scene/SCenarer2")]
        private static void LoadSceneSCenarer2()
        {
            EditorSceneManager.OpenScene("Assets/SCenarer2.unity");
        }
            [MenuItem("Tsukat/Load scene/NewScene1")]
        private static void LoadSceneNewScene1()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/New Scene 1.unity");
        }
            [MenuItem("Tsukat/Load scene/SampleScene")]
        private static void LoadSceneSampleScene()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity");
        }
            [MenuItem("Tsukat/Load scene/NewScene")]
        private static void LoadSceneNewScene()
        {
            EditorSceneManager.OpenScene("Assets/Temp/New Scene.unity");
        }
    
    }
}