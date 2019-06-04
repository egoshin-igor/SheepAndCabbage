using Assets;
using UnityEngine;
using UnityEngine.Video;

public class Tutorial : MonoBehaviour
{
    private VideoPlayer _tutorial;
    private SceneLoader _sceneLoader;

    void Start()
    {
        _tutorial = Camera.main.GetComponent<VideoPlayer>();
        _tutorial.loopPointReached += _tutorial_loopPointReached; ;
        _sceneLoader = GameObject.Find( "ScenesLoader" ).GetComponent<SceneLoader>();
    }

    private void _tutorial_loopPointReached( VideoPlayer source )
    {
        _sceneLoader.GoToMenu();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
