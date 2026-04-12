using UnityEditor.Rendering;
using UnityEngine;

public class GrabbableItem : MonoBehaviour
{
    private GameObject parentObj;
    private bool isGrabbed;
    private Vector2 offset;
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
    }

    public void Grabbed(GameObject parent)
    {
        gameObject.tag = "Grabbed";
        parentObj = parent;
        gameObject.transform.parent = parentObj.transform;
        isGrabbed = true;
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
}
