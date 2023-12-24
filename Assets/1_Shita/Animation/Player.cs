using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator Anim;
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Anim.SetBool("fannel",true);
        }
        if(Input.GetMouseButtonUp(0))
        {
            Anim.SetBool("fannel",false);
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            Anim.SetTrigger("beforeWarp");

            
        }
        
        
    }
    public void Translate()
    {
        Vector3 playerPos = this.transform.position;
        playerPos.x += 5.0f;
        this.transform.position = playerPos;
        Anim.SetTrigger("afterWarp");


    }
}
