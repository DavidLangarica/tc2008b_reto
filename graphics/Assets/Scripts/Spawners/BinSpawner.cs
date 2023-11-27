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
        }
    }

    /// <summary>
    /// The SpawnBin method is responsible for spawning the Bin.
    /// </summary>
    public void SpawnBin(Bin bin)
    {
        int binX = bin.X;
        int binZ = bin.Y;

        GameObject go = Instantiate(gameManager.binPrefab, new Vector3(binX, 0, binZ), Quaternion.identity);
        go.transform.position = new Vector3(binX, 38.5f, binZ);
        go.transform.parent = transform;
    }
}
