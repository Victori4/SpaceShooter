using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceShooter.Sprites;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Content.Managers
{
    public class EnemyManager
    {
        // how long been going for
        private float _timer;
        // potential textures for ship
        private List<Texture2D> _textures;
        
        public bool CanAdd { get; set; }

        public Bullet Bullet { get; set;}

        public int MaxEnemies { get; set; }
        // how often something spawns
        public float SpawnTimer { get; set; }

        // constructor
        public EnemyManager(ContentManager content)
        {
            _textures = new List<Texture2D>()
            {
                content.Load<Texture2D>("Ships/Enemy_1"),
                content.Load<Texture2D>("Ships/Enemy_2")
            };

            MaxEnemies = 10;
            // enemy can spawn every 2.5 sec
            SpawnTimer = 2.5f;
        }

        public void Update(GameTime gameTime)
        {
            // increment timer to total seconds and set CanAdd to F
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            CanAdd = false;

            // check _timer to see if more than SpawnTimer
            // if yes, CanAdd = T
            if(_timer > SpawnTimer)
            {
                CanAdd = true;
                _timer = 0f;                 
            }
        }
        // creates enemy and return to list later
        public Enemy CreateEnemy()
        {
            // texture can be any using Random
            var texture = _textures[Game1.Random.Next(0, _textures.Count)];

            return new Enemy(texture)
            {
                Colour = Color.Red,
                Bullet = Bullet,
                Health = 5,
                Layer = 0.2f,
                Position = new Vector2(Game1.ScreenWidth + texture.Width, Game1.Random.Next(0, Game1.ScreenHeight)), // just off right of screen and any random pt bw top and bottom of window

                Speed = 2 + (float)Game1.Random.NextDouble(),
                ShootTimer = 1.5f + (float)Game1.Random.NextDouble()
            };
        }
    }
}
