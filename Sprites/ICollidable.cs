using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Sprites
{
    public interface ICollidable
    {
        // method saying sprites can only collide with sprites
        void OnCollide(Sprite sprite);
    }
}
