

using Articy.Captainschairdemo;
using Articy.Unity;
using Articy.Unity.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptainsChair : MonoBehaviour, IArticyFlowPlayerCallbacks
{
   // getridofme MCP;
    ArticyFlowPlayer FlowPlayer;
    public Player Player;
    Branch SelectedBranch;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("CaptainsChair");
        // MCP = FindObjectOfType<getridofme>();
        FlowPlayer = GetComponent<ArticyFlowPlayer>();
        Slot_Container sc = ArticyDatabase.GetObject<Slot_Container>("Start_On_Object");
        FlowPlayer.StartOn = sc.Template.Slot_Feature.Slot_Feature_Slot;            
        //  Branch b = MCP.GetStartBranch();
        //IArticyObject o = (IArticyObject)b.Target;
        //FlowPlayer.StartOn = o;
        //FlowPlayer = GetComponent<ArticyFlowPlayer>();
        // MCP = FindObjectOfType<MCP>();
        // FlowPlayer = GetComponent<ArticyFlowPlayer>();
        // MCP.PlayFlowPlayer();
    }

    
    Branch NextBranch = null;
    bool GoToNextBranch = false;
    public void OnFlowPlayerPaused(IFlowObject aObject)
    {
        if(aObject == null) { Debug.LogWarning("NULL aObject in OnFlowPlayerPaused()."); return; }
        Debug.Log("OnFlowPlayerPaused() IFlowObject Type: " + aObject.GetType());        

        var flowFragType = aObject as IObjectWithFeatureFlow_Fragment_Type;
        if (flowFragType != null)
        {            
            Flow_Fragment_Type type = flowFragType.GetFeatureFlow_Fragment_Type().Flow_Fragment_Type;
            FlowFragment flowFrag = aObject as FlowFragment;
            Debug.Log("Fragment name: " + flowFrag.DisplayName + ", has text: " + flowFrag.Text);
            Debug.Log("enum val: " + type);
        }
        IArticyObject o = aObject as IArticyObject;
    }

    public void OnBranchesUpdated(IList<Branch> aBranches)
    {
        if(aBranches == null || aBranches.Count == 0) { Debug.LogWarning("NULL or 0 branches in OnBranchesUpdated()."); return; }
        Debug.Log("Num branches: " + aBranches.Count);
        //Branch b = aBranches[0];
        
       // IArticyObject o = b as IArticyObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
