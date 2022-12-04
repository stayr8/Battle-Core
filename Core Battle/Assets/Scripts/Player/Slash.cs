using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{

    public GameObject slash;
    private GameObject Slashs;

    public Transform ST;

    public GameObject currentWeaponInHand;

    public void StartSlash()
    {
        Slashs = Instantiate(slash, currentWeaponInHand.transform.position, ST.rotation);
        StartDealDamage();
    }

    public void EndSlash()
    {
        EndDealDamage();
        Destroy(Slashs);
    }

    public void StartDealDamage()
    {
        currentWeaponInHand.GetComponentInChildren<DamageDealer>().StartDealDamage();
    }

    public void EndDealDamage()
    {
        currentWeaponInHand.GetComponentInChildren<DamageDealer>().EndDealDamage();
    }
}

