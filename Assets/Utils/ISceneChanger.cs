using System.Collections.Generic;

namespace GameSystems
{
    namespace Scene
    {
        public interface ISceneChanger
        {
            public abstract void ChangeScene(string sceneName);
            public abstract List<string> GetSceneList();
        }
    }
}