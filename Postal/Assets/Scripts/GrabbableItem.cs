using UnityEngine;

public class GrabbableItem : MonoBehaviour
{
    private GameObject parentObj;
    private bool isGrabbed;
    private Vector2 offset;
    private Rigidbody2D rb;

    //throwing stuff
    private float throwStartingSpeed = 8.5f;
    private float throwSlowingMultiplier = 0.965f;
    private float throwMinimumSpeed = 0.25f;
    private bool thrown = false;
    private Vector2 throwDirection;

    //spell stuff
    private bool recalled = false;
    private bool postRecall = false;
    private float recallSpeed = 7.5f;
    private float recallSlowDistance = 1.25f;
    private float recallSlowingMultiplier = 0.925f;
    private float recallMinimumSpeed = 0.35f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parentObj = null;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isGrabbed) 
        { 
            UpdateOffset();
        }
        if (thrown)
        {
            rb.linearVelocity *= throwSlowingMultiplier;
            if (rb.linearVelocity.magnitude <= throwMinimumSpeed)
            {
                rb.linearVelocity = Vector2.zero;
                thrown = false;
            }
        }
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

    public void Grabbed(GameObject parent)
    {
        gameObject.tag = "Grabbed";
        parentObj = parent;
        gameObject.transform.parent = parentObj.transform;
        rb.linearVelocity = Vector2.zero;
        isGrabbed = true;
        thrown = false;
        recalled = false;
        postRecall = false;
    }

    public void Recalled(GameObject parent)
    {
        parentObj = parent;
        rb.linearVelocity = Vector2.zero;
        recalled = true;
        thrown = false;
    }

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

    public void Dropped()
    {
        gameObject.tag = "Grabbable";
        this.gameObject.transform.parent = null;
        parentObj = null;
        isGrabbed = false;
    }

    public void Thrown(Vector2 target)
    {
        thrown = true;
        throwDirection = (target - (Vector2)parentObj.transform.position).normalized;

        transform.position = parentObj.transform.position;
        rb.linearVelocity = throwDirection * throwStartingSpeed;

        Dropped();
    }
}
