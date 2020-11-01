using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

using Newtonsoft.Json;
/* 

https://community.monogame.net/t/solved-pipeline-extension-3-8-not-finding-nuget-files/13808/15

To copy Newtonsoft assemblies to output folder, add this to project file:
<PropertyGroup>
    <CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
</PropertyGroup>

*/

namespace Lofionic.AnimatedModelPipelineExtension {

    [ContentImporter(".json", DisplayName = "Animated Model Importer", DefaultProcessor = "Animated Model Processor")]
    public class AnimatedModelImporter : ContentImporter<AnimatedModelIntermediateType> {

        private class AnimatedModelClipInfo {
            public readonly string name;
            public readonly int startFrame;
            public readonly int endFrame;

            public AnimatedModelClipInfo(string name, int startFrame, int endFrame) {
                this.name = name;
                this.startFrame = startFrame;
                this.endFrame = endFrame;
            }
        }

        private struct AnimatedModelInfo {
            public readonly string model;
            public readonly int totalFrames;
            public readonly AnimatedModelClipInfo[] clips;

            public AnimatedModelInfo(string model, int totalFrames, AnimatedModelClipInfo[] clips) {
                this.model = model;
                this.totalFrames = totalFrames;
                this.clips = clips;
            }
        }

        private readonly FbxImporter fbxImporter = new FbxImporter();

        public override AnimatedModelIntermediateType Import(string filename, ContentImporterContext context) {

            AnimatedModelInfo animatedModelInfo = JsonConvert.DeserializeObject<AnimatedModelInfo>(File.ReadAllText(filename));

            string root = Path.GetDirectoryName(filename);
            string extension = "fbx";
            string modelFilename = Path.ChangeExtension(Path.Combine(root, animatedModelInfo.model), extension);
            NodeContent nodeContent = fbxImporter.Import(modelFilename, context);

            AnimatedModelClipContent[] clipContent = new AnimatedModelClipContent[animatedModelInfo.clips.Length];
            for (int i = 0; i < animatedModelInfo.clips.Length; i++) {
                AnimatedModelClipInfo clipInfo = animatedModelInfo.clips[i];
                clipContent[i] = new AnimatedModelClipContent(clipInfo.name, clipInfo.startFrame, clipInfo.endFrame);
            }

            return new AnimatedModelIntermediateType(nodeContent, animatedModelInfo.totalFrames, clipContent);
        }
    }
}
