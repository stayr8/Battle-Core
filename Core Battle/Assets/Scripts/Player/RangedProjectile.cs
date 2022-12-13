using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedProjectile : MonoBehaviour
{
    public float speed = 15f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    //public GameObject hits;
    //public GameObject flash;
    //public GameObject[] Detached;
    public GameObject missileTarget;

    public float damage = 10;
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
        FieldOfView fieldOfView = GameObject.Find("Player2").GetComponent<FieldOfView>();
        missileTarget = fieldOfView.targeting;
        
        /*
        if(flash != null)
        {
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            //var flashPs = flashInstance.GetComponent<ParticleSystem>();
            
            if(flashPs!=null)
            {
                Destroy(flashInstance, flashPs.main, DurationUnit);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }*/
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        if(speed != 0)
        {
            if(missileTarget != null)
            {
                rigid.velocity = transform.forward * speed;
                Vector3 targetPos = missileTarget.transform.position - transform.position;
                transform.rotation = Quaternion.LookRotation(targetPos);
            }
            else
            {
                rigid.velocity = transform.forward * speed;
            }
        }

        Destroy(this.gameObject, 3f);
    }

    private void OnTriggerEnter(Collider col)
    {
        rigid.constraints = RigidbodyConstraints.FreezeAll;
        speed = 0;

        if (col.gameObject.tag == "Enemy")
        {
            int layerMask = 1 << 9;
            if (Physics.Raycast(transform.position, transform.position - transform.up, out hit, ArrowLength, layerMask))
            {
                if (hit.transform.TryGetComponent(out Enemy enemy))
                {
                    enemy.TakeDamage(damage);
                    enemy.HitVFX(hit.point);
                    Destroy(gameObject);
                }
            }

        }
        else if (col.gameObject.tag == "Player")
        {
            int layerMask2 = 1 << 8;
            if (Physics.Raycast(transform.position, transform.position - transform.up, out hit, ArrowLength, layerMask2))
            {
                if (hit.transform.TryGetComponent(out HealthSystem health))
                {
                    health.TakeDamage(damage);
                    health.HitVFX(hit.point);
                    Destroy(gameObject);
                }
            }
        }
        else if (col.gameObject.tag == "Box")
        {
            col.transform.GetComponent<Box>().Mining();
            Destroy(gameObject);
        }
    }
    
    

}
