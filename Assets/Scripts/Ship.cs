using Articy.Captainschairdemo;
using Articy.Unity;
using Articy.Unity.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship : CaptainsChairSceneRoot
{
    public Text DebugText;
    Branch NextBranch = null;
    IFlowObject CurPauseObject = null;
    List<Branch> CurBranches = new List<Branch>();
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
            base.ShowDialogueFragment(dialogueFrag);            
        }
        Debug.Log("**********DoOnFlowPlayerPaused() END");
    }

    public void DialogueButtonCallback( int buttonIndex )
    {
        NextBranch = CurBranches[buttonIndex];
        base.PrintBranchInfo(CurBranches[buttonIndex], "DialogueButtonCallback");
        if(CurBranches[buttonIndex].GetType().Equals(typeof(Dialogue_Fragment)) == false)
        {
            Debug.Log("we're done with the current dialogue tree, so shut off the UI and let the flow handle itself");
            base.ShutOffDialogueUI();
        }            
    }

    protected override void DoOnBranchesUpdated(IList<Branch> aBranches)
    {        
        Debug.Log("**********DoOnBranchesUpdated()");        
        Debug.Log("Num branches: " + aBranches.Count);
       
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
            // only one valid branch and it's a hub so move to it
            Debug.Log("only one valid branch and it's a hub called: " + aBranches[0].DefaultDescription + " so Play() it");
            NextBranch = aBranches[0];
            base.PrintBranchInfo(NextBranch, "move to hub");
            //inventorySystem.RemoveItem(heldItem);
            //inventorySystem.RemoveItem(thisRepresentedItem);
        }
        else
        {            
            if(validBranches.Count == 1 && CurPauseObject.GetType().Equals(typeof(Hub)))
            {
                s = "1 valid branch on a hub...check what it is: ";
                s += base.ReturnBranchInfo(validBranches[0], "checking 1 branch on hub");
                Debug.Log(s);
                if(validBranches[0].Target.GetType().Equals(typeof(OutputPin)))
                {
                    Debug.Log("only valid output is an OutputPin...which is odd but fuck it we stay here since we're obviously expected to have the next bit triggered by a collision");
                }
                else
                {
                    Debug.Log("only valid output is something else that an OutputPin...so ROCK IT via Play(NextBranch)");
                   // DebugText.text += "chose branch: " + validBranches[0].BranchId;
                    NextBranch = validBranches[0];
                    /*Dialogue_Fragment d = (Dialogue_Fragment)NextBranch.Target;
                    if (d.InputPins.Count != 1) Debug.LogWarning("WTF...more than one input pin on this Dialogue_Fragment?: " + d.InputPins.Count);
                    foreach(InputPin i in d.InputPins)
                    {
                        Debug.Log("connection script: " + i.Text.RawScript);
                       // DebugText.text = i.Text.RawScript;
                    }*/
                }                
            }
            else
            {
                Debug.Log("---------------------------------------------------------------------------------We're in a situation where " +
                    "(BranchCount ==1 && Type==Hub is FALSE....we're waiting for a button press that'll take us to a Jump." +
                    " If the Jump is to a Hub it goes there via code.  If not, we get back here and wait to figure out what to do");
                // waiting for a Continue button press where we'll Jump to a Hub
                // waiting for a Continue button press where we'll Jump to a mini-game...in this case we come back here since Hub==FALSE...so we have to figure out how to get the mini-game started
                // FOR MIni-Game: Pause is Jump
                // Branch is to MiniGameFragment
                Debug.Log("Still in situation above.  CurPauseObject type: " + CurPauseObject.GetType());
                Debug.Log("first branch type: " + validBranches[0].Target.GetType());
                if(validBranches.Count == 1 )
                {
                    if (CurPauseObject.GetType().Equals(typeof(Jump)) && validBranches[0].Target.GetType().Equals(typeof(MiniGameFragment)))
                    {
                        base.GoToMiniGame(validBranches[0]);                        
                    }
                }                
                else Debug.LogWarning("more than 1 valid branch here...not sure what's up.");
            }
        }
        Debug.Log("************************************************************DoOnBranchesUpdated() END");
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
    //  --- You see the dialogue/picture with a Continue button....click the Continue button ... ---
    //  -> Paused on a Jump fragment, Jump_To_Ship_Wandering_Hub
    //      1 valid branch which is the Ship Wandering Hub, so Play() it
    //  -> Paused on Ship Wandering Hub
    //      1 valid branch, but it's an OutputPin (so the Hub isn't linked to anything), so do nothing but wait for collision

    // --- ready to start the cargo game so go talk to the computer ---
    //  -> Paused on NPCFragment Station_Ref_Tech_Name.  enum val NPC
    //      1 branch.  Cargo Station Ref Hub, so since it's 1 link to a Hub go to it via Play()
    //  -> Paused on Hub Cargo_Station_Start_Hub
    //      5 branches, only 1 valid, which is Dialogue_Fragment Ready to Rock (ie ready to play game)
    //  -> Paused on the Dialogue_Fragment that's telling us to prepare for the cargo game with a Continue button
    //      1 branch that's a Jump but not a hub so hang while waiting for input
    //  --- clicks Continue button ---
    //  --- UI is shut off ---
    //  -> Paused on Jump called Jump_To_Cargo_Game_Frag
    //      1 valid branch, but it's not a hub so we're hanging...****ADD NEW LOGIC HERE*****


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


    void Scratch()
    {
        ArticyRef ArticyRef;
        ArticyRef = this.GetComponent<ArticyReference>().reference;
        int x = 0;
        x++;

        FlowFragment f = (FlowFragment)ArticyRef;
        List<ArticyObject> attachments = f.Attachments;
        int i = 0;
        foreach (ArticyObject ao in attachments)
        {
            string s = ao.GetArticyType() + ", ";
            s += ao.Id + ", " + ao.InstanceId + ", ";
            s += ao.name + ", " + ao.TechnicalName;
            Debug.Log("attachment: " + i + " has this info: " + s);
            // attachment: 0 has this info: NPC (Articy.Unity.ArticyType), 72057602627868540, 0, , 
            // Captain_Jones_Tech_Name
            Articy.Captainschairdemo.NPC npc = (Articy.Captainschairdemo.NPC)ao;
            s = npc.DisplayName + ", ";
            s += npc.GetArticyType() + ", ";
            s += npc.name + ", ";
            s += npc.TechnicalName + ", ";
            s += npc.Text;
            Debug.Log("npc has valus: " + s);
            // npc has valus: Captain Jones Entity, NPC(Articy.Unity.ArticyType), , 
            // Captain_Jones_Tech_Name,             
            Articy.Captainschairdemo.Features.Basic_Character_AttributesFeature features;
            features = npc.GetFeatureBasic_Character_Attributes();
            s = features.GetType() + ", ";
            s += features.NPC_Feature_Name;
            Debug.Log("has features: " + s);
            // has features: Articy.Captainschairdemo.Features.Basic_Character_AttributesFeature, 
            // Captain Jones is my Basic Character Attribute Name
        }
        x++;
    }

}


