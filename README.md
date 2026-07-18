# Rocket Boost

My second Unity project, using C# — a rocket landing game focused on thrust, rotation and physics-based movement.

## Latest Progress (07/17/26):

In this stage of the project, I expanded the game by adding new levels, visual effects, sound effects, scene transitions, and improved collision handling.

### New Features

- Added collision-based sound effects using `OnCollisionEnter()`.
- Learned and implemented a `switch` statement to handle different collision tags.
- Added two new scenes, bringing the game to a total of three levels.
- Added particle effects for successful landings and crashes.
- Added a delay before reloading the current level after a crash.
- Added a delay before loading the next level after a successful landing.
- Implemented automatic scene progression using the current scene's build index.
- Added logic to return to the first level after completing the final scene.

### Code Organization

- Created separate methods for:
  - stopping player movement;
  - handling the success sequence;
  - handling the crash sequence;
  - loading the next level;
  - reloading the current level.
- Used a boolean variable to prevent the collision logic from running multiple times.
- Improved code readability by separating responsibilities into smaller methods.

---

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
- I learned that changes made to one prefab instance are not automatically applied to every scene.
- To fix this, I applied the changes to the original prefab so that all instances across the project received the updated configuration.

### Physics

- Configured the Rigidbody's **Linear Damping** to control the rocket's deceleration.
- Adjusted Unity's default gravity from **-9.81** to **-4** to better fit the game's physics and improve the overall gameplay feel.

### Audio

- Learned how to use the **AudioSource** component.
- Controlled the rocket's thrust sound through code.
- Solved an issue where the engine sound kept starting and stopping while the thrust key was held by checking `audioSource.isPlaying` before calling `Play()`.
- I learned how to use `AudioSource.PlayOneShot()` to play different sound effects for crashes and successful landings.
- I also stopped the rocket engine sound before playing the collision sound effect.

### Tools

- Learned how to use **OBS Studio** to record gameplay footage for GitHub documentation and LinkedIn posts.

### Collision Handling

- I learned how to use `OnCollisionEnter()` to detect collisions and respond differently depending on the tag of the object.
- I used a `switch` statement to organize the collision logic:
- `Friendly` objects allow the rocket to continue.
- `Finish` objects trigger the success sequence.
- Any other object triggers the crash sequence.

### Scene Management

I learned how to:

- get the current scene using `SceneManager.GetActiveScene()`;
- access the scene's `buildIndex`;
- calculate the next scene;
- reload the current scene;
- return to the first scene after the final level.

### Particle Systems

I added separate particle effects for:

- successful landings;
- crashes.
- These effects are triggered through code when the corresponding collision sequence starts.

### Delayed Method Calls

I learned how to use `Invoke()` to delay scene transitions, allowing the player to see and hear the crash or success effects before the scene changes.

---

##  Problems Solved

### Engine sound repeatedly starting and stopping

**Issue**

While holding the thrust key, the engine sound kept starting and stopping every frame.

**Cause**

Both `Play()` and `Stop()` were being executed within the same input condition. Since `Update()` runs every frame, the audio continuously alternated between playing and stopping.

**Solution**

I used `audioSource.isPlaying` to ensure the audio only starts when it isn't already playing, and moved `Stop()` to execute only when the thrust key is released.+

---

### Rocket engine sound continued after a collision

**Issue**

After the rocket crashed or reached the landing platform, the engine sound continued playing.

**Cause**

The movement input was disabled, but the currently playing engine audio was not stopped.

**Solution**

I called:

```csharp
audioSource.Stop(); before playing the crash or success sound effect.

---

### Scene changed too quickly after a collision

**Issue**

The level reloaded or changed immediately after a crash or successful landing, making the sound and particle effects difficult to notice.

**Cause**

The scene transition happened as soon as the collision sequence started.

**Solution**

I used `Invoke()` with a configurable delay:

```csharp
Invoke("ReloadLevel", levelLoadDelay); and:

```csharp
Invoke("LoadNextLevel", levelLoadDelay);
```

This gives the sound and particle effects time to play before changing scenes.

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
- Completing the final level returns the player to the first scene.

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

