using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class GameController : MonoBehaviour
{
    public Transform goal;
    public GameObject Agent;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = Agent.transform.GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
