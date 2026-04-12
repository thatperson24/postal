using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    private List<GrabZoneController> grabZones;
    private List<GameObject> grabbableObjects;
    private GameObject grabbedObject;

    private bool canMove = true;
    private float moveSpeed = 5.0f;

    private Vector2 velocity = Vector2.zero;
    private DIRECTION direction = DIRECTION.UP;

    public enum DIRECTION
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        grabZones = new List<GrabZoneController> { 
            GameObject.Find("Up Grabber").GetComponent<GrabZoneController>(),
            GameObject.Find("Down Grabber").GetComponent<GrabZoneController>(),
            GameObject.Find("Left Grabber").GetComponent<GrabZoneController>(),
            GameObject.Find("Right Grabber").GetComponent<GrabZoneController>()
        };
    }

    void FixedUpdate()
    {
        PlayerMovement();
        CarryItem();
    }

    private void PlayerMovement()
    {
        velocity = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            velocity += new Vector2(0, 1);
            direction = DIRECTION.UP;
        } 
        if (Input.GetKey(KeyCode.S))
        {
            velocity += new Vector2(0, -1);
            direction = DIRECTION.DOWN;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity += new Vector2(-1, 0);
            direction = DIRECTION.LEFT;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity += new Vector2(1, 0);
            direction = DIRECTION.RIGHT;
        }
        velocity.Normalize();

        if (canMove)
        {
            rb.MovePosition(rb.position + velocity * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void CarryItem()
    {
        //If colliding
        if (Input.GetMouseButtonDown(0))
        {
            //Tell object youre held
            Debug.Log("Holdin that johgn");

            GameObject grabbableObj = null;
            switch (direction) {
                case DIRECTION.UP:
                    grabbableObj = grabZones[0].GetNearestObj();
                    break;
                case DIRECTION.DOWN:
                    grabbableObj = grabZones[1].GetNearestObj();
                    break;
                case DIRECTION.LEFT:
                    grabbableObj = grabZones[2].GetNearestObj();
                    break;
                case DIRECTION.RIGHT:
                    grabbableObj = grabZones[3].GetNearestObj();
                    break;
            }

            if (grabbableObj != null)
            {
                grabbableObj.GetComponent<GrabbableItem>().Grabbed(this.gameObject);
                grabbedObject = grabbableObj;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (grabbedObject != null) 
            {
                grabbedObject.GetComponent<GrabbableItem>().Dropped();
                grabbedObject = null;
            }
        }
    }

    public DIRECTION GetDirection()
    {
        return direction;
    }
}
