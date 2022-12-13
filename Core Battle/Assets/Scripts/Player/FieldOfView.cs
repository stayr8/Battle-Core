using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<GameObject> visibleTargets = new List<GameObject>();
    public GameObject targeting;
    public float shortDis;

    public GameObject targetingUI;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("FindTargetsDelay", 0.2f);
    }


    IEnumerator FindTargetsDelay(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            FindTargets();
        }
    }

    void FindTargets()
    {
        visibleTargets.Clear();
        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for(int i = 0; i < targetInViewRadius.Length; i++)
        {
            Transform target = targetInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if(!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target.transform.gameObject);
                    print("raycast hit!");
                    Debug.DrawRay(transform.position, dirToTarget * 10f, Color.red, 5f);
                }
            }
        }

        if(visibleTargets.Count != 0)
        {
            if(targetInViewRadius != null)
            {
                targetingUI.SetActive(false);
            }

            targeting = visibleTargets[0];

            shortDis = Vector3.Distance(transform.position, visibleTargets[0].transform.position);
            foreach(GameObject found in visibleTargets)
            {
                float distance = Vector3.Distance(transform.position, found.transform.position);
                if(distance < shortDis)
                {
                    targeting = found;
                    shortDis = distance;
                }
            }

            Debug.Log(targeting.name);

            targetingUI = targeting.transform.Find("Canvas").transform.gameObject;

            targetingUI.SetActive(true);
        }

        else if(visibleTargets.Count == 0 && targetingUI != null)
        {
            targetingUI.SetActive(false);
        }
    }

    
}
