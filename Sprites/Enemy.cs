using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Sprites
{
    public class Enemy : Ship
    {
        private float _timer;
        public float ShootTimer = 1.75f;
        public Enemy(Texture2D texture) : base(texture)
        {
            Speed = 2f;
        }

        public override void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(_timer >= ShootTimer)
            {
                Shoot(-5f);
                _timer = 0;
            }
            // moves on L
            Position += new Vector2(-Speed, 0);

            // if enemy too far L/off screen, remove
            if (Position.X < -_texture.Width)
                IsRemoved = true;
        }

        public override void OnCollide(Sprite sprite)
        {
            // if enemy hits player that is still alive, player gets 1pt 
            // enemy is removed
            if(sprite is Player && !((Player)sprite).IsDead)
            {
                ((Player)sprite).Score.Value++;

                IsRemoved = true;
            }
            // if sprite that collides is bullet and comes from player
            // -1 health from enemy and if enemy health 0 or below, remove
            // player gets 1 pt
            if(sprite is Bullet && sprite.Parent is Player)
            {
                Health--;

                if(Health <= 0)
                {
                    IsRemoved = true;

                    ((Player)sprite.Parent).Score.Value++;
                }
            }
        }
    }
}
