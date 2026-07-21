using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip SuccessSFX;
    [SerializeField] AudioClip CrashSFX;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;

    bool  isControllable = true;
    bool isCollidable = true;

    private void Start()
    {
      audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isControllable || !isCollidable) { return; }
        
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                StartSuccessSequence();                
                break;
            default:
                StartCrashSequence();                
                break;
        }
    } 

    private void StopMovement()
    {
        GetComponent<Movement>().enabled = false;
    }
    
    private void StartSuccessSequence()
    {
        isControllable = false;
        audioSource.Stop();        
        audioSource.PlayOneShot(SuccessSFX);
        successParticles.Play();
        StopMovement();
        Invoke(nameof(LoadNextLevel), levelLoadDelay);
    }

    private void StartCrashSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(CrashSFX);
        crashParticles.Play();
        StopMovement();
        Invoke(nameof(ReloadLevel), levelLoadDelay); 
    }   

    private void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }
        
        SceneManager.LoadScene(nextScene); 
    }

    private void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
    
}
