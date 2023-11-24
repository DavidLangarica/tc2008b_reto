/// <summary>
/// The Waiter class is responsible for controlling the waiter in each step.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waiter : MonoBehaviour
{
    [HideInInspector]
    public string id;
    [HideInInspector]
    public int CarryingFood;
    [HideInInspector]
    public int Step;
    [HideInInspector]
    public int X;
    [HideInInspector]
    public int Y;

    private bool outsideOfGrid = false;
    private GameManager gameManager;

    /// <summary>
    /// The moveWaiter method is responsible for moving the waiter in each step.
    /// </summary>
    void Start(){
        gameManager = GetComponentInParent<GameManager>();
        StartCoroutine(moveWaiterRoutine());
    }    

    /// <summary>
    /// The moveWaiterRoutine method is responsible for moving the waiter in each step.
    /// </summary>
    IEnumerator moveWaiterRoutine()
    {
        //  TODO: CHANGE VALIDATION
        while (!outsideOfGrid)
        {
            yield return new WaitForSeconds(1);
            float newX = transform.position.x;
            float newY = transform.position.z + 1;
            moveWaiter(newX, newY);
            outsideOfGrid = transform.position.z >= gameManager.height - 1;
        }
    }

    /// <summary>
    /// The moveWaiter method is responsible for moving the waiter in each step.
    /// </summary>
    void moveWaiter(float newX, float newY)
    {        
        float timeToMove = 0.2f;
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
    /// The pickFood method is responsible for picking up the food.
    /// </summary>
    public void pickFood(string foodId){
        GameObject food = GameObject.FindWithTag(foodId);

        food.transform.parent = transform;
        food.transform.position = new Vector3(transform.position.x, 0.7f, transform.position.z);
    }

    public void placeFood(string foodId){
        GameObject food = GameObject.FindWithTag(foodId);
        
        Destroy(food);
    }
}
