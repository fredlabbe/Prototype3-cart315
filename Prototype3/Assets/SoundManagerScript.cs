using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip pickup;
    static AudioSource audioSrc; 

    // Start is called before the first frame update
    void Start()
    {
        pickup = Resources.Load<AudioClip>("pickup");
        audioSrc = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    } 

    public static void PlaySound (string clip)
    {
        audioSrc.PlayOneShot(pickup);
    }
}
