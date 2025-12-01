using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] float thrustStrength = 100f;
    [SerializeField] InputAction rotation;
    [SerializeField] float rotationStrength = 100f;
    [SerializeField] AudioClip mainEngineSFX;
    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem leftThrustParticle;
    [SerializeField] ParticleSystem rightThrustParticle;

    Rigidbody rb;
    AudioSource audioSource;

    private void OnEnable() {
        thrust.Enable();
        rotation.Enable();
    }

    void Start() 
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();   
    }

    // Update is called once per frame
    private void FixedUpdate() 
    {     
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StopThrusting()
    {
        audioSource.Stop();
        mainEngineParticle.Stop();
    }

    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngineSFX);
        }
        if (!mainEngineParticle.isPlaying)
        {
            mainEngineParticle.Play();
        }
    }

    void ProcessRotation()
    {
       float rotationInput = rotation.ReadValue<float>();
       if (rotationInput < 0)
        {
            RotateRight();
        }
        else if (rotationInput > 0)
        {
            RotateLeft();
        }
        else
        {
            StopRotating();
        }
    }

    private void StopRotating()
    {
        rightThrustParticle.Stop();
        leftThrustParticle.Stop();
    }

    private void RotateLeft()
    {
        ApplyRotation(-rotationStrength);
        if (!rightThrustParticle.isPlaying)
        {
            leftThrustParticle.Stop();
            rightThrustParticle.Play();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(rotationStrength);
        if (!leftThrustParticle.isPlaying)
        {
            rightThrustParticle.Stop();
            leftThrustParticle.Play();
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }
    
}
