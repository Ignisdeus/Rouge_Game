using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_By_Distance : MonoBehaviour
{
    public FMOD.Studio.EventInstance fMoDAudio;
    public string pathToAudio, effectedParimater;
    public GameObject player;
    public float repeatDelay = 0.2f;
    void Start()
    {
        fMoDAudio = FMODUnity.RuntimeManager.CreateInstance(pathToAudio);
        fMoDAudio.start();
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("DistanceCheck", repeatDelay, repeatDelay);
    }


   
    void DistanceCheck() {

    }
}
