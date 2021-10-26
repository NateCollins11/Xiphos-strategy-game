using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MenuManager : MonoBehaviour
{
    public LevelLoader levelLoader;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayGame()
    {
        StartCoroutine(levelLoader.LoadNextScene("PartyManagement"));
    }

    public void Options()
    {


    }
}
