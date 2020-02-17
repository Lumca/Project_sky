using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    //Config
    [SerializeField] float rcsTrust = 100f;
    [SerializeField] float mainTrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip rocketCrash;
    [SerializeField] AudioClip levelLoad;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem rocketCrashParticles;
    [SerializeField] ParticleSystem levelLoadParticles;


    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State {Alive, Dying, Transcending};
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }
    void OnCollisionEnter(Collision collision) 
    {
        if (state != State.Alive) { return; } // stops the function, ignors collisions

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Alive");
                break;
            case "Unfriendly":
                StartDyingSequence();
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                print("Collision Error");
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        levelLoadParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(levelLoad);
        Invoke("LoadNextLevel", 1f); // time to next scene
    }

    private void StartDyingSequence()
    {
        state = State.Dying;
        rocketCrashParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(rocketCrash);
        Invoke("LoadFirstLevel", 1f); // time to next scene
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); //add more than 2 levels
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation
        float rotationThisFrame = rcsTrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // resume
    }

    void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            ApplyThrust();
            mainEngineParticles.Play();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        float TrustThisFrame = mainTrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * TrustThisFrame);
        mainEngineSound();
    }

    private void mainEngineSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }
}
