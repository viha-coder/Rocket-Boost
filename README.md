# Rocket Boost

My second Unity project, built with C#. In this game, the player controls a rocket and must safely land on different platforms while avoiding obstacles.

## Play the Game

- WebGL: https://play.unity.com/en/games/abc918bf-4f8b-4e0e-bf17-9a827b107f0e/webgl

- Windows Build: https://github.com/viha-coder/Rocket-Boost/releases/tag/V1.1

## Final Development Update — July 2026

This project is now complete.

During development, I expanded the project by adding new levels, particle effects, sound effects, scene transitions, post-processing, and improving the overall game structure.

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
- Designed the first level using modular environment assets.
- Added post-processing effects to improve the scene's visual quality.
- Built a cave environment to guide the player's path naturally.
- Added oscillating obstacles to create moving hazards across the level.
- Implemented a Quit Application shortcut for standalone builds.

### Code Organization

- Created separate methods for:
  - stopping player movement;
  - handling the success sequence;
  - handling the crash sequence;
  - loading the next level;
  - reloading the current level.
- Used a boolean variable to prevent the collision logic from running multiple times.
- Kept `OnCollisionEnter()` focused on selecting the correct sequence, while separate methods handle movement, effects, and scene transitions.
- I also refactored the collision flow to keep each method responsible for a single task, making the code easier to maintain and extend.
- Created reusable components, such as the Oscillating script, allowing different objects to share the same movement behaviour with configurable values through the Inspector.

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

Particle Systems

Learned how to:

- create Particle Systems
- trigger them through code
- start and stop effects using `Play()` and `Stop()`
- use different effects for thrust, success and explosions

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

---

## Level Design

- Built the first playable level using modular assets.
- Learned how to compose an environment by combining rocks, buildings and obstacles.
- Used the environment to naturally guide the player's path.
- Improved the level's visual composition without affecting gameplay.

---

## Post Processing

- Learned how to use Unity's Post Processing.
- Improved the overall lighting and atmosphere of the level.
- Understood how post-processing can enhance the game's visual presentation without changing gameplay mechanics.

## Gameplay

- Implemented moving obstacles using an Oscillating script.
- Learned how to create reusable movement behaviour by exposing variables in the Inspector.
- Used oscillating objects to make levels more dynamic and challenging.

## Application Management

- Learned how to use Application.Quit() to close standalone builds.
- Used Debug.Log() to verify that the quit input was being detected while testing in the Unity Editor.

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

### Issue

The WebGL build suddenly started failing after previously working correctly.

### Cause

The project itself was not the issue. The problem happened during the WebGL build process.

### Solution

After cleaning the build environment and rebuilding the project, the WebGL version was successfully generated again.

Both Windows and WebGL builds are now available.

---

## Current Game Structure

The game currently contains three playable levels with increasing difficulty.

Players control a rocket using thrust and rotation while avoiding static and moving obstacles.

- Crashing reloads the current level.
- Landing successfully loads the next level.
- Completing the final level returns the player to the first level.

Moving obstacles use a reusable Oscillating component, making the levels more dynamic without requiring custom movement code for each object.

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

