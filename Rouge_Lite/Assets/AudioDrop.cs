using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDrop : MonoBehaviour
{
    public GameObject audioClip; 
     public void AudioInteration() {
        Instantiate(audioClip, transform.position, Quaternion.identity);
     } 
}
