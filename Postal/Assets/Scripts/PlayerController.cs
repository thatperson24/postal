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

    void Update()
    {
        PlayerMovement();
        CarryItem();
        CastSpells();
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
        //On down click, check if there is an object in the direction you are looking
        if (Input.GetMouseButtonDown(0))
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
            gameObject.GetComponent<SpellController>().CastSpell(spellString);
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
}
