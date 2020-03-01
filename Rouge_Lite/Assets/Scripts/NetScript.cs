using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetScript : MonoBehaviour
{
    Rigidbody myRid;
    public FMOD.Studio.EventInstance cought;
    public string pathToAudio, effectedParimater;
    public GameObject dustMiss, dustHit; 
    // Start is called before the first frame update
    void Start()
    {
        cought = FMODUnity.RuntimeManager.CreateInstance(pathToAudio);
        
        myRid = GetComponent<Rigidbody>(); 
    }
    bool canCatch = true; 
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag != "BadGuy" && canCatch) {
            Destroy(myRid);
            tag = "Untagged";
            Instantiate(dustMiss, transform.position, Quaternion.identity);
            //Destroy(GetComponent<NetScript>());
            Destroy(gameObject, 2f); 
        }

        if (other.gameObject.tag == "BadGuy" && canCatch) {
            canCatch = false;
            cought.start();
            Instantiate(dustHit, transform.position, Quaternion.identity);
            GridBuilder.gm.GetComponent<GridBuilder>().EnemyCought();
            other.gameObject.GetComponent<BadGuy>().BeenCought();
            GameMaster.score++; 
        }
    }
}
