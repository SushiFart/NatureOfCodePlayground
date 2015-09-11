﻿using UnityEngine;
using System.Collections;

// http://wiki.unity3d.com/index.php?title=DontGoThroughThings
/*
Attach the script to any object that might move fast enough to go through other colliders. Make sure that the LayerMask does not include the layer of the object the script is attached to, otherwise the object will collide with itself. Leave Skin Width at 0.1 unless the object still passes through other colliders. If this happens, increase the skin width until the issue stops or you reach a value of 1.0.
    */
public class DontGoThroughThings : MonoBehaviour
{
    // Careful when setting this to true - it might cause double
    // events to be fired - but it won't pass through the trigger
    public bool sendTriggerMessage = false;

    public LayerMask layerMask = -1; //make sure we aren't in this layer 
    public float skinWidth = 0.1f; //probably doesn't need to be changed 

    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;
    private Vector2 previousPosition;
    private Rigidbody2D myRigidbody;
    private Collider2D myCollider;

    public bool HasHit { get; set; }

    //initialize values 
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        previousPosition = myRigidbody.position;
        minimumExtent = Mathf.Min(Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y), myCollider.bounds.extents.z);
        partialExtent = minimumExtent * (1.0f - skinWidth);
        sqrMinimumExtent = minimumExtent * minimumExtent;
    }

    void FixedUpdate()
    {
        //have we moved more than our minimum extent? 
        Vector2 movementThisStep = myRigidbody.position - previousPosition;
        float movementSqrMagnitude = movementThisStep.sqrMagnitude;

        if (movementSqrMagnitude > sqrMinimumExtent)
        {
            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
            RaycastHit2D hitInfo = Physics2D.Raycast(previousPosition, movementThisStep, movementMagnitude, layerMask.value);

            //check for obstructions we might have missed 
            //if ()
            {
                if (!hitInfo.collider)
                {
                    this.HasHit = false;
                    myCollider.SendMessage("OnTriggerExit2D", myCollider);
                    return;
                }

                this.HasHit = true;

                if (hitInfo.collider.isTrigger)
                    hitInfo.collider.SendMessage("OnTriggerEnter2D", myCollider);

                    
                // This is disables so that I do no have to add an additional  collider that will stop the object
                //if (!hitInfo.collider.isTrigger)
                myRigidbody.position = hitInfo.point - (movementThisStep / movementMagnitude) * partialExtent;

            }
        } else if(HasHit)
        {
            myCollider.SendMessage("OnTriggerStay2D", myCollider);
        }

        previousPosition = myRigidbody.position;
    }
}