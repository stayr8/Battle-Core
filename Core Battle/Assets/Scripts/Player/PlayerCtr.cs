using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCtr : MonoBehaviourPun, IPunObservable
{
    [Header("Controls")]
    public string playerName = "chan";
    public float playerSpeed = 5.0f;
    public float playerAtk = 10.0f;
    public float playerAtkSpeed = 1.0f;
    public bool Melee = false;
    public bool Ranged = false;


    public StateMachine movementSM;
    public StandingState standing;
    public AttackState attacking;
    public SkillState skilling;
    

    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Camera camera;
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public Rigidbody rigid;
    [HideInInspector]
    public Vector3 playerVelocity;
    [HideInInspector]
    public HeroCombat heroCombat;


    public bool skill;

    //Photon
    public Vector3 currPos;
    public Quaternion currRot;

    private PhotonView pv;
    HealthSystem HS;


    private void Start()
    {
        pv = GetComponent<PhotonView>();

        pv.ObservedComponents[0] = this;

    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponentInChildren<Rigidbody>();
        heroCombat = GetComponent<HeroCombat>();
        agent.updateRotation = false;


        movementSM = new StateMachine();
        standing = new StandingState(this, movementSM);
        attacking = new AttackState(this, movementSM);
        skilling = new SkillState(this, movementSM);

        movementSM.Initialize(standing);
    }

    void Update()
    {
        if(pv.IsMine && HS.isDie == false)
        {
            movementSM.currentState.HandleInput();

            movementSM.currentState.LogicUpdate();
        }
        
    }

    private void FixedUpdate()
    {
        movementSM.currentState.PhysicsUpdate();
    }

    //Photon
    public void SetPlayerName(string name)
    {
        this.playerName = name;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(name);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            SetPlayerName((string)stream.ReceiveNext());
        }
    }

}
