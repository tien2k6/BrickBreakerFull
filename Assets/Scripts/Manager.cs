using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [Header("Pause UI")]
    public GameObject pausePanel; // Bảng thông báo Pause
    private bool isPaused = false;

    [Header("UI Progress")]
    public Slider progressBar;

    [Header("Core Groups")]
    public GameObject overGameUI;
    public GameObject gameplayManager;

    [Header("UI Panels")]
    public GameObject mainMenuUI;
    public GameObject settingsPanel;
    public GameObject darkOverlay;
    public GameObject victoryPanel;
    public GameObject losePanel;

    private static bool shouldAutoPlayOnLoad = false;
    private bool hasEndedSession = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (shouldAutoPlayOnLoad)
        {
            shouldAutoPlayOnLoad = false;
            hasEndedSession = false;
            StartGameplay();
        }
        else
        {
            BackToMenu();
        }
    }

    public void UpdateProgressBar(float fillAmount)
    {
        if (progressBar != null)
        {
            progressBar.value = fillAmount;
        }
    }

    public void OnPlayButtonClicked()
    {
        if (hasEndedSession)
        {
            shouldAutoPlayOnLoad = true;
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        StartGameplay();
    }

    private void StartGameplay()
    {
        if (overGameUI != null) overGameUI.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (darkOverlay != null) darkOverlay.SetActive(false);

        if (gameplayManager != null) gameplayManager.SetActive(true);
        Time.timeScale = 1f;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
    }

    public void ShowVictory()
    {
        hasEndedSession = true;
        Time.timeScale = 0f;

        if (overGameUI != null) overGameUI.SetActive(true);
        if (mainMenuUI != null) mainMenuUI.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        if (darkOverlay != null) darkOverlay.SetActive(true);
        if (victoryPanel != null) victoryPanel.SetActive(true);
    }

    public void ShowLose()
    {
        hasEndedSession = true;
        Time.timeScale = 0f;

        if (overGameUI != null) overGameUI.SetActive(true);
        if (mainMenuUI != null) mainMenuUI.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);

        if (darkOverlay != null) darkOverlay.SetActive(true);
        if (losePanel != null) losePanel.SetActive(true);
    }

    public void OpenSettings()
    {
        if (mainMenuUI != null) mainMenuUI.SetActive(false);
        if (darkOverlay != null) darkOverlay.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void BackToMenu()
    {
        Time.timeScale = 0f;

        if (overGameUI != null) overGameUI.SetActive(true);
        if (mainMenuUI != null) mainMenuUI.SetActive(true);

        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (darkOverlay != null) darkOverlay.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        if (gameplayManager != null) gameplayManager.SetActive(false);
    }

    public void OnRetryButtonClicked()
    {
        shouldAutoPlayOnLoad = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMenuButtonClicked()
    {
        BackToMenu();
    }

    // Gắn vào nút PAUSE ở góc màn hình khi đang chơi
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (darkOverlay != null) darkOverlay.SetActive(true);
        if (pausePanel != null) pausePanel.SetActive(true);
    }

    // Gắn vào nút RESUME trong PausePanel
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null) pausePanel.SetActive(false);
        if (darkOverlay != null) darkOverlay.SetActive(false);
    }
}