using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        if(target == null)
        {
            return;
        }

        FieldOfView fow = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Vector3 vierAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 vierAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + vierAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + vierAngleB * fow.viewRadius);

        Handles.color = Color.red;
        if (target != null)
        {
            foreach (Transform visibleTarget in fow.visibleTargets)
            {
                //Handles.DrawLine(fow.transform.position, visibleTarget.position);
            }
        }
    }
}
