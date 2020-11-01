using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Lofionic.AnimatedModel {

    public sealed class AnimatedModel {

        public readonly Model model;
        public readonly Dictionary<string, AnimationClip> clips;

        internal readonly AnimatedModelData animationData;
        
        internal AnimatedModel(Model model, AnimationClip[] clips, string[] clipNames) {
            this.model = model;

            Dictionary<string, object> tag = model.Tag as Dictionary<string, object>;
            animationData = tag["AnimatedModelData"] as AnimatedModelData;

            this.clips = new Dictionary<string, AnimationClip>();
            for (int i = 0; i < clips.Length; i++) {
                this.clips[clipNames[i]] = clips[i];
            }
        }
    }
}
