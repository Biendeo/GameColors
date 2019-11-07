using UnityEngine;

namespace GameColors
{
    public class Loader
    {
        public static void LoadTweak()
        {
            Loader.go = new GameObject();
            Loader.go.AddComponent<TweakMain>();
            Object.DontDestroyOnLoad(Loader.go);
            Loader.go.SetActive(true);
        }

        public static void UnloadTweak()
        {
            Object.DestroyImmediate(Loader.go);
        }

        private static GameObject go;
    }
}
