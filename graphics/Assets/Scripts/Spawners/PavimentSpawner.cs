/// <summary>
/// The PavimentSpawner method is responsible for spawning the paviment.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PavimentSpawner : MonoBehaviour
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
            SpawnPaviment();
        }
    }

    /// <summary>
    /// The SpawnPaviment method is responsible for spawning the paviment.
    /// </summary>
    void SpawnPaviment()
    {
        int width = gameManager.width;
        int height = gameManager.height;
        int binPositionX = gameManager.binPosition[0];
        int binPositionY = gameManager.binPosition[1];
        GameObject pavimentPrefab = gameManager.pavimentPrefab;
        
        for (int i = 0; i <= width; ++i)
        {
            for (int j = 0; j <= height; j++)
            {
                GameObject go = Instantiate(pavimentPrefab, new Vector3(i, 0, j), Quaternion.identity);
                go.transform.parent = transform;

                Collider[] hitColliders = Physics.OverlapSphere(go.transform.position, 0.1f);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.gameObject.tag == "Tree")
                    {
                        Destroy(hitCollider.gameObject);
                    }
                }

                if (i == binPositionX && j == binPositionY)
                {
                    Renderer renderer = go.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        Material material = renderer.material;
                        material.color = new Color(1f, 0.49f, 0.09f);
                    }
                    else
                    {
                        Debug.LogError("Renderer component not found on Paviment object.");
                    }
                } else if ((i + j) % 2 == 0)
                {
                    Renderer renderer = go.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        Material material = renderer.material;
                        material.color = new Color(0.5f, 0.5f, 0.5f);
                    }
                    else
                    {
                        Debug.LogError("Renderer component not found on Paviment object.");
                    }
                }
            }
        }
    }
}
