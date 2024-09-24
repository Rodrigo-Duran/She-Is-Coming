using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PreGameController : MonoBehaviour
{

    #region Variables

    // History
    [SerializeField] private GameObject history0, history1, history2, history3;

    // Panels
    [SerializeField] private GameObject preGamePanel, chooseCharacterPanel;

    // ChooseCharacter
    [SerializeField] private Button joaoButton, mariaButton;

    // Audio
    [SerializeField] private AudioSource menuAudioSource, buttonsAudioSource;
    [SerializeField] private AudioClip menuMusic, buttonsSoundEffect;

    #endregion

    #region MainMethods

    // Start is called before the first frame update
    void Awake()
    {
        // Activating the time in the scene
        Time.timeScale = 1f;
        StartCoroutine("StartPreGame");

        // Getting all audio sources in the scene and changing their volumes
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios) audio.volume = VolumeController.volume;

        // Playing the menu music
        menuAudioSource.clip = menuMusic;
        menuAudioSource.Play();
    }

    #endregion

    #region HistoryHandler

    // Coroutine to show the history and switch to character selection
    IEnumerator StartPreGame()
    {
        // After 1 second, switch to the next chapter of the history
        yield return new WaitForSeconds(1f);
        history0.SetActive(false);
        history1.SetActive(true);
        Debug.Log("History 1 active");
        // After 5 seconds, switch to the next chapter of the history
        yield return new WaitForSeconds(5f);
        history1.SetActive(false);
        history2.SetActive(true);
        Debug.Log("History 2 active");
        // After 5 seconds, switch to the next chapter of the history
        yield return new WaitForSeconds(5f);
        history2.SetActive(false);
        history3.SetActive(true); 
        Debug.Log("History 3 active");
        // After 5 seconds, switch to the character selection
        yield return new WaitForSeconds(5f);
        ChooseCharacterPanel();
    }

    #endregion

    #region ChangePanelHandler

    // Method to switch to the character selection (Called by pressing the pass button)
    public void ChooseCharacterPanel()
    {
        StopCoroutine("StartPreGame");
        preGamePanel.SetActive(false);
        chooseCharacterPanel.SetActive(true);

    }

    #endregion

    #region CharacterChosenHandler

    // Method called by the character button, to pass a parameter to LoadGame coroutine
    public void CharacterChosen(int character)
    {
        StartCoroutine(LoadGame(character));
    }

    #endregion

    #region GameHandler

    // Coroutine to block the button of the character that was not chosen, informs the GameController which character was chosen and loads the Game scene 
    public IEnumerator LoadGame(int character)
    {
        // If the parameter is 1, Joao was the chosen character, so it blocks the MariaButton's interaction
        if (character == 1) mariaButton.interactable = false;
        // If the parameter is 2, Maria was the chosen character, so it blocks the JoaoButton's interaction
        else if (character == 2) joaoButton.interactable = false;
        yield return new WaitForSeconds(1f);
        // Informs GameController which character was chosen
        GameController.characterToPlay = character;
        // Loads the Game scene
        SceneManager.LoadScene("Game");
    }

    #endregion

    #region AudioHandler

    // Method to play the button sound effect when pressed
    public void PlayButtonSoundEffect()
    {
        buttonsAudioSource.clip = buttonsSoundEffect;
        buttonsAudioSource.Play();
    }

    #endregion

}
