using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTargeting : MonoBehaviour
{

    public GameObject selectedHero;
    public bool heroPlayer;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        selectedHero = GameObject.FindGameObjectWithTag("Player");   
    }

    // Update is called once per frame
    void Update()
    {
        //Minion Targeting
        if(Input.GetMouseButtonDown(1))
        {
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                // If the minion is targetable
                if(hit.collider.GetComponent<Targetable>() != null)
                {
                    if(hit.collider.gameObject.GetComponent<Targetable>().targetType == Targetable.TargetType.Minion)
                    {
                        selectedHero.GetComponent<HeroCombat>().targetedEnemy = hit.collider.gameObject;
                    }
                    else if (hit.collider.gameObject.GetComponent<Targetable>().targetType == Targetable.TargetType.item)
                    {
                        Interactable interactable = hit.collider.GetComponent<Interactable>();
                        selectedHero.GetComponent<HeroCombat>().targetedItem = hit.collider.gameObject;
                        if (interactable != null)
                        {
                            //SetFocus(interactable, hit.point);
                            interactable.OnFocused(selectedHero.transform);
                        }
                    }
                }
                else if(hit.collider.gameObject.GetComponent<Targetable>() == null)
                {
                    selectedHero.GetComponent<HeroCombat>().targetedEnemy = null;
                    selectedHero.GetComponent<HeroCombat>().targetedItem = null;
                }
            }
        }
        
    }
}
