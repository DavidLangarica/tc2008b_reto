/// <summary>
/// The WaiterSpawner class is responsible for spawning the waiters.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterSpawner : MonoBehaviour
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
            SpawnWaiters();
        }
    }

     /// <summary>
    /// The SpawnWaiters method is responsible for spawning the waiters and setting their colors.
    /// </summary>
    void SpawnWaiters()
    {
        gameManager.initWaiters();
        int width = gameManager.width;
        int height = gameManager.height;
        GameObject waiterPrefab = gameManager.waiterPrefab;
        Waiter[] waiters = gameManager.waiters;
        Random.InitState(System.DateTime.Now.Millisecond);

        for (int i = 0; i < waiters.Length; ++i)
        {
            GameObject go = Instantiate(waiterPrefab, new Vector3(waiters[i].X, 0.1f, waiters[i].Y), Quaternion.identity);
            go.name = waiters[i].id;
            go.transform.parent = transform;

            Transform sphere = go.transform.Find("Sphere");

            if (sphere != null)
            {
                Renderer renderer = sphere.GetComponent<Renderer>();
                if (renderer != null)
                {
                    // Body
                    Material bodyMaterial = renderer.materials[0]; 
                    // Eyes
                    Material eyesMaterial = renderer.materials[1]; 
                    // Arms and antenna
                    Material extremitiesMaterial = renderer.materials[2]; 
                    
                    float r = Random.Range(0.0f, 1.0f);
                    float g = Random.Range(0.0f, 1.0f);
                    float b = Random.Range(0.0f, 1.0f);

                    bodyMaterial.color = new Color(r, g, b) * 0.7f;
                    eyesMaterial.color = new Color(r, g, b) * 0.3f;
                    extremitiesMaterial.color = new Color(r, g, b);                   
                }
                else
                {
                    Debug.LogError("Renderer component not found on Sphere object.");
                }
            }
            else
            {
                Debug.LogError("Sphere object not found within the waiter prefab.");
            }

        }
    }
}
