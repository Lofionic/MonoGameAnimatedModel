using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

using Lofionic.AnimatedModel;

namespace Lofionic.AnimatedModelPipelineExtension {

    [ContentProcessor(DisplayName = "Skinned Model Processor")]
    public class SkinnedModelProcessor : ModelProcessor {

        public override ModelContent Process(NodeContent input, ContentProcessorContext context) {

            AnimatedModelData animatedModelData = ExtractSkeletonAndAnimations(input, context);

            Dictionary<string, object> dictionary = new Dictionary<string, object> {
                { "AnimatedModelData", animatedModelData }
            };

            ModelContent modelContent = base.Process(input, context);
            modelContent.Tag = dictionary;

            return modelContent;
        }

        private AnimatedModelData ExtractSkeletonAndAnimations(NodeContent input, ContentProcessorContext context) {
            // Find the root node
            BoneContent skeleton = MeshHelper.FindSkeleton(input);

            // Bake local transform into skeleton
            FlattenTransforms(input, skeleton);

            // Flatten bone hierarchy into list
            IList<BoneContent> boneList = MeshHelper.FlattenSkeleton(skeleton);

            // Create arrays bind pose, inverse bind pose, and parent for each bone
            Matrix[] bonesBindPose = new Matrix[boneList.Count];
            Matrix[] bonesInverseBindPose = new Matrix[boneList.Count];
            int[] bonesParentIndex = new int[boneList.Count];

            // Indexed list of bone names
            List<string> boneNameList = new List<string>(boneList.Count);

            for (int i = 0; i < boneList.Count; i++) {
                bonesBindPose[i] = boneList[i].Transform;
                bonesInverseBindPose[i] = Matrix.Invert(boneList[i].AbsoluteTransform);

                // Look up index of parent bone using its name
                bonesParentIndex[i] = boneNameList.IndexOf(boneList[i].Parent.Name);

                boneNameList.Add(boneList[i].Name);
            }

            // Extract all animations
            AnimationClip[] animations = ExtractAnimations(
                skeleton.Animations, boneNameList, context);

            return new AnimatedModelData(
                bonesBindPose,
                bonesInverseBindPose,
                bonesParentIndex,
                animations);
        }

        private static void FlattenTransforms(NodeContent node, BoneContent skeleton) {
            foreach (NodeContent child in node.Children) {
                // Don't process the skeleton, because that is special.
                if (child == skeleton) {
                    continue;
                }

                // Bake the local transform into the actual geometry.
                MeshHelper.TransformScene(child, child.Transform);

                // Having baked it, we can now set the local
                // coordinate system back to identity.
                child.Transform = Matrix.Identity;

                // Recurse.
                FlattenTransforms(child, skeleton);
            }
        }

        private AnimationClip[] ExtractAnimations(
            AnimationContentDictionary animationDictionary,
            List<string> boneNameList,
            ContentProcessorContext context) {
            if (animationDictionary.Count < 1) {
                context.Logger.LogImportantMessage("Warning: No animations found.");
            }

            AnimationClip[] animations = new AnimationClip[animationDictionary.Count];

            int animationCount = 0;
            foreach (AnimationContent animationContent in animationDictionary.Values) {
                List<Keyframe> keyframes = new List<Keyframe>();

                // Each bone has its own channel
                foreach (string bone in animationContent.Channels.Keys) {
                    AnimationChannel animationChannel = animationContent.Channels[bone];
                    int boneIndex = boneNameList.IndexOf(bone);

                    foreach (AnimationKeyframe keyframe in animationChannel) {
                        keyframes.Add(new Keyframe(
                            keyframe.Time, boneIndex, keyframe.Transform));
                    }
                }

                // Sort all animation frames by time
                keyframes.Sort();

                animations[animationCount++] = new AnimationClip(
                    animationContent.Name,
                    animationContent.Duration,
                    keyframes.ToArray());
            }

            return animations;
        }
    }
}
