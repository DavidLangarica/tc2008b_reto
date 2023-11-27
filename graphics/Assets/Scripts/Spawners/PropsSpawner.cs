/// <summary>
/// The PropsSpawner method is responsible for spawning the props.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsSpawner : MonoBehaviour
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
            SpawnTrees();
        }
    }

    /// <summary>
    /// The SpawnTrees method is responsible for spawning the trees.
    /// </summary>
    void SpawnTrees()
    {
        int numTrees = 150; 
        GameObject treeA = gameManager.treeA;
        GameObject treeB = gameManager.treeB;

        GameObject environment = GameObject.Find("ENVIRONMENT");
        GameObject terrainObject = environment.transform.Find("Terrain").gameObject;

        if (terrainObject == null)
        {
            Debug.LogError("Terrain object not found");
            return;
        }

        MeshCollider terrainCollider = terrainObject.GetComponent<MeshCollider>();

        if (terrainCollider == null)
        {
            Debug.LogError("MeshCollider not found on the terrain object");
            return;
        }

        Bounds terrainBounds = terrainCollider.bounds;

        for (int i = 0; i < numTrees; ++i)
        {
            bool treePlaced = false;
            while (!treePlaced)
            {
                float x = Random.Range(terrainBounds.min.x, terrainBounds.max.x);
                float z = Random.Range(terrainBounds.min.z, terrainBounds.max.z);
                Vector3 position = new Vector3(x, 100, z);

                if (Physics.Raycast(position, -Vector3.up, out RaycastHit hit, Mathf.Infinity))
                {
                    if (hit.collider.gameObject == terrainObject)
                    {
                        Vector3 treePosition = hit.point;
                        GameObject treeToSpawn = (Random.Range(0, 2) == 0) ? treeA : treeB;
                        GameObject go = Instantiate(treeToSpawn, treePosition, Quaternion.identity);
                        go.transform.parent = transform;
                        go.transform.Rotate(-90, 0, 0);
                        treePlaced = true;
                    }
                }
            }
        }
    }
}
