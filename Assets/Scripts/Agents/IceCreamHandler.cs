using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamHandler : MonoBehaviour
{
    public FlavorType managerFlavor;
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
        }
        if (swapTime)
        {
            //next flavor
            materialsArrayIndex = (materialsArrayIndex + 1) % 4;

            //assign a type based on the index for scoop flavors currently equipped by the player
            switch(materialsArrayIndex)
            {
                case 0:
                    managerFlavor = FlavorType.CHOCOLATE;
                    break;
                case 1:
                    managerFlavor = FlavorType.MINT;
                    break;
                case 2:
                    managerFlavor = FlavorType.BERRY;
                    break;
                case 3:
                    managerFlavor = FlavorType.VANILLA;
                    break;

            }
            swapTime = false;
        }
    }
}
