using Microsoft.Xna.Framework;
using SpaceShooter.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Sprites
{

    public class Explosion : Sprite

    {
        // keep track of explosion length
        private float _timer = 0f;
        public Explosion(Dictionary<string, Animation> animations) 
            : base(animations)
        {
        }

        public override void Update(GameTime gameTime)
        {
            _animationManager.Update(gameTime);
            // increment timer
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            // check timer
            // if we have 3 frames, 3 * 0.2 = 0.6f, meaning explosion will be removed after 0.6sec
            if (_timer > _animationManager.CurrentAnimation.FrameCount * _animationManager.CurrentAnimation.FrameSpeed)
                IsRemoved = true;
            

          
        }
    }
}
