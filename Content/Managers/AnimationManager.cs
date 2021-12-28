using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Content.Managers
{
    // ICloneable so we can do more deep clone 
    public class AnimationManager : ICloneable
    {
        private Animation _animation;

        private float _timer;

        // animation property that returns current animation
        public Animation CurrentAnimation
        {
            get
            {
                return _animation;
            }
        }
        // vars from 26-34 used in Draw()
        public float Layer { get; set; }

        public Vector2 Origin { get; set; }

        public Vector2 Position { get; set; }

        public float Rotation { get; set; }

        public float Scale { get; set; }

        public AnimationManager(Animation animation)
        {
            _animation = animation;

            Scale = 1f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
              _animation.Texture,
              Position,
              new Rectangle(
                _animation.CurrentFrame * _animation.FrameWidth,
                0,
                _animation.FrameWidth,
                _animation.FrameHeight
                ),
              Color.White,
              Rotation,
              Origin,
              Scale,
              SpriteEffects.None,
              Layer
              );
        }

        public void Play(Animation animation)
        {
            if (_animation == animation)
                return;

            _animation = animation;

            _animation.CurrentFrame = 0;

            _timer = 0;
        }

        public void Stop()
        {
            _timer = 0f;

            _animation.CurrentFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0f;

                _animation.CurrentFrame++;

                if (_animation.CurrentFrame >= _animation.FrameCount)
                    _animation.CurrentFrame = 0;
            }
        }

        public object Clone()
        {
            // shallow clone (creates an object that points to the original obj)
            var animationManager = this.MemberwiseClone() as AnimationManager;
            // becomes more a deep clone of the Animation class in models folder
            animationManager._animation = animationManager._animation.Clone() as Animation;
            // returns new animationaManager
            return animationManager;
        }
    }
}
