using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header ("Game Over")]
    [SerializeField] private AudioClip gameOverSound;
    [Header("Pause")]
    [SerializeField] private GameObject PauseScreen;

    [SerializeField] private GameObject[] gameOverButtons;
    [SerializeField] private GameObject[] pauseButtons;
    [SerializeField] private Slider progressBar;
    [SerializeField] private float progressMinValue;
    
    private PlayerController playerController;
    private PlayerRespawn playerRespawn;
    private bool isGameOver;
    private EventSystem eventSystem;

    private void Awake()
    {
        playerRespawn = FindObjectOfType<PlayerRespawn>();
        playerController = FindObjectOfType<PlayerController>();
        PauseScreen.SetActive(false);
        eventSystem = GetComponentInChildren<EventSystem>();
    }

    private void Update() {
        if (Input.GetButtonDown("Pause") && !isGameOver) {
            PauseGame(!IsPaused());
        }

        UpdateProgressBar();
    }

    private void UpdateProgressBar() {
        progressBar.value = Mathf.Max(progressMinValue, playerRespawn.GetPlayerProgress());
    }

    #region Game Over
    public void GameOver(float delay = 0) {
        isGameOver = true;
        StartCoroutine(GameOverRoutine(delay));
    }

    private IEnumerator GameOverRoutine(float delay) {
        playerController.GameOver();
        pauseButtons.SetActive(false);
        gameOverButtons.SetActive(true);
        yield return new WaitForSeconds(delay);
        PauseScreen.SetActive(true);
        eventSystem.SetSelectedGameObject(gameOverButtons[^1]);
        Time.timeScale = 0;
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
        gameOverButtons.SetActive(false);
        pauseButtons.SetActive(true);
        PauseScreen.SetActive(status);
        eventSystem.SetSelectedGameObject(pauseButtons[^1]);
        Time.timeScale = status ? 0 : 1;
    }

    public bool IsPaused() {
        return PauseScreen.activeInHierarchy;
    }
}
