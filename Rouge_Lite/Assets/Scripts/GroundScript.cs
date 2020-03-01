using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour
{
    public float height = 0.015f;
    public GameObject[] spawnOnMe;
    public bool canMoveHere = true, safeLand = false; 

    private void Start() {
        height = (int)Random.Range(1, 3) * height;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, height);
        if (!safeLand) {
            if (Random.Range(0f,1f) < 0.2f) {
            canMoveHere = false; 
            Vector3 spawnPoint= new Vector3(transform.position.x, height * 2, transform.position.z );
            
                GameObject x = Instantiate(spawnOnMe[Random.Range(0, spawnOnMe.Length)], spawnPoint, Quaternion.Euler(-90, 0, 0));
                if (x.tag == "Fire" || x.tag == "Mine") {
                    canMoveHere = true;
                }
            }

        }
    }
}
