using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScaleController : MonoBehaviour
{
    public static ScaleController Instance = null;

    public static bool isGameOver = false;

    Camera mainCamera;

    public Slider slider;
    public float minValue = 1f;
    public float maxValue = 10f;

    public TMP_Text timerText;

    public GameObject gameOverUI, plusIcon, minusIcon;
    public TMP_Text scoreText;

    private int enemiesHit = 0;

    private AudioSource audioSource;

    // Timer variables
    private float timer = 3f;
    private bool timerActive = false;

    private void Awake()
    {
        isGameOver = false;
        enemiesHit = 0;

        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();

        // Initialize the slider value based on the initial orthographic size
        if (mainCamera != null && slider != null)
        {
            UpdateSliderValue();
        }

        StartCoroutine(IncreaseTimeScaleOverTime());
    }

    IEnumerator IncreaseTimeScaleOverTime()
    {
        float elapsedTime = 0f;
        float initialTimeScale = Time.timeScale;

        while (elapsedTime < 120f)
        {
            float newTimeScale = Mathf.Lerp(initialTimeScale, 2f, elapsedTime / 120f);
            Time.timeScale = newTimeScale;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Time.timeScale = 2f; // Ensure we reach the exact target at the end
    }

    private void Update()
    {
        // Check if the timer is active
        if (timerActive)
        {
            // Update the timer
            timer -= Time.deltaTime;

            timerText.text = timer.ToString("F1");

            // Check if the timer has reached 0
            if (timer <= 0)
            {
                // Game over logic
                GameOver();
            }
        }
    }

    public void IncreaseOrthographicSize()
    {
        if (isGameOver) return;

        enemiesHit += 1;

        if (mainCamera != null)
        {
            mainCamera.orthographicSize += 1;

            // Update the slider value
            if (slider != null)
            {
                UpdateSliderValue();
            }

            // Reset the timer
            ResetTimer();

            // Check if the orthographic size reaches the maximum
            if (mainCamera.orthographicSize >= maxValue)
            {
                GameOver();
            }
        }
        else
        {
            Debug.LogError("Main camera not found.");
        }
    }

    public void DecreaseOrthographicSize()
    {
        if (isGameOver) return;

        enemiesHit += 1;

        if (mainCamera != null)
        {
            mainCamera.orthographicSize -= 1;

            // Update the slider value
            if (slider != null)
            {
                UpdateSliderValue();
            }

            // Reset the timer
            ResetTimer();

            // Check if the orthographic size reaches the minimum
            if (mainCamera.orthographicSize <= minValue)
            {
                GameOver();
            }
        }
        else
        {
            Debug.LogError("Main camera not found.");
        }
    }

    void UpdateSliderValue()
    {
        // Set the slider value
        slider.value = mainCamera.orthographicSize;

        audioSource.Play();
    }

    void GameOver()
    {
        // Your game over logic here
        Debug.Log("Game Over");

        isGameOver = true;

        gameOverUI.SetActive(true);
        scoreText.text = "<size=192>" + enemiesHit + "</size>\n POINTS!!";

        if (slider.value == minValue)
        {
            minusIcon.SetActive(false);
            plusIcon.SetActive(true);
        }
        else if (slider.value == maxValue)
        {
            minusIcon.SetActive(true);
            plusIcon.SetActive(false);
        }
        else
        {
            minusIcon.SetActive(false);
            plusIcon.SetActive(false);
        }
    }

    void ResetTimer()
    {
        // Reset the timer to 3 seconds
        timer = 3f;
        timerActive = true;
    }
}
