using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PrimitiveExpander;

namespace ConsoleApp12
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        private Vector2 _a, _b, _c, _d, _e, _f;
        private float _x;

        private bool _pickA, _pickB, _pickD;
        private float _radiusA, _radiusB, _radiusD;

        private List<Vector2> _curve;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
        }


        protected override void Update(GameTime gameTime)
        {
            if (!IsActive) return;

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var totalTime = (float)gameTime.TotalGameTime.TotalSeconds;

            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            UpdatePrimitiveRenderer();

            var (width, height) = GraphicsDevice.Viewport.Bounds.Size.ToVector2();
            var mousePosition = mouseState.Position.ToVector2();
            mousePosition -= new Vector2(width, height) / 2;

            var distanceA = Vector2.Distance(_a, mousePosition);
            var distanceB = Vector2.Distance(_b, mousePosition);
            var distanceD = Vector2.Distance(_d, mousePosition);

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (!_pickA && !_pickB && !_pickD)
                {
                    if (distanceA < distanceB && distanceA < distanceD)
                    {
                        if (distanceA < 20)
                        {
                            _pickA = true;
                        }
                    }

                    if (distanceB < distanceA && distanceB < distanceD)
                    {
                        if (distanceB < 20)
                        {
                            _pickB = true;
                        }
                    }

                    if (distanceD < distanceA && distanceD < distanceB)
                    {
                        if (distanceD < 20)
                        {
                            _pickD = true;
                        }
                    }
                }
            }
            else
            {
                _pickA = false;
                _pickB = false;
                _pickD = false;
                if (distanceA < distanceB && distanceA < distanceD)
                {
                    _radiusA = distanceA < 20 ? 10f : 5f;
                }

                if (distanceB < distanceA && distanceB < distanceD)
                {
                    _radiusB = distanceB < 20 ? 10f : 5f;
                }

                if (distanceD < distanceA && distanceD < distanceB)
                {
                    _radiusD = distanceD < 20 ? 10f : 5f;
                }
            }

            if (_pickA)
            {
                _a = mousePosition;
            }

            if (_pickB)
            {
                _b = mousePosition;
            }

            if (_pickD)
            {
                _d = mousePosition;
            }

            GenerateCurve(10);

            base.Update(gameTime);
        }

        private void GenerateCurve(int segments)
        {
            _curve.Clear();
            _x = 0;

            var change = 100 / segments;
            var x = 0;

            while (x <= 100)
            {
                _x = x / 100f;
                _c = Program.GetCoords(_x, _a, _b);
                _e = Program.GetCoords(_x, _b, _d);
                _f = Program.GetCoords(_x, _c, _e);
                _curve.Add(new Vector2(_f.X, _f.Y));

                x += change;
            }
        }

        private void UpdatePrimitiveRenderer()
        {
            var (width, height) = GraphicsDevice.Viewport.Bounds.Size.ToVector2();
            var viewPosition = new Vector3(width, height, 0) / 2;

            PrimitiveRenderer.World =
                Matrix.CreateWorld(new Vector3(GraphicsDevice.Viewport.Bounds.Center.ToVector2(), 0), Vector3.Forward,
                    Vector3.Up);
            PrimitiveRenderer.View = Matrix.CreateLookAt(
                viewPosition + Vector3.Backward,
                viewPosition + Vector3.Forward,
                Vector3.Up
            );
            PrimitiveRenderer.Projection = Matrix.CreateOrthographic(
                width / 1,
                -height / 1,
                0.1f, 10f
            );
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            PrimitiveRenderer.DrawCircleF(
                null,
                Color.Red,
                _a,
                _radiusA
            );

            PrimitiveRenderer.DrawCircleF(
                null,
                Color.Red,
                _b,
                _radiusB
            );

            PrimitiveRenderer.DrawCircleF(
                null,
                Color.Red,
                _d,
                _radiusD
            );

            /*PrimitiveRenderer.DrawLine(
                null,
                Color.Black,
                _a, _b
            );

            PrimitiveRenderer.DrawLine(
                null,
                Color.Black,
                _b, _d
            );

            PrimitiveRenderer.DrawLine(
                null,
                Color.Blue,
                _c, _e
            );

            PrimitiveRenderer.DrawCircleF(
                null,
                Color.Red,
                _c,
                2f
            );

            PrimitiveRenderer.DrawCircleF(
                null,
                Color.Red,
                _e,
                2f
            );

            PrimitiveRenderer.DrawCircleF(
                null,
                Color.Red,
                _f,
                2f
            );*/

            for (int index = 1; index < _curve.Count; index += 1)
            {
                var a = _curve[index - 1];
                var b = _curve[index - 0];
                PrimitiveRenderer.DrawLine(
                    null,
                    Color.Red,
                    a, b
                );
            }

            base.Draw(gameTime);
        }

        protected override void Initialize()
        {
            PrimitiveRenderer.Initialise(GraphicsDevice);

            _curve = new List<Vector2>();

            _a = new Vector2(0, 0);
            _b = new Vector2(100, 100);
            _d = new Vector2(-100, 100);

            _radiusA = 5f;
            _radiusB = 5f;
            _radiusD = 5f;

            base.Initialize();
        }
    }
}