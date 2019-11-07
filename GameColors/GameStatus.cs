using UnityEngine.SceneManagement;

namespace GameColors
{
    public class GameStatus
    {
        public static GameState gameState
        {
            get
            {
                if (SceneManager.GetActiveScene().name.ToLower().Contains("gameplay"))
                {
                    return GameState.PlayingSong;
                }

                if (SceneManager.GetActiveScene().name.ToLower().Contains("endofsong"))
                {
                    return GameState.StatScreen;
                }

                if (SceneManager.GetActiveScene().name.ToLower().Contains("main menu"))
                {
                    return GameState.MainMenu;
                }

                if (SceneManager.GetActiveScene().name.ToLower().Contains("credits"))
                {
                    return GameState.MainMenu;
                }

                Debug.Log("Unknown scene: " + SceneManager.GetActiveScene().name);
                return GameState.Unknown;
            }
        }

        public enum GameState
        {
            MainMenu,
            PlayingSong,
            StatScreen,
            Credits,
            Unknown = -1
        }
    }
}
