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

        private Vector2 _a, _b, _c, _d;
        private Vector2 _offsetA, _offsetD;
        private float _x;

        private bool _pickA, _pickB, _pickC, _pickD;
        private float _radiusA, _radiusB, _radiusC, _radiusD;

        private List<Vector2> _curve;

        private MouseState _currMouseState;
        private MouseState _prevMouseState;

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
            
            _prevMouseState = _currMouseState;
            _currMouseState = Mouse.GetState();

            var mouseDelta = (_currMouseState.Position - _prevMouseState.Position).ToVector2();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            UpdatePrimitiveRenderer();

            var (width, height) = GraphicsDevice.Viewport.Bounds.Size.ToVector2();
            var mousePosition = _currMouseState.Position.ToVector2();
            mousePosition -= new Vector2(width, height) / 2;

            var distanceA = Vector2.Distance(_a, mousePosition);
            var distanceB = Vector2.Distance(_b, mousePosition);
            var distanceC = Vector2.Distance(_c, mousePosition);
            var distanceD = Vector2.Distance(_d, mousePosition);

            if (_currMouseState.LeftButton == ButtonState.Pressed)
            {
                if (!_pickA && !_pickB && !_pickC && !_pickD)
                {
                    if (distanceA < distanceB && distanceA < distanceC && distanceA < distanceD)
                    {
                        if (distanceA < 20)
                        {
                            _pickA = true;
                            _offsetA = _a - mousePosition;
                        }
                    }

                    if (distanceB < distanceA && distanceB < distanceC && distanceB < distanceD)
                    {
                        if (distanceB < 20)
                        {
                            _pickB = true;
                        }
                    }

                    if (distanceC < distanceA && distanceC < distanceB && distanceC < distanceD)
                    {
                        if (distanceC < 20)
                        {
                            _pickC = true;
                        }
                    }
                    
                    if (distanceD < distanceA && distanceD < distanceB && distanceD < distanceC)
                    {
                        if (distanceD < 20)
                        {
                            _pickD = true;
                            _offsetD = _d - mousePosition;
                        }
                    }
                }
            }
            else
            {
                _pickA = false;
                _pickB = false;
                _pickC = false;
                _pickD = false;
                if (distanceA < distanceB && distanceA < distanceC)
                {
                    _radiusA = distanceA < 20 ? 10f : 5f;
                }

                if (distanceB < distanceA && distanceB < distanceC)
                {
                    _radiusB = distanceB < 20 ? 10f : 5f;
                }

                if (distanceC < distanceA && distanceC < distanceB)
                {
                    _radiusC = distanceC < 20 ? 10f : 5f;
                }
                
                if (distanceD < distanceA && distanceD < distanceB && distanceD < distanceC)
                {
                    _radiusD = distanceD < 20 ? 10f : 5f;
                }
            }

            if (_pickA)
            {
                _a = mousePosition + _offsetA;
                _b += mouseDelta;
            }

            if (_pickB)
            {
                _b = mousePosition;
            }

            if (_pickC)
            {
                _c = mousePosition;
            }
            
            if (_pickD)
            {
                _d = mousePosition + _offsetD;
                _c += mouseDelta;
            }

            GenerateCurve(50);

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

                var (xa, ya) = _a;
                var (xb, yb) = _b;
                var (xc, yc) = _c;
                var (xd, yd) = _d;

                var (xcurve, ycurve) = Bezier.CubicBezier(
                    _x,
                    xa, ya,
                    xb, yb,
                    xc, yc,
                    xd, yd
                );

                _curve.Add(new Vector2(xcurve, ycurve));

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
                Color.Blue,
                _b,
                _radiusB
            );

            PrimitiveRenderer.DrawCircleF(
                null,
                Color.Blue,
                _c,
                _radiusC
            );
            
            PrimitiveRenderer.DrawCircleF(
                null,
                Color.Red,
                _d,
                _radiusD
            );

            PrimitiveRenderer.DrawLine(
                null,
                Color.Blue,
                _a, _b
            );
            
            PrimitiveRenderer.DrawLine(
                null,
                Color.Blue,
                _c, _d
            );
            
            DrawBezier();

            base.Draw(gameTime);
        }

        private void DrawBezier()
        {
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
        }

        protected override void Initialize()
        {
            PrimitiveRenderer.Initialise(GraphicsDevice);

            _curve = new List<Vector2>();

            _a = new Vector2(0, 0);
            _b = new Vector2(0, 100);
            _c = new Vector2(-100, 0);
            _d = new Vector2(-100, 100);

            _radiusA = 5f;
            _radiusB = 5f;
            _radiusC = 5f;
            _radiusD = 5f;

            base.Initialize();
        }
    }
}