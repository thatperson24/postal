using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private List<Collider2D> grabCheckColliders;

    private bool canMove = true;
    private Vector2 direction = Vector2.zero;
    private float moveSpeed = 5.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        grabCheckColliders = new List<Collider2D>();
        grabCheckColliders.Add(GameObject.Find("Left Grabber").GetComponent<Collider2D>());
        grabCheckColliders.Add(GameObject.Find("Right Grabber").GetComponent<Collider2D>());
        grabCheckColliders.Add(GameObject.Find("Up Grabber").GetComponent<Collider2D>());
        grabCheckColliders.Add(GameObject.Find("Down Grabber").GetComponent<Collider2D>());
    }

    void FixedUpdate()
    {
        PlayerMovement();
        HandleColliders();
        CarryItem();
    }

    private void PlayerMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            direction += new Vector2(0, 1);
        } 
        if (Input.GetKey(KeyCode.S))
        {
            direction += new Vector2(0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += new Vector2(-1, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += new Vector2(1, 0);
        }
        direction.Normalize();

        if (canMove)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void HandleColliders()
    {
        foreach (Collider2D col in grabCheckColliders)
        {
            col.gameObject.SetActive(false);
        }

        if (direction.x < 0)
        {
            //Turn on left collider
            grabCheckColliders[1].gameObject.SetActive(true);
        }
        else if (direction.x > 0)
        {
            //Turn on right collider
            grabCheckColliders[0].gameObject.SetActive(true);
        }
        else if (direction.y > 0)
        {
            //Turn on up collider
            grabCheckColliders[2].gameObject.SetActive(true);
        }
        else if (direction.y > 0)
        {
            //Turn on down collider
            grabCheckColliders[3].gameObject.SetActive(true);
        }
    }

    private void CarryItem()
    {
        //If colliding
        if (Input.GetMouseButtonDown(0))
        {
            //Tell object youre held
            Debug.Log("Holdin that johgn");
        }
        if (Input.GetMouseButtonUp(0))
        {
            //Tell object youre not held
            Debug.Log("Dropped that John");
        }
    }
}
