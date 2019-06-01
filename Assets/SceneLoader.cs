using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets
{
    public class SceneLoader : MonoBehaviour
    {
        public void GoToMenu()
        {
            SceneManager.LoadScene( "Menu" );
        }

        public void GoToGame()
        {
            SceneManager.LoadScene( "MainGame" );
        }

        public void GoToAchievements()
        {
            SceneManager.LoadScene( "Achievements" );
        }

        public void GoToTutorial()
        {
            SceneManager.LoadScene( "Tutorial" );
        }

        public void GoToExit()
        {
            Application.Quit();
        }
    }
}
