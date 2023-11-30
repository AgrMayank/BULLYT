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
            float randomX = Random.Range(0f, width);
            float randomY = Random.Range(0f, height);

            Vector3 randomPosition = new Vector3(randomX, randomY, 0f);
            Instantiate(spritePrefab, randomPosition, Quaternion.identity);
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
                float x = col * spacing;
                float y = row * spacing;

                Vector3 position = new Vector3(x, y, 0f);
                Instantiate(sprites[Random.Range(0, sprites.Length)], position, Quaternion.identity);
            }
        }
    }
}
