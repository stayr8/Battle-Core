using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedProjectile : MonoBehaviour
{
    public float damage;
    public GameObject target;

    public bool targetSet;
    public string targetType;
    public float velocity = 10;
    public bool stopProjectile;

    public Rigidbody rigid;
    RaycastHit hit;

    [SerializeField]
    float ArrowLength;


    private void Start()
    {
        rigid = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target)
        {
            if(target == null)
            {
                Destroy(gameObject);
            }

            Vector3 targetpos = new Vector3(target.transform.position.x, target.transform.position.y + 1.0f, target.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetpos, velocity * Time.deltaTime);
            
        }
        

        Destroy(this.gameObject, 3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            int layerMask = 1 << 9;
            if (Physics.Raycast(transform.position, transform.position - transform.up, out hit, ArrowLength, layerMask))
            {
                if (hit.transform.TryGetComponent(out Enemy enemy))
                {
                    enemy.TakeDamage(damage);
                    enemy.HitVFX(hit.point);
                }
            }
            
            Destroy(gameObject);
        }
    }
    

}
