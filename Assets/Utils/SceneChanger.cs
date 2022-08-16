using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

namespace GameSystems
{
    namespace Scene
    {
        public class SceneChanger : SingletonMonoPersistent<SceneChanger>, ISceneChanger
        {
            public const string SceneMainMenu = "MainMenu";

            private List<string> m_Scenes = new List<string>();

            private void Start()
            {
                m_Scenes = GetSceneList();
            }

            public IEnumerator ChangeScene(string name, Action StartGame)
            {
                string sceneName;
                if(name == SceneMainMenu)
                {
                    sceneName = name;
                }
                else
                {
                    sceneName = m_Scenes.Find(x => x == name);
                }
                Debug.Log("Середина загрузки");
                if (sceneName != null)
                {
                    var scene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

                    while (!scene.isDone)
                    {
                        yield return null;
                    }
                    Debug.Log("Конец загрузки");
                    StartGame();
                }
                else 
                {
                    throw new Exception($"Scene {sceneName} not found.");
                }
                
            }

            public List<string> GetSceneList()
            {
                int count = SceneManager.sceneCountInBuildSettings;
                List<string> sceneList = new List<string>();
                for (int i = 0; i < count; i++)
                {
                    string sceneName = SceneUtility.GetScenePathByBuildIndex(i);
                    GetSceneNameFromPath(ref sceneName);
                    if(sceneName == SceneMainMenu)
                    {
                        continue;
                    }
                    sceneList.Add(sceneName);
                }
                return sceneList;
            }

            private string GetSceneNameFromPath(ref string path)
            {
                var indexFirst = path.LastIndexOf("/");
                path = path.Remove(0, indexFirst + 1);
                var indexLast = path.LastIndexOf(".");
                path = path.Remove(indexLast, path.Length - indexLast);
                return path;
            }

        }
    }
}
