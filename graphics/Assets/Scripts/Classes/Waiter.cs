/// <summary>
/// The Waiter class is responsible for controlling the waiter in each step.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waiter : MonoBehaviour
{
    public Transform carryingPoint;
    public int interpolationFramesCount = 45;

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
    public int newX;
    
    [HideInInspector]
    public int newY;

    [HideInInspector]
    public Color mainColor;

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
    /// The Update method is responsible for updating the waiter's position.
    /// </summary>
    void Update(){
        bool changePositionX = transform.position.x != newX;
        bool changePositionZ = transform.position.z != newY;

        if ((changePositionX || changePositionZ) && gameManager.steps != 0){
            moveWaiter(newX, newY);
        }
    }

    /// <summary>
    /// The moveWaiter method is responsible for moving the waiter in each step.
    /// </summary>
    public void moveWaiter(float newX, float newY)
    {        
        Vector3 oldPosition = transform.position;
        Vector3 newPosition = new Vector3(newX, 0, newY);

        if (oldPosition.x > newPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        else if (oldPosition.x < newPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (oldPosition.z > newPosition.z)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (oldPosition.z < newPosition.z)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        StartCoroutine(animateWaiter(oldPosition, newPosition));
    }

    /// <summary>
    /// The animateWaiter method is responsible for animating the waiter.
    /// </summary>
    IEnumerator animateWaiter(Vector3 oldPosition, Vector3 newPosition)
    {
        float interpolationRatio = 0;
        while (interpolationRatio < 1)
        {
            interpolationRatio += 1f / interpolationFramesCount;
            transform.position = Vector3.Lerp(oldPosition, newPosition, interpolationRatio);
            yield return null;
        }
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

    void activateEmission(){
        Renderer renderer = GetComponentInChildren<Renderer>();
        Material material = renderer.materials[0];
        material.SetColor("_EmissionColor", mainColor * 1.5f);
    }

    void deactivateEmission(){
        Renderer renderer = GetComponentInChildren<Renderer>();
        Material material = renderer.materials[0];
        material.SetColor("_EmissionColor", mainColor * 0);
    }

    /// <summary>   
    /// The pickFood method is responsible for picking up the food.
    /// </summary>
    public void pickFood(GameObject food){
        activateEmission();
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
        deactivateEmission();
        foodCarried = null;
        Destroy(food);
        animator.SetFloat("Blend", 0f);
    }
}
