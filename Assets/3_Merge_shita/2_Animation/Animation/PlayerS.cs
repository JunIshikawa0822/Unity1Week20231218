using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerS : MonoBehaviour
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
        
        
        
    }
    public void WarpMove()
    {
        Vector3 playerPos = this.transform.position;
        this.transform.position = playerPos;
        Anim.SetTrigger("warp_2");


    }
}
