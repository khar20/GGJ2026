using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("UI References")]
    public GameObject gameOverPanel;

    public void LoadGame()
    {
        // Resets time scale in case we were paused
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Exited");
    }

    public void TriggerGameOver()
    {
        Debug.Log("Game Over Triggered");
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        // Optional: Slow motion effect instead of instant freeze
        Time.timeScale = 0f; 
    }
}