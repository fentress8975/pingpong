using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



namespace GameSystems
{
    namespace Scene
    {

        public class SceneChanger : SingletonMonoPersistent<SceneChanger>, ISceneChanger
        {

            public void ChangeScene(string sceneName)
            {
                var scene = SceneManager.LoadSceneAsync(sceneName);
                scene.allowSceneActivation = false;

                //SomeCode

                scene.allowSceneActivation = true;
            }

            public List<string> GetSceneList()
            {
                int count = SceneManager.sceneCountInBuildSettings;
                List<string> sceneList = new List<string>();
                for(int i = 0; i < count; i++)
                {
                    string sceneName = SceneUtility.GetScenePathByBuildIndex(i);
                    GetSceneNameFromPath(ref sceneName);
                    sceneList.Add(sceneName);
                }
                return sceneList;
            }

            private string GetSceneNameFromPath(ref string path)
            {
                var indexFirst = path.LastIndexOf("/");
                path = path.Remove(0, indexFirst+1);
                var indexLast = path.LastIndexOf(".");
                path = path.Remove(indexLast, path.Length - indexLast);
                return path;
            }
        }
    }
}
