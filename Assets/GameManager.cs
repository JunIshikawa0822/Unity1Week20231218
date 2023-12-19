using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    //public Transform goal;
    private NavMeshAgent agent;
    public float speed = 5f;
    public GameObject player;
    public GameObject agentObject;
    Vector3 _inputDirection;
    private Rigidbody rb;
    Vector3 force;
    // Start is called before the first frame update
    void Start()
    {
        agent = agentObject.transform.GetComponent<NavMeshAgent>();
        agent.destination = player.transform.position;
        
       
        rb = player.transform.GetComponent<Rigidbody>();
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        _inputDirection = new Vector3(Input.GetAxis("Horizontal") * speed,0,0);
        if(_inputDirection.x < 0){
            
            force = new Vector3 (-0.5f, 0.0f, 0.0f);
            rb.AddForce(force, ForceMode.Impulse);
        }

        if(_inputDirection.x > 0){
            force = new Vector3 (0.5f, 0.0f, 0.0f);
            rb.AddForce(force, ForceMode.Impulse);
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            force = new Vector3 (0.0f, 10.0f, 0.0f);
            rb.AddForce(force, ForceMode.Impulse);
        }
        
    
       
        
    }

    
}
