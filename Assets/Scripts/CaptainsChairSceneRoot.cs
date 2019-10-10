using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Articy.Unity;
using Articy.Unity.Interfaces;
using Articy.Captainschairdemo;
using UnityEngine.UI;

public class CaptainsChairSceneRoot : MonoBehaviour, IArticyFlowPlayerCallbacks
{
    protected PlayerCharacter PC;
    ArticyFlowPlayer FlowPlayer;
    public GameObject DialogueUI;
    public Image SpeakerImage;
    public Text SpeakerText;
    //public Button[] DialogueButtons;
    protected virtual void DoOnFlowPlayerPaused(IFlowObject aObject) { }
    protected virtual void DoOnBranchesUpdated(IList<Branch> aBranches) { }

    // Start is called before the first frame update
    public virtual void Start()
    {
        PC = ArticyDatabase.GetObject<PlayerCharacter>("Player_Character_Entity_Tech_Name");
        Debug.Log("Our player name is: " + PC.Template.Basic_Character_Attributes.NPC_Feature_Name);

        FlowPlayer = GetComponent<ArticyFlowPlayer>();
        Slot_Container sc = ArticyDatabase.GetObject<Slot_Container>("Start_On_Object");
        if(sc.Template.Slot_Feature.Slot_Feature_Slot != null )
        {
            SetFlowPlayerStartOn(sc.Template.Slot_Feature.Slot_Feature_Slot);
        }

        DialogueUI.gameObject.SetActive(false);
        //FlowPlayer.StartOn = sc.Template.Slot_Feature.Slot_Feature_Slot;
    }
    
    protected void ShutOffDialogueUI()
    {
        DialogueUI.gameObject.SetActive(false);
    }
    protected void ShowDialogueFragment(Dialogue_Fragment dialogueFrag)
    {
        DialogueUI.gameObject.SetActive(true);        
        SetSpeakerImage(dialogueFrag.Speaker);
        SpeakerText.text = dialogueFrag.Text;
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
        Debug.Log("SetFlowPlayerStartOn(): type: " + articyObject.GetType() + ", name: " + articyObject.name);
        FlowPlayer.StartOn = articyObject;
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
        Debug.Log(s);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
