using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    private List<GrabZoneController> grabZones;
    private GameObject grabbedObject;

    private bool canMove = true;
    private float moveSpeed = 5.0f;
    private bool castingSpell = false;
    private string spellString = "";
    private SpellController spellController;

    private Vector2 velocity = Vector2.zero;
    private DIRECTION direction = DIRECTION.UP;

    private Camera mainCamera;
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

        mainCamera = Camera.main;

        spellController = GetComponent<SpellController>();
    }

    void Update()
    {
        Throw();
        PlayerMovement();
        PlayerDirection();
        CarryItem();
        CastSpells();
    }

    private void PlayerDirection()
    {
        //This section will set direction to the most recent key pressed
        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = DIRECTION.UP;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            direction = DIRECTION.DOWN;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            direction = DIRECTION.LEFT;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            direction = DIRECTION.RIGHT;
        }

        //This section will reset the direction if a key is let go of and there is still velocity
        if (velocity == Vector2.up) 
        {
            direction = DIRECTION.UP;
        }
        if (velocity == Vector2.down)
        {
            direction = DIRECTION.DOWN;
        }
        if (velocity == Vector2.left)
        {
            direction = DIRECTION.LEFT;
        }
        if (velocity == Vector2.right)
        {
            direction = DIRECTION.RIGHT;
        }
    }

    private void PlayerMovement()
    {
        velocity = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            velocity += Vector2.up;
        } 
        if (Input.GetKey(KeyCode.S))
        {
            velocity += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            velocity += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity += Vector2.right;
        }
        velocity.Normalize();

        if (canMove)
        {
            rb.MovePosition(rb.position + velocity * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void CarryItem()
    {
        //On down click, check if there is an object in the direction you are looking
        if (Input.GetMouseButtonDown(0) && !spellController.AimingSpell())
        {
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
        //On let go of click, drop the item you are holding
        if (Input.GetMouseButtonUp(0))
        {
            if (grabbedObject != null) 
            {
                grabbedObject.GetComponent<GrabbableItem>().Dropped();
                grabbedObject = null;
            }
        }
    }

    private void CastSpells()
    {
        if (grabbedObject != null) return;

        //On space press, start recording key inputs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!castingSpell)
            {
                castingSpell = true;
            }
        }
        //On let go of space, send the spell combo to spell manager
        if (Input.GetKeyUp(KeyCode.Space))
        {
            castingSpell = false;
            spellController.CastSpell(spellString);
            spellString = "";
        }
    }

    //This records the key presses, only functions while castingSpell (holding down space bar)
    void OnGUI()
    {
        if (castingSpell && Event.current.isKey && Event.current.type == EventType.KeyDown)
        {
            switch (Event.current.keyCode)
            {
                case KeyCode.UpArrow:
                    spellString += "U";
                    break;
                case KeyCode.DownArrow:
                    spellString += "D";
                    break;
                case KeyCode.LeftArrow:
                    spellString += "L";
                    break;
                case KeyCode.RightArrow:
                    spellString += "R";
                    break;
            }
        }
    }

    public DIRECTION GetDirection()
    {
        return direction;
    }

    public void Throw()
    {
        if (Input.GetMouseButtonDown(1) && grabbedObject != null)
        {
            Vector2 mousePos = Input.mousePosition;

            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector2(mousePos.x, mousePos.y)
            );

            grabbedObject.GetComponent<GrabbableItem>().Thrown(mouseWorldPos);
            grabbedObject = null;
        }
    }
}
