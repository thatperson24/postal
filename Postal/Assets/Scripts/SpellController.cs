using UnityEngine;

public class SpellController : MonoBehaviour
{
    private bool aimingSpell = false;
    private Spell currentSpell;
    public struct Spell
    {
        private string spellName;
        private bool aimingSpell;
        private bool targetsObject;

        public Spell(string spellName, bool aimingSpell)
        {
            this.spellName = spellName;
            this.aimingSpell = aimingSpell;
            targetsObject = false;
        }

        public Spell(string spellName, bool aimingSpell, bool inWorldSpace)
        {
            this.spellName = spellName;
            this.aimingSpell = aimingSpell;
            this.targetsObject = inWorldSpace;
        }

        public string GetSpellName()
        {
            return spellName; 
        }

        public bool GetTargetsObject()
        {
            return targetsObject;
        }

        public bool GetAimingSpell() 
        { 
            return aimingSpell; 
        }

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Spell funnyBusiness = new Spell("Funny Business", true);
        Spell recallSpell = new Spell("Recall", true);
        Spell deflectSpell = new Spell("Deflect", true, true);
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

        switch (spellString)
        {
            case "LUL":
                Debug.Log("Casting funny business");
                currentSpell = new Spell("Funny Business", true, true);
                break;
            case "UDUD":
                Debug.Log("Casting recall");
                currentSpell = new Spell("Recall", true, true);
                break;
            case "UDLR":
                Debug.Log("Casting deflect");
                currentSpell = new Spell("Deflect", true);
                break;
            default:
                Debug.Log("Not a real spell");
                currentSpell = new Spell("No Spell", false);
                break;
        }

        aimingSpell = currentSpell.GetAimingSpell();
        
    }

    //If a spell is being cast/aimed, this is how we get the area they clicked on to cast the spell at that location
    private void AimSpell()
    {
        if (Input.GetMouseButtonDown(0) && aimingSpell && currentSpell.GetSpellName() != "No Spell")
        {
            Debug.Log("Fired spell");
            aimingSpell = false;

            //Checks for object on mouse click
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.transform && currentSpell.GetTargetsObject())
            {
                GameObject clickedObject = hit.transform.gameObject;
                if (clickedObject.tag == "Grabbable")
                {
                    switch (currentSpell.GetSpellName())
                    {
                        case "Funny Business":
                            Debug.Log(clickedObject.name + " just got joked on.");
                            aimingSpell = false;
                            break;
                        case "Recall":
                            Debug.Log("Recalling " + clickedObject.name);
                            clickedObject.GetComponent<GrabbableItem>().Recalled(this.gameObject);
                            break;
                    }
                }
            }
            currentSpell = new Spell("No Spell", false);
        }
    }

    public bool GetAimingSpell()
    {
        return aimingSpell;
    }
}
