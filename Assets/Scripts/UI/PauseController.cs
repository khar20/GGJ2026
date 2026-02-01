using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    void Start()
    {
        // Force the menu to hide when game starts
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        // Ensure time is running
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Simple check for Phase 1; will be moved to Input Actions in Phase 2
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}