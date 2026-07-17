# Rocket Boost

My second Unity project, using C# — a rocket landing game focused on thrust, rotation and physics-based movement.

## Things I've Learned:

### New Unity Input System:

- Learned to use Unity's new Input System package instead of the old `Input.GetAxis()` approach.
- Created a `Movement` script using `InputAction` fields for thrust (button-style, read with `.IsPressed()`) and rotation (axis-style, read with `.ReadValue<float>()`).
- Learned to call `.Enable()` on each `InputAction` inside `OnEnable()`, so input starts working as soon as the object becomes active.

### Tuning Variables and Framerate Independence:

- Added `[SerializeField]` tuning variables (`thrustForce`, `rotationForce`) to test and adjust values directly in the Inspector, instead of hardcoding numbers.
- Multiplied force and rotation by `Time.fixedDeltaTime` inside `FixedUpdate()`, keeping physics calculations consistent regardless of frame rate.

### Mixing transform.Rotate() with Rigidbody:

- Used `transform.Rotate()` to control the rocket's rotation on a single axis (Z), while still using `Rigidbody.AddRelativeForce()` for thrust.
- Understood why this combination works fine here: rotation is purely visual/directional in this game, since nothing needs to physically react to the rocket spinning (unlike a spinning obstacle that needs to push a player on collision).

### Imported Assets and Prefabs:

- Learned that imported assets (like models from the Asset Store or other sources) come as prefabs that shouldn't be edited directly, since changes could break if the asset is updated or re-imported.
- To customize an imported prefab safely, the correct approach is creating a "Prefab Variant" (a prefab of the prefab) — this preserves the original asset while allowing custom overrides.

### Physics

- Configured the Rigidbody's **Linear Damping** to control the rocket's deceleration.
- Adjusted Unity's default gravity from **-9.81** to **-4** to better fit the game's physics and improve the overall gameplay feel.

### Audio

- Learned how to use the **AudioSource** component.
- Controlled the rocket's thrust sound through code.
- Solved an issue where the engine sound kept starting and stopping while the thrust key was held by checking `audioSource.isPlaying` before calling `Play()`.

### Tools

- Learned how to use **OBS Studio** to record gameplay footage for GitHub documentation and LinkedIn posts.

---

## Problem Solved

### Engine sound repeatedly starting and stopping

**Issue**

While holding the thrust key, the engine sound kept starting and stopping every frame.

**Cause**

Both `Play()` and `Stop()` were being executed within the same input condition. Since `Update()` runs every frame, the audio continuously alternated between playing and stopping.

**Solution**

I used `audioSource.isPlaying` to ensure the audio only starts when it isn't already playing, and moved `Stop()` to execute only when the thrust key is released.
