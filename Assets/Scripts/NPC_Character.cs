using Articy.Captainschairdemo;
using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Character : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log(this.name + " OnTriggerEnter() other: " + other.name);
    }
    private void OnTriggerExit(Collider other)
    {
       // Debug.Log(this.name + " OnTriggerExit() other: " + other.name);
    }

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
    // Update is called once per frame
    void Update()
    {
        
    }    
}