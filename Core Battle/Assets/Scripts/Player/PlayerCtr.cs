using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class PlayerCtr : MonoBehaviour
{
    [Header("Controls")]
    public string playerName = "chan";
    public float playerSpeed = 5.0f;
    public float playerAtk = 10.0f;
    public float playerAtkSpeed = 1.0f;


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
        
            movementSM.currentState.HandleInput();

            movementSM.currentState.LogicUpdate();
        
    }

    private void FixedUpdate()
    {
        movementSM.currentState.PhysicsUpdate();
    }


}
