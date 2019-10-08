using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Captainschairdemo;
using UnityEngine.SceneManagement;

public class LoadingLogic : MonoBehaviour, IArticyFlowPlayerCallbacks
{
    //getridofme MCP;
    ArticyFlowPlayer FlowPlayer;
    // Start is called before the first frame update
    void Start()
    {
       // MCP = FindObjectOfType<getridofme>();
        FlowPlayer = GetComponent<ArticyFlowPlayer>();

    }

    public void OnFlowPlayerPaused(IFlowObject aObject)
    {
        if (aObject == null) { Debug.LogWarning("NULL aObject in OnFlowPlayerPaused()."); return; }
        Debug.Log("OnFlowPlayerPaused() IFlowObject Type: " + aObject.GetType());

        /* var flowFragType = aObject as IObjectWithFeatureFlow_Fragment_Type;
        if (flowFragType != null)
        {
            Flow_Fragment_Type type = flowFragType.GetFeatureFlow_Fragment_Type().Flow_Fragment_Type;
            FlowFragment flowFrag = aObject as FlowFragment;
            Debug.Log("Fragment name: " + flowFrag.DisplayName + ", has text: " + flowFrag.Text);
            Debug.Log("enum val: " + type);
        }*/
        FlowPlayer.StartOn = (IArticyObject)aObject;
    }

    public ArticyRef articyRef;
    public void OnBranchesUpdated(IList<Branch> aBranches)
    {
        if (aBranches == null || aBranches.Count == 0) { Debug.LogWarning("NULL or 0 branches in OnBranchesUpdated()."); return; }
        Debug.Log("Num branches: " + aBranches.Count);
        for(int i=0; i<aBranches.Count; i++)
        {
            Branch b = aBranches[i];
           // Debug.Log("branch id: " + b.BranchId);
            Debug.Log(b.DefaultDescription);
            IFlowObject t = b.Target;
           // Debug.Log(t.GetHashCode());
            IArticyObject o = (IArticyObject)b.Target;
        }        
        int x = Random.Range(0, 2);
        x = 0;
        Debug.Log("want to start on branch: " + x);
        Branch branch = aBranches[x];
        ArticyObject a = (ArticyObject)branch.Target;
        //ArticyObject soc = ArticyDatabase.GetObject("Start_On_Object");
        Slot_Container sc = ArticyDatabase.GetObject<Slot_Container>("Start_On_Object");
        sc.Template.Slot_Feature.Slot_Feature_Slot = a;
        //FlowPlayer.StartOn = a;        

        /* ArticyObject a = ArticyDatabase.GetObject("Start_On_Container");
        // FlowFragment f = a as FlowFragment;
         Start_On_Fragment sof = a as Start_On_Fragment;

         ArticyObject ao = sof.Template.Start_On_Feature.Start_On_Slot;
         x++;

         ArticyObject aa = ArticyDatabase.GetObject("Other_Location_Fragment");
         sof.Template.Start_On_Feature.Start_On_Slot = aa;
         x++;*/
        /*Debug.Log("b4 num attachments: " + f.Attachments.Count);
        for(int i=0; i< f.Attachments.Count; i++)
        {
            ArticyObject ao = f.Attachments[0];
            x++;
        }
        x++;
        f.Attachments.Clear();
        
        Debug.Log("aR num attachments: " + f.Attachments.Count);*/
        /*Branch startBranch = aBranches[x];
        IArticyObject ob = (IArticyObject)startBranch.Target;
        ArticyObject a = ArticyDatabase.GetObject("Start_On_Container");
        IFlowFragment f = a as IFlowFragment;        
        FlowFragment d = (FlowFragment)a;
        List<ArticyObject> attachments = d.Attachments;        
        for(int i=0; i<attachments.Count; i++)
        {
            Debug.Log(attachments[i].TechnicalName);
            Debug.Log(attachments[i].name);
        }*/

        // MCP.SetBranchToStartOn(aBranches[x]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonClicked()
    {
        SceneManager.LoadScene("Ship");
    }

}
