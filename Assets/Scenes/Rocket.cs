using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip nextLevelSound;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem nextLevelParticles;

    [SerializeField] float delayTime = 2f;



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
            RespondToThrust();
            RespondToRotate();
        }
    }
    public void RespondToThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            m_MyAudioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);

            if (!m_MyAudioSource.isPlaying)
            {

                m_MyAudioSource.PlayOneShot(mainEngine);

            }
        mainEngineParticles.Play();
    }

    public void RespondToRotate()
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
                nextLevelParticles.Play();
                state = State.Transcending;
                Invoke("LoadNextScene", delayTime); 
                break;
            default:
                deathParticles.Play();
                LoadDeath();
                break;
        }
    }

    private void LoadDeath()
    {
        state = State.Dying;
        m_MyAudioSource.Stop();
        m_MyAudioSource.PlayOneShot(death);
        //Kill the Rocket Ship
        Invoke("LoadFirstScene", delayTime);
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        m_MyAudioSource.Stop();
        m_MyAudioSource.PlayOneShot(nextLevelSound);
        SceneManager.LoadScene(1);
    }
}