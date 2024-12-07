using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    #region Variables
    
    // Audio
    [SerializeField] private AudioSource menuAudioSource, buttonsAudioSource;
    [SerializeField] private AudioClip menuMusic, buttonsSoundEffect;

    // Help variables
    private float actualVolume;

    //Panels
    [SerializeField] private GameObject mainMenuPanel, creditsPanel, configurationsPanel;

    #endregion

    #region MainMethods

    void Awake()
    {
        // Activating the time in the scene
        Time.timeScale = 1f;

        // Help variable
        actualVolume = VolumeController.volume;

        // Playing the menu music
        menuAudioSource.clip = menuMusic;
        menuAudioSource.Play();

    }

    private void Update()
    {
        // If the volume changes
        if (actualVolume != VolumeController.volume)
        {
            // Help variable gets the new volume value
            actualVolume = VolumeController.volume;

            // Getting all audio sources in the scene and changing their volumes to the new value
            AudioSource[] audios = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audio in audios) audio.volume = VolumeController.volume;
        }
    }

    #endregion

    #region StartButton

    public void StartGame()
    {
        StartCoroutine("LoadPreGameScene");
    }

    IEnumerator LoadPreGameScene()
    {
        yield return new WaitForSeconds(0.01f);
        SceneManager.LoadScene("PreGame");
    }

    #endregion

    #region CreditsButton

    public void Credits(string action)
    {
        StartCoroutine(HandleCredits(action));
    }

    IEnumerator HandleCredits(string action)
    {
        yield return new WaitForSeconds(0.01f);
        if (action == "in") 
        {
            mainMenuPanel.SetActive(false);
            creditsPanel.SetActive(true);
        }
        else if (action == "out")
        {
            creditsPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
    }

    #endregion

    #region ConfigurationsButton

    public void Configurations(string action)
    {
        StartCoroutine(HandleConfigurations(action));
    }

    IEnumerator HandleConfigurations(string action)
    {
        yield return new WaitForSeconds(0.01f);
        if (action == "in")
        {
            mainMenuPanel.SetActive(false);
            configurationsPanel.SetActive(true);
        }
        else if (action == "out")
        {
            configurationsPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
    }

    #endregion

    #region QuitButton

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion

    #region AudioHandler

    // Method to play the button sound effect when pressed
    public void PlayButtonSoundEffect()
    {
        buttonsAudioSource.Play();
    }

    #endregion

    #region LanguageHandler

    public void ChangeLanguage(int value)
    {
        GameController.language = value;
    }

    #endregion

}
