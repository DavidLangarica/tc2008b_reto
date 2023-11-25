/// <summary>
/// The Food class is responsible for the food.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [HideInInspector]
    public string id;
    [HideInInspector]
    public int X { get; set; }
    [HideInInspector]
    public int Y { get; set; }

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;
        string collidedObjectName = collidedObject.name;
    }
}
