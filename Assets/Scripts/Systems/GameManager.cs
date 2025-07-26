using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Start,
    Playing,
    Paused,
    Victory,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Estado actual del juego")]
    public GameState currentState = GameState.Start;

    [Header("Referencias")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

//    [SerializeField] private HUDManager hud; // Opcional: Si tenés uno
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private PlayerController player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetGameState(GameState.Start);
    }

    private void Update()
    {
        if (currentState == GameState.Playing && Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        else if (currentState == GameState.Paused && Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }
    }

    public void SetGameState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.Start:
                Time.timeScale = 1f;
                pauseMenu?.SetActive(false);
                gameOverPanel?.SetActive(false);
                victoryPanel?.SetActive(false);
                break;

            case GameState.Playing:
                Time.timeScale = 1f;
                pauseMenu?.SetActive(false);
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                pauseMenu?.SetActive(true);
                break;

            case GameState.Victory:
                Time.timeScale = 0f;
                victoryPanel?.SetActive(true);
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;
                gameOverPanel?.SetActive(true);
                break;
        }
    }

    public void StartGame()
    {
        SetGameState(GameState.Playing);
        waveManager.enabled = true;
    }

    public void PauseGame()
    {
        SetGameState(GameState.Paused);
    }

    public void ResumeGame()
    {
        SetGameState(GameState.Playing);
    }

    public void GameOver()
    {
        SetGameState(GameState.GameOver);
    }

    public void Victory()
    {
        SetGameState(GameState.Victory);
    }

    // Podés conectar estos desde botones UI
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
