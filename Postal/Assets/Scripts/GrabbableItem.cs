using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class GrabbableItem : MonoBehaviour
{
    private GameObject parentObj;
    private bool isGrabbed;
    private Vector2 offset;

    //throwing stuff
    private float moveSpeed = 2.0f;
    private bool thrown = false;
    private Vector2 targetPos;
    private float maxThrowDistance = 3f;

    //spell stuff
    private bool recalled = false;
    private bool postRecall = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parentObj = null;
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
            gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, targetPos, moveSpeed * Time.deltaTime);
            if(Vector2.Distance(gameObject.transform.position, targetPos) < 0.1f)
            {
                thrown = false;
            }
        }
        if (recalled)
        {
            if (!postRecall)
            {
                Vector2 position1 = gameObject.transform.position;
                gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, parentObj.transform.position, moveSpeed * Time.deltaTime);
                Vector2 position2 = gameObject.transform.position;
                if (Vector2.Distance(position1, position2) < 0.075f)
                {
                    postRecall = true;
                    targetPos = parentObj.transform.position;
                }
            }
            else
            {
                gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, targetPos, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(gameObject.transform.position, targetPos) < 0.1f)
                {
                    recalled = false;
                    postRecall = false;
                }
            }
        }
    }

    public void Grabbed(GameObject parent)
    {
        gameObject.tag = "Grabbed";
        parentObj = parent;
        gameObject.transform.parent = parentObj.transform;
        isGrabbed = true;
        thrown = false;
        recalled = false;
        postRecall = false;
    }

    public void Recalled(GameObject parent)
    {
        parentObj = parent;
        recalled = true;
        thrown = false;
        targetPos = parent.transform.position;
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
        targetPos = target;

        if (Vector2.Distance(parentObj.transform.position, target) > maxThrowDistance && parentObj != null)
        {
            Vector2 direction = target - (Vector2)parentObj.transform.position;
            Vector2 newTarget = (Vector2)parentObj.transform.position + direction.normalized * maxThrowDistance;
           
            targetPos = newTarget;
        }
        transform.position = parentObj.transform.position;
        Dropped();
    }
}
