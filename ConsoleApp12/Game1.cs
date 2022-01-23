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

        private Vector3 _cameraPosition;
        
        private Vector2 _a, _b, _c, _d, _e, _f, _g;
        private Vector2 _offsetA, _offsetD, _offsetG;
        private float _x;

        private bool _pickA, _pickB, _pickC, _pickD, _pickE, _pickF, _pickG;
        private float _radiusA, _radiusB, _radiusC, _radiusD, _radiusE, _radiusF, _radiusG;

        private List<Vector2> _curveA;
        private List<Vector2> _curveB;

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
            mouseDelta *= PrimitiveRenderer.Scale;
            
            
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (_currMouseState.ScrollWheelValue > _prevMouseState.ScrollWheelValue)
            {
                PrimitiveRenderer.Scale -= 1f * deltaTime;
            }
            
            if (_currMouseState.ScrollWheelValue < _prevMouseState.ScrollWheelValue)
            {
                PrimitiveRenderer.Scale += 1f * deltaTime;
            }

            if (_currMouseState.MiddleButton == ButtonState.Pressed)
            {

                _cameraPosition.X -= mouseDelta.X;
                _cameraPosition.Y -= mouseDelta.Y;

            }

            UpdatePrimitiveRenderer();

            var (width, height) = GraphicsDevice.Viewport.Bounds.Size.ToVector2();
            var mousePosition = _currMouseState.Position.ToVector2();
            mousePosition -= new Vector2(width, height) / 2;
            
            mousePosition *= PrimitiveRenderer.Scale;
            
            mousePosition.X += _cameraPosition.X;
            mousePosition.Y += _cameraPosition.Y;

            

            var distanceA = Vector2.Distance(_a, mousePosition);
            var distanceB = Vector2.Distance(_b, mousePosition);
            var distanceC = Vector2.Distance(_c, mousePosition);
            var distanceD = Vector2.Distance(_d, mousePosition);
            var distanceE = Vector2.Distance(_e, mousePosition);
            var distanceF = Vector2.Distance(_f, mousePosition);
            var distanceG = Vector2.Distance(_g, mousePosition);

            if (_currMouseState.LeftButton == ButtonState.Pressed)
            {
                if (!_pickA && !_pickB && !_pickC && !_pickD && !_pickE && !_pickF && !_pickG)
                {
                    if (distanceA < 20)
                    {
                        _pickA = true;
                        _offsetA = _a - mousePosition;
                    }

                    else if (distanceB < 20)
                    {
                        _pickB = true;
                    }

                    else if (distanceC < 20)
                    {
                        _pickC = true;
                    }

                    else if (distanceD < 20)
                    {
                        _pickD = true;
                        _offsetD = _d - mousePosition;
                    }

                    else if (distanceE < 20)
                    {
                        _pickE = true;
                    }

                    else if (distanceF < 20)
                    {
                        _pickF = true;
                    }

                    else if (distanceG < 20)
                    {
                        _pickG = true;
                        _offsetG = _g - mousePosition;
                    }
                }
            }
            else
            {
                _pickA = false;
                _pickB = false;
                _pickC = false;
                _pickD = false;
                _pickE = false;
                _pickF = false;
                _pickG = false;

                _radiusA = distanceA < 20 ? 10f : 5f;
                _radiusB = distanceB < 20 ? 10f : 5f;
                _radiusC = distanceC < 20 ? 10f : 5f;
                _radiusD = distanceD < 20 ? 10f : 5f;
                _radiusE = distanceE < 20 ? 10f : 5f;
                _radiusF = distanceF < 20 ? 10f : 5f;
                _radiusG = distanceG < 20 ? 10f : 5f;
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
                _e += mouseDelta;
            }

            if (_pickE)
            {
                _e = mousePosition;
            }

            if (_pickF)
            {
                _f = mousePosition;
            }

            if (_pickG)
            {
                _g = mousePosition + _offsetG;
                _f += mouseDelta;
            }

            if (_pickA || _pickB || _pickC || _pickD || _pickE || _pickF || _pickG)
            {
                GenerateCurve(25);
            }

            base.Update(gameTime);
        }

        private void GenerateCurve(int segments)
        {
            _curveA.Clear();
            _curveB.Clear();
            
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
                var (xe, ye) = _e;
                var (xf, yf) = _f;
                var (xg, yg) = _g;

                float xcurve, ycurve;
                    
                (xcurve, ycurve) = Bezier.CubicBezier(
                    _x,
                    xa, ya,
                    xb, yb,
                    xc, yc,
                    xd, yd
                );

                _curveA.Add(new Vector2(xcurve, ycurve));
                
                (xcurve, ycurve) = Bezier.CubicBezier(
                    _x,
                    xd, yd,
                    xe, ye,
                    xf, yf,
                    xg, yg
                );

                _curveB.Add(new Vector2(xcurve, ycurve));

                x += change;
            }
        }

        private void UpdatePrimitiveRenderer()
        {
            PrimitiveRenderer.ViewOffset = _cameraPosition;
            PrimitiveRenderer.UpdateDefaultCamera();
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

            PrimitiveRenderer.DrawCircleH(
                null,
                Color.Red,
                _d,
                _radiusD
            );
            
            PrimitiveRenderer.DrawCircleF(
                null,
                Color.Blue,
                _e,
                _radiusE
            );
            
            PrimitiveRenderer.DrawCircleF(
                null,
                Color.Blue,
                _f,
                _radiusF
            );
            
            PrimitiveRenderer.DrawCircleF(
                null,
                Color.Red,
                _g,
                _radiusG
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

            PrimitiveRenderer.DrawLine(
                null,
                Color.Blue,
                _d, _e
            );

            PrimitiveRenderer.DrawLine(
                null,
                Color.Blue,
                _f, _g
            );

            PrimitiveRenderer.DrawCircleF(
                null,
                Color.Green,
                GetBezierPosition((float)gameTime.TotalGameTime.TotalMilliseconds / 50),
                4f
            );
            
            DrawBezier();

            base.Draw(gameTime);
        }

        private Vector2 GetBezierPosition(float x)
        {
            x = (int)x;
            if (x < _curveA.Count)
            {
                return _curveA[(int)x];
            }
            
            x -= _curveA.Count - 1;
            if (x < _curveB.Count)
            {
                return _curveB[(int)x];
            }
            
            x -= _curveB.Count - 1;
            return GetBezierPosition(x);
        }

        private void DrawBezier()
        {
            for (int index = 1; index < _curveA.Count; index += 1)
            {
                var a = _curveA[index - 1];
                var b = _curveA[index - 0];
                PrimitiveRenderer.DrawLine(
                    null,
                    Color.Red,
                    a, b
                );
            }
            
            for (int index = 1; index < _curveB.Count; index += 1)
            {
                var a = _curveB[index - 1];
                var b = _curveB[index - 0];
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

            _curveA = new List<Vector2>();
            _curveB = new List<Vector2>();

            _a = new Vector2(-100, 100);
            _d = new Vector2(0, 0);
            _g = new Vector2(100, -100);
            
            _b = new Vector2(0, 100);
            _c = new Vector2(-100, 0);
            _e = new Vector2(100, 0);
            _f = new Vector2(0, -100);

            _radiusA = 5f;
            _radiusB = 5f;
            _radiusC = 5f;
            _radiusD = 5f;
            _radiusE = 5f;
            _radiusF = 5f;
            _radiusG = 5f;

            GenerateCurve(25);

            base.Initialize();
        }
    }
}