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
        Vector3 pos = new Vector3(Boxbreak.transform.position.x, Boxbreak.transform.position.y + 0.2f, Boxbreak.transform.position.z);
        for(int i = 0; i<count; i++)
        {
            Instantiate(go_item_prefab, pos, Quaternion.identity);
        }
        //col.enabled = false;
        Boxbreak.SetActive(false);
        BoxEffect.SetActive(true);
        Destroy(this.gameObject, destroyTime);
    }
}
