using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    bool camDealDamage;
    List<GameObject> hasDealtDamage;

    [SerializeField]
    float weaponLength;
    [SerializeField]
    private float weaponDamage;

    

    void Start()
    {
        camDealDamage = false;
        hasDealtDamage = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(camDealDamage)
        {
            RaycastHit hit;

            int layerMask = 1 << 9;
            if(Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
            {
                if(hit.transform.TryGetComponent(out Enemy enemy) && !hasDealtDamage.Contains(hit.transform.gameObject))
                {
                    enemy.TakeDamage(weaponDamage);
                    enemy.HitVFX(hit.point);
                    hasDealtDamage.Add(hit.transform.gameObject);
                }
            }
            int layerMask2 = 1 << 8;
            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask2))
            {
                if (hit.transform.TryGetComponent(out HealthSystem health) && !hasDealtDamage.Contains(hit.transform.gameObject))
                {
                    health.TakeDamage(weaponDamage);
                    health.HitVFX(hit.point);
                    hasDealtDamage.Add(hit.transform.gameObject);
                }
            }
            int layerMask3 = 1 << 14;
            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask3))
            {
                if (hit.transform.TryGetComponent(out Box box) && !hasDealtDamage.Contains(hit.transform.gameObject))
                {
                    box.Mining();
                }
            }
        }
    }

    public void StartDealDamage()
    {
        camDealDamage = true;
        hasDealtDamage.Clear();

    }

    public void EndDealDamage()
    {
        camDealDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}
