using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamHandler : MonoBehaviour
{
    public GameObject scoop;
    public int materialsArrayIndex = 0;
    private bool swapTime = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        SwapProjectile();
    }

    void SwapProjectile()
    {
        if (Input.GetButtonDown("NextFlavor"))
        {
            swapTime = true;
            //Debug.Log("Truuuuuuuue");
        }
        if (swapTime)
        {
            //Debug.Log("Time to swap");
            Debug.Log(materialsArrayIndex);
            materialsArrayIndex = (materialsArrayIndex + 1) % 4;
            //rend.sharedMaterial = icymaterialsList[materialsArrayIndex];
            swapTime = false;
        }
    }
}
