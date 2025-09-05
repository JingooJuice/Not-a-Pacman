using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerView : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI winScoreText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private IGameViewModel viewModel;

    public void Initialize(IGameViewModel vm)
    {
        viewModel = vm;
        viewModel.Model.OnDataChanged += OnModelChanged;

        // Настройка кнопок
        if (pauseButton != null) pauseButton.onClick.AddListener(OnPauseButtonClicked);
        if (restartButton != null) restartButton.onClick.AddListener(RestartGame);
        if (resumeButton != null) resumeButton.onClick.AddListener(OnResumeButtonClicked);

        // Инициализация UI
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
        if (winScreen != null) winScreen.SetActive(false);
        if (pauseScreen != null) pauseScreen.SetActive(false);

        UpdateHearts();
    }

    private void Update()
    {
        if (viewModel == null) return;

        if (viewModel.Model.IsGameOver && gameOverScreen != null && !gameOverScreen.activeSelf)
        {
            ShowGameOver();
        }

        if (!viewModel.Model.IsGameWon && !viewModel.Model.IsGameOver && !viewModel.Model.IsPaused)
        {
            viewModel.CheckWinCondition();
        }
    }

    private void OnModelChanged()
    {
        UpdateHearts();
        UpdateScore();
        UpdateGameState();

        if (viewModel.Model.IsGameWon)
            ShowWinScreen();
    }

    private void UpdateHearts()
    {
        if (hearts == null) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
                hearts[i].SetActive(i < viewModel.Model.Lives);
        }
    }

    private void UpdateScore()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + viewModel.Model.Score.ToString();
    }

    private void UpdateGameState()
    {
        if (viewModel.Model.IsPaused && pauseScreen != null)
            pauseScreen.SetActive(true);
        else if (pauseScreen != null)
            pauseScreen.SetActive(false);

        Time.timeScale = viewModel.Model.IsPaused || viewModel.Model.IsGameOver || viewModel.Model.IsGameWon ? 0f : 1f;
    }

    private void ShowGameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
            if (finalScoreText != null)
                finalScoreText.text = "Score: " + viewModel.Model.Score.ToString();
        }
    }

    private void ShowWinScreen()
    {
        if (winScreen != null)
        {
            winScreen.SetActive(true);
            if (winScoreText != null)
                winScoreText.text = "Score: " + viewModel.Model.Score.ToString();
        }
    }

    // Public methods for UI buttons
    public void OnPauseButtonClicked() => viewModel?.TogglePause();
    public void OnResumeButtonClicked() => viewModel?.ResumeGame();

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private void OnDestroy()
    {
        if (viewModel != null)
            viewModel.Model.OnDataChanged -= OnModelChanged;
    }
}