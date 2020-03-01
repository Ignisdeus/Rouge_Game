using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGuy : MonoBehaviour
{
    public int moves;
    public GameObject me;
    private void Start() {
       
        StartCoroutine(FlashMyPos());
    }

    public void BeenCought() {

        tag = "Untagged";
        me.SetActive(true);
    }

    private void Update() {
        if (tag == "Untagged") {
            me.SetActive(true);
        } 
    }

    public void Flash() {

        StartCoroutine(FlashMyPos()); 
    }
    IEnumerator FlashMyPos() {

        bool canSee = false; 
        for (int i = 0; i < 5; i++) {
            canSee = !canSee; 
            yield return new WaitForSeconds(0.25f);
            me.SetActive(canSee);
        }
        me.SetActive(false);
    }
}
