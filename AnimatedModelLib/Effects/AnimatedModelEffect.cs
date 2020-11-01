using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lofionic.AnimatedModel.Effects {

    public interface IAnimatedModelEffect {
        AnimatedModelEffect AnimatedModelEffect { get; }
    }

    public class AnimatedModelEffect : LightingEffect {
        private readonly EffectParameter bones;
        private readonly EffectParameter texture;

        public Texture2D Texture { get => texture.GetValueTexture2D(); set => texture.SetValue(value); }

        public AnimatedModelEffect(GraphicsDevice graphics) : base(graphics, EffectResource.AnimatedModel.Bytecode) {
            texture = Parameters["Texture"];
            bones = Parameters["Bones"];
        }

        public void SetBones(Matrix[] boneData) {
            bones.SetValue(boneData);
        }
    }
}
