using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    private Transform mtransform;
    public Transform FollowCamera;

    private void Awake()
    {
        mtransform = this.transform;
    }

    private void FixedUpdate()
    {
        if(FollowCamera == null)
        {
            return;
        }

        Vector3 position = mtransform.position;
        Vector3 targetPosition = FollowCamera.position;

        Vector3 nextPosition = new Vector3(targetPosition.x, position.y, targetPosition.z);

        this.mtransform.position = nextPosition;


    }
}
