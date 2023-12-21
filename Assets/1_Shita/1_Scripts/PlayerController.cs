using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    float moveSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();

        
    }
    void movePlayer()
    {
        Vector3 dir = Vector3.zero;
        //_inputDirection = new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0);
        if(Input.GetKey(KeyCode.UpArrow)){
            
            dir +=new Vector3 (-0.4f, 0.0f, 0.0f);
            
        }

        if(Input.GetKey(KeyCode.DownArrow)){
            dir +=new Vector3 (0.4f, 0.0f, 0.0f);
           
        }
        if(Input.GetKey(KeyCode.RightArrow)){
            
            dir +=new Vector3 (0.0f, 0.0f, -0.4f);
            
        }

        if(Input.GetKey(KeyCode.LeftArrow)){
            dir += new Vector3 (0.0f, 0.0f, 0.4f);
            
        }
        if(Vector3.zero == dir) return;

        rb.position += dir.normalized * moveSpeed * Time.deltaTime;

        
    }
}
