using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    #region Variables

    // Global variables
    public static bool GameIsOn = false;
    public static int numberOfEnemies = 0;
    public static int enemyBasicLife = 10;
    public static int characterToPlay = 1;
    public static int language = 0;

    // Character's activation
    [SerializeField] private GameObject joao, maria;
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    // Enemies' spawn
    [SerializeField] private List<GameObject> enemiesSpots = new List<GameObject>();
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    // Weapons' spawn
    [SerializeField] private List<Weapon> weaponsList = new List<Weapon>();
    [SerializeField] private List<GameObject> weaponsSpots = new List<GameObject>();
    public List<GameObject> _weaponsSpots
    {
        get { return weaponsSpots; }
        set { weaponsSpots = value; }
    }
    [SerializeField] private GameObject weapon;

    // Candles' spawn
    [SerializeField] private List<GameObject> candlesSpots = new List<GameObject>();
    [SerializeField] private GameObject candle;

    // HUD
    [SerializeField] private TextMeshProUGUI waveLabel;
    [SerializeField] private GameObject nextWavePanel;
    [SerializeField] private TextMeshProUGUI waveCount;
    [SerializeField] private TextMeshProUGUI timerLabel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject roundController;

    // Timer
    private float timer;

    // Rounds
    private int wave = 0;
    private bool nextWave = false;
    private bool waveIsActive = false;

    // Audio
    [SerializeField] private AudioSource environmentSource, buttonsAudioSource;
    [SerializeField] private AudioClip environmentAudio;

    #endregion

    #region MainMethods

    void Awake()
    {
        Debug.Log("VOLUME: " + VolumeController.volume);
        StartGame();
    }

    void Update()
    {
        // Check all this only if the game is started
        if (GameIsOn)
        {
            // Survival timer
            timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            timerLabel.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            // Updating the round in the HUD
            waveLabel.text = wave.ToString();

            // If the start of the next round is released
            if (nextWave)
            {
                // Start next round
                nextWave = false;
                StartCoroutine("StartWave");
            }

            // When the round starts
            if (waveIsActive)
            {
                // If all enemies are dead
                if (numberOfEnemies == 0)
                {
                    // Start the round change process
                    waveIsActive = false;
                    StartCoroutine("NextWave");
                }
            }
        }
    }

    #endregion

    #region GameStatusHandler

    public void PauseGame()
    {
        // Getting all looping AudioSources in the Scene
        List<AudioSource> audios = new List<AudioSource>();
        audios.Add(GameObject.FindGameObjectWithTag("PlayerSource").GetComponent<AudioSource>());
        audios.Add(GameObject.FindGameObjectWithTag("EnvironmentSource").GetComponent<AudioSource>());


        // Pause Game
        if (GameIsOn)
        {
            GameIsOn = false;
            Time.timeScale = 0f;
            // Debug.Log("JOGO PAUSADO");

            // Pausing all looping audios
            foreach (AudioSource audio in audios) audio.Pause();


        }

        // Unpause Game
        else
        {
            GameIsOn = true;
            // Debug.Log("JOGO DESPAUSADO");
            Time.timeScale = 1f;

            // Unpausing all audio
            foreach (AudioSource audio in audios) audio.Play();
        }
    }

    void StartGame()
    {
        // Resetting all global variables for the first round
        numberOfEnemies = 0;
        GameIsOn = false;
        enemyBasicLife = 10;

        // Playing ambient audio
        environmentSource.clip = environmentAudio;
        environmentSource.Play();

        // Instantiating the player
        // If Joao is the character
        if (characterToPlay == 1)
        {
            // Active player object corresponding to joao
            joao.SetActive(true);
            // Making the virtual camera follow joao
            virtualCamera.Follow = joao.GetComponent<Transform>();
            virtualCamera.LookAt = joao.GetComponent<Transform>();
            // Debug.Log("PERSONAGEM ESCOLHIDO: JOAO !!");
        }
        // If Maria is the character
        else if (characterToPlay == 2)
        {
            // Active player object corresponding to Maria
            maria.SetActive(true);
            // Making the virtual camera follow Maria
            virtualCamera.Follow = maria.GetComponent<Transform>();
            virtualCamera.LookAt = maria.GetComponent<Transform>();
            // Debug.Log("PERSONAGEM ESCOLHIDO: MARIA !!");
        }

        // Setting the volume
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios) audio.volume = VolumeController.volume;

        // Activating the game
        Time.timeScale = 1f;
        GameIsOn = true;
        // Starting the game in the first round
        wave = 1;
        // Releasing the start of the first round
        nextWave = true;

        Debug.Log("QUANTIDADE DE INIMIGOS VIVOS: " + numberOfEnemies);
        Debug.Log("ONDA: " + wave);
        Debug.Log("JOGO ATIVO? : " + GameIsOn);

        // Starting coroutines to spawn weapons and candles
        StartCoroutine("SpawnWeapons");
        StartCoroutine("SpawnCandles");

    }

    public void QuitGame()
    {
        // Desactivating the game
        GameIsOn = false;
        // Desactivating player objects corresponding to maria and joao
        joao.SetActive(false);
        maria.SetActive(false);
        // Loading scene MainMenu
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        // Desactivating the game
        GameIsOn = false;
        // Desactivating player objects corresponding to maria and joao
        joao.SetActive(false);
        maria.SetActive(false);
        // Loading scene PreGame
        SceneManager.LoadScene("PreGame");
    }

    public void GameOver()
    {
        // Desactivating the game
        Time.timeScale = 0f;
        StartCoroutine("FinishingGame");
    }

    IEnumerator FinishingGame()
    {
        yield return new WaitForSecondsRealtime(1f);

        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audioSources) audio.Stop();

        yield return new WaitForSecondsRealtime(1f);

        // Opening the Game Over panel
        gameOverPanel.SetActive(true);
    }

    #endregion

    #region WavesHandler
    
    // Coroutine to start a new round
    IEnumerator StartWave()
    {
        // Help variable to know the amount of enemies to be spawned
        int amountOfEnemies;

        // The number of enemies is triple the level of the round, if it is round 10 or higher, the number is 30 enemies per round
        if (wave <= 10) amountOfEnemies = wave * 3;
        else amountOfEnemies = 30;

        Debug.Log("Wave: " + wave + "  / Enemies: " + amountOfEnemies);

        // Spawning enemies
        for (int i = 0; i < amountOfEnemies; i++)
        {
            // Getting a random value in the enemies spots list
            int spot = Random.Range(0, enemiesSpots.Capacity);
            // Getting a random enemie in enemies list
            GameObject enemy = enemies[Random.Range(0,enemies.Capacity)];
            // Instantiating an enemy at this spawn point
            Instantiate(enemy, enemiesSpots[spot].transform.position, Quaternion.identity);
            // Increasing the number of enemies alives in this round
            numberOfEnemies += 1;
            // Waiting 0.2 seconds to spawn a new enemy
            yield return new WaitForSeconds(0.2f);
        }
        // After all enemies are spawned, start the round and its verification
        waveIsActive = true;
        Debug.Log("Number of Enemies Alive: " + numberOfEnemies);
    }

    // Coroutine to start the round change process
    IEnumerator NextWave()
    {
        Debug.Log("TROCANDO ROUND");
        // TOCAR ANIMAÇÃO DE TROCA DE ROUND
        roundController.GetComponent<Animator>().SetInteger("transition", 1);
        // ESPERAR ANIMAÇÃO ACABAR
        yield return new WaitForSeconds(4.1f);
        // Increase the round value by one
        wave++;
        Debug.Log("PROXIMO ROUND!!!");
        // If the round value is less then 10, increase the enemies' life by 2, otherwise, if it is greater than 10, increase it by 1 only
        if (wave <= 10) enemyBasicLife += 2;
        else enemyBasicLife += 1;
       /* // Starting the round counter
        waveCount.text = 3.ToString();
        nextWavePanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        waveCount.text = 2.ToString();
        yield return new WaitForSeconds(1f);
        waveCount.text = 1.ToString();
        yield return new WaitForSeconds(1f);
        nextWavePanel.SetActive(false);
        // Release the start of the next round*/
        nextWave = true;
    }

    #endregion

    #region WeaponsHandler
    
    // Coroutine to spawn weapons
    IEnumerator SpawnWeapons()
    {
        yield return new WaitForSeconds(2f);

        // Choose a random weapon from the list of weapons scriptable objects
        Weapon weaponToSpawn = weaponsList[Random.Range(0, weaponsList.Capacity)];
        // Choose a random spawn point in weaponsSpots' list
        //int spot = Random.Range(0, weaponsSpots.Capacity);
       // GameObject weaponSpot = new GameObject();
        int spot = Random.Range(0, weaponsSpots.Capacity);
        GameObject weaponSpot = weaponsSpots[spot];
        weaponsSpots.RemoveAt(spot);
        weapon.GetComponent<WeaponController>().spawnPoint = weaponSpot;
        weapon.GetComponent<WeaponController>().gameController = this;

        // Determines the weapon type of the object that will be instantiated
        weapon.GetComponent<WeaponController>().weapon = weaponToSpawn;
        // Instantiate the weapon object, at the chosen spawn point, with a rotation of 28 degrees in z
        Instantiate(weapon, weaponSpot.transform.position, Quaternion.Euler(0, 0, 28));
        //Debug.Log("Arma spawnada");

        yield return new WaitForSeconds(2f);

        // Restart the coroutine
        //Debug.Log("Chamando corrotina novamente");
        if (GameIsOn) StartCoroutine("SpawnWeapons");
    }

    #endregion

    #region CandlesHandler

    // Coroutine to spawn candles
    IEnumerator SpawnCandles()
    {
        yield return new WaitForSeconds(3f);

        // Choose a random spawn point in the candlesSpots' list
        int spot = Random.Range(0, candlesSpots.Capacity);
        // Instantiate the candle object, at the chosen spawn point
        Instantiate(candle, candlesSpots[spot].transform.position, Quaternion.identity);
        //Debug.Log("Vela spawnada");

        yield return new WaitForSeconds(3f);

        // Restart the coroutine
        if (GameIsOn) StartCoroutine("SpawnCandles");
    }

    #endregion

    #region FireHandler
/*
    // Method called by the attack button
    public void Fire()
    {
        Debug.Log("DANO: " + PlayerController.weaponDamage);

        // If the player has a weapon
        if (PlayerController.hasWeapon) {

            Debug.Log("ATIRAR");
            Debug.Log("CHARACTER: " + characterToPlay);
            Debug.Log("TEM ARMA? : " + PlayerController.hasWeapon);
            Debug.Log("TIPO DE ARMA: " + PlayerController.weaponType);

            // Analyzes which player shot, gets the shooting controller component and calls the corresponding character's fire method 
            // Joao
            if (characterToPlay == 1)
            {
                Debug.Log("JOAO ATIROU");
                joao.GetComponentInChildren<ShootingController>().Fire();
            }
            // Maria
            else if (characterToPlay == 2)
            {
                Debug.Log("MARIA ATIROU");
                maria.GetComponentInChildren<ShootingController>().Fire();
            }
        }
    }
*/
    #endregion

    #region LanguageHandler

    public void ChangeLanguage(int value)
    {
        language = value;
    }

    #endregion

    #region AudioHandler

    // Method to play the button sound effect when pressed
    public void PlayButtonSoundEffect()
    {
        buttonsAudioSource.Play();
    }

    #endregion

}
