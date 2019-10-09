using Articy.Captainschairdemo;
using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : CaptainsChairSceneRoot
{
    Branch NextBranch = null;
    IFlowObject CurPauseObject = null;
    // Start is called before the first frame update
    public override void Start()
    {        
        base.Start();
    }

    protected override void DoOnFlowPlayerPaused(IFlowObject aObject)
    {
        Debug.Log("**********************************************************************DoOnFlowPlayerPaused()");
        Debug.Log("OnFlowPlayerPaused() IFlowObject Type: " + aObject.GetType() + ", with TechnicalName: " + ((ArticyObject)aObject).TechnicalName);
        CurPauseObject = aObject;
        // Note...a Dialogue Fragment is NOT a Flow Fragment
        var flowFragType = aObject as IObjectWithFeatureFlow_Fragment_Type;
        if (flowFragType != null)
        {
            Flow_Fragment_Type type = flowFragType.GetFeatureFlow_Fragment_Type().Flow_Fragment_Type;
            FlowFragment flowFrag = aObject as FlowFragment;
            Debug.Log("Fragment name: " + flowFrag.DisplayName + ", has text: " + flowFrag.Text);
            Debug.Log("enum val: " + type);
        }
        var dialogueFrag = aObject as Dialogue_Fragment;
        if(dialogueFrag != null )
        {
            Debug.Log("We have a dialogue fragment...so DO SOMETHING");
            base.StartDialogue(dialogueFrag);            
        }
        Debug.Log("**********DoOnFlowPlayerPaused() END");
    }

    protected override void DoOnBranchesUpdated(IList<Branch> aBranches)
    {
        Debug.Log("**********DoOnBranchesUpdated()");
        Debug.Log("Num branches: " + aBranches.Count);                    
        if(aBranches.Count == 1 && aBranches[0].IsValid && aBranches[0].Target.GetType().Equals(typeof(Hub)))
        {
            // only one valid branch and it's a hub so move to it
            Debug.Log("only one valid branch and it's a hub called: " + aBranches[0].DefaultDescription + " so Play() it");
            NextBranch = aBranches[0];
            base.PrintBranchInfo(NextBranch, "move to hub");
            //inventorySystem.RemoveItem(heldItem);
            //inventorySystem.RemoveItem(thisRepresentedItem);
        }
        else
        {
            List<Branch> validBranches = new List<Branch>();
            foreach (Branch b in aBranches)
            {
                base.PrintBranchInfo(b, "branch loop");
                if (b.IsValid == true) validBranches.Add(b);
            }
            if(validBranches.Count == 1 && CurPauseObject.GetType().Equals(typeof(Hub)))
            {
                string s = "1 valid branch on a hub...check what it is: ";
                s += base.ReturnBranchInfo(validBranches[0], "checking 1 branch on hub");
                Debug.Log(s);
                if(validBranches[0].Target.GetType().Equals(typeof(OutputPin)))
                {
                    Debug.Log("only valid output is an OutputPin...which is odd but fuck it we stay here since we're obviously expected to have the next bit triggered by a collision");
                }
                else
                {
                    Debug.Log("only valid output is something else that an OutputPin...so ROCK IT via Play(NextBranch)");
                    NextBranch = validBranches[0];
                }                
            }
            else
            {
                Debug.Log("We're in a situation where (BranchCount ==1 && Type==Hub is FALSE....so we just hang I guess.");
            }
        }
        Debug.Log("************************************************************DoOnBranchesUpdated() END");
    }

    // ********************************** IGNORE VALID BRANCHES OFF (pause on FlowFragments, Dialogues, Diaogue Fragments, Hubs, Jumps **************************************
    // -> Paused on Ship (Main Ship), which is a Location Fragment, and is the Frag set up in the Flow Player in the editor since the previous screen would have picked it to start
    //      Since there's only 1 valid branch and it's a hub called Ship Wandering so get to it via Play(NextBranch)
    // -> Paused on Ship_Wandering_Hub
    //      1 valid branch that's an OutputPin, which isn't any kid of fragment so don't advance either way
    //  --- WE ARE NOW WAITING FOR A COLLISION WITH AN NPC ------
    //  --- We collide with Captain Jones, who's an NPC, so set the FlowPlayer to StartOn() it.
    //  -> Paused on NPCFragment that we set above called Captain_Jones_Ref has an enum val of NPC
    //      1 valid branch, which is a hub called Captain Jones Hub, so Play() it via NextBranch
    //  -> Paused on a Hub called Captain_Jones_Start_Hub
    //      4 branches...3 invalid 1 valid...1 valid branch and it's NOT an OutputPin so ROCK IT via Play() NextBranch
    //  -> Paused on a Dialogue_Fragment called Captain Jones First Talk...since it's a Dialogue_Fragment it's TIME TO SHOW SOMETHING
    //      1 valid branch which is a Jump to the Ship_Wandering_Hub...

    // ********************************** IGNORE VALID BRANCHES ON (pause on FlowFragments, Dialogues, Diaogue Fragments, Hubs, Jumps **************************************
    // -> Paused on Ship (Main Ship), which is a Location Fragment, and is the Frag set up in the Flow Player in the editor since the previous screen would have picked it to start
    //      1 valid branch, a Hub called Ship Wandering to Play() it
    //  -> Paused on Ship_Wandering_Hub
    //      1 valid branch that's an OutputPin, which isn't any kid of fragment so don't advance either way
    //  --- WE ARE NOW WAITING FOR A COLLISION WITH AN NPC ------
    //  --- We collide with Captain Jones, who's an NPC, so set the FlowPlayer to StartOn() it.
    //  -> Paused on NPCFragment that we set above called Captain_Jones_Ref has an enum val of NPC    
    //      1 valid branch, which is a hub called Captain Jones Hub, so Play() it via NextBranch
    //  -> Paused on a Hub called Captain_Jones_Start_Hub
    //***** DIFF HERE ********** 1 valid branch on the hub...it's not an output pin (it's a Dialogue_Fragment) so ROCK IT via Play(NextBranch)
    //  -> Paused on a Dialogue_Fragment called Captain Jones First Talk...since it's a Dialogue_Fragment it's TIME TO SHOW SOMETHING
    //      1 valid branch which is a Jump to the Ship_Wandering_Hub...


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
