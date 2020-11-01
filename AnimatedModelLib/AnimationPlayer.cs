using System;
using Microsoft.Xna.Framework;

namespace Lofionic.AnimatedModel {

    public class AnimationPlayer {

        private readonly AnimatedModelData modelData;
        private AnimationClip clip;

        private int keyframe;
        private TimeSpan time;

        private bool isPlaying = false;
        private bool looping = true;
        private float playbackSpeed = 1f;

        private readonly Matrix[] bones;
        private readonly Matrix[] bonesAbsolute;
        private readonly Matrix[] bonesAnimation;

        public AnimationPlayer(AnimatedModel model) : this(model.animationData) { }

        public AnimationPlayer(AnimatedModelData modelData) {
            this.modelData = modelData;

            bones = new Matrix[modelData.BonesBindPose.Length];
            bonesAbsolute = new Matrix[modelData.BonesBindPose.Length];
            bonesAnimation = new Matrix[modelData.BonesBindPose.Length];

            for (int i = 0; i < bones.Length; i++) {
                bones[i] = modelData.BonesBindPose[i];
            }
        }

        public void StartClip(AnimationClip clip, float playbackSpeed, bool looping) {
            this.clip = clip;
            this.playbackSpeed = playbackSpeed;
            this.looping = looping;
            keyframe = 0;
            time = TimeSpan.Zero;

            isPlaying = true;
        }

        public void Update(GameTime gameTime) {

            if (clip == null || !isPlaying) { return; }

            time += new TimeSpan((long)(gameTime.ElapsedGameTime.Ticks * playbackSpeed));

            if (time > clip.Duration && !looping) {
                isPlaying = false;
            }

            else if (time > clip.Duration) {
                long elapsedTicks = time.Ticks % clip.Duration.Ticks;
                time = new TimeSpan(elapsedTicks);
                keyframe = 0;
            }

            // When starting an animation, put bones into bind pose
            if (keyframe == 0) {
                for (int i = 0; i < bones.Length; i++) {
                    bones[i] = modelData.BonesBindPose[i];
                }
            }

            // Find the next keyframe
            int index = 0;
            Keyframe[] keyframes = clip.Keyframes;
            while (index < keyframes.Length && keyframes[index].Time <= time + clip.Offset) {
                int boneIndex = keyframes[index].Bone;
                bones[boneIndex] = keyframes[index].Transform;
                index++;
            }

            keyframe = index - 1;

            // Calculate the absolute coordinate of the bone
            bonesAbsolute[0] = bones[0];
            for (int i = 1; i < bonesAnimation.Length; i++) {
                int boneParent = modelData.BonesParent[i];
                // Transform the bone configuration by its
                // parent configuration
                bonesAbsolute[i] = bones[i] * bonesAbsolute[boneParent];
            }

            // Before we can transform the vertices we
            // need to put the vertices in the coordinate system of the
            // bone that is linked to it
            for (int i = 0; i < bonesAnimation.Length; i++) {
                bonesAnimation[i] = modelData.BonesInverseBindPose[i] * bonesAbsolute[i];
            }
        }

        public Matrix[] GetAnimationTransforms() {
            return bonesAnimation;
        }
    }
}
