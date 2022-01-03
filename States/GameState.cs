using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShooter.Content.Managers;
using SpaceShooter.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceShooter.States
{
    // this class comes from:
    // https://github.com/Oyyou/MonoGame_Tutorials/blob/master/MonoGame_Tutorials/Tutorial020/States/GameState.cs
    // comments were added where needed for my own understanding

    public class GameState : State
    {
        private EnemyManager _enemyManager;

        private SpriteFont _font;

        private List<Player> _players;

        private ScoreManager _scoreManager;

        private List<Sprite> _sprites;

        public int PlayerCount;

        // constructor
        public GameState(Game1 game, ContentManager content)
          : base(game, content)
        {
        }

        // loading in different items into game state
        public override void LoadContent()
        {
            var playerTexture = _content.Load<Texture2D>("Ships/Player_plane");
            var bulletTexture = _content.Load<Texture2D>("Bullet");

            _font = _content.Load<SpriteFont>("Font");

            _scoreManager = ScoreManager.Load();
            // create and populate list of sprites
            _sprites = new List<Sprite>()
      {
        new Sprite(_content.Load<Texture2D>("Background/Game"))
        {
          Layer = 0.0f,
          Position = new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2),
        }
      };
            // add bullet, which has an explosion
            // animation constructor needs texture and frame rate
            // also declaring the framespeed of said explosion on same line
            var bulletPrefab = new Bullet(bulletTexture)
            {
                Explosion = new Explosion(new Dictionary<string, Models.Animation>()
            {
              { "Explode", new Models.Animation(_content.Load<Texture2D>("Explosion"), 3) { FrameSpeed = 0.1f, } }
            })
                {
                    Layer = 0.5f,
                }
            };
            // if player is greater or equal 1, need to create and add player
            if (PlayerCount >= 1)
            {
                _sprites.Add(new Player(playerTexture)
                {
                    Colour = Color.Blue,
                    Position = new Vector2(100, 50),
                    Layer = 0.3f,
                    Bullet = bulletPrefab,
                    Input = new Models.Input()
                    {
                        Up = Keys.W,
                        Down = Keys.S,
                        Left = Keys.A,
                        Right = Keys.D,
                        Shoot = Keys.Space,
                    },
                    Health = 10,
                    Score = new Models.Score()
                    {
                        PlayerName = "Player 1",
                        Value = 0,
                    },
                });
            }
            // if greater of equal 2, create and add second player
            if (PlayerCount >= 2)
            {
                _sprites.Add(new Player(playerTexture)
                {
                    Colour = Color.Green,
                    Position = new Vector2(100, 200),
                    Layer = 0.4f,
                    Bullet = bulletPrefab,
                    Input = new Models.Input()
                    {
                        Up = Keys.Up,
                        Down = Keys.Down,
                        Left = Keys.Left,
                        Right = Keys.Right,
                        Shoot = Keys.Enter,
                    },
                    Health = 10,
                    Score = new Models.Score()
                    {
                        PlayerName = "Player 2",
                        Value = 0,
                    },
                });
            }
            // adding players from _sprites from above into the list of _players
            // place any Players inside _sprites into _players
            _players = _sprites.Where(c => c is Player).Select(c => (Player)c).ToList();
            // creates enemy manager, loading in content and add bullets
            _enemyManager = new EnemyManager(_content)
            {
                Bullet = bulletPrefab,
            };
        }

        public override void Update(GameTime gameTime)
        {
            // if user presses esc, menu screen comes up
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ChangeState(new MenuState(_game, _content));

            foreach (var sprite in _sprites)
                sprite.Update(gameTime);

            _enemyManager.Update(gameTime);
            // if CanAdd is T and sprites in list that are enemies are lower than the max allowed, add another enemy
            // if CanAdd is T and sprites in list that are enemies are lower than the max allowed, add another enemy
            if (_enemyManager.CanAdd && _sprites.Where(c => c is Enemy).Count() < _enemyManager.MaxEnemies)
            {
                _sprites.Add(_enemyManager.CreateEnemy());
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // creating list of collidable sprites
            var collidableSprites = _sprites.Where(c => c is ICollidable);

            foreach (var spriteA in collidableSprites)
            {
                foreach (var spriteB in collidableSprites)
                {
                    // Don't do anything if they're the same sprite!
                    if (spriteA == spriteB)
                        continue;
                    // if 2 diff sprites are not colliding, do nothing
                    if (!spriteA.CollisionArea.Intersects(spriteB.CollisionArea))
                        continue;
                    // if 2 diff spriites collide, call OnCollide method
                    if (spriteA.Intersects(spriteB))
                        ((ICollidable)spriteA).OnCollide(spriteB);
                }
            }

            // Add the children sprites to the list of sprites (ie bullets)
            int spriteCount = _sprites.Count;
            for (int i = 0; i < spriteCount; i++)
            {
                var sprite = _sprites[i];
                foreach (var child in sprite.Children)
                    _sprites.Add(child);

                sprite.Children = new List<Sprite>();
            }

            for (int i = 0; i < _sprites.Count; i++)
            {
                if (_sprites[i].IsRemoved)
                {
                    _sprites.RemoveAt(i);
                    i--;
                }
            }

            // If all the players are dead, save the scores, and return to the highscore state
            if (_players.All(c => c.IsDead))
            {
                foreach (var player in _players)
                    _scoreManager.Add(player.Score);

                ScoreManager.Save(_scoreManager);

                _game.ChangeState(new HighscoresState(_game, _content));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (var sprite in _sprites)
                sprite.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();

            float x = 10f;
            foreach (var player in _players)
            {
                spriteBatch.DrawString(_font, "Player: " + player.Score.PlayerName, new Vector2(x, 10f), Color.White);
                spriteBatch.DrawString(_font, "Health: " + player.Health, new Vector2(x, 30f), Color.White);
                spriteBatch.DrawString(_font, "Score: " + player.Score.Value, new Vector2(x, 50f), Color.White);

                x += 150;
            }
            spriteBatch.End();
        }
    }
}

