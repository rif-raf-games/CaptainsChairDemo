using Articy.Captainschairdemo;
using Articy.Unity;
using Articy.Unity.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class getridofme : MonoBehaviour, IArticyFlowPlayerCallbacks
{
    public static getridofme Instance;
    Branch BranchToStartOn;
    ArticyFlowPlayer FlowPlayer;

    void Start()
    {
        
    }

    public void OnFlowPlayerPaused(IFlowObject aObject)
    {
        if (aObject == null) { Debug.LogWarning("NULL aObject in OnFlowPlayerPaused()."); return; }
        Debug.Log("OnFlowPlayerPaused() IFlowObject Type: " + aObject.GetType());

        var flowFragType = aObject as IObjectWithFeatureFlow_Fragment_Type;
        if (flowFragType != null)
        {
            Flow_Fragment_Type type = flowFragType.GetFeatureFlow_Fragment_Type().Flow_Fragment_Type;
            FlowFragment flowFrag = aObject as FlowFragment;
            Debug.Log("Fragment name: " + flowFrag.DisplayName + ", has text: " + flowFrag.Text);
            Debug.Log("enum val: " + type);
        }
    }

    public void PlayFlowPlayer()
    {
        FlowPlayer.Play();
    }
    public void OnBranchesUpdated(IList<Branch> aBranches)
    {
        if (aBranches == null || aBranches.Count == 0) { Debug.LogWarning("NULL or 0 branches in OnBranchesUpdated()."); return; }
        Debug.Log("Num branches: " + aBranches.Count);   
    }
    public void SetBranchToStartOn(Branch b)
    {
        IFlowObject t = b.Target;
      //  Debug.Log(t.GetHashCode());
        IArticyObject o = (IArticyObject)b.Target;
        int x = 0;
        x++;

        BranchToStartOn = b;
    }

    public Branch GetStartBranch()
    {
        return BranchToStartOn;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            FlowPlayer = GetComponent<ArticyFlowPlayer>();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void StartButtonClicked()
    {
        SceneManager.LoadScene("Ship");
    }
    
}
