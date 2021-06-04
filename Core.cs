using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#if KEYBOARD_MOUSE || GAMEPADS
using Microsoft.Xna.Framework.Input;
#endif
#if TOUCH
using Microsoft.Xna.Framework.Input.Touch;
#endif
using NQuad.Utils.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace NQuad
{
    public static class Core
    {

        private static WindowConfigFlag windowConfigFlag;
        internal static Game Game;
        private static GraphicsDeviceManager Graphics;
#if KEYBOARD_MOUSE
        private static KeyboardState KeyboardCurrentState, KeyboardPreviousState;
        private static MouseState MouseCurrentState, MousePreviousState;
#endif
#if GAMEPADS
        private static GamePadState[] GamePadCurrentStates, GamePadPreviouStates;
#endif
#if TOUCH
        private static TouchCollection TouchState;
#endif
        private static DateTime time;
        private static GameTime timeGame;

        private static Random random;

        public static void InitCore(in Game game, in GraphicsDeviceManager manager, in string title, in int width, in int height, in WindowConfigFlag windowConfigFlag, in string contentLocationFolder = "Content") {
            Game = game;
            Game.Content.RootDirectory = contentLocationFolder;
            Graphics = manager;
            
            Render.InitRender();
            Game.Window.Title = title;

            Game.IsMouseVisible = true;
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
            time = DateTime.Now;

            random = new Random();

            if ((windowConfigFlag & WindowConfigFlag.VSYNC) > 0) Graphics.SynchronizeWithVerticalRetrace = true;
            else Graphics.SynchronizeWithVerticalRetrace = false;

            if ((windowConfigFlag & WindowConfigFlag.FULLSCREEN_MODE) > 0) Graphics.IsFullScreen = true;

            if ((windowConfigFlag & WindowConfigFlag.WINDOW_RESIZABLE) > 0) Game.Window.AllowUserResizing = true;
            else Game.Window.AllowUserResizing = false;

            if ((windowConfigFlag & WindowConfigFlag.WINDOW_UNDECORATED) > 0) Game.Window.IsBorderless = true;

            if ((windowConfigFlag & WindowConfigFlag.FIXED_TIME_STEP) > 0) Game.IsFixedTimeStep = true;
            else Game.IsFixedTimeStep = false;

            if ((windowConfigFlag & WindowConfigFlag.MSAA_4X_HINT) > 0) Graphics.PreferMultiSampling = true;
            else Graphics.PreferMultiSampling = false;

            if ((windowConfigFlag & WindowConfigFlag.ALLOW_ALT_F4) > 0) Game.Window.AllowAltF4 = true;
            else Game.Window.AllowAltF4 = false;

            Graphics.PreferredBackBufferWidth = width;
            Graphics.PreferredBackBufferHeight = height;
            Graphics.ApplyChanges();

        }
        public static void Update(in GameTime gameTime) {
            timeGame = gameTime;
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

        #region Window-related functions
        public static void CloseWindow() {
            Game.Exit();
        }
        public static bool IsWindowFocused() {
            return Game.IsActive;
        }
        public static bool IsWindowState(in WindowConfigFlag flag) {
            return (windowConfigFlag & flag) > 0;
        }
        public static void SetWindowState(in WindowConfigFlag flags) {
            if (((windowConfigFlag & WindowConfigFlag.VSYNC) != (flags & WindowConfigFlag.VSYNC)) && ((flags & WindowConfigFlag.VSYNC) > 0)) {
                Graphics.SynchronizeWithVerticalRetrace = true;
                windowConfigFlag |= WindowConfigFlag.VSYNC;
            }

            if ((windowConfigFlag & WindowConfigFlag.FULLSCREEN_MODE) != (flags & WindowConfigFlag.FULLSCREEN_MODE)) {
                Graphics.IsFullScreen = true;
                windowConfigFlag |= WindowConfigFlag.FULLSCREEN_MODE;
                if ((windowConfigFlag & WindowConfigFlag.VSYNC) == WindowConfigFlag.VSYNC) {
                    Graphics.SynchronizeWithVerticalRetrace = true;
                }
            }
            if (((windowConfigFlag & WindowConfigFlag.WINDOW_RESIZABLE) != (flags & WindowConfigFlag.WINDOW_RESIZABLE)) && ((flags & WindowConfigFlag.WINDOW_RESIZABLE) > 0)) {
                Game.Window.AllowUserResizing = true;
                windowConfigFlag |= WindowConfigFlag.WINDOW_RESIZABLE;
            }

            if (((windowConfigFlag & WindowConfigFlag.WINDOW_UNDECORATED) != (flags & WindowConfigFlag.WINDOW_UNDECORATED)) && ((flags & WindowConfigFlag.WINDOW_UNDECORATED) > 0)) {
                Game.Window.IsBorderless = true;
                windowConfigFlag |= WindowConfigFlag.WINDOW_UNDECORATED;
            }

            if ((windowConfigFlag & WindowConfigFlag.MSAA_4X_HINT) != (flags & WindowConfigFlag.MSAA_4X_HINT) && (flags & WindowConfigFlag.MSAA_4X_HINT) > 0) {
                Graphics.PreferMultiSampling = true;
                windowConfigFlag |= WindowConfigFlag.MSAA_4X_HINT;
            }

            if ((windowConfigFlag & WindowConfigFlag.FIXED_TIME_STEP) != (flags & WindowConfigFlag.FIXED_TIME_STEP) && (flags & WindowConfigFlag.FIXED_TIME_STEP) > 0) {
                Game.IsFixedTimeStep = true;
                windowConfigFlag |= WindowConfigFlag.FIXED_TIME_STEP;
            }

            if ((windowConfigFlag & WindowConfigFlag.ALLOW_ALT_F4) != (flags & WindowConfigFlag.ALLOW_ALT_F4) && (flags & WindowConfigFlag.ALLOW_ALT_F4) > 0) {
                Game.Window.AllowAltF4 = true;
                windowConfigFlag |= WindowConfigFlag.ALLOW_ALT_F4;
            }

            if ((windowConfigFlag & WindowConfigFlag.ALLOW_ALT_F4) != (flags & WindowConfigFlag.ALLOW_ALT_F4) && (flags & WindowConfigFlag.ALLOW_ALT_F4) > 0) {
                Game.Window.AllowAltF4 = true;
                windowConfigFlag |= WindowConfigFlag.ALLOW_ALT_F4;
            }

            Graphics.ApplyChanges();

        }
        public static void ClearWindowState(in WindowConfigFlag flags) {
            if (((windowConfigFlag & WindowConfigFlag.VSYNC) > 0) && ((flags & WindowConfigFlag.VSYNC) > 0)) {
                Graphics.SynchronizeWithVerticalRetrace = false;
                windowConfigFlag &= ~WindowConfigFlag.VSYNC;
            }

            if (((windowConfigFlag & WindowConfigFlag.FULLSCREEN_MODE) > 0) && ((flags & WindowConfigFlag.FULLSCREEN_MODE) > 0)) {
                Graphics.IsFullScreen = false;
                windowConfigFlag &= ~WindowConfigFlag.FULLSCREEN_MODE;
                if ((windowConfigFlag & WindowConfigFlag.VSYNC) == WindowConfigFlag.VSYNC) {
                    Graphics.SynchronizeWithVerticalRetrace = true;
                }
            }

            if (((windowConfigFlag & WindowConfigFlag.WINDOW_RESIZABLE) > 0) && ((flags & WindowConfigFlag.WINDOW_RESIZABLE) > 0)) {
                Game.Window.AllowUserResizing = false;
                windowConfigFlag &= ~WindowConfigFlag.WINDOW_RESIZABLE;
            }

            if (((windowConfigFlag & WindowConfigFlag.WINDOW_UNDECORATED) > 0) && ((flags & WindowConfigFlag.WINDOW_UNDECORATED) > 0)) {
                Game.Window.IsBorderless = false;
                windowConfigFlag &= ~WindowConfigFlag.WINDOW_UNDECORATED;
            }

            if (((windowConfigFlag & WindowConfigFlag.MSAA_4X_HINT) > 0) && ((flags & WindowConfigFlag.MSAA_4X_HINT) > 0)) {
                Graphics.PreferMultiSampling = false;
                windowConfigFlag &= ~WindowConfigFlag.MSAA_4X_HINT;
            }

            if (((windowConfigFlag & WindowConfigFlag.FIXED_TIME_STEP) > 0) && ((flags & WindowConfigFlag.FIXED_TIME_STEP) > 0)) {
                Game.IsFixedTimeStep = false;
                windowConfigFlag &= ~WindowConfigFlag.FIXED_TIME_STEP;
            }

            if (((windowConfigFlag & WindowConfigFlag.ALLOW_ALT_F4) > 0) && ((flags & WindowConfigFlag.ALLOW_ALT_F4) > 0)) {
                Game.Window.AllowAltF4 = false;
                windowConfigFlag &= ~WindowConfigFlag.ALLOW_ALT_F4;
            }
        }
        public static void SetWindowTitle(in string title) {
            Game.Window.Title = title;
        }
        public static void SetWindowPosition(in int x, in int y) {
#if DESKTOP && !NETFX_CORE && !WINDOWS_UAP
            Game.Window.Position = new Point(x, y);
#else
            Println(LOG.WARNING, "This function is not available on this platform, enter the preprocessor directive DESKTOP if you use Desktop, not UWP.");
#endif
        }
        public static void SetTextInputEvent(in EventHandler<TextInputEventArgs> eventHandler) {
#if DESKTOP
            Game.Window.TextInput += eventHandler;
#else
            Println(LOG.WARNING, "This function is not available on this platform, enter the preprocessor directive DESKTOP if you use Desktop, not UWP.");
#endif
        }
        public static void SetWindowSize(in int width, in int height) {
            Graphics.PreferredBackBufferWidth = width;
            Graphics.PreferredBackBufferHeight = height;
            Graphics.ApplyChanges();
        }
        public static void IsWindowResize(in EventHandler<EventArgs> eventWindowResize) {
            Game.Window.ClientSizeChanged += eventWindowResize;
        }
        public static void IsWindowChangeOrientation(in EventHandler<EventArgs> eventWindowChangeOrientation) {
            Game.Window.OrientationChanged += eventWindowChangeOrientation;
        }
        public static DisplayOrientation GetCurrentWindowOrientation() {
            return Game.Window.CurrentOrientation;
        }
        public static int GetWindowWidth() {
            return Game.Window.ClientBounds.Width;
        }
        public static int GetWindowHeight() {
            return Game.Window.ClientBounds.Height;
        }
        public static int GetMonitorCount() {
            return GraphicsAdapter.Adapters.Count;
        }
        public static int GetMonitorWidth(in int monitor) {
            return GraphicsAdapter.Adapters[monitor].CurrentDisplayMode.Width;
        }
        public static int GetMonitorHeight(in int monitor) {
            return GraphicsAdapter.Adapters[monitor].CurrentDisplayMode.Height;
        }
        public static string GetMonitorCurrentGameName() {
            return Game.Window.ScreenDeviceName;
        }
        public static string GetMonitorDescription(in int monitor) {
            return GraphicsAdapter.Adapters[monitor].Description;
        }
        public static bool IsMonitorWideScreen(in int monitor) {
            return GraphicsAdapter.Adapters[monitor].IsWideScreen;
        }
        public static Point GetWindowPosition() {
            return Game.Window.ClientBounds.Location;
        }
        public static IntPtr GetWindowHandle() {
            return Game.Window.Handle;
        }
#endregion Window-related functions

#region Timing-related functions
        public static void SetTargetFPS(in double FPS) {
            Game.TargetElapsedTime = TimeSpan.FromSeconds(1d / FPS);
        }
        public static double GetFPS() {
            return 1d / timeGame.ElapsedGameTime.TotalSeconds;
        }
        public static double GetFrameTime() {
            return timeGame.ElapsedGameTime.TotalSeconds;
        }
        public static TimeSpan GetTime() {
            return (DateTime.Now - time);
        }
#endregion Timing-related functions

#region Misc. functions
        public static void Print(in object message) {
            Console.Write(message);
        }
        public static void Print(in LOG type, in object message) {
            switch (type) {
                case LOG.DEBUG:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[DEBUG]: {message}");
                    break;
                case LOG.INFO:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"[INFO]: {message}");
                    break;
                case LOG.WARNING:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[WARNING]: {message}");
                    break;
                case LOG.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[ERROR]: {message}");
                    break;
            }
            Console.ResetColor();
        }
        public static void Println(in object message) {
            Console.WriteLine(message);
        }
        public static void Println(in LOG type, in object message) {
            switch (type) {
                case LOG.DEBUG:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[DEBUG]: {message}");
                    break;
                case LOG.INFO:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"[INFO]: {message}");
                    break;
                case LOG.WARNING:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[WARNING]: {message}");
                    break;
                case LOG.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[ERROR]: {message}");
                    break;
            }
            Console.ResetColor();
        }

        public static int GetRandomValue(in int min, in int max) {
            return random.Next(min, max);
        }
        public static int GetRandomValue(in Random r, in int min, in int max) {
            return r.Next(min, max + 1);
        }
        public static float GetRandomValue(in float maximum, in float minimum) {
            return (float)(random.NextDouble() * (maximum - minimum) + minimum);
        }
        public static float GetRandomValue(in Random r, in float maximum, in float minimum) {
            return (float)(r.NextDouble() * (maximum - minimum) + minimum);
        }

        public static void TakeScreenShot(in string filename, in string extension) {
            int width = GetWindowWidth();
            int height = GetWindowHeight();
            Color[] data = new Color[GetWindowWidth() * GetWindowHeight()];
            Graphics.GraphicsDevice.GetBackBufferData(data);
            Texture2D texture = new Texture2D(Graphics.GraphicsDevice, GetWindowWidth(), GetWindowHeight());
            texture.SetData(data);
            using (Stream stream = new FileStream($"{filename}.{extension}", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite)) {
                switch (extension) {
                    case "png":
                        texture.SaveAsPng(stream, texture.Width, texture.Height);
                        Println(LOG.DEBUG, "The screenshot was saved");
                        break;
                    case "jpeg":
                        texture.SaveAsJpeg(stream, texture.Width, texture.Height);
                        Println(LOG.DEBUG, "The screenshot was saved");
                        break;
                    default:
                        Println(LOG.WARNING, "Only png and jpeg are saved");
                        break;
                }
            }
            texture.Dispose();
            texture = null;
            data = null;
        }
        public static void SaveTexture2D(in Color[] data, in int width, in int height, in string filename, in string extension) {
            Texture2D texture = new Texture2D(Graphics.GraphicsDevice, width, height);
            texture.SetData(data);
            Stream dest = new FileStream($"{filename}.{extension}", FileMode.Create, FileAccess.Write, FileShare.None);
            switch (extension) {
                case "png":
                    texture.SaveAsPng(dest, width, height);
                    Println(LOG.DEBUG, "The screenshot was saved");
                    break;
                case "jpg":
                    texture.SaveAsJpeg(dest, width, height);
                    Println(LOG.DEBUG, "The screenshot was saved");
                    break;
                default:
                    Println(LOG.WARNING, "Only png and jpeg are saved");
                    break;
            }
            dest.Dispose();
            dest = null;
            texture.Dispose();
            texture = null;
        }

        public static Vector2 MeasureText(in string text, int fontSize) {
            return Render.GetDefaultFont().MeasureString(text) * fontSize;
        }
        public static Vector2 MeasureTextEx(in SpriteFont font, in string text, in float fontSize) {
            return font.MeasureString(text) * fontSize;
        }

        public static void OpenURL(in string url) {
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = url;
            System.Diagnostics.Process.Start(psi);
        }

        public static Texture2D CreateTexture<T>(in int width, in int height, in T[] Data) where T : unmanaged {
            Texture2D tx = new Texture2D(Game.GraphicsDevice, width, height);
            tx.SetData(Data);
            return tx;
        }
        public static RenderTarget2D CreateRenderTarget2D(in int width, in int height, in bool mipMaps = false, in SurfaceFormat format = SurfaceFormat.Color, in DepthFormat depthFormat = DepthFormat.None) {
            return new RenderTarget2D(Game.GraphicsDevice, width, height, mipMaps, format, depthFormat);
        }

        /// <summary>
        /// When assets are loaded it is advisable to clean the memory
        /// </summary>
        public static void GarbageClearMemory() {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
#endregion Misc. functions

#region  Files management functions
        public static T ContentLoad<T>(in string filename) {
            return Game.Content.Load<T>(filename);
        }
        public static SpriteFont LoadFontFromImage(in string filename) {
            Texture2D texture = ContentLoad<Texture2D>(filename);
            Color[] pixels = new Color[texture.Width * texture.Height];
            texture.GetData(pixels);

            const int MAX_GLYPHS_FROM_IMAGE = 256;
            int charSpacing = 0;
            int lineSpacing = 0;

            int x = 0;
            int y = 0;

            Color key = Color.Magenta;
            const int firstChar = 32;

            int[] tempCharValues = new int[MAX_GLYPHS_FROM_IMAGE];
            Rectangle[] tempCharRecs = new Rectangle[MAX_GLYPHS_FROM_IMAGE];

            // Parse image data to get charSpacing and lineSpacing
            for (y = 0; y < texture.Height; y++) {
                for (x = 0; x < texture.Width; x++) {
                    if (!(pixels[y * texture.Width + x] == key)) break;
                }

                if (!(pixels[y * texture.Width + x] == key)) break;
            }

            charSpacing = x;
            lineSpacing = y;

            int charHeight = 0;
            int j = 0;

            while (!(pixels[(lineSpacing + j) * texture.Width + charSpacing] == key)) j++;

            charHeight = j;

            // Check array values to get characters: value, x, y, w, h
            int index = 0;
            int lineToRead = 0;
            int xPosToRead = charSpacing;

            while ((lineSpacing + lineToRead * (charHeight + lineSpacing)) < texture.Height) {

                while ((xPosToRead < texture.Width) &&
                    !((pixels[(lineSpacing + (charHeight + lineSpacing) * lineToRead) * texture.Width + xPosToRead]) == key)) {
                    tempCharValues[index] = firstChar + index;

                    tempCharRecs[index].X = xPosToRead;
                    tempCharRecs[index].Y = (lineSpacing + lineToRead * (charHeight + lineSpacing));
                    tempCharRecs[index].Height = charHeight;

                    int charWidth = 0;

                    while (!(pixels[(lineSpacing + (charHeight + lineSpacing) * lineToRead) * texture.Width + xPosToRead + charWidth] == key)) charWidth++;

                    tempCharRecs[index].Width = charWidth;

                    index++;

                    xPosToRead += (charWidth + charSpacing);
                }
                lineToRead++;
                xPosToRead = charSpacing;
            }

            for (int i = 0; i < texture.Height * texture.Width; i++) if ((pixels[i] == key)) pixels[i] = Color.Transparent;

            texture.SetData(pixels);

            List<Rectangle> glyphs = new List<Rectangle>(), cropping = new List<Rectangle>();
            List<char> characters = new List<char>();
            List<Vector3> kerning = new List<Vector3>();

            for (int i = 0; i < index; i++) {
                characters.Add(char.ConvertFromUtf32(i + 32)[0]);

                // Get character rectangle in the font atlas texture
                glyphs.Add(tempCharRecs[i]);

                // NOTE: On image based fonts (XNA style), character offsets and xAdvance are not required (set to 0)
                cropping.Add(Rectangle.Empty);
                Vector3 ker = new Vector3(0, glyphs[i].Width, 1);
                kerning.Add(ker);
            }
            return new SpriteFont(texture, glyphs, cropping, characters, charHeight + lineSpacing, 0, kerning, '?');
        }

        public static object LoadFileData(in string fileName) {
            byte[] bytes = File.ReadAllBytes(fileName);
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(bytes, 0, bytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            object output = binForm.Deserialize(memStream);
            memStream.Dispose();
            return output;
        }
        public static void SaveFileData(in string fileName, in object data) {
            if (data != null) {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, data);
                byte[] bytes = ms.ToArray();
                File.WriteAllBytes(fileName, bytes);
                ms.Dispose();
            }
        }
        public static string LoadFileText(in string filename) {
            return File.ReadAllText(filename);
        }
        public static void SaveFileText(in string fileName, in string text) {
            if (File.Exists(fileName)) {
                File.AppendAllText(fileName, text);
            } else {
                File.WriteAllText(fileName, text);
            }

        }
        public static bool FileExists(in string fileName) {
            return File.Exists(fileName);
        }
        public static bool IsFileExtension(in string fileName, in string ext) {
            if (File.Exists(fileName + ext))
                return true;
            return false;
        }
        public static bool DirectoryExists(in string dirPath) {
            return Directory.Exists(dirPath);
        }
        public static string GetFileExtension(in string fileName) {
            return Path.GetExtension(fileName);
        }
        public static string GetFileName(in string filePath) {
            return Path.GetFileName(filePath);
        }
        public static string GetFileNameWithoutExt(in string filePath) {
            return Path.GetFileNameWithoutExtension(filePath);
        }
        public static string GetDirectoryPath(in string filePath) {
            return Path.GetDirectoryName(filePath);
        }
        public static string GetPrevDirectoryPath(in string dirPath) {
            return Directory.GetParent(dirPath).FullName;
        }
        public static string GetWorkingDirectory() {
            return Directory.GetCurrentDirectory();
        }
        public static string[] GetDirectoryFiles(in string dirPath) {
            return Directory.GetFiles(dirPath);
        }
        public static void ChangeDirectory(in string dir) {
            Directory.SetCurrentDirectory(dir);
        }
        public static DateTime GetFileModTime(in string fileName) {
            return File.GetLastWriteTime(fileName);
        }
        public static byte[] CompressData(in string data) {
            return Encoding.UTF8.GetBytes(data);
        }
        public static string DecompressData(in byte[] data) {
            return Encoding.UTF8.GetString(data);
        }

        public static object LoadStorageValue(in int position) {

            if (FileExists(GetWorkingDirectory() + "/storage.data")) {
                byte[] bytes = File.ReadAllBytes(GetWorkingDirectory() + "/storage.data");
                MemoryStream memStream = new MemoryStream();
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(bytes, 0, bytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                Dictionary<int, object> data = (Dictionary<int, object>)binForm.Deserialize(memStream);
                memStream.Dispose();
                return data[position];
            }
            return null;
        }
        public static Dictionary<int, object> LoadAllStorageValues() {
            byte[] bytes = File.ReadAllBytes(GetWorkingDirectory() + "/storage.data");
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(bytes, 0, bytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Dictionary<int, object> data = (Dictionary<int, object>)binForm.Deserialize(memStream);
            memStream.Dispose();
            return data;
        }
        public static void SaveStorageValue(in int position, in object value) {
            if (value != null) {
                Dictionary<int, object> data;
                if (FileExists(GetWorkingDirectory() + "/storage.data")) {
                    data = LoadAllStorageValues();
                } else {
                    data = new Dictionary<int, object>();
                }
                if (data.ContainsKey(position)) {
                    data[position] = value;
                } else {
                    data.Add(position, value);
                }
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, data);
                byte[] bytes = ms.ToArray();

                File.WriteAllBytes(GetWorkingDirectory() + "/storage.data", bytes);
                ms.Dispose();
            }
        }
#endregion  Files management functions

#if KEYBOARD_MOUSE
#region Input-related functions: keyboard
        public static bool IsKeyPressed(in Keys key) {
            return KeyboardCurrentState.IsKeyDown(key) && !KeyboardPreviousState.IsKeyDown(key);
        }
        public static bool IsKeyDown(in Keys key) {
            return KeyboardCurrentState.IsKeyDown(key);
        }
        public static bool IsKeyReleased(in Keys key) {
            return KeyboardCurrentState.IsKeyUp(key) && KeyboardPreviousState.IsKeyDown(key);
        }
        public static bool IsKeyUp(in Keys key) {
            return KeyboardCurrentState.IsKeyUp(key);
        }
#endregion Input-related functions: keyboard

#region Input-related functions: mouse 
        public static void ShowCursor() {
            Game.IsMouseVisible = true;
        }
        public static void HideCursor() {
            Game.IsMouseVisible = false;
        }
        public static bool IsCursorHidden() {
            return Game.IsMouseVisible;
        }
        public static void SetMouseIcon(in MouseCursor mouseCursor) {
            Mouse.SetCursor(mouseCursor);
        }
        public static bool IsMouseButtonPressed(in MouseButton button) {
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
        public static bool IsMouseButtonDown(in MouseButton button) {
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
        public static bool IsMouseButtonReleased(in MouseButton button) {
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
        public static bool IsMouseButtonUp(in MouseButton button) {
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
        public static void SetMousePosition(in int x, in int y) {
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
        public static void SetGamePadsNumber(in byte number) {
            GamePadCurrentStates = new GamePadState[number];
            for (int i = 0; i < GamePadCurrentStates.Length; i++) {
                GamePadCurrentStates[i] = GamePad.GetState(i);
            }
            GamePadPreviouStates = new GamePadState[number];
        }
        public static bool IsGamepadAvailable(in int gamepad) {
            return GamePad.GetState(gamepad).IsConnected;
        }
        public static bool IsGamepadButtonPressed(in int gamepad, in Buttons button) {
            return GamePadCurrentStates[gamepad].IsButtonDown(button) && !GamePadPreviouStates[gamepad].IsButtonDown(button);
        }
        public static bool IsGamepadButtonDown(in int gamepad, in Buttons button) {
            return GamePadCurrentStates[gamepad].IsButtonDown(button);
        }
        public static bool IsGamepadButtonReleased(in int gamepad, in Buttons button) {
            return GamePadCurrentStates[gamepad].IsButtonUp(button) && GamePadPreviouStates[gamepad].IsButtonDown(button);
        }
        public static bool IsGamepadButtonUp(in int gamepad, in Buttons button) {
            return GamePadCurrentStates[gamepad].IsButtonUp(button);
        }
        public static Vector2? GetGamepadThumbStick(in int gamepad, in ThumbSticks thumbStick) {
            switch (thumbStick) {
                case ThumbSticks.Left:
                    return GamePadCurrentStates[gamepad].ThumbSticks.Left;
                case ThumbSticks.Right:
                    return GamePadCurrentStates[gamepad].ThumbSticks.Right;
                default:
                    return null;
            }

        }
        public static float? GetGamepadTrigger(in int gamepad, in Triggers trigger) {
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
        public static void SetTouchGestures(in GestureType gestures) {
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
        public static Vector2? GetTouchPosition(in int finger) {
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
