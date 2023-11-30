using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    public GameObject[] sprites; // Array of sprites to be placed
    public float width = 10f; // Width of the area
    public float height = 10f; // Height of the area
    public float spacing = 1f; // Spacing between sprites

    void Start()
    {
        // PlaceSpritesRandomly();
        PlaceSpritesEvenly();
    }

    void PlaceSpritesRandomly()
    {
        foreach (GameObject spritePrefab in sprites)
        {
            float randomX = Random.Range(-width / 2f, width / 2f);
            float randomY = Random.Range(-height / 2f, height / 2f);

            Vector3 randomPosition = new Vector3(randomX, randomY, 0f);
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

            Instantiate(spritePrefab, randomPosition, randomRotation);
        }
    }

    void PlaceSpritesEvenly()
    {
        int rows = Mathf.FloorToInt(height / spacing);
        int columns = Mathf.FloorToInt(width / spacing);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                float x = col * spacing - width / 2f;
                float y = row * spacing - height / 2f;

                Vector3 position = new Vector3(x, y, 0f);
                Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

                Instantiate(sprites[Random.Range(0, sprites.Length)], position, randomRotation);
            }
        }
    }
}
