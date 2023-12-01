using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScaleController : MonoBehaviour
{
    public static ScaleController Instance = null;

    public static bool isGameOver = false;

    Camera mainCamera;

    public Slider slider;
    public float minValue = 1f;
    public float maxValue = 10f;

    public GameObject gameOverUI;
    public TMP_Text scoreText;

    private int enemiesHit = 0;

    private AudioSource audioSource;

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
        scoreText.text = "<size=192>" + enemiesHit + "</size>/nATOMS FIXED!!!";
    }
}
