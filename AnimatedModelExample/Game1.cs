using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lofionic.AnimatedModel;
using Lofionic.AnimatedModel.Effects;
using System.Linq;

namespace AnimatedModelExample
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Matrix view;
        Matrix projection;

        AnimatedModel model;
        AnimatedModelEffect effect;
        AnimationPlayer animation;
        AnimationClip[] clips;
        int currentClipIndex = 1;

        KeyboardState previousKeyboardState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            model = Content.Load<AnimatedModel>("human");

            effect = new AnimatedModelEffect(GraphicsDevice);
            effect.DiffuseLightDirection = new Vector3(1, 1, -1);
            effect.DiffuseLightColor = Color.White;
            effect.DiffuseLightIntensity = 0.2f;

            effect.AmbientLightColor = Color.White;
            effect.AmbientIntensity = 0.8f;

            animation = new AnimationPlayer(model);
            clips = model.clips.Values.ToArray();
            animation.StartClip(clips[currentClipIndex], 1, true);


            Vector3 cameraPosition = new Vector3(-30, -30, 20);
            float aspectRatio = GraphicsDevice.PresentationParameters.BackBufferWidth / (float)GraphicsDevice.PresentationParameters.BackBufferHeight;

            view = Matrix.CreateLookAt(cameraPosition, new Vector3(0, 0, 20), Vector3.UnitZ);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, aspectRatio, 1, 500);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
                Exit();

            if (
                !previousKeyboardState.IsKeyDown(Keys.Space) &&
                keyboard.IsKeyDown(Keys.Space))
            {
                currentClipIndex = (currentClipIndex + 1) % clips.Length;
                animation.StartClip(clips[currentClipIndex], 1f, true);
            }
            previousKeyboardState = keyboard;

            animation.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            effect.SetViewProjection(view, projection);
            effect.SetWorld(Matrix.Identity);

            Matrix[] animationTransforms = animation.GetAnimationTransforms();
            effect.SetBones(animationTransforms);

            foreach (ModelMesh modelMesh in model.model.Meshes) {
                foreach (ModelMeshPart meshPart in modelMesh.MeshParts) {
                    meshPart.Effect = effect;
                }
                modelMesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
