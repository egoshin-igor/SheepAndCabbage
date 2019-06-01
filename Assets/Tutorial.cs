using Assets;
using UnityEngine;
using UnityEngine.Video;

public class Tutorial : MonoBehaviour
{
    private VideoPlayer _tutorial;
    void Start()
    {
        _tutorial = Camera.main.GetComponent<VideoPlayer>();
        _tutorial.loopPointReached += _tutorial_loopPointReached; ;
    }

    private void _tutorial_loopPointReached( VideoPlayer source )
    {
        new SceneLoader().GoToMenu();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
