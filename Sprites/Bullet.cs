using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Sprites
{
    public class Bullet : Sprite, ICollidable
    {
        private float _timer;
        //prefab
        public Explosion Explosion;
        public float LifeSpan { get; set; }
        public Vector2 Velocity { get; set; }

        public Bullet(Texture2D texture)
            : base(texture)
        {

        }

        public override void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer >= LifeSpan)
                IsRemoved = true;

            Position += Velocity;
        }

        public void OnCollide(Sprite sprite)
        {

            // possible switch case taken for future ref from:
            // https://github.com/Oyyou/MonoGame_Tutorials/blob/master/MonoGame_Tutorials/Tutorial020/Sprites/Bullet.cs

            //switch (sprite)
            //{
            //  case Bullet b:
            //    return;
            //  case Enemy e1 when e1.Parent is Enemy:

            //    if (e1.Parent is Enemy)
            //      return;

            //    if()

            //    return;
            //  case Enemy e2 when 

            //  case Player p when (p.IsDead || p.Parent is Player):
            //    return;
            //}

            // Check that bullets do not collide with each other
            if (sprite is Bullet)
                return;

            // Check that enemies cannot shoot each other
            if (sprite is Enemy && this.Parent is Enemy)
                return;

            // Check that playeres cannot shoot each other
            if (sprite is Player && this.Parent is Player)
                return;
            // Check that dead players cannot be hit
            // sprite needs to be cast as player
            if (sprite is Player && ((Player)sprite).IsDead)
                return;

            // if obj hitting is enemy and shooter is player, remove bullet, add explosion
            if(sprite is Enemy && this.Parent is Player)
            {
                IsRemoved = true;
                AddExplosion();
            }

            // if enemy bullet hits a player, remove and explosion
            if(sprite is Player && this.Parent is Enemy)
            {
                IsRemoved = true;
                AddExplosion();
            }
        }

        public void AddExplosion()
        {
            // bullets do not necessarily explode
            if (Explosion == null)
                return;

            // create clone of explosion as new obj and set position to that of bullet
            var explosion = Explosion.Clone() as Explosion;
            explosion.Position = this.Position;
            // add to list of children, added to list of sprites at end to be drawn
            Children.Add(explosion);

        }
    }
}
