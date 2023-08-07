using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoopController : MonoBehaviour
{

    public Animator anim;
    public KeyCode shootkey;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("shootkey"))
        {
            Debug.Log("Should shoot");
            anim.SetTrigger("ShootPressed");
        }
    }
}
