# Rocket Boost

My second Unity project, built with C# — a rocket-landing game focused on thrust, rotation, and physics-based movement.

## Development Update — July 17, 2026

In this stage of the project, I expanded the game by adding new levels, visual effects, sound effects, scene transitions, and improved collision handling.

### New Features

- Added crash and success sound effects triggered by collisions detected with `OnCollisionEnter()`.
- Implemented collision handling with a `switch` statement for different tags.
- Added two new scenes, bringing the game to a total of three levels.
- Added particle effects for successful landings and crashes.
- Added configurable delays before reloading the current level or loading the next one.
- Implemented automatic scene progression using the current scene's build index.
- Added logic to return to the first level after completing the final level.

### Code Organization

- Created separate methods for:
  - stopping player movement;
  - handling the success sequence;
  - handling the crash sequence;
  - loading the next level;
  - reloading the current level.
- Used a boolean variable to prevent the collision logic from running multiple times.
- Kept `OnCollisionEnter()` focused on selecting the correct sequence, while separate methods handle movement, effects, and scene transitions.

---

## Things I've Learned

### New Unity Input System

- Learned to use Unity's new Input System package instead of the old `Input.GetAxis()` approach.
- Created a `Movement` script using `InputAction` fields for thrust (button-style, read with `.IsPressed()`) and rotation (axis-style, read with `.ReadValue<float>()`).
- Learned to call `.Enable()` on each `InputAction` inside `OnEnable()`, so input starts working as soon as the object becomes active.

### Tuning Variables and Physics Updates

- Added `[SerializeField]` tuning variables (`thrustForce` and `rotationForce`) to adjust gameplay values directly in the Inspector instead of hardcoding numbers.
- Handled thrust and rotation inside `FixedUpdate()` so that movement stays synchronized with Unity's physics updates.
- Multiplied the direct rotation amount by `Time.fixedDeltaTime` so that `rotationForce` represents rotation speed over time.
- Applied thrust through `Rigidbody.AddRelativeForce()`.

### Combining `transform.Rotate()` with `Rigidbody`

- Used `transform.Rotate()` to provide direct and responsive control over the rocket's orientation on the Z axis.
- Used `Rigidbody.AddRelativeForce()` for physics-based thrust.
- This was a gameplay-oriented choice for the project: rotation is controlled directly, while thrust, gravity, and collisions are handled through the Rigidbody.

### Prefabs and Prefab Instances

- Learned the difference between a prefab asset and a prefab instance.
- Learned how applying instance overrides to the original prefab updates its other instances across the project.

### Physics

- Configured the Rigidbody's **Linear Damping** to control the rocket's deceleration.
- Adjusted Unity's default gravity from **-9.81** to **-4** to better fit the game's physics and improve the overall gameplay feel.

### Audio

- Learned how to use the `AudioSource` component.
- Controlled the rocket's engine sound through code.
- Used `audioSource.isPlaying` to avoid restarting an audio clip that is already playing.
- Used `AudioSource.PlayOneShot()` to play crash and success sound effects.
  
### Tools

- Learned how to use **OBS Studio** to record gameplay footage for GitHub documentation and LinkedIn posts.

### Collision Handling

- Learned how to use `OnCollisionEnter()` to detect collisions and respond according to the collided object's tag.
- Used a `switch` statement to organize the collision logic:
  - `Friendly` allows the rocket to continue;
  - `Finish` starts the success sequence;
  - any other tag starts the crash sequence.

### Scene Management

I learned how to:

- get the current scene using `SceneManager.GetActiveScene()`;
- access the scene's `buildIndex`;
- calculate the next scene;
- reload the current scene;
- return to the first level after completing the final level.

### Particle Systems

- Learned how to trigger separate particle systems through code for:
  - successful landings;
  - crashes.

### Delayed Method Calls

- Learned how to use `Invoke()` to schedule a method call after a configurable delay.

---

## Problems Solved

### Engine sound repeatedly starting and stopping

**Issue**

While holding the thrust key, the engine sound kept starting and stopping repeatedly.

**Cause**

Both `Play()` and `Stop()` were being executed within the same input condition. Because this condition was evaluated repeatedly while the thrust key was held, the audio continuously alternated between playing and stopping.

**Solution**

I used `audioSource.isPlaying` to ensure the audio only starts when it isn't already playing, and moved `Stop()` to execute only when the thrust key is released.

---

### Rocket engine sound continued after a collision

**Issue**

After the rocket crashed or reached the landing platform, the engine sound continued playing.

**Cause**

The movement input was disabled, but the currently playing engine audio was not stopped.

**Solution**

I stopped the engine audio before playing the crash or success sound effect:

```csharp
audioSource.Stop();
```

---

### Scene transition happened too quickly after a collision

**Issue**

The level reloaded or changed immediately after a crash or successful landing, making the sound and particle effects difficult to notice.

**Cause**

The scene transition happened as soon as the collision sequence started.

**Solution**

I used `Invoke()` with a configurable delay:

```csharp
Invoke(nameof(ReloadLevel), levelLoadDelay);
```

and:

```csharp
Invoke(nameof(LoadNextLevel), levelLoadDelay);
```

This gives the sound and particle effects time to play before the scene changes.

---

### Prefab changes were not appearing in other scenes

**Issue**

Changes made to the rocket in one scene were not appearing in the other levels.

**Cause**

The modifications had only been made to the prefab instance in the current scene.

**Solution**

I applied the instance changes to the original prefab, updating all prefab instances across the project.

This helped me understand the difference between editing a prefab instance and applying those changes to the original prefab asset.

---

## Current Game Structure

The game currently contains three playable levels.

The player must guide the rocket from the starting platform to the landing platform while avoiding obstacles.

- Crashing reloads the current level.
- Landing successfully loads the next level.
- Completing the final level returns the player to the first level.

The scene flow is controlled using each scene's `buildIndex`.

```csharp
int currentScene = SceneManager.GetActiveScene().buildIndex;
int nextScene = currentScene + 1;
```

If the calculated next scene reaches the total number of scenes included in the Build Settings, the game returns to scene `0`.

```csharp
if (nextScene == SceneManager.sceneCountInBuildSettings)
{
    nextScene = 0;
}
```

This creates a complete level loop and allows the player to restart from the first level after completing the game.

---

