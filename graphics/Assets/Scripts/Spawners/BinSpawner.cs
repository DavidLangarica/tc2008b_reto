/// <summary>
/// The BinSpawner class is responsible for spawning the restaurant.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinSpawner : MonoBehaviour
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
            SpawnBin();
        }
    }

    /// <summary>
    /// The SpawnBin method is responsible for spawning the Bin.
    /// </summary>
    void SpawnBin()
    {
        int binX = gameManager.binPosition[0];
        int binZ = gameManager.binPosition[1];

        GameObject bin = Instantiate(gameManager.binPrefab, new Vector3(binX, 0, binZ), Quaternion.identity);
        bin.transform.position = new Vector3(binX, 38.5f, binZ);
        bin.transform.parent = transform;
    }


}
