using System;
using Microsoft.Xna.Framework.Content;

namespace Lofionic.AnimatedModel {

    public class AnimationClip {

        private TimeSpan duration;
        private TimeSpan offset = TimeSpan.Zero;

        public AnimationClip(TimeSpan duration, Keyframe[] keyframes) {
            this.duration = duration;
            Keyframes = keyframes;
        }

        public AnimationClip(string name, TimeSpan duration, Keyframe[] keyframes) {
            Name = name;
            this.duration = duration;
            Keyframes = keyframes;
        }

        private AnimationClip(TimeSpan offset, TimeSpan duration, Keyframe[] keyframes) {
            this.offset = offset;
            this.duration = duration;
            Keyframes = keyframes;
        }

        public AnimationClip Divide(int startFrame, int endFrame, int totalFrames) {
            double clipLengthMs = Duration.TotalMilliseconds;
            double frameLengthMs = clipLengthMs / totalFrames;

            return new AnimationClip(
                TimeSpan.FromMilliseconds(frameLengthMs * startFrame),
                TimeSpan.FromMilliseconds(frameLengthMs * (endFrame - startFrame)),
                Keyframes);
        }

        private AnimationClip() { }

        [ContentSerializer]
        public string Name { get; set; }

        [ContentSerializer]
        public TimeSpan Duration {
            get => duration;
            set => duration = value;
        }

        [ContentSerializer(Optional = true)]
        public TimeSpan Offset {
            get => offset;
            set => offset = value;
        }

        [ContentSerializer]
        public Keyframe[] Keyframes { get; set; }
    }
}
