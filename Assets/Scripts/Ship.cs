using Articy.Captainschairdemo;
using Articy.Unity;
using Articy.Unity.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship : CaptainsChairSceneRoot
{
   // public Text DebugText;
    Branch NextBranch = null;
    IFlowObject CurPauseObject = null;
    List<Branch> CurBranches = new List<Branch>();
    // Start is called before the first frame update
    public override void Start()
    {        
        base.Start();
    }

    //To put it simple: IFlowObject is the basic type of anything the FlowPlayer can encounter while traversing.. 
    // That means every node, every pin and every connection is also an IFlowObject.If for some reason the passed 
    // aObject is null, we have encountered a Dead End.
    protected override void DoOnFlowPlayerPaused(IFlowObject aObject)
    {
        // Debug.Log("**********************************************************************DoOnFlowPlayerPaused()");
        //Debug.Log("OnFlowPlayerPaused() IFlowObject Type: " + aObject.GetType() + ", with TechnicalName: " + ((ArticyObject)aObject).TechnicalName);
        StaticStuff.FlowDebug("**********************************************************************DoOnFlowPlayerPaused()");
        StaticStuff.FlowDebug("OnFlowPlayerPaused() IFlowObject Type: " + aObject.GetType() + ", with TechnicalName: " + ((ArticyObject)aObject).TechnicalName);
        CurPauseObject = aObject;
        // Note...a Dialogue Fragment is NOT a Flow Fragment
        var flowFragType = aObject as IObjectWithFeatureFlow_Fragment_Type;
        if (flowFragType != null)
        {
            Flow_Fragment_Type type = flowFragType.GetFeatureFlow_Fragment_Type().Flow_Fragment_Type;
            FlowFragment flowFrag = aObject as FlowFragment;
            //Debug.Log("Fragment name: " + flowFrag.DisplayName + ", has text: " + flowFrag.Text);
            //Debug.Log("enum val: " + type);
            StaticStuff.FlowDebug("Fragment name: " + flowFrag.DisplayName + ", has text: " + flowFrag.Text);
            StaticStuff.FlowDebug("enum val: " + type);
        }
        var dialogueFrag = aObject as Dialogue_Fragment;
        if(dialogueFrag != null )
        {
            //Debug.Log("We have a dialogue fragment...so DO SOMETHING");
            StaticStuff.FlowDebug("We have a dialogue fragment...so don't do anything since branch update will set up the dialogue fragment stuff there");            
            // monote - "Ship.DoOnFlowPlayerPaused() about to show dialogue fragment"
            //Debug.LogWarning("We're changing the dialogue setup from Pause to BranchesUpdated because we can have more than 1");
           // base.ShowDialogueFragment(dialogueFrag);            
        }
        //Debug.Log("**********DoOnFlowPlayerPaused() END");
        StaticStuff.FlowDebug("**********DoOnFlowPlayerPaused() END");
    }

    // iFlowObject vs Branch
    //  Branch.Target is an iFlowObjects

    protected override void DoOnBranchesUpdated(IList<Branch> aBranches)
    {
        //Debug.Log("**********DoOnBranchesUpdated()");        
        //Debug.Log("Num branches: " + aBranches.Count);
        StaticStuff.FlowDebug("**********DoOnBranchesUpdated()");
        StaticStuff.FlowDebug("Num branches: " + aBranches.Count);

        List<Branch> validBranches = new List<Branch>();
        CurBranches.Clear();
        string s = "";
        foreach (Branch b in aBranches)
        {
            base.PrintBranchInfo(b, "branch loop");
            Dialogue_Fragment d = b.Target as Dialogue_Fragment;           
            if(d != null)
            {                
                s += "Branch " + b.BranchId + ": ";
                s += d.InputPins[0].Text.RawScript + "\n";                
            }
            //if (s != "") DebugText.text = s;
            CurBranches.Add(b);            
            if (b.IsValid == true) validBranches.Add(b);
        }

        if (aBranches.Count == 1 && aBranches[0].IsValid && aBranches[0].Target.GetType().Equals(typeof(Hub)))
        {
            StaticStuff.FlowDebug("only one valid branch and it's a hub called: " + aBranches[0].DefaultDescription + " so Play() it");
            NextBranch = aBranches[0];
            base.PrintBranchInfo(NextBranch, "move to hub");
            //inventorySystem.RemoveItem(heldItem);
            //inventorySystem.RemoveItem(thisRepresentedItem);
        }
        else
        {            
            if(validBranches.Count == 1 && CurPauseObject.GetType().Equals(typeof(Hub)))
            {
                s = "1 valid branch on a hub pause object...check what it is: ";
                s += base.ReturnBranchInfo(validBranches[0], "checking 1 branch on hub");                
                StaticStuff.FlowDebug(s);
                if(validBranches[0].Target.GetType().Equals(typeof(OutputPin)))
                {                    
                    StaticStuff.FlowDebug("only valid output is an OutputPin...which means that we're not linked to any other flow fragments so just sit tight and wait for something" +
                        "like the player colliding with an NPC or something else to trigger the next StartOn");
                }
                else
                {                    
                    StaticStuff.FlowDebug("only valid output is something else that an OutputPin...so ROCK IT via Play(NextBranch)");                 
                    NextBranch = validBranches[0];                    
                }                
            }
            else
            {                
                StaticStuff.FlowDebug("--------------------------NOT in a situation where we have 1 valid Hub branch, so find out what to do.");
                if(CurPauseObject.GetType().Equals(typeof(Dialogue_Fragment)))
                {
                    StaticStuff.FlowDebug("We're on a dialogue fragment, so set up the dialogue via the CaptainsChairSceneRoot");
                    base.ShowDialogueFragment(CurPauseObject as Dialogue_Fragment, validBranches);
                }
                else
                {
                    StaticStuff.FlowDebug("on something other than a dialogue fragment, so find out what to do");
                    if (validBranches.Count == 1 && CurPauseObject.GetType().Equals(typeof(Jump)) && validBranches[0].Target.GetType().Equals(typeof(MiniGameFragment)))
                    {
                        StaticStuff.FlowDebug("we have 1 valid branch that's a Jump to a MiniGame so go to that mini game");
                        base.GoToMiniGame(validBranches[0]);
                    }
                    else
                    {
                        StaticStuff.FlowDebugWarning("not a 1 valid brach that's a Jump to a MiniGame so figure out what to do");
                    }
                }                
            }
        }        
        StaticStuff.FlowDebug("************************************************************DoOnBranchesUpdated() END");
    }

    public void DialogueButtonCallback(int buttonIndex)
    {
        StaticStuff.FlowDebug("DialogueButtonCallback() buttonIndex: " + buttonIndex);
        NextBranch = CurBranches[buttonIndex];
        base.PrintBranchInfo(CurBranches[buttonIndex], "DialogueButtonCallback");        
        if (CurBranches[buttonIndex].Target.GetType().Equals(typeof(Dialogue_Fragment)) == false)
        {
            //Debug.Log("we're done with the current dialogue tree, so shut off the UI and let the flow handle itself");
            StaticStuff.FlowDebug("Chosen branch isn't a dialogue fragment, so for now assume we're done talking and shut off the UI");
            base.ShutOffDialogueUI();
        }
        else
        {
            //Debug.LogWarning("Need to account for a flow fragment off of a dialogue UI button press");
            StaticStuff.FlowDebug("Chosen branch is a dialogue fragment, so just let the engine handle the next phase.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(NextBranch != null)
        {
            Branch b = NextBranch;
            NextBranch = null;
            base.PlayNextFlow(b);
        }
    }    
}


