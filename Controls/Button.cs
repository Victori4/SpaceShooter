using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceShooter.Controls
{
    // this class comes from:
    // https://github.com/Oyyou/MonoGame_Tutorials/blob/master/MonoGame_Tutorials/Tutorial020/Controls/Button.cs
    // comments were added where needed for my own understanding

    public class Button : Component
    {
            #region Fields

            private MouseState _currentMouse;

            private SpriteFont _font;

            private bool _isHovering;

            private MouseState _previousMouse;

            private Texture2D _texture;

            #endregion

            #region Properties

            public EventHandler Click;

            public bool Clicked { get; private set; }

            public float Layer { get; set; }

            public Vector2 Origin
            {
                get
                {
                    return new Vector2(_texture.Width / 2, _texture.Height / 2);
                }
            }

            public Color PenColour { get; set; }

            public Vector2 Position { get; set; }

            public Rectangle Rectangle
            {
                get
                {
                    return new Rectangle((int)Position.X - ((int)Origin.X), (int)Position.Y - (int)Origin.Y, _texture.Width, _texture.Height);
                }
            }

            public string Text;

            #endregion

            #region Methods

            public Button(Texture2D texture, SpriteFont font)
            {
                _texture = texture;

                _font = font;

                PenColour = Color.Black;
            }

            public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
            {
                var colour = Color.White;

                if (_isHovering)
                    colour = Color.Gray;

                spriteBatch.Draw(_texture, Position, null, colour, 0f, Origin, 1f, SpriteEffects.None, Layer);

            // if there is text, draw it on the button in the specified colour, location and layer
                if (!string.IsNullOrEmpty(Text))
                {
                    var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                    var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                    spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, Layer + 0.01f);
                }
            }

            public override void Update(GameTime gameTime)
            {
                _previousMouse = _currentMouse;
                _currentMouse = Mouse.GetState();

                var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

                _isHovering = false;
                // if the mouse intersects with the rectangled, then it is hovering over it and colour changes to grey (stated above)
                if (mouseRectangle.Intersects(Rectangle))
                {
                    _isHovering = true;
                    // is user is hovering and clicks, invoke method/calls event attached to the button 
                    if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                    {
                        Click?.Invoke(this, new EventArgs());
                    }
                }
            }

            #endregion
        }
    }

