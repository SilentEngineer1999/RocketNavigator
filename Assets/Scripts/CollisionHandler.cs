using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delayDuration = 2f;
    [SerializeField] AudioClip crashSFX;
    [SerializeField] AudioClip successSFX;
    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem crashParticle;

    
    int currentScreen;
    AudioSource audioSource;
    bool isControllable = true;
    bool isCollideable = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentScreen = SceneManager.GetActiveScene().buildIndex;
        audioSource = GetComponent<AudioSource>();  
    }

    // Update is called once per frame
    private void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            NextLevel();
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            isCollideable = !isCollideable;
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (!isControllable || !isCollideable) {return;}
        
        switch (other.gameObject.tag)
        {
            case "Fuel":
                Debug.Log("Fuel");
                break;
            case "Friendly":
                Debug.Log("Friendly");
                break;
            case "Finish":
                StartChangeSequence("NextLevel", successSFX, successParticle);
                break;
            default:
                StartChangeSequence("ReloadLevel", crashSFX, crashParticle);
                break;
        }
    }

    void StartChangeSequence(string functionName, AudioClip audioSFX, ParticleSystem particleEffect)
    {
        audioSource.Stop();
        isControllable = false;
        audioSource.PlayOneShot(audioSFX);
        particleEffect.Play();
        GetComponent<Movement>().enabled = false;
        Invoke(functionName, delayDuration);
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(currentScreen);
    }
    

    void NextLevel()
    {
        int nextScene = currentScreen + 1;
        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene);
    }

}
