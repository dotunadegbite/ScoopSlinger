using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyFlavorTypeToScoop : MonoBehaviour
{
    public Material[] iceCreamMaterialsList = new Material[4];

    void Start() // apply ice cream color and type on start
    {
        //find index for ice cream scoop flavor based on game manager
        GameObject manager = GameObject.Find("GameManager");
        int iceCreamindex = manager.GetComponent<IceCreamHandler>().materialsArrayIndex;
        
        //change material to new one based on materials array
        Renderer rend = GetComponentInChildren<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = iceCreamMaterialsList[iceCreamindex];
    }
}
