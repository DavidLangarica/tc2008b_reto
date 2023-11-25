/// <summary>
/// The Waiter class is responsible for controlling the waiter in each step.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waiter : MonoBehaviour
{
    public Transform carryingPoint;

    [HideInInspector]
    public string id;
    [HideInInspector]
    public int CarryingFood { get; set; }
    [HideInInspector]
    public int Step { get; set; }
    [HideInInspector]
    public int X { get; set; }
    [HideInInspector]
    public int Y { get; set; }

    private GameObject foodCarried = null;
    private GameManager gameManager;
    private Animator animator;

    /// <summary>
    /// The moveWaiter method is responsible for moving the waiter in each step.
    /// </summary>
    void Start(){
        gameManager = GetComponentInParent<GameManager>();
        animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// The moveWaiter method is responsible for moving the waiter in each step.
    /// </summary>
    public void moveWaiter(float newX, float newY)
    {        
        float timeToMove = 0.5f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(newX, 0, newY);
        float elapsedTime = 0f;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
        }
        transform.position = endPosition;
        
    }

    /// <summary>
    /// The onTriggerEnter method is responsible for detecting collisions.
    /// <param name="other">The other collider</param>  
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        GameObject collidedObject = other.gameObject;
        string collidedObjectName = collidedObject.name;

        if (CarryingFood == 0 && gameManager.isBinFound && collidedObjectName.Contains("food") && foodCarried == null){
            pickFood(collidedObject);
        }
        else if (collidedObjectName.Contains("Bin") && foodCarried != null){
            placeFood(foodCarried);
        }
    }

    /// <summary>   
    /// The pickFood method is responsible for picking up the food.
    /// </summary>
    public void pickFood(GameObject food){
        animator.SetFloat("Blend", 1f);
        foodCarried = food;

        food.GetComponent<Rigidbody>().isKinematic = true;
        food.GetComponent<Collider>().enabled = false;

        food.transform.position = carryingPoint.position;
        food.transform.parent = carryingPoint;
        animator.SetFloat("Blend", 2f);
    }

    /// <summary>
    /// The placeFood method is responsible for placing the food.
    /// </summary>
    public void placeFood(GameObject food){
        foodCarried = null;
        Destroy(food);
        animator.SetFloat("Blend", 0f);
    }
}
