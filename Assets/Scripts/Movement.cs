using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] private float thrustForce = 1000f;
    [SerializeField] private float rotationForce = 100f;
    [SerializeField] AudioClip mainEngineSFX;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;

    AudioSource audioSource;
    Rigidbody rb;

   private void Start()
{
   rb = GetComponent<Rigidbody>();
   audioSource = GetComponent<AudioSource>();
}    

   private void OnEnable()
 {
    thrust.Enable();
    rotation.Enable();
 }

   private void FixedUpdate()
 {
      ProcessThrust();
      ProcessRotation();
 }

   private void ProcessThrust()
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

   private void StartThrusting()
 {
      rb.AddRelativeForce(Vector3.up * thrustForce);            
         if (!audioSource.isPlaying)
            {
            audioSource.PlayOneShot(mainEngineSFX);   
            }
         if(!mainEngineParticles.isPlaying)
            {
               mainEngineParticles.Play();               
            }
 }

   private void StopThrusting()
 {
      audioSource.Stop();
      mainEngineParticles.Stop();
 }

   private void ProcessRotation()
 {
  float rotationInput = rotation.ReadValue<float>();
  if (rotationInput < 0f)
      {         
         RotateRight();
      }
   else  if (rotationInput > 0f)
      {
         RotateLeft();
      }
      else
      {
         StopRotating();
      }
 }

   private void RotateRight()
   {
      ApplyRotation(rotationForce);
      if(!rightThrusterParticles.isPlaying)
         {
            leftThrusterParticles.Stop();
            rightThrusterParticles.Play();               
         }
   }

   private void RotateLeft()
   {
      ApplyRotation(-rotationForce);
         if(!leftThrusterParticles.isPlaying)
            {
               rightThrusterParticles.Stop();
               leftThrusterParticles.Play();            
            }
   }

   private void StopRotating()
   {
      leftThrusterParticles.Stop();
      rightThrusterParticles.Stop();
   }

   private void ApplyRotation(float rotationThisFrame)
      {
         rb.freezeRotation = true;
         transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
         rb.freezeRotation = false;
      }
 }

