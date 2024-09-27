# Procedural Walking Animation
This is a simple project made in Unity of a 2D human character walking using a procedurally generated animation.

The project contains two scripts:
- A CharacterController class that implements the procedural animation feature
- A GroundScript class that generates random terrains for the character to walk on

The CharacterController class contains the following key functions/coroutines:
- BezierCoroutine, for smoothly moving the back leg forwards in a bezier curve over time for a single step.
- TorsoCorutine, for a natural movement of the torso when taking a step.
- ArmCoroutine, for a natural swing of the arms using a cosine wave.
- HandleInput function, that takes user input to move the character either forwards or backwards. As soon as the step is taken, raycasting offset horizontally by a certain distance from the character is used to determine the position for the next step.

This is a clip of the animation in action.

https://github.com/user-attachments/assets/fd23df57-e090-4584-851b-4819c9bc8708

