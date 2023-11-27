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
    public int CarryingFood;

    [HideInInspector]
    public int Step;

    [HideInInspector]
    public int X;

    [HideInInspector]
    public int Y;

    [HideInInspector]
    public Color mainColor;

    [HideInInspector]
    public Animator animator;

    [HideInInspector]
    public Vector3 targetPosition;

    private GameObject foodCarried = null;
    private GameManager gameManager;
    private float moveDuration = 1f;

    /// <summary>
    /// The Start method is responsible for moving the waiter in each step.
    /// </summary>
    void Start(){
        gameManager = GetComponentInParent<GameManager>();
        animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// The Update method is responsible for updating the waiter's position.
    /// </summary>
    void Update(){
        if ((transform.position != targetPosition && gameManager.steps > 0)){
            MoveWaiter();
        }
    }
    
    /// <summary>
    /// The MoveWaiter method is responsible for moving the waiter in each step.
    /// </summary>
    public void MoveWaiter()
    {        
        Vector3 oldPosition = transform.position;

        moveDuration = gameManager.GetStepUpdateTime();
        StartCoroutine(AnimateWaiterRotation(oldPosition));
        StartCoroutine(AnimateWaiterPosition(oldPosition));
    }

    /// <summary>
    /// The AnimateWaiterRotation method is responsible for animating the waiter's rotation.
    /// </summary>
    IEnumerator AnimateWaiterRotation(Vector3 oldPosition)
    {
        Vector3 direction = targetPosition - oldPosition;

        if (direction == Vector3.zero)
        {
            yield break;
        }

        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime / moveDuration * 2f);
        yield return null;
    }

    /// <summary>
    /// The AnimateWaiterPosition method is responsible for animating the waiter.
    /// </summary>
    IEnumerator AnimateWaiterPosition(Vector3 oldPosition)
    {
        moveDuration /= 2f;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / moveDuration);
        yield return null;
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
            PickFood(collidedObject);
        }
        else if (collidedObjectName.Contains("Bin") && foodCarried != null){
            PlaceFood(foodCarried);
        }
    }

    /// <summary>
    /// The ActivateEmission method is responsible for activating the emission.
    /// </summary>
    void ActivateEmission(){
        Renderer renderer = GetComponentInChildren<Renderer>();
        Material material = renderer.materials[0];
        material.SetColor("_EmissionColor", mainColor * 1.8f);
    }

    /// <summary>
    /// The DeactivateEmission method is responsible for deactivating the emission.
    /// </summary>
    void DeactivateEmission(){
        Renderer renderer = GetComponentInChildren<Renderer>();
        Material material = renderer.materials[0];
        material.SetColor("_EmissionColor", mainColor * 0);
    }

    /// <summary>   
    /// The PickFood method is responsible for picking up the food.
    /// </summary>
    public void PickFood(GameObject food){
        ActivateEmission();
        animator.SetFloat("Blend", 1f);

        foodCarried = food;

        food.GetComponent<Rigidbody>().isKinematic = true;
        food.GetComponent<Collider>().enabled = false;

        food.transform.position = carryingPoint.position;
        food.transform.parent = carryingPoint;
        animator.SetFloat("Blend", 2f);
    }

    /// <summary>
    /// The PlaceFood method is responsible for placing the food.
    /// </summary>
    public void PlaceFood(GameObject food){
        DeactivateEmission();
        foodCarried = null;
        Destroy(food);
        animator.SetFloat("Blend", 0f);
    }
}