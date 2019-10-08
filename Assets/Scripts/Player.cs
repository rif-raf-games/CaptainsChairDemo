using Articy.Captainschairdemo;
using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField]
    PlayerCharacter PC;

    NavMeshAgent NavMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        PC = ArticyDatabase.GetObject<PlayerCharacter>("Player_Character");
        Debug.Log("Our player name is: " + PC.Template.Basic_Character_Attributes.Name);

        NavMeshAgent = this.GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter() other: " + other.name);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit() other: " + other.name);
    }

    public void SetNavMeshDestination( Vector3 dest )
    {
        NavMeshAgent.SetDestination(dest);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
