using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MousePosition : MonoBehaviour
{

    [SerializeField]
    private Transform playerTr;
    [SerializeField]
    private Camera mainCamera;

    Vector3 mousePos;
    Vector3 followPt;


    private void Start()
    {
        mainCamera = Camera.main;
        //playerTr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseposition();

    }

    private void mouseposition()
    {
        if (playerTr == null)
        {
            return;
        }

        mousePos = Mouse3D.GetMouseWorldPosition();

        mousePos.y = 0f;
        // 마우스 위치 8등분
        followPt = (mousePos + playerTr.position) / 2;
        followPt = (followPt + playerTr.position) / 2;
        followPt = (followPt + playerTr.position) / 2;
        followPt = (followPt + playerTr.position) / 2;
        transform.position = followPt;


    }
}
