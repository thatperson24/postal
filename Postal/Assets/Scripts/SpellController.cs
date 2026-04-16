using UnityEngine;

public class SpellController : MonoBehaviour
{
    private bool aimingSpell = false;
    private string spellName = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AimSpell();
    }

    //Public method that takes the spell input from the player and checks if it matches a spell pattern we recognize
    public void CastSpell(string spellString)
    {
        Debug.Log("Casting spell: " + spellString);
        spellName = spellString;

        switch (spellString)
        {
            case "LUL":
                Debug.Log("Casting funny business");
                aimingSpell = true;
                break;
            case "UDUD":
                Debug.Log("Casting recall");
                aimingSpell = true;
                break;
            default:
                Debug.Log("Not a real spell");
                aimingSpell = false;
                spellName = "";
                break;
        }  
    }

    //If a spell is being cast/aimed, this is how we get the area they clicked on to cast the spell at that location
    private void AimSpell()
    {
        if (Input.GetMouseButtonDown(0) && aimingSpell)
        {
            Debug.Log("Fired spell");
            aimingSpell = false;

            //Checks for object on mouse click
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.transform)
            {
                GameObject clickedObject = hit.transform.gameObject;
                if (clickedObject.tag == "Grabbable")
                {
                    switch (spellName)
                    {
                        case "LUL":
                            Debug.Log(clickedObject.name + " just got joked on.");
                            aimingSpell = false;
                            break;
                        case "UDUD":
                            Debug.Log("Recalling " + clickedObject.name);
                            clickedObject.GetComponent<GrabbableItem>().Recalled(this.gameObject);
                            break;
                    }
                }
            }
            spellName = "";
        }
    }

    public bool AimingSpell()
    {
        return aimingSpell;
    }
}
