using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class AIControl : MonoBehaviour {

    public NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    public ThirdPersonCharacter character { get; private set; } // the character we are controlling
    private PhotonView photonview;
    private float wanderTimer;
    private float timer;
    private bool move;
    private Vector3 dest;
    private float t;
    private float oldT;
    private bool moveStage;
    private PhotonView photonView;

    private void Start()
    {
        photonview = GetComponent<PhotonView>();
        // get the components on the object we need ( should not be null due to require component so no need to check )
        agent = GetComponent<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();
        agent.updateRotation = false;
        agent.updatePosition = true;
        timer = wanderTimer;
        move = true;
        moveStage = true;
        t = 0;
        oldT = 0;
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (t > oldT)
        {
            oldT = t;
            move = false;
            if (moveStage)
            {
                photonView.RPC("updateMove", PhotonTargets.OthersBuffered, move);
                moveStage = false;
            }
        }
        else if (t == oldT)
        {
            move = true;
            if (!moveStage)
            {
                photonView.RPC("updateMove", PhotonTargets.OthersBuffered, move);
                moveStage = true;
                Debug.Log("move");
                Debug.Log(move);
            }
        }
        timer += Time.deltaTime;
        if (move)
        {
            //Time's up -> change path and reset timer
            if (timer >= wanderTimer && photonview.isMine)
            {
                if (photonview.isMine)
                {
                    float wanderRadius = UnityEngine.Random.Range(15, 30);
                    wanderTimer = UnityEngine.Random.Range(1, 20);
                    dest = RandomNavSphere(transform.position, wanderRadius, -1);
                    photonview.RPC("updateNewDest", PhotonTargets.AllBufferedViaServer, wanderRadius, dest);
                }
            }
            // Still not get there -> continue move
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                character.Move(agent.desiredVelocity / 3, false, false);
            }
            else
            {
                // If player leaves, move is true and NPC set up new destination and start moving again
                timer = wanderTimer;
            }
        }
        // Stop
        else
        {
            agent.SetDestination(this.transform.position);
            character.Move(Vector3.zero, false, false);
        }

        

    }

    // Choose a random dsetination on NavMes
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    // Got hit, stop for 0-3s then change destination and start walking
    void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("ground"))
        {
            timer = wanderTimer - UnityEngine.Random.Range(0, 3);
            agent.ResetPath();
        }
        if (!collision.collider.CompareTag("npc_bound")){
            
        }
    }

    public void stop(Vector3 pointTo)
    {
        t = Time.time;
        transform.LookAt(pointTo);
    }

    [PunRPC]
    void updateMove(bool m)
    {
        move = m;
    }

    [PunRPC]
    void updateNewDest(float wanderRadius, Vector3 thisdest)
    {
        agent.SetDestination(thisdest);
        timer = 0;
    }
}
