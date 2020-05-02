using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    Rigidbody rigidBody;
    AudioSource m_MyAudioSource;

    enum State { Alive, Dying, Transcending}
    State state = State.Alive;

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
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
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

        if (state != State.Alive)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
               
                break;
            case "Finish":
                //The Rocket Ship has reached the Finish Line
                state = State.Transcending;
                Invoke("LoadNextScene", 1f); 
                break;
            default:
                LoadDeath();
                break;
        }
    }

    private void LoadDeath()
    {
        state = State.Dying;
        //Kill the Rocket Ship
        Invoke("LoadFirstScene", 1f);
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }
}