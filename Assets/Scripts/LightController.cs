using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    public bool isPositive = false;

    private int speed = 600;

    private float lightEnd;
    private float minLightValue;
    private float maxLightValue;
    private float maxIntensity = 2f;
    private Light2D light2D;

    private void Start()
    {
        // Get the Light2D component attached to the GameObject
        light2D = GetComponent<Light2D>();

        // Set initial values
        lightEnd = Random.Range(90f, 140f);
        minLightValue = Random.Range(90f, 140f);
        maxLightValue = Random.Range(240f, 300f);

        // Start the coroutine
        StartCoroutine(LoopLightValue());
    }

    private void Update()
    {
        // Check if the mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            // Get the mouse position in world coordinates
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Check if the mouse click hits the collider of this GameObject
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

            if (hitCollider != null && hitCollider.gameObject == gameObject)
            {
                // The mouse was pressed over this GameObject
                Debug.Log("Mouse pressed over GameObject");

                if (isPositive == true)
                {
                    ScaleController.Instance.DecreaseOrthographicSize();
                }
                else
                {
                    ScaleController.Instance.IncreaseOrthographicSize();
                }

                Destroy(gameObject);
            }
        }
    }

    private IEnumerator LoopLightValue()
    {
        while (true)
        {
            // Increase lightEnd until it reaches maxLightValue
            while (lightEnd < maxLightValue)
            {
                lightEnd += Time.deltaTime * speed; // Adjust the speed of change as needed
                UpdateLight();
                yield return null;
            }

            // Randomly change maxLightValue, minLightValue, and intensity
            maxLightValue = Random.Range(240f, 300f);
            minLightValue = Random.Range(90f, 140f);
            maxIntensity = Random.Range(0.5f, 2f);

            // Decrease lightEnd until it reaches minLightValue
            while (lightEnd > minLightValue)
            {
                lightEnd -= Time.deltaTime * speed; // Adjust the speed of change as needed
                UpdateLight();
                yield return null;
            }

            // Randomly change maxLightValue, minLightValue, and intensity
            maxLightValue = Random.Range(240f, 300f);
            minLightValue = Random.Range(90f, 140f);
            maxIntensity = Random.Range(0.5f, 2f);
        }
    }

    private void UpdateLight()
    {
        // Update the outer spot angle of the Light2D component
        light2D.pointLightOuterAngle = lightEnd;

        // Update the volumetric intensity of the Light2D component
        light2D.volumeIntensity = Mathf.Lerp(light2D.volumeIntensity, maxIntensity, Time.deltaTime);
    }
}
