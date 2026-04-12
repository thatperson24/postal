using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrabZoneController : MonoBehaviour
{
    private List<GameObject> grabbableObjects = new List<GameObject>();
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetNearestObj()
    {
        if (grabbableObjects.Count == 0)
        {
            return null;
        }

        GameObject nearestObj = grabbableObjects[0];
        foreach (GameObject obj in grabbableObjects)
        {
            if (Vector2.Distance(obj.transform.position, player.transform.position) < Vector2.Distance(nearestObj.transform.position, player.transform.position))
            {
                nearestObj = obj;
            }
        }
        return nearestObj;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Grabbable")
        {
            grabbableObjects.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(grabbableObjects.Contains(collision.gameObject))
        {
            grabbableObjects.Remove(collision.gameObject);
        }
    }
}
