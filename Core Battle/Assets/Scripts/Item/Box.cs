using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField]
    private GameObject go_item_prefab;
    [SerializeField]
    private int count;

    [SerializeField]
    private int hp;

    [SerializeField]
    private int destroyTime;

    [SerializeField]
    private BoxCollider col;

    [SerializeField]
    private GameObject BoxEffect;
    [SerializeField]
    private GameObject Boxbreak;


    public void Mining()
    {
        hp--;
        if(hp <= 0)
        {
            Destruction();
        }
    }

    private void Destruction()
    {

        for(int i = 0; i<count; i++)
        {
            Instantiate(go_item_prefab, Boxbreak.transform.position, Quaternion.identity);
        }
        //col.enabled = false;
        Boxbreak.SetActive(false);
        BoxEffect.SetActive(true);
        Destroy(this.gameObject, destroyTime);
    }
}
