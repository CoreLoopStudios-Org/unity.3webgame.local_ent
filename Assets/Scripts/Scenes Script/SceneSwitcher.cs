using UnityEngine;
using UnityEngine.SceneManagement;
using VirtueSky.Events;


public class SceneSwitcher : MonoBehaviour
{
    public string HomeScene = "Game 2 LobbyScene";
    public string GameScene = "Game 2 GameScene";
    public BooleanEvent play;
    public BooleanEvent home;
    

    private void OnEnable()
    {
        play.AddListener(PlayNow);
        home.AddListener(BackToHome);
    }
    
    private void OnDisable()
    {
        play.RemoveListener(PlayNow);
        home.RemoveListener(BackToHome);
    }
    
    public void PlayNow(bool isPlay)
    {
        if (isPlay)
        {
            SceneManager.LoadScene(GameScene);
        }
        /*else
        {
            SceneManager.UnloadSceneAsync(GameScene);
        }*/
    }
    
    
    public void BackToHome(bool isHome)
    {
        if (isHome)
        {
            SceneManager.LoadScene(HomeScene);
        }
        /*else
        {
            SceneManager.UnloadSceneAsync(HomeScene);
        }*/
    }
}
 