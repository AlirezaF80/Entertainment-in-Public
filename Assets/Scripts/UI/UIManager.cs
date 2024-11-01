using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header ("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;
    [Header("Pause")]
    [SerializeField] private GameObject PauseScreen;

    private void Awake()
    {
        gameOverScreen.SetActive(false);
        PauseScreen.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause")) {
            PauseGame(!IsPaused());
        }
    }
    #region Game Over
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        Restart();
        // SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Restart();
    //     Application.Quit();
    // #if UNITY_EDITOR
    //     UnityEditor.EditorApplication.isPlaying = false;
    // #endif
    }
    #endregion

    public void PauseGame(bool status) {
        PauseScreen.SetActive(status);
        Time.timeScale = status ? 0 : 1;
    }

    public bool IsPaused() {
        return PauseScreen.activeInHierarchy;
    }
}
