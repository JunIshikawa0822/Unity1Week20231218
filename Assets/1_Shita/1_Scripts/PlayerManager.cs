using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    //public Transform goal;
    
    float moveSpeed = 5f;
    public GameObject player;
    
    // //Vector3 _inputDirection;
    private Rigidbody rb;
    // Vector3 force;
    // int interval;
    //List<bool> IsEntered = 
    // Start is called before the first frame update
    void Awake()
    {
        
    }
    void Start()
    {
        rb = player.transform.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
       
        
    }
    public void movePlayer()
    {
        Vector3 dir = Vector3.zero;
        //_inputDirection = new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0);
        if(Input.GetKey(KeyCode.UpArrow)){
            
            dir +=new Vector3 (0.0f, 0.0f, 0.4f);
            
        }

        if(Input.GetKey(KeyCode.DownArrow)){
            dir +=new Vector3 (0.0f, 0.0f, -0.4f);
           
        }
        if(Input.GetKey(KeyCode.RightArrow)){
            
            dir +=new Vector3 (0.4f, 0.0f, 0.0f);
            
        }

        if(Input.GetKey(KeyCode.LeftArrow)){
            dir += new Vector3 (-0.4f, 0.0f, 0.0f);
            
        }
        if(Vector3.zero == dir) return;

        rb.position += dir.normalized * moveSpeed * Time.deltaTime;

        
    }
    // IEnumerator EnemyGenerate()
    // {
        
    //     interval = 30;
        
    //     //
    //     for(int i = 0;i < interval ;i ++)
    //     {
    //         yield return null;
    //     }
        
    //     interval = Random.Range(30, 100);
    // }
        
        
        
    

    
}
