using Articy.Captainschairdemo;
using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerObject : MonoBehaviour
{
    public Text DebugText;
    public GameObject DestSphere;

    NavMeshAgent NavMeshAgent;
    CaptainsChairSceneRoot SceneRoot;
    private Vector3 CamOffset;
    private bool MovementBlocked;

    // Start is called before the first frame update
    void Start()
    {        
        NavMeshAgent = this.GetComponent<NavMeshAgent>();
        SceneRoot = GameObject.FindObjectOfType<CaptainsChairSceneRoot>();

        CamOffset = Camera.main.transform.position - this.transform.position;
        MovementBlocked = false;
        /*
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Floor");
        foreach (GameObject o in objs) o.GetComponent<MeshRenderer>().enabled = false;
        objs = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject o in objs) o.GetComponent<MeshRenderer>().enabled = false;*/
    }

    public void ToggleMovementBlocked(bool val)
    {
        MovementBlocked = val;
    }
    
    private void OnTriggerEnter(Collider other)
    {        
        StaticStuff.FlowDebug(this.name + " OnTriggerEnter() other: " + other.name);
        
        ArticyReference colliderArtRef = other.gameObject.GetComponent<ArticyReference>();
        if(colliderArtRef == null )
        {
            //Debug.LogWarning("null ArticyReference on the thing we collided with.");
            StaticStuff.FlowDebugWarning("null ArticyReference on the thing we collided with.");
        }
        else
        {
            //Debug.Log("we connected with something that has an ArticyRef.  Now lets see if it's an NPC.");
            StaticStuff.FlowDebug("we connected with something that has an ArticyRef.  Now lets see what it is.");                               
            Dialogue dialogue = colliderArtRef.reference.GetObject() as Dialogue;            
            if(dialogue != null)
            {
                StaticStuff.FlowDebug("we have a dialogue, so set the FlowPlayer to start on it");
                SceneRoot.SetFlowPlayerStartOn(dialogue);
                NavMeshAgent.SetDestination(this.transform.position);
            }
            else
            {
                StaticStuff.FlowDebugWarning("not sure what to do with this type yet: " + colliderArtRef.reference.GetObject().GetType());
            }            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log(this.name + " OnTriggerExit() other: " + other.name);
    }

    public void SetNavMeshDestination( Vector3 dest )
    {
        NavMeshAgent.SetDestination(dest);
    }
    // Update is called once per frame
    void Update()
    {
        if (MovementBlocked == false && Input.GetMouseButtonDown(0))
        {
            //Debug.Log("clicked button");
            LayerMask mask = LayerMask.GetMask("Floor");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                Vector3 dest = hit.point;
                DestSphere.transform.position = dest;
                SetNavMeshDestination(dest);
            }
        }
        
        DebugStuff();
    }

    private void LateUpdate()
    {
        Camera.main.transform.position = this.transform.position + CamOffset;
    }

    void DebugStuff()
    {
        string s = "";
        s += "agentTypeID: " + NavMeshAgent.agentTypeID + "\n";
        s += "autoBraking: " + NavMeshAgent.autoBraking + "\n";
        s += "autoRepath: " + NavMeshAgent.autoRepath + "\n";
        s += "avoidancePriority: " + NavMeshAgent.avoidancePriority + "\n";
        s += "baseOffset: " + NavMeshAgent.baseOffset + "\n";
        s += "hasPath: " + NavMeshAgent.hasPath + "\n";
        s += "height: " + NavMeshAgent.height + "\n";
        s += "isPathStale: " + NavMeshAgent.isPathStale + "\n";
        s += "isStopped: " + NavMeshAgent.isStopped + "\n";
        s += "obstacleAvoidanceType: " + NavMeshAgent.obstacleAvoidanceType.ToString() + "\n";
        s += "remainingDistance: " + NavMeshAgent.remainingDistance + "\n";
        s += "pathStatus: " + NavMeshAgent.pathStatus.ToString() + "\n";
        //DebugText.text = s;
        DebugText.text = MovementBlocked.ToString();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //NavMeshAgent.Stop();
            NavMeshAgent.isStopped = true;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            NavMeshAgent.isStopped = false;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            NavMeshAgent.SetDestination(this.transform.position);
        }
    }
}
