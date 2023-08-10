using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyFlavor : MonoBehaviour
{
    // Start is called before the first frame update
    public Material[] iceCreamMaterialsList = new Material[4];
    //public 

    void Start() // apply ice cream color on start
    {
        GameObject manager = GameObject.Find("GameManager");
        int iceCreamindex = manager.GetComponent<IceCreamHandler>().materialsArrayIndex;
        Renderer rend = GetComponentInChildren<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = iceCreamMaterialsList[iceCreamindex];
    }
}
