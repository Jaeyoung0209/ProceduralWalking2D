# Procedural Walking Animation
This is a simple project made in Unity of a 2D human character walking using a procedurally generated animation.

The project contains two scripts:
- A CharacterController class that implements the procedural animation feature
- A GroundScript class that generates random terrains for the character to walk on

The CharacterController class contains the following key functions/coroutines:
- BezierCoroutine, for smoothly moving the back leg in a bezier curve over time for a single step.
- TorsoCorutine, for a natural movement of the torso when taking a step.
- ArmCoroutine, for a natural swing of the arms using a cosine wave.
- HandleInput function, that takes user input to move the character either forwards or backwards.
