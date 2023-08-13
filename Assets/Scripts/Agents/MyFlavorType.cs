using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Establish global enum typing for different flavors
public enum FlavorType
{
    CHOCOLATE,
    MINT,
    BERRY,
    VANILLA
}
public class MyFlavorType : MonoBehaviour
{
    public FlavorType scoopFlavor;
}
