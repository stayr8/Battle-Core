using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDie : MonoBehaviour
{
    
    void Start()
    {
        Destroy(this.gameObject, 2f);
    }

}
