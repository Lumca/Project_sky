using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    //Config
    [SerializeField] float rcsTrust = 100f;
    [SerializeField] float mainTrust = 100f;
    Rigidbody rigidBody;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Trust();
        Rotate();
    }

    void OnCollisionEnter(Collision collision) 
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Alive");
                break;
            case "Unfriendly":
                print("Dead");
                break;
            default:
                print("Collision Error");
                break;
        }
    }

    private void Rotate()
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

    void Trust()
    {
        if (Input.GetKey(KeyCode.W))
        {
            float TrustThisFrame = mainTrust * Time.deltaTime;
            rigidBody.AddRelativeForce(Vector3.up * TrustThisFrame);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
    }
}
