using System;
using UnityEngine.SceneManagement;

namespace Outpath_Modding.Events.EventArguments
{
    public class ChangedSceneEventArgs : EventArgs
    {
        public ChangedSceneEventArgs(Scene scene, LoadSceneMode loadSceneMode)
        {
            Scene = scene;
            LoadSceneMode = loadSceneMode;
        }

        public Scene Scene { get; }
        public LoadSceneMode LoadSceneMode { get; }
    }
}
