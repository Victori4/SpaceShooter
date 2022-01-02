using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Sprites
{
    public class Ship : Sprite, ICollidable
    {
        public int Health { get; set; }

        public Bullet Bullet { get; set; }

        public float Speed { get; set; }

        // constructor
        public Ship(Texture2D texture) : base(texture)
        {
        }

        // takes float for speed/what direction we are going in
        public void Shoot(float speed)
        {
            // check if bullet is null, if so break out
            if (Bullet == null)
                return;

            // clone of prefab above and cast as Bullet
            var bullet = Bullet.Clone() as Bullet;
            // set basics
            bullet.Position = this.Position;
            bullet.Colour = this.Colour;
            bullet.Layer = 0.1f;
            bullet.LifeSpan = 5f;
            // speed is direction since only going on X axis
            bullet.Velocity = new Microsoft.Xna.Framework.Vector2(speed, 0f);
            // parent is current sprite, which will be enemy or player
            bullet.Parent = this.Parent;
            
            Children.Add(bullet);
        }
        // virtual which means in player and enemy class we can override this method
        public virtual void OnCollide(Sprite sprite)
        {
            throw new NotImplementedException();
        }
    }
}
