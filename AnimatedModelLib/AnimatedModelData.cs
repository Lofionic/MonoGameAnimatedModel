using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Lofionic.AnimatedModel {

    public class AnimatedModelData {
        public AnimatedModelData(
            Matrix[] bonesBindPose,
            Matrix[] bonesInverseBindPose,
            int[] bonesParent,
            AnimationClip[] animations) {
            BonesParent = bonesParent;
            BonesBindPose = bonesBindPose;
            BonesInverseBindPose = bonesInverseBindPose;
            Animations = animations;
        }

        // Used by XNB serializer
        private AnimatedModelData() { }

        [ContentSerializer]
        public int[] BonesParent { get; set; }

        [ContentSerializer]
        public Matrix[] BonesBindPose { get; set; }

        [ContentSerializer]
        public Matrix[] BonesInverseBindPose { get; set; }

        [ContentSerializer]
        public AnimationClip[] Animations { get; set; }
    }
}
