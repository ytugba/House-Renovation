using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;

    public void OnStartClick()
    {
        StartCoroutine(LoadAscnchronously("UserHouse"));
    }

    public void OnExitClick()
    {
        Application.Quit(0);
    }

    IEnumerator LoadAscnchronously(string scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress);

            slider.value = progress;

            yield return null;
        }
    }
}
