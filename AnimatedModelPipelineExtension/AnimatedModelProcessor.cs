using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace Lofionic.AnimatedModelPipelineExtension {

    [ContentProcessor(DisplayName = "Animated Model Processor")]
    public class AnimatedModelProcessor : ContentProcessor<AnimatedModelIntermediateType, AnimatedModelContent> {

        private readonly SkinnedModelProcessor modelProcessor = new SkinnedModelProcessor();

        public override AnimatedModelContent Process(AnimatedModelIntermediateType input, ContentProcessorContext context) {
            ModelContent modelContent = modelProcessor.Process(input.nodeContent, context);
            return new AnimatedModelContent(modelContent, input.frameCount, input.clips);
        }
    }
}