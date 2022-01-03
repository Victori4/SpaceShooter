using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Sprites
{
    public class Player : Ship
    {

        private KeyboardState _currentKey;

        private KeyboardState _previousKey;

        // how often we can shoot
        private float _shootTimer = 0;

        // T if health <= 0
        public bool IsDead
        {
            get
            {
                return Health <= 0;
            }
        }

        public Input Input { get; set; }

        public Score Score { get; set; }

        public Player(Texture2D texture) : base(texture)
        {
            Speed = 3f;
        }

        public override void Update(GameTime gameTime)
        {
            // do not update dead sprite
            if (IsDead)
                return;

            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            var velocity = Vector2.Zero;
            _rotation = 0;

            // check if up is pressed, if so assign Y velo to -speed
            // and tilt up a bit
            if(_currentKey.IsKeyDown(Input.Up))
            {
                velocity.Y -= Speed;
                _rotation = MathHelper.ToRadians(-15);
            }

            else if (_currentKey.IsKeyDown(Input.Down))
            {
                velocity.Y += Speed;
                _rotation = MathHelper.ToRadians(15);
            }

            if (_currentKey.IsKeyDown(Input.Left))
            {
                velocity.X -= Speed;
            }

            else if (_currentKey.IsKeyDown(Input.Right))
            {
                velocity.X += Speed;
            }

            _shootTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // check to see if pressing shoot key and it's been more than 1/4 of sec
            // this means we can keep holding space bar down and it will keep shooting but not too fast
            if(_currentKey.IsKeyDown(Input.Shoot) && _shootTimer > 0.25f )
            {
                Shoot(Speed * 2);
                _shootTimer = 0f;
            }

            Position += velocity;

            // clamp position bw 2 points
            // ship should move bw rectangle area of screen, do not go off screen
            // 80 on L, 0 at top, quarter to R and all the way to bottom
            Position = Vector2.Clamp(Position, new Vector2(80, 0), new Vector2(Game1.ScreenWidth / 4, Game1.ScreenHeight));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(IsDead)
                return;

            base.Draw(gameTime, spriteBatch);
        }

        public override void OnCollide(Sprite sprite)
        {
            if (IsDead)
                return;

            // if sprite colloding a bullet from enemy, minus 1 health
            if (sprite is Bullet && ((Bullet)sprite).Parent is Enemy)
                Health--;

            // if colloding with enemy, lose 3 health
            if (sprite is Enemy)
                Health -= 3;
        }

    }
}
