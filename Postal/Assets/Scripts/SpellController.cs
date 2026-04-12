using UnityEngine;

public class SpellController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CastSpell(string spellString)
    {
        Debug.Log("Casting spell: " + spellString);
        switch (spellString)
        {
            case "LUL":
                Debug.Log("Casting funny business");
                break;
            default:
                Debug.Log("Not a real spell");
                break;
        }
            
    }
}
