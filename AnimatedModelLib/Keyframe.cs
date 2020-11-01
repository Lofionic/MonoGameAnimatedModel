using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Lofionic.AnimatedModel {

    public class Keyframe : IComparable {
        private TimeSpan time;
        private Matrix transform;

        [ContentSerializer]
        public TimeSpan Time {
            get => time;
            set => time = value;
        }

        [ContentSerializer]
        public int Bone { get; set; }

        [ContentSerializer]
        public Matrix Transform {
            get => transform;
            set => transform = value;
        }

        public Keyframe(TimeSpan time, int boneIndex, Matrix transform) {
            this.time = time;
            Bone = boneIndex;
            this.transform = transform;
        }

        public int CompareTo(object obj) {
            Keyframe keyframe = obj as Keyframe;
            if (obj == null) {
                throw new ArgumentException("Object is not a Keyframe.");
            }

            return time.CompareTo(keyframe.Time);
        }

        private Keyframe() { }
    }
}
