using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string sceneToLoad;
    AudioSource audioSource;
    public AudioClip startButtonSound, QuitButtonSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(sceneToLoad);
        audioSource.clip = startButtonSound;
        audioSource.Play();
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
        audioSource.clip = QuitButtonSound;
        audioSource.Play();
    }
}
