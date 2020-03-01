using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour {

	[Range(0, 15)]
    public float destroyTimer; 
	void Start () {
		
        Invoke("DestroyThisObject", destroyTimer);
	}
	
    void DestroyThisObject(){

        Destroy(gameObject); 

    }
}
