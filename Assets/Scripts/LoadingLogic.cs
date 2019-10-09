using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Captainschairdemo;
using UnityEngine.SceneManagement;

public class LoadingLogic : MonoBehaviour, IArticyFlowPlayerCallbacks
{    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnFlowPlayerPaused(IFlowObject aObject)
    {
        if (aObject == null) { Debug.LogWarning("NULL aObject in OnFlowPlayerPaused()."); return; }
        Debug.Log("OnFlowPlayerPaused() IFlowObject Type: " + aObject.GetType());                
    }
    
    public void OnBranchesUpdated(IList<Branch> aBranches)
    {
        if (aBranches == null || aBranches.Count == 0) { Debug.LogWarning("NULL or 0 branches in OnBranchesUpdated()."); return; }
        Debug.Log("Num branches: " + aBranches.Count);
          
        int branchID = Random.Range(0, aBranches.Count);
        branchID = 0; // normally this would be random but for now just go to the Ship        
        Branch branch = aBranches[branchID];
        ArticyObject targetObject = (ArticyObject)branch.Target;        
        Slot_Container slotContainer = ArticyDatabase.GetObject<Slot_Container>("Start_On_Object");
        slotContainer.Template.Slot_Feature.Slot_Feature_Slot = targetObject;        
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