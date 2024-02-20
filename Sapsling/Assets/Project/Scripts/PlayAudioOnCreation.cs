using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//created by Ribhu Hooja

// Add this script to an object to make it play an audio when it is instantiated
// Of course, the object needs to have a audio source component attached to it
public class PlayAudioOnCreation : MonoBehaviour {
    [SerializeField] private bool loop;         // loop or not?
    [SerializeField] private float pitch;   // change the pitch
    [SerializeField] private float startTime;   // for beginning in the middle of the clip

    // to probabilistically choose which audio clip is played
    // if you only want one, only fill in preferred clip
    // if you want to play multiple, add the rest in the otherClips section
    // and provide a matching array of probabilities
    // It is upto you to make sure the probabilities make sense 
    // (add to 1 - whateverYouWantThePreferredClipProbabilityToBe), etc.
    [SerializeField] private AudioClip preferredClip;       
    [SerializeField] private AudioClip[] otherClips;
    [SerializeField] private float[] otherClipProbabilities;

    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        
        // default to playing the preferred clip
        if (otherClips.Length == 0 || otherClips.Length > otherClipProbabilities.Length) {
            audioSource.clip = preferredClip;
        } else {
            // intuitively, assign a probability "length" to each of the clips
            // lay this out on a line
            // and choose a point on the line
            // to do this in math we have to keep a cumulative probability
            // to know when we have "crossed" the chosen point
            bool chosen = false;
            float r = Random.value;
            float cumProb = 0;      // cumulative probability
            for (int i = 0; i < otherClips.Length; i++) {
                cumProb += otherClipProbabilities[i];
                if (r < cumProb) {
                    audioSource.clip = otherClips[i];
                    chosen = true;
                    break;
                }
            }
            // this could happen if the probabilitie are malformed, say, negative or don't add to 1
            if (!chosen) audioSource.clip = preferredClip;
        }
        audioSource.pitch = pitch;
        audioSource.loop = loop;
        audioSource.time = startTime;
        audioSource.Play();
    }
}
