# MonoGame Animated Model Example
This is an example implementation of model animation in MonoGame using a skinned effect shader.

## Building
Build the pipeline extension first. This creates the library used by the content builder to import and process the skinned model and its animation clips (from `model.json`).

## Implementation
During content build, the `AnmatedModelImporter` in  `AnimatedModelPipelineExtension` imports the FBX model and animation clip information from the json content.

`AnimatedModelProcessor` processes the model using the fbx processor and also extracts the bone data, using an implementation based on the Microsoft XNA example.

The `AnimatedModel` class wraps the model with its animation data, and is loaded into the example project using the content builder as usual.

When the animation is played, bone data is updated via the `AnimationPlayer`, and sent to the `AnimatedModelEffect` which is used to draw the model.

The HLSL source code for the shader is inclued in `AnimatedModelLib/Effects/Source`.

## Animation Clips
Since this processor does not support multiple animation clips in the FBX format, the animations are baked into the fbx as one long animation.

The animation clips represent slices of the timeline used for playing each animation.

When playing the example animation, use 'Space' to cycle through the animation clips.

## Assets
The rigged models and animations were sourced from [Kenney's Animated Characters 1](https://www.kenney.nl/assets/animated-characters) under CC0 1.0 Universal license.

## Author
Chris Rivers, chris@lofionic.co.uk
