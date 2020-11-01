using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace Lofionic.AnimatedModelPipelineExtension {

    public class AnimatedModelClipContent {
        internal readonly string name;
        internal readonly int startFrame;
        internal readonly int endFrame;

        public AnimatedModelClipContent(string name, int startFrame, int endFrame) {
            this.name = name;
            this.startFrame = startFrame;
            this.endFrame = endFrame;
        }
    }

    public class AnimatedModelContent {

        internal readonly ModelContent model;
        internal readonly int frameCount;
        internal readonly AnimatedModelClipContent[] clips;

        public AnimatedModelContent(ModelContent model, int frameCount, AnimatedModelClipContent[] clips) {
            this.model = model;
            this.frameCount = frameCount;
            this.clips = clips;
        }
    }

    public struct AnimatedModelIntermediateType {

        public NodeContent nodeContent;
        public int frameCount;
        public AnimatedModelClipContent[] clips;

        public AnimatedModelIntermediateType(NodeContent model, int frameCount, AnimatedModelClipContent[] clips) {
            this.nodeContent = model;
            this.frameCount = frameCount;
            this.clips = clips;
        }
    }
}
