using System;
using System.Collections;
using System.Collections.Generic;

namespace GameSystems
{
    namespace Scene
    {
        public interface ISceneChanger
        {
            IEnumerator ChangeScene(string sceneName, Action start);
            abstract List<string> GetSceneList();
        }
    }
}