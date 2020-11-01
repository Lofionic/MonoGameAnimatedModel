using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lofionic.AnimatedModel.Effects {

    public abstract class LightingEffect : SceneEffect {
        private readonly EffectParameter ambientLightColor;
        private readonly EffectParameter ambientLightIntensity;

        private readonly EffectParameter diffuseLightDirection;
        private readonly EffectParameter diffuseLightColor;
        private readonly EffectParameter diffuseLightIntensity;

        private readonly EffectParameter shadowMap;
        private readonly EffectParameter shadowMapView;
        private readonly EffectParameter shadowMapProjection;
        private readonly EffectParameter shadowDepthBias;

        public Color AmbientLightColor { get => new Color(ambientLightColor.GetValueVector3()); set => ambientLightColor.SetValue(value.ToVector3()); }
        public float AmbientIntensity { get => ambientLightIntensity.GetValueSingle(); set => ambientLightIntensity.SetValue(value); }

        public Vector3 DiffuseLightDirection {
            get => diffuseLightDirection.GetValueVector3();
            set {
                value.Normalize();
                diffuseLightDirection.SetValue(value);
            }
        }
        public Color DiffuseLightColor { get => new Color(diffuseLightColor.GetValueVector3()); set => diffuseLightColor.SetValue(value.ToVector3()); }
        public float DiffuseLightIntensity { get => diffuseLightIntensity.GetValueSingle(); set => diffuseLightIntensity.SetValue(value); }

        public Texture2D ShadowMap { get => shadowMap.GetValueTexture2D(); set => shadowMap.SetValue(value); }
        public Matrix ShadowMapView { get => shadowMapView.GetValueMatrix(); set => shadowMapView.SetValue(value); }
        public Matrix ShadowMapProjection { get => shadowMapProjection.GetValueMatrix(); set => shadowMapProjection.SetValue(value); }
        public float ShadowDepthBias { get => shadowDepthBias.GetValueSingle(); set => shadowDepthBias.SetValue(value); }

        internal LightingEffect(GraphicsDevice grahics, byte[] byteCode) : base(grahics, byteCode) {
            ambientLightIntensity = Parameters["AmbientLightIntensity"];
            ambientLightColor = Parameters["AmbientLightColor"];

            diffuseLightDirection = Parameters["DiffuseLightDirection"];
            diffuseLightColor = Parameters["DiffuseLightColor"];
            diffuseLightIntensity = Parameters["DiffuseLightIntensity"];

            shadowMap = Parameters["ShadowMap"];
            shadowMapView = Parameters["ShadowMapView"];
            shadowMapProjection = Parameters["ShadowMapProjection"];
            shadowDepthBias = Parameters["ShadowDepthBias"];
        }
    }
}
