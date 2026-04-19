using UnityEngine;

public class GrabbableItem : MonoBehaviour
{
    private Rigidbody2D rb;

    //grabbing stuff
    private bool isGrabbed = false;
    private GameObject parentObj;
    private Vector2 offset;

    //throwing stuff
    private bool thrown = false;
    private float throwStartingSpeed = 8.5f;
    private float throwSlowingMultiplier = 0.965f;
    private float throwMinimumSpeed = 0.25f;
    private Vector2 throwDirection;

    //spell stuff
    private bool recalled = false;
    private bool postRecall = false;
    private float recallSpeed = 7.5f;
    private float recallSlowDistance = 1.25f;
    private float recallSlowingMultiplier = 0.925f;
    private float recallMinimumSpeed = 0.35f;

    void Start()
    {
        parentObj = null;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //If grabbed, place the package on the right side of the player
        if (isGrabbed) 
        { 
            UpdateOffset();
        }
        //If the package is thrown (in flight), slowly reduce its speed until it hits throwMinimumSpeed, then conside it no longer thrown
        if (thrown)
        {
            rb.linearVelocity *= throwSlowingMultiplier;
            if (rb.linearVelocity.magnitude <= throwMinimumSpeed)
            {
                rb.linearVelocity = Vector2.zero;
                thrown = false;
            }
        }
        //If the recall spell has been cast on the object, move it towards the player until it gets close enough
        if (recalled)
        {
            if (!postRecall)
            {
                Vector2 direction = (Vector2)parentObj.transform.position - (Vector2)gameObject.transform.position;
                rb.linearVelocity = direction.normalized * recallSpeed;

                if (Vector2.Distance(gameObject.transform.position, parentObj.transform.position) < recallSlowDistance)
                {
                    postRecall = true;
                }
            }
            else 
            {
                rb.linearVelocity *= recallSlowingMultiplier;
                if (rb.linearVelocity.magnitude <= recallMinimumSpeed)
                {
                    rb.linearVelocity = Vector2.zero;
                    recalled = false;
                    postRecall = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Deals with thrown object bouncing off of the wall
        if (collision.gameObject.tag == "Wall")
        {
            float currentSpeed = rb.linearVelocity.magnitude;
            Vector2 normal = ((Vector2)transform.position - collision.ClosestPoint(transform.position)).normalized;

            if (thrown)
            {
                rb.linearVelocity = Vector2.Reflect(throwDirection, normal) * currentSpeed * 0.75f;
            }
            else if (recalled)
            {
                rb.linearVelocity = Vector2.zero;
                recalled = false;
                postRecall = false;
            }
        }
    }

    //When an object gets grabbed, set the needed fields and child it to the player
    public void Grabbed(GameObject parent)
    {
        gameObject.tag = "Grabbed";
        parentObj = parent;
        gameObject.transform.parent = parentObj.transform;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        isGrabbed = true;
        thrown = false;
        recalled = false;
        postRecall = false;
    }

    //Place the object on the side of the player according to the direction they are facing
    public void UpdateOffset()
    {

        //TODO: Update to yoinker script maybe
        switch (parentObj.GetComponent<PlayerController>().GetDirection()) 
        {
            case PlayerController.DIRECTION.UP:
                offset = new Vector2 (0f, 0.5f);
                break;
            case PlayerController.DIRECTION.DOWN:
                offset = new Vector2(0f, -0.5f);
                break;
            case PlayerController.DIRECTION.LEFT:
                offset = new Vector2(-0.5f, 0f);
                break;
            case PlayerController.DIRECTION.RIGHT:
                offset = new Vector2(0.5f, 0f);
                break;
        }
        
        this.gameObject.transform.localPosition = offset;
    }

    //When no longer carrying the package, update its fields to show it has been dropped
    public void Dropped()
    {
        gameObject.tag = "Grabbable";
        this.gameObject.transform.parent = null;
        parentObj = null;
        rb.simulated = true;
        isGrabbed = false;
    }

    //Set an initial velocity for a thrown object and set the field "thrown" to true so that it can be tracked in Update
    public void Thrown(Vector2 target)
    {
        thrown = true;
        throwDirection = (target - (Vector2)parentObj.transform.position).normalized;

        transform.position = parentObj.transform.position;
        rb.linearVelocity = throwDirection * throwStartingSpeed;

        Dropped();
    }

    //Set the object as recalled and give it the parent object to track where it needs to go
    public void Recalled(GameObject parent)
    {
        parentObj = parent;
        rb.linearVelocity = Vector2.zero;
        recalled = true;
        thrown = false;
    }
}
