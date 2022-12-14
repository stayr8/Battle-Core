using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCtrl : MonoBehaviourPun
{
    public Transform firePos;
    public TextMesh playerName;

    public float speed = 10f;
    public float rotSpeed = 100f;

    private Transform tr;
    private PhotonView pv;

    private Vector3 currPos;
    private Quaternion currRot;

    private int hp = 100;
    private float respawnTime = 3.0f;
    private float h = 0f;
    private float v = 0f;
    private string name = "";
    private bool isDie = false;

    public void SetPlayerName(string name)
    {
        this.name = name;

        GetComponent<PlayerCtrl>().playerName.text = this.name;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
            stream.SendNext(name);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            SetPlayerName((string)stream.ReceiveNext());

        }
    }

    private void Start()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();

        pv.ObservedComponents[0] = this;
    }
    private void Update()
    {
        if(pv.IsMine && isDie == false)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
            tr.Translate(moveDir.normalized * Time.deltaTime * speed);
            tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("Mouse X"));


        }
        else if(!pv.IsMine)
        {
            if (tr.position != currPos)
            {
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
            }
            else
            {
            }

            if (tr.rotation != currRot)
            {
                tr.rotation = Quaternion.Lerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
            }
        }
    }
}
