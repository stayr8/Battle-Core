using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{

    public GameObject[] slash;
    private GameObject[] Slashs;

    public Transform ST;
    public Transform ST1;
    public Transform ST2;
    public Transform ST3;

    public GameObject currentWeaponInHand;

    public void StartSlash()
    {
        Slashs[0] = Instantiate(slash[0], currentWeaponInHand.transform.position, ST.rotation);
        StartDealDamage();
    }

    public void EndSlash()
    {
        EndDealDamage();
        Destroy(Slashs[0]);
    }

    public void StartSlash1()
    {
        Slashs[1] = Instantiate(slash[1], currentWeaponInHand.transform.position, ST1.rotation);
        StartDealDamage();
    }

    public void EndSlash1()
    {
        EndDealDamage();
        Destroy(Slashs[1]);
    }
    public void StartSlash2()
    {
        Slashs[2] = Instantiate(slash[2], currentWeaponInHand.transform.position, ST2.rotation);
        StartDealDamage();
    }

    public void EndSlash2()
    {
        EndDealDamage();
        Destroy(Slashs[2]);
    }
    public void StartSlash3()
    {
        Slashs[3] = Instantiate(slash[3], currentWeaponInHand.transform.position, ST3.rotation);
        StartDealDamage();
    }

    public void EndSlash3()
    {
        EndDealDamage();
        Destroy(Slashs[3]);
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

