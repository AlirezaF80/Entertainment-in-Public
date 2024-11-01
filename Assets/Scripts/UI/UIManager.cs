using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header ("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;
    [Header("Pause")]
    [SerializeField] private GameObject PauseScreen;

    [SerializeField] private GameObject gameOverDefaultButton;
    [SerializeField] private GameObject pauseDefaultButton;
    
    private PlayerController playerController;
    private EventSystem eventSystem;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        eventSystem = GetComponentInChildren<EventSystem>();
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
    public void GameOver(float delay = 0)
    {
        StartCoroutine(GameOverRoutine(delay));
    }

    private IEnumerator GameOverRoutine(float delay) {
        playerController.enabled = false;
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0;
        eventSystem.SetSelectedGameObject(gameOverScreen);
        gameOverScreen.SetActive(true);
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
        eventSystem.SetSelectedGameObject(pauseDefaultButton);
        Time.timeScale = status ? 0 : 1;
    }

    public bool IsPaused() {
        return PauseScreen.activeInHierarchy;
    }
}
