using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NQuad.Utils.Input;

namespace NQuad {
    public static class Input {

#if KEYBOARD_MOUSE
        private static KeyboardState KeyboardCurrentState { get; set; }
        private static KeyboardState KeyboardPreviousState { get; set; }
        private static MouseState MouseCurrentState { get; set; }
        private static MouseState MousePreviousState { get; set; }
#endif
#if GAMEPADS
        private static GamePadState[] GamePadCurrentStates { get; set; }
        private static GamePadState[] GamePadPreviouStates { get; set; }
#endif
#if TOUCH
        private static TouchCollection TouchState { get; set; }
#endif

        internal static void Init() {
#if KEYBOARD_MOUSE
            KeyboardCurrentState = Keyboard.GetState();
            MouseCurrentState = Mouse.GetState();
#endif
#if GAMEPADS
            GamePadCurrentStates = new GamePadState[1];
            for (int i = 0; i < GamePadCurrentStates.Length; i++) {
                GamePadCurrentStates[i] = GamePad.GetState(i);
            }
            GamePadPreviouStates = new GamePadState[1];
#endif
        }

        internal static void Update() {
#if KEYBOARD_MOUSE
            KeyboardPreviousState = KeyboardCurrentState;
            KeyboardCurrentState = Keyboard.GetState();
            MousePreviousState = MouseCurrentState;
            MouseCurrentState = Mouse.GetState();
#endif
#if GAMEPADS
            for (int i = 0; i < GamePadCurrentStates.Length; i++) {
                GamePadPreviouStates[i] = GamePadCurrentStates[i];
                GamePadCurrentStates[i] = GamePad.GetState(i);
            }
#endif
#if TOUCH
            TouchState = TouchPanel.GetState();
#endif
        }

#if KEYBOARD_MOUSE
        #region Input-related functions: keyboard
        public static bool IsKeyPressed(Keys key) {
            return KeyboardCurrentState.IsKeyDown(key) && !KeyboardPreviousState.IsKeyDown(key);
        }
        public static bool IsKeyDown(Keys key) {
            return KeyboardCurrentState.IsKeyDown(key);
        }
        public static bool IsKeyReleased(Keys key) {
            return KeyboardCurrentState.IsKeyUp(key) && KeyboardPreviousState.IsKeyDown(key);
        }
        public static bool IsKeyUp(Keys key) {
            return KeyboardCurrentState.IsKeyUp(key);
        }
        #endregion Input-related functions: keyboard

        #region Input-related functions: mouse 
        public static void ShowCursor() {
            Core.Game.IsMouseVisible = true;
        }
        public static void HideCursor() {
            Core.Game.IsMouseVisible = false;
        }
        public static bool IsCursorHidden() {
            return Core.Game.IsMouseVisible;
        }
        public static void SetMouseIcon(MouseCursor mouseCursor) {
            Mouse.SetCursor(mouseCursor);
        }
        public static bool IsMouseButtonPressed(MouseButton button) {
            switch (button) {
                case MouseButton.Left:
                    return MouseCurrentState.LeftButton == ButtonState.Pressed && MousePreviousState.LeftButton != ButtonState.Pressed;
                case MouseButton.Right:
                    return MouseCurrentState.RightButton == ButtonState.Pressed && MousePreviousState.RightButton != ButtonState.Pressed;
                case MouseButton.Middle:
                    return MouseCurrentState.MiddleButton == ButtonState.Pressed && MousePreviousState.MiddleButton != ButtonState.Pressed;
                case MouseButton.XButton1:
                    return MouseCurrentState.XButton1 == ButtonState.Pressed && MousePreviousState.XButton1 != ButtonState.Pressed;
                case MouseButton.XButton2:
                    return MouseCurrentState.XButton2 == ButtonState.Pressed && MousePreviousState.XButton2 != ButtonState.Pressed;
                default:
                    return false;
            }
        }
        public static bool IsMouseButtonDown(MouseButton button) {
            switch (button) {
                case MouseButton.Left:
                    return MouseCurrentState.LeftButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return MouseCurrentState.RightButton == ButtonState.Pressed;
                case MouseButton.Middle:
                    return MouseCurrentState.MiddleButton == ButtonState.Pressed;
                case MouseButton.XButton1:
                    return MouseCurrentState.XButton1 == ButtonState.Pressed;
                case MouseButton.XButton2:
                    return MouseCurrentState.XButton2 == ButtonState.Pressed;
                default:
                    return false;
            }
        }
        public static bool IsMouseButtonReleased(MouseButton button) {
            switch (button) {
                case MouseButton.Left:
                    return MouseCurrentState.LeftButton == ButtonState.Released && MousePreviousState.LeftButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return MouseCurrentState.RightButton == ButtonState.Released && MousePreviousState.RightButton == ButtonState.Pressed;
                case MouseButton.Middle:
                    return MouseCurrentState.MiddleButton == ButtonState.Released && MousePreviousState.MiddleButton == ButtonState.Pressed;
                case MouseButton.XButton1:
                    return MouseCurrentState.XButton1 == ButtonState.Released && MousePreviousState.XButton1 == ButtonState.Pressed;
                case MouseButton.XButton2:
                    return MouseCurrentState.XButton2 == ButtonState.Released && MousePreviousState.XButton2 == ButtonState.Pressed;
                default:
                    return false;
            }
        }
        public static bool IsMouseButtonUp(MouseButton button) {
            switch (button) {
                case MouseButton.Left:
                    return MouseCurrentState.LeftButton == ButtonState.Released;
                case MouseButton.Right:
                    return MouseCurrentState.RightButton == ButtonState.Released;
                case MouseButton.Middle:
                    return MouseCurrentState.MiddleButton == ButtonState.Released;
                case MouseButton.XButton1:
                    return MouseCurrentState.XButton1 == ButtonState.Released;
                case MouseButton.XButton2:
                    return MouseCurrentState.XButton2 == ButtonState.Released;
                default:
                    return false;
            }
        }
        public static int GetMouseX() {
            return MouseCurrentState.X;
        }
        public static int GetMouseY() {
            return MouseCurrentState.Y;
        }
        public static Vector2 GetMousePosition() {
            return MouseCurrentState.Position.ToVector2();
        }
        public static void SetMousePosition(int x, int y) {
            Mouse.SetPosition(x, y);
        }
        public static int GetMouseHorizontalWheelMove() {
            return MouseCurrentState.HorizontalScrollWheelValue - MousePreviousState.HorizontalScrollWheelValue;
        }
        public static int GetMouseHorizontalWheelPosition() {
            return MouseCurrentState.HorizontalScrollWheelValue;
        }
        public static int GetMouseVerticalWheelMove() {
            return MouseCurrentState.ScrollWheelValue - MousePreviousState.ScrollWheelValue;
        }
        public static int GetMouseVerticalWheelPosition() {
            return MouseCurrentState.ScrollWheelValue;
        }
        #endregion Input-related functions: mouse 
#endif
#if GAMEPADS
        #region Input-related functions: gamepads
        public static void SetGamePadsNumber( byte number) {
            GamePadCurrentStates = new GamePadState[number];
            for (int i = 0; i < GamePadCurrentStates.Length; i++) {
                GamePadCurrentStates[i] = GamePad.GetState(i);
            }
            GamePadPreviouStates = new GamePadState[number];
        }
        public static bool IsGamepadAvailable( int gamepad) {
            return GamePad.GetState(gamepad).IsConnected;
        }
        public static bool IsGamepadButtonPressed( int gamepad,  Buttons button) {
            return GamePadCurrentStates[gamepad].IsButtonDown(button) && !GamePadPreviouStates[gamepad].IsButtonDown(button);
        }
        public static bool IsGamepadButtonDown( int gamepad,  Buttons button) {
            return GamePadCurrentStates[gamepad].IsButtonDown(button);
        }
        public static bool IsGamepadButtonReleased( int gamepad,  Buttons button) {
            return GamePadCurrentStates[gamepad].IsButtonUp(button) && GamePadPreviouStates[gamepad].IsButtonDown(button);
        }
        public static bool IsGamepadButtonUp( int gamepad,  Buttons button) {
            return GamePadCurrentStates[gamepad].IsButtonUp(button);
        }
        public static Vector2? GetGamepadThumbStick( int gamepad,  ThumbSticks thumbStick) {
            switch (thumbStick) {
                case ThumbSticks.Left:
                    return GamePadCurrentStates[gamepad].ThumbSticks.Left;
                case ThumbSticks.Right:
                    return GamePadCurrentStates[gamepad].ThumbSticks.Right;
                default:
                    return null;
            }

        }
        public static float? GetGamepadTrigger( int gamepad,  Triggers trigger) {
            switch (trigger) {
                case Triggers.Left:
                    return GamePadCurrentStates[gamepad].Triggers.Left;
                case Triggers.Right:
                    return GamePadCurrentStates[gamepad].Triggers.Right;
                default:
                    return null;
            }

        }
        #endregion Input-related functions: gamepads
#endif
#if TOUCH
        #region Input-related functions: Touch
        public static void SetTouchGestures(GestureType gestures) {
            TouchPanel.EnabledGestures = gestures;
        }
        public static GestureType GetTouchGestures() {
            return TouchPanel.EnabledGestures;
        }
        public static void EnableMouseTouchSimulate() {
            TouchPanel.EnabledGestures = GestureType.FreeDrag;
            TouchPanel.EnableMouseTouchPoint = true;
            TouchPanel.EnableMouseGestures = true;
        }
        public static void DisableMouseTouchSimulate() {
            TouchPanel.EnableMouseTouchPoint = false;
            TouchPanel.EnableMouseGestures = false;
        }
        public static int GetTouchPointsCount() {
            return TouchState.Count;
        }
        public static Vector2? GetTouchPosition(int finger) {
            if (TouchState.Count > finger)
                return TouchState[finger].Position;
            return null;
        }
        public static void ReadGestures(ref GestureSample? gesture) {
            gesture = null;
            while (TouchPanel.IsGestureAvailable) {
                gesture = TouchPanel.ReadGesture();
            }
        }
        #endregion Input-related functions: Touch
#endif


    }
}
