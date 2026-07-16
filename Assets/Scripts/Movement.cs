using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] private float thrustForce = 1000f;
    [SerializeField] private float rotationForce = 100f;
    Rigidbody rb;

private void Start()
{
   rb = GetComponent<Rigidbody>();
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
        rb.AddRelativeForce(Vector3.up * thrustForce * Time.fixedDeltaTime);
    }
 }

 private void ProcessRotation()
 {
  float rotationInput = rotation.ReadValue<float>();
  if (rotationInput < 0f)
      {
         ApplyRotation(rotationForce);
      }
   else  if (rotationInput > 0f)
      {
         ApplyRotation(-rotationForce);
      }

  void ApplyRotation(float rotationThisFrame)
      {
         transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
      }
 }
}

