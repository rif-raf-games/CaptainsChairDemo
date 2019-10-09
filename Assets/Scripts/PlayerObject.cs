using Articy.Captainschairdemo;
using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerObject : MonoBehaviour
{
    NavMeshAgent NavMeshAgent;
    CaptainsChairSceneRoot SceneRoot;

    // Start is called before the first frame update
    void Start()
    {        
        NavMeshAgent = this.GetComponent<NavMeshAgent>();
        SceneRoot = GameObject.FindObjectOfType<CaptainsChairSceneRoot>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.name + " OnTriggerEnter() other: " + other.name);
        GameObject go = other.gameObject;        
        ArticyReference artReference = go.GetComponent<ArticyReference>();
        if(artReference == null )
        {
            Debug.LogWarning("null ArticyReference on the thing we collided with.");
        }
        else
        {
            Debug.Log("we connected with something that has an ArticyRef.  Now lets see if it's an NPC.");
            ArticyRef artRef = artReference.reference;
            FlowFragment flowFrag = (FlowFragment)artRef;
            List<ArticyObject> attachments = flowFrag.Attachments;
            if(attachments.Count == 1 )
            {
                ArticyObject ao = attachments[0];
                Debug.Log("flowFrag attachment[0] type: " + ao.GetType());                
                
                if(ao.GetType().Equals(typeof(NPC)))
                {
                    NPC npc = (NPC)ao;
                    ArticyObject ao2 = (ArticyObject)artRef;
                    Debug.Log("this mofo we collided with is an NPC, so set the FlowPlayer to start on it.");
                    SceneRoot.SetFlowPlayerStartOn(ao2);
                    int x = 0;
                    x++;
                }
                else
                {
                    Debug.LogWarning("the first attachment is NOT an NPC");
                }
            }
            else
            {
                Debug.LogWarning("not sure if we need a warning but i'm expecting only 1 attachment, which should be an NPC");
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
        
    }
}
