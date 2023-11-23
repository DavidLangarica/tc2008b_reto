/// <summary>
/// The RestaurantSpawner class is responsible for spawning the restaurant.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantSpawner : MonoBehaviour
{
    private GameManager gameManager;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
         if (gameManager == null)
        {
            Debug.LogError("GameManager component not found.");
        } else {
            SpawnRestaurant();
        }
    }

    /// <summary>
    /// The SpawnRestaurant method is responsible for spawning the restaurant.
    /// </summary>
    void SpawnRestaurant()
    {
        // Obtener la posición del bin
        int binX = gameManager.binPosition[0];
        int binZ = gameManager.binPosition[1];

        // Definir una distancia segura fuera del grid del pavimento
        int safeDistance = 3;

        // Inicializar la posición del restaurante
        Vector3 restaurantPosition = new Vector3();

        // Determinar en qué borde del grid se encuentra el bin y colocar el restaurante en frente
        if (binX <= safeDistance) // Bin cerca del borde oeste
        {
            restaurantPosition = new Vector3(-safeDistance, 0, binZ);
        }
        else if (binX >= gameManager.width - safeDistance) // Bin cerca del borde este
        {
            restaurantPosition = new Vector3(gameManager.width + safeDistance, 0, binZ);
        }
        else if (binZ <= safeDistance) // Bin cerca del borde sur
        {
            restaurantPosition = new Vector3(binX, 0, -safeDistance);
        }
        else // Bin cerca del borde norte
        {
            restaurantPosition = new Vector3(binX, 0, gameManager.height + safeDistance);
        }

        // Orientar el restaurante hacia el bin
        Vector3 binPosition = new Vector3(binX, 0, binZ);
        Quaternion rotation = Quaternion.LookRotation(binPosition - restaurantPosition);

        // Instanciar el restaurante
        Instantiate(gameManager.Restaurant, restaurantPosition, rotation);
    }


}
