using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

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
        Thrust();
        Rotate();
    }

    public void Thrust()
    {

        float thrust = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {

            rigidBody.AddRelativeForce(Vector3.up * thrust);

            if (!m_MyAudioSource.isPlaying)
            {

                m_MyAudioSource.Play();

            }
        }
        else
        {
            m_MyAudioSource.Stop();
        }
    }
    public void Rotate()
    {

        //Take Manual control of rotation
        rigidBody.freezeRotation = true;

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(rotationThisFrame * Vector3.forward);   
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(rotationThisFrame * -Vector3.forward);
            }
        }

        rigidBody.freezeRotation = false;
    }
    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //Print Freindly
                print("Friendly");
                break;
            case "Fuel":
                //Print Dead
                print("Fuel");
                break;
            default:
                print("Dead");
                break;
        }
    }
}