using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lofionic.AnimatedModel.Effects {

    public abstract class SceneEffect : Effect {

        public EffectTechnique RenderTechnique { get; private set; }
        public EffectTechnique ShadowTechnique { get; private set; }

        public EffectPassCollection Passes => CurrentTechnique.Passes;

        private readonly EffectParameter world;
        private readonly EffectParameter view;
        private readonly EffectParameter projection;

        public Matrix World { get => world.GetValueMatrix(); private set => world.SetValue(value); }
        public Matrix View { get => view.GetValueMatrix(); private set => view.SetValue(value); }
        public Matrix Projection { get => projection.GetValueMatrix(); private set => projection.SetValue(value); }

        internal SceneEffect(GraphicsDevice graphics, byte[] byteCode) : base(graphics, byteCode) {
            world = Parameters["World"];
            view = Parameters["View"];
            projection = Parameters["Projection"];

            RenderTechnique = Techniques["Render"];
            ShadowTechnique = Techniques["Shadow"]; //TODO : Rename 'depthmap' and use standard view projection

            CurrentTechnique = RenderTechnique;
        }

        public void SetViewProjection(Matrix view, Matrix projection) {
            View = view;
            Projection = projection;
        }

        public void SetWorld(Matrix world) {
            World = world;
        }
    }
}
