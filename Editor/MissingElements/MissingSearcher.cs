#region Info

// Tsukat tool - by Horlov Andrii (andreygorlovv@gmail.com)
// Tsukat -> https://tsukat.com/

#endregion

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TsukatTool.Editor.MissingElements
{
	public class MissingSearcher : MonoBehaviour
	{
		private enum MissingType
		{
			Components,
			Prefabs,
			Materials
		}

		private const string MissingComponentPath = "Tsukat/Find.../Find Missing Components";
		private const string MissingPrefabsPath = "Tsukat/Find.../Find Missing Prefabs";
		private const string MissingMaterialsPath = "Tsukat/Find.../Find Missing Materials";
		
		private const string MissingScriptFound = "Game object <b><color=green>{0}</color></b> has missing components on scene -> <b>{1}</b> !";
		private const string MissingPrefabFound = "Missing prefab <b><color=green>{0}</color></b> was find on scene -> <b>{1}</b> !";
		private const string MissingMaterialFound = "Missing material on mesh -> <b><color=green>{0}</color></b> was find on scene -> <b>{1}</b> !";
		private const string SearchingMaterialsStart = "<i><color=yellow>Searching of missing <b>Materials</b> has been started</color></i>";
		private const string SearchingPrefabsStart = "<i><color=yellow>Searching of missing <b>Prefabs</b> has been started</color></i>";
		private const string SearchingComponentsStart = "<i><color=yellow>Searching of missing <b>Components</b> has been started</color></i>";
		private const string SearchingDone = "<i><color=yellow>Searching is done</color></i>";

		private static ILogger Logger => Debug.unityLogger;

		[MenuItem(MissingComponentPath)]
		public static bool FindMissingComponents()
		{
			CustomLogger(SearchingComponentsStart);
			return SearchMissingObjects(missingType: MissingType.Components);
		}

		[MenuItem(MissingPrefabsPath)]
		public static bool FindMissingPrefabs()
		{
			CustomLogger(SearchingPrefabsStart);
			return SearchMissingObjects(missingType: MissingType.Prefabs);
		}

		[MenuItem(MissingMaterialsPath)]
		public static bool FindMissingMaterials()
		{
			CustomLogger(SearchingMaterialsStart);
			return SearchMissingObjects(missingType: MissingType.Materials);
		}

		private static bool SearchMissingObjects(MissingType missingType)
		{
			bool hasMissing = false;
			foreach (Scene scene in ScenesGetter.OpenSceneOneByOne())
			{
				foreach (GameObject gameObject in GetAllGameObjects(scene))
				{
					switch (missingType)
					{
						case MissingType.Components:
						{
							bool hasMissingComponents = FindMissingComponents(gameObject, scene);
							if (hasMissingComponents && !hasMissing)
							{
								hasMissing = true;
							}

							break;
						}
						case MissingType.Prefabs:
						{
							bool hasMissingComponents = FindMissingPrefabs(gameObject, scene);
							if (hasMissingComponents && !hasMissing)
							{
								hasMissing = true;
							}

							break;
						}
						case MissingType.Materials:
						{
							bool hasMissingComponents = FindMissingMaterials(gameObject, scene);
							if (hasMissingComponents && !hasMissing)
							{
								hasMissing = true;
							}

							break;
						}
						default:
							throw new ArgumentOutOfRangeException(nameof(missingType), missingType, null);
					}
				}
			}

			CustomLogger(SearchingDone);
			return hasMissing;
		}

		private static bool FindMissingComponents(GameObject gameObject, Scene scene)
		{
			bool hasMissingScripts = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(gameObject) > 0;
			if (hasMissingScripts)
			{
				CustomLogger(string.Format(MissingScriptFound, gameObject.name, scene.name), LogType.Error);
			}

			return hasMissingScripts;
		}

		private static bool FindMissingPrefabs(GameObject gameObject, Scene scene)
		{
			bool hasMissingPrefabs = PrefabUtility.IsPrefabAssetMissing(gameObject);
			if (hasMissingPrefabs)
			{
				CustomLogger(string.Format(MissingPrefabFound, gameObject.name, scene.name), LogType.Error);
			}

			return hasMissingPrefabs;
		}

		private static bool FindMissingMaterials(GameObject gameObject, Scene scene)
		{
			MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
			if (meshRenderer == null)
			{
				return false;
			}

			if (meshRenderer.sharedMaterials.Length < 1)
			{
				CustomLogger(string.Format(MissingMaterialFound, gameObject.name, scene.name), LogType.Error);
				return true;
			}

			if (meshRenderer.sharedMaterial == null)
			{
				CustomLogger(string.Format(MissingMaterialFound, gameObject.name, scene.name), LogType.Error);
				return true;
			}

			return false;
		}

		private static void CustomLogger(string text, LogType logType = LogType.Log)
		{
			switch (logType)
			{
				case LogType.Log:
					Logger.LogFormat(LogType.Log, text);
					break;
				case LogType.Warning:
					Logger.LogFormat(LogType.Warning, text);
					break;
				case LogType.Error:
					Logger.LogFormat(LogType.Error, text);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(logType), logType, null);
			}
		}

		private static IEnumerable<GameObject> GetAllGameObjects(Scene scene)
		{
			Queue<GameObject> gameObjectsQuee = new Queue<GameObject>(scene.GetRootGameObjects());

			while (gameObjectsQuee.Count > 0)
			{
				GameObject gameObjectDequeue = gameObjectsQuee.Dequeue();
				yield return gameObjectDequeue;

				foreach (Transform child in gameObjectDequeue.transform)
				{
					gameObjectsQuee.Enqueue(child.gameObject);
				}
			}
		}
	}
}
