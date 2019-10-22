using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Captainschairdemo;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CaptainsChairSceneRoot : MonoBehaviour, IArticyFlowPlayerCallbacks
{
    [SerializeField]
    protected PlayerObject PlayerObject;    
    public Image SpeakerImage;
    public Text SpeakerText;
    ArticyFlowPlayer FlowPlayer;

    /**** Dialogue Options ****/
    public GameObject DialogueUI;
    public GameObject[] DialogueOptions;
    protected virtual void DoOnFlowPlayerPaused(IFlowObject aObject) { }
    protected virtual void DoOnBranchesUpdated(IList<Branch> aBranches) { }
    
    // Start is called before the first frame update
    public virtual void Start()
    {
        PlayerCharacter PC = ArticyDatabase.GetObject<PlayerCharacter>("Player_Character_Entity_Tech_Name");
        //Debug.Log("Our player name is: " + PC.Template.Basic_Character_Attributes.NPC_Feature_Name);
        StaticStuff.FlowDebug("Our player name is: " + PC.Template.Basic_Character_Attributes.NPC_Feature_Name);

        FlowPlayer = GetComponent<ArticyFlowPlayer>();
        Slot_Container sc = ArticyDatabase.GetObject<Slot_Container>("Start_On_Object");
        if (sc.Template.Slot_Feature.Slot_Feature_Slot != null)
        {
            //Debug.Log("CaptainsChairSceneRoot.Start() setting StartOn: " + sc.Template.Slot_Feature.Slot_Feature_Slot.name);
            StaticStuff.FlowDebug("CaptainsChairSceneRoot.Start() setting StartOn: " + sc.Template.Slot_Feature.Slot_Feature_Slot.name);
            SetFlowPlayerStartOn(sc.Template.Slot_Feature.Slot_Feature_Slot);
        }
        else
        {
            //Debug.Log("CaptainsChairSceneRoot.Start() no StartOn");
            StaticStuff.FlowDebug("CaptainsChairSceneRoot.Start() no StartOn");
        }

        if(DialogueUI != null ) DialogueUI.gameObject.SetActive(false);
        //FlowPlayer.StartOn = sc.Template.Slot_Feature.Slot_Feature_Slot;
    }
    
    protected void GoToMiniGame(Branch branch)
    {
        var miniGameFrag = branch.Target as IObjectWithFeatureMini_Game_Name;
        if (miniGameFrag != null)
        {
            //Debug.Log("go to this mini game NOW: " + miniGameFrag.GetFeatureMini_Game__Name().Mini_Game_Name);
            StaticStuff.FlowDebug("go to this mini game NOW: " + miniGameFrag.GetFeatureMini_Game_Name().Mini_Game_Name);
            // FlowPlayer.StartOn = branch.Target as IArticyObject;            
            Slot_Container slotContainer = ArticyDatabase.GetObject<Slot_Container>("Start_On_Object");
            slotContainer.Template.Slot_Feature.Slot_Feature_Slot = (ArticyObject)branch.Target;
            SceneManager.LoadScene(miniGameFrag.GetFeatureMini_Game_Name().Mini_Game_Name);
        }
        else
        {
            //Debug.LogError("ERROR: trying to go to a mini game with no name");
            StaticStuff.FlowDebug("ERROR: trying to go to a mini game with no name");
        }
    }
    protected void ShutOffDialogueUI()
    {
        DialogueUI.gameObject.SetActive(false);

        PlayerObject.ToggleMovementBlocked(false);
    }
    protected void ShowDialogueFragment(Dialogue_Fragment dialogueFrag, List<Branch> dialogueOptions)
    {
        StaticStuff.FlowDebug("going to set up a dialogue fragment with speaker: " + dialogueFrag.Speaker + " with text: " + dialogueFrag.Text);
        StaticStuff.FlowDebug("this dialogue fragment has: " + dialogueOptions.Count + " options");

        DialogueUI.gameObject.SetActive(true);
        SetSpeakerImage(dialogueFrag.Speaker);
        SpeakerText.text = dialogueFrag.Text;

        foreach (GameObject go in DialogueOptions) go.SetActive(false);
        for(int i=0; i<dialogueOptions.Count; i++)
        {           
            DialogueOptions[i].SetActive(true);
            Dialogue_Fragment df = dialogueOptions[i].Target as Dialogue_Fragment;
            if(df != null )
            {
                DialogueOptions[i].GetComponentInChildren<Text>().text = df.MenuText;
            }
            else
            {
                string buttonText = dialogueFrag.Template.Dialogue_Fragment_Default_Button_Text.Default_Button_Text;
                StaticStuff.FlowDebug("ok we are not going to a dialogue fragment, so get the button text by the template: " + buttonText);
                if( buttonText.Equals("") == false )
                {
                    DialogueOptions[i].GetComponentInChildren<Text>().text = buttonText;
                }                
                else
                {

                    string val = ArticyDatabase.DefaultGlobalVariables.GetVariableByString<string>("Misc_Globals.defaultDialogueFragmentButtonText");
                    DialogueOptions[i].GetComponentInChildren<Text>().text = val;
                }
            }                        
        }

        PlayerObject.ToggleMovementBlocked(true);
    }
    protected void SetSpeakerImage(ArticyObject speaker)
    {        
        Asset speakerImageAsset = ((speaker as IObjectWithPreviewImage).PreviewImage.Asset as Asset);
        if(speakerImageAsset != null )
        {            
            SpeakerImage.sprite = speakerImageAsset.LoadAssetAsSprite();
        }
        else
        {
            Debug.LogWarning("Tried to load speaker image with null image asset");
        }        
    }
    

    public void OnFlowPlayerPaused(IFlowObject aObject)
    {
        if (aObject == null) { Debug.LogWarning("NULL aObject in OnFlowPlayerPaused()."); return; }
        DoOnFlowPlayerPaused(aObject);
    }
    public void OnBranchesUpdated(IList<Branch> aBranches)
    {
        if (aBranches == null || aBranches.Count == 0) { Debug.LogWarning("NULL or 0 branches in OnBranchesUpdated()."); return; }
        DoOnBranchesUpdated(aBranches);
    }
    public void SetFlowPlayerStartOn( ArticyObject articyObject )
    {
        //Debug.Log("SetFlowPlayerStartOn(): type: " + articyObject.GetType() + ", name: " + articyObject.name);
        StaticStuff.FlowDebug("SetFlowPlayerStartOn(): type: " + articyObject.GetType() + ", name: " + articyObject.name);
        FlowPlayer.StartOn = articyObject;
    }
    
    protected void PlayFirstBranch()
    {
        FlowPlayer.Play(0);
    }
    protected void PlayNextFlow(Branch b)
    {
        FlowPlayer.Play(b);
    }
    string GetBranchInfo(Branch b, string src = "")
    {
        string s = "BRANCH INFO (" + src + "): " + b.BranchId.ToString() + ", ";
        s += b.Target.GetType() + ", ";                
        s += b.DefaultDescription + ", ";
        s += b.IsValid;
        return s;
    }
    protected string ReturnBranchInfo(Branch b, string src = "")
    {
        return GetBranchInfo(b, src);
    }
    protected void PrintBranchInfo(Branch b, string src = "")
    {
        string s = GetBranchInfo(b, src);
        //Debug.Log(s);
        StaticStuff.FlowDebug(s);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
