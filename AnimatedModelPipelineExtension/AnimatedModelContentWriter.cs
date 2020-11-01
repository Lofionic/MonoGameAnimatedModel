using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Lofionic.AnimatedModelPipelineExtension {

    [ContentTypeWriter]
    internal class AnimatedModelContentWriter : ContentTypeWriter<AnimatedModelContent> {
        public override string GetRuntimeReader(TargetPlatform targetPlatform) {
            return "Lofionic.AnimatedModel.AnimatedModelReader, AnimatedModelLib";
        }

        protected override void Write(ContentWriter output, AnimatedModelContent value) {
            output.WriteObject(value.model);
            output.Write(value.frameCount);
            output.Write(value.clips.Length);

            foreach (AnimatedModelClipContent clip in value.clips) {
                output.Write(clip.name);
                output.Write(clip.startFrame);
                output.Write(clip.endFrame);
            }
        }
    }
}
