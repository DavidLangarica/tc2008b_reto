/// <summary>
/// The FoodSpawner class is responsible for spawning the food.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    private GameManager gameManager;
    
    /// <summary>
    /// The Start method is called before the first frame update
    /// </summary>
    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager component not found.");
        }
    }

    public void AddFood(string foodId, Vector3 position)
    {
        if (!gameManager.spawnedFood.ContainsKey(foodId))
        {
            SpawnFood(foodId, position);
        }
    }
    

    /// <summary>
    /// The SpawnFood method is responsible for spawning the food.
    /// </summary>
    void SpawnFood(string foodId, Vector3 position)
    {
        int foodType = Random.Range(1, 4);
        GameObject foodPrefab = foodType == 1 ? gameManager.food1 : foodType == 2 ? gameManager.food2 : gameManager.food3;

        GameObject food = Instantiate(foodPrefab, position, Quaternion.identity);
        food.name = foodId;
        food.transform.parent = transform;

        float x = position.x;
        float z = position.z;
        float y = 0.15f;

        food.transform.position = new Vector3(x, y, z);
        food.transform.Rotate(0, Random.Range(0, 360), 0);

        gameManager.spawnedFood.Add(foodId, food);
    }
}
