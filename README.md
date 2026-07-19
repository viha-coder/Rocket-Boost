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
- Added debug keys to speed up testing during development.
- Added thrust particle effects while the rocket engine is active.
- Added rotation particle effects while rotating left or right.

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

# Things I've Learned

## Input System

- Learned to use Unity's new Input System instead of `Input.GetAxis()`.
- Created `InputAction` fields for thrust and rotation.
- Enabled input actions inside `OnEnable()`.
- Read button input using `.IsPressed()`.
- Read axis input using `.ReadValue<float>()`.

---

## Physics

- Used `Rigidbody.AddRelativeForce()` for thrust.
- Used `transform.Rotate()` for direct rotation.
- Learned the difference between movement and rotation.
- Configured `Linear Damping`.
- Adjusted Unity's gravity.

---

## Particle Systems

- Added thrust particles.
- Added left rotation particles.
- Added right rotation particles.
- Added crash particles.
- Added success particles.
- Controlled particle systems through code using `Play()` and `Stop()`.

---

## Audio

- Learned how to use `AudioSource`.
- Used `PlayOneShot()` for collision sounds.
- Prevented engine audio from restarting repeatedly.
- Stopped engine audio before collision sounds.

---

## Collision Handling

- Used `OnCollisionEnter()`.
- Learned how to use `switch`.
- Separated collision logic into multiple methods.
- Used a boolean flag to avoid multiple collision events.

---

## Scene Management

- Loaded the next level.
- Reloaded the current level.
- Used `SceneManager.GetActiveScene()`.
- Used `buildIndex`.
- Returned to the first level after the final level.
- Delayed scene transitions with `Invoke()`.

---

## Prefabs

- Learned the difference between prefab assets and prefab instances.
- Applied instance overrides to the original prefab.
- Understood how prefab changes propagate across scenes.

---

## Debug Tools

- Added debug keyboard shortcuts.
- Learned how to speed up testing during development.
- Created a shortcut to load the next level.
- Created a shortcut to toggle collision detection.

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

