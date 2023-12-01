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

    private void Awake()
    {
        isGameOver = false;

        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;

        // Initialize the slider value based on the initial orthographic size
        if (mainCamera != null && slider != null)
        {
            UpdateSliderValue();
        }
    }

    public void IncreaseOrthographicSize()
    {
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
        // Calculate the proportional value for the slider
        float proportionalValue = Mathf.InverseLerp(minValue, maxValue, mainCamera.orthographicSize);

        // Set the slider value
        slider.value = proportionalValue;
    }

    void GameOver()
    {
        // Your game over logic here
        Debug.Log("Game Over");

        isGameOver = true;
    }
}
