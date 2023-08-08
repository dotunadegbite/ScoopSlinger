using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoopController : MonoBehaviour
{

    public Animator anim;
    public KeyCode shootKey;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("shootKey"))
        {
            anim.SetTrigger("ShootTrigger");
        }
    }
}
