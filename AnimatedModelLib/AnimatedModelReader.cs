using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Lofionic.AnimatedModel {

    public class AnimatedModelReader : ContentTypeReader<AnimatedModel> {
        protected override AnimatedModel Read(ContentReader input, AnimatedModel existingInstance) {
            Model model = input.ReadObject<Model>();
            int frameCount = input.ReadInt32();
            int clipCount = input.ReadInt32();

            Dictionary<string, object> tag = model.Tag as Dictionary<string, object>;
            AnimatedModelData animationData = tag["AnimatedModelData"] as AnimatedModelData;
            AnimationClip clip = animationData.Animations[0];

            AnimationClip[] clips = new AnimationClip[clipCount];
            string[] clipNames = new string[clipCount];

            for (int i = 0; i < clipCount; i++) {
                clipNames[i] = input.ReadString();

                int startFrame = input.ReadInt32();
                int endFrame = input.ReadInt32();
                clips[i] = clip.Divide(startFrame, endFrame, frameCount);
            }

            return new AnimatedModel(model, clips, clipNames);
        }
    }
}
