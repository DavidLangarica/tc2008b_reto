/// <summary>
/// The FoodSpawner class is responsible for spawning the food.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    private GameManager gameManager;
    public int totalFoodSpawned = 0;
    
    /// <summary>
    /// The Start method is called before the first frame update
    /// </summary>
    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager component not found.");
        } else {
            StartCoroutine(SpawnFoodRoutine());
        }
    }

    /// <summary>
    /// The SpawnFoodRoutine method is responsible for spawning the food.
    /// </summary>
    IEnumerator SpawnFoodRoutine()
    {
        while (totalFoodSpawned < 47)
        {
            SpawnFood();
            yield return new WaitForSeconds(5);
        }
    }

    /// <summary>
    /// The SpawnFood method is responsible for spawning the food.
    /// </summary>
    void SpawnFood()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager component not found.");
            return;
        }

        int foodToSpawn = Random.Range(2, 6);
        for (int i = 0; i < foodToSpawn; i++)
        {
            int foodType = Random.Range(1, 4);
            GameObject foodPrefab = foodType == 1 ? gameManager.food1 : foodType == 2 ? gameManager.food2 : gameManager.food3;

            int x = Random.Range(0, gameManager.width);
            int z = Random.Range(0, gameManager.height);

            Vector3 spawnPosition = new Vector3(x, 0, z);

            GameObject food = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
            food.transform.parent = transform;

            float y = foodType == 1 ? 0.7f : 0.15f;

            food.transform.position = new Vector3(x, y, z);
            food.transform.Rotate(0, Random.Range(0, 360), 0);

            totalFoodSpawned++;
        }
    }
}
