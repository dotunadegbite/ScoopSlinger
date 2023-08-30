using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScript : MonoBehaviour
{
    private GameObject Credits1;
    private GameObject Credits2;
    private GameObject Credits3;
    private GameObject Credits4;
    // Start is called before the first frame update
    void Start()
    {
        Credits1 = GameObject.Find("Credits1");
        Credits2 = GameObject.Find("Credits2");
        Credits3 = GameObject.Find("Credits3");
        Credits4 = GameObject.Find("Credits4");
        Credits2.SetActive(false);
        Credits3.SetActive(false);
        Credits4.SetActive(false);

        StartCoroutine(LoadCredits());
    }
    IEnumerator LoadCredits()
    {
        yield return new WaitForSeconds(5);
        Credits1.SetActive(false);
        Credits2.SetActive(true);
        yield return new WaitForSeconds(5);
        Credits2.SetActive(false);
        Credits3.SetActive(true);
        yield return new WaitForSeconds(5);
        Credits3.SetActive(false);
        Credits4.SetActive(true);
    }
}
