using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody rigidBody;
    AudioSource m_MyAudioSource;

    //Play the music
    bool m_Play;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        //Fetch the AudioSource from the GameObject
        m_MyAudioSource = GetComponent<AudioSource>();
        //Ensure the toggle is set to true for the music to play at start-up
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }
    private void ProcessInput(){
        if(Input.GetKey(KeyCode.Space)){
            rigidBody.AddRelativeForce(Vector3.up);
            if (!m_MyAudioSource.isPlaying)
            {
                m_MyAudioSource.Play();
            }
        }
        else{ m_MyAudioSource.Stop();}

        if(Input.GetKey(KeyCode.A)){
            transform.Rotate(Vector3.forward);
        } else {
            if(Input.GetKey(KeyCode.D)){
                transform.Rotate(-Vector3.forward);
        }}
    }
}
