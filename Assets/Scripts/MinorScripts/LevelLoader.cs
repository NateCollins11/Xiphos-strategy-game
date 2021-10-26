using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public Animator transition;

    // Start is called before the first frame update
    public IEnumerator LoadNextScene(string sceneName)
    {
        float transitionTime = 1.6f;
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
