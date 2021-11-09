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

namespace NQuad {
    public static class Core {

        public static Game Game { get; private set; }
        public static GraphicsDeviceManager Graphics { get; private set; }
        public static GameTime GameTime { get; private set; }
        public static WindowConfigFlag ConfigFlag { get; private set; }

        private static Random random { get; set; }

        public static void InitCore(Game game, string title, int width, int height, WindowConfigFlag flags, string contentLocationFolder = "Content", int msaaCount = 2) {
            Game = game;
            Graphics = new GraphicsDeviceManager(Game);
            Game.Content.RootDirectory = contentLocationFolder;

            Graphics.DeviceCreated += (e, p) => {
                Render.InitRender();
                Input.Init();

                if ((flags & WindowConfigFlag.VSYNC) > 0) Graphics.SynchronizeWithVerticalRetrace = true;
                else Graphics.SynchronizeWithVerticalRetrace = false;

                if ((flags & WindowConfigFlag.FULLSCREEN_MODE) > 0) Graphics.IsFullScreen = true;

                if ((flags & WindowConfigFlag.WINDOW_RESIZABLE) > 0) Game.Window.AllowUserResizing = true;
                else Game.Window.AllowUserResizing = false;

                if ((flags & WindowConfigFlag.WINDOW_UNDECORATED) > 0) Game.Window.IsBorderless = true;

                if ((flags & WindowConfigFlag.FIXED_TIME_STEP) > 0) Game.IsFixedTimeStep = true;
                else Game.IsFixedTimeStep = false;

                if ((flags & WindowConfigFlag.MSAA) > 0) {
                    Graphics.PreferMultiSampling = true;
                    Graphics.GraphicsDevice.PresentationParameters.MultiSampleCount = msaaCount;
                } else Graphics.PreferMultiSampling = false;

                if ((flags & WindowConfigFlag.ALLOW_ALT_F4) > 0) Game.Window.AllowAltF4 = true;
                else Game.Window.AllowAltF4 = false;

                ConfigFlag = flags;

                Graphics.PreferredBackBufferWidth = width;
                Graphics.PreferredBackBufferHeight = height;
                Graphics.ApplyChanges();
            };

            Game.Window.Title = title;
            Game.IsMouseVisible = true;

            random = new Random();
        }
        public static void Update(GameTime gameTime) {
            GameTime = gameTime;
            Input.Update();
        }

        #region Window-related functions
        public static bool IsWindowState(WindowConfigFlag flag) {
            return (ConfigFlag & flag) > 0;
        }
        public static void SetWindowState(WindowConfigFlag flags) {
            if (((ConfigFlag & WindowConfigFlag.VSYNC) != (flags & WindowConfigFlag.VSYNC)) && ((flags & WindowConfigFlag.VSYNC) > 0)) {
                Graphics.SynchronizeWithVerticalRetrace = true;
                ConfigFlag |= WindowConfigFlag.VSYNC;
            }

            if ((ConfigFlag & WindowConfigFlag.FULLSCREEN_MODE) != (flags & WindowConfigFlag.FULLSCREEN_MODE)) {
                Graphics.IsFullScreen = true;
                ConfigFlag |= WindowConfigFlag.FULLSCREEN_MODE;
                if ((ConfigFlag & WindowConfigFlag.VSYNC) == WindowConfigFlag.VSYNC) {
                    Graphics.SynchronizeWithVerticalRetrace = true;
                }
            }
            if (((ConfigFlag & WindowConfigFlag.WINDOW_RESIZABLE) != (flags & WindowConfigFlag.WINDOW_RESIZABLE)) && ((flags & WindowConfigFlag.WINDOW_RESIZABLE) > 0)) {
                Game.Window.AllowUserResizing = true;
                ConfigFlag |= WindowConfigFlag.WINDOW_RESIZABLE;
            }

            if (((ConfigFlag & WindowConfigFlag.WINDOW_UNDECORATED) != (flags & WindowConfigFlag.WINDOW_UNDECORATED)) && ((flags & WindowConfigFlag.WINDOW_UNDECORATED) > 0)) {
                Game.Window.IsBorderless = true;
                ConfigFlag |= WindowConfigFlag.WINDOW_UNDECORATED;
            }

            if ((ConfigFlag & WindowConfigFlag.MSAA) != (flags & WindowConfigFlag.MSAA) && (flags & WindowConfigFlag.MSAA) > 0) {
                Graphics.PreferMultiSampling = true;
                ConfigFlag |= WindowConfigFlag.MSAA;
            }

            if ((ConfigFlag & WindowConfigFlag.FIXED_TIME_STEP) != (flags & WindowConfigFlag.FIXED_TIME_STEP) && (flags & WindowConfigFlag.FIXED_TIME_STEP) > 0) {
                Game.IsFixedTimeStep = true;
                ConfigFlag |= WindowConfigFlag.FIXED_TIME_STEP;
            }

            if ((ConfigFlag & WindowConfigFlag.ALLOW_ALT_F4) != (flags & WindowConfigFlag.ALLOW_ALT_F4) && (flags & WindowConfigFlag.ALLOW_ALT_F4) > 0) {
                Game.Window.AllowAltF4 = true;
                ConfigFlag |= WindowConfigFlag.ALLOW_ALT_F4;
            }

            Graphics.ApplyChanges();

        }
        public static void ClearWindowState(WindowConfigFlag flags) {
            if (((ConfigFlag & WindowConfigFlag.VSYNC) > 0) && ((flags & WindowConfigFlag.VSYNC) > 0)) {
                Graphics.SynchronizeWithVerticalRetrace = false;
                ConfigFlag &= ~WindowConfigFlag.VSYNC;
            }

            if (((ConfigFlag & WindowConfigFlag.FULLSCREEN_MODE) > 0) && ((flags & WindowConfigFlag.FULLSCREEN_MODE) > 0)) {
                Graphics.IsFullScreen = false;
                ConfigFlag &= ~WindowConfigFlag.FULLSCREEN_MODE;
                if ((ConfigFlag & WindowConfigFlag.VSYNC) == WindowConfigFlag.VSYNC) {
                    Graphics.SynchronizeWithVerticalRetrace = true;
                }
            }

            if (((ConfigFlag & WindowConfigFlag.WINDOW_RESIZABLE) > 0) && ((flags & WindowConfigFlag.WINDOW_RESIZABLE) > 0)) {
                Game.Window.AllowUserResizing = false;
                ConfigFlag &= ~WindowConfigFlag.WINDOW_RESIZABLE;
            }

            if (((ConfigFlag & WindowConfigFlag.WINDOW_UNDECORATED) > 0) && ((flags & WindowConfigFlag.WINDOW_UNDECORATED) > 0)) {
                Game.Window.IsBorderless = false;
                ConfigFlag &= ~WindowConfigFlag.WINDOW_UNDECORATED;
            }

            if (((ConfigFlag & WindowConfigFlag.MSAA) > 0) && ((flags & WindowConfigFlag.MSAA) > 0)) {
                Graphics.PreferMultiSampling = false;
                ConfigFlag &= ~WindowConfigFlag.MSAA;
            }

            if (((ConfigFlag & WindowConfigFlag.FIXED_TIME_STEP) > 0) && ((flags & WindowConfigFlag.FIXED_TIME_STEP) > 0)) {
                Game.IsFixedTimeStep = false;
                ConfigFlag &= ~WindowConfigFlag.FIXED_TIME_STEP;
            }

            if (((ConfigFlag & WindowConfigFlag.ALLOW_ALT_F4) > 0) && ((flags & WindowConfigFlag.ALLOW_ALT_F4) > 0)) {
                Game.Window.AllowAltF4 = false;
                ConfigFlag &= ~WindowConfigFlag.ALLOW_ALT_F4;
            }

            Graphics.ApplyChanges();
        }
        public static string WindowTitle {
            get => Game.Window.Title;
            set => Game.Window.Title = value;
        }
        public static Point WindowSize {
            get => Game.Window.ClientBounds.Size;
            set {
                Graphics.PreferredBackBufferWidth = value.X;
                Graphics.PreferredBackBufferHeight = value.Y;
                Graphics.ApplyChanges();
            }
        }
        public static Point WindowPosition {
            get => Game.Window.Position;
            set => Game.Window.Position = value;
        }
        public static DisplayOrientation GetCurrentWindowOrientation => Game.Window.CurrentOrientation;

        #endregion Window-related functions

        #region Monitor-related functions

        public static int MonitorCount => GraphicsAdapter.Adapters.Count;
        public static GraphicsAdapter CurrentMonitor => GraphicsAdapter.DefaultAdapter;
        public static GraphicsAdapter GetMonitor(int monitor) => GraphicsAdapter.Adapters[monitor];

        #endregion Monitor-related functions

        #region Timing-related functions

        public static TimeSpan GetTargetFPS => Game.TargetElapsedTime;
        public static void SetTargetFPS(double FPS) => Game.TargetElapsedTime = TimeSpan.FromSeconds(1d / FPS);
        public static double GetFPS => 1d / GameTime.ElapsedGameTime.TotalSeconds;
        public static double GetFrameTime => GameTime.ElapsedGameTime.TotalSeconds;
        public static TimeSpan GetTimePlaying => GameTime.TotalGameTime;

        #endregion Timing-related functions

        #region Misc. functions
        public static void Print(object message, LOG type = LOG.NONE) {
            
            switch (type) {
                case LOG.NONE:
                    Console.ResetColor();
                    Console.Write($"{message}");
                    break;
                case LOG.DEBUG:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"[DEBUG]: {message}");
                    break;
                case LOG.INFO:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"[INFO]: {message}");
                    break;
                case LOG.WARNING:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"[WARNING]: {message}");
                    break;
                case LOG.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"[ERROR]: {message}");
                    break;
            }
        }
        public static void Println(object message, LOG type = LOG.NONE) {
            switch (type) {
                case LOG.NONE:
                    Console.ResetColor();
                    Console.WriteLine($"{message}");
                    break;
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
        }

        public static int GetRandomValue(int min, int max) {
            return random.Next(min, max);
        }
        public static int GetRandomValue(Random r, int min, int max) {
            return r.Next(min, max + 1);
        }
        public static float GetRandomValue(float maximum, float minimum) {
            return (float)(random.NextDouble() * (maximum - minimum) + minimum);
        }
        public static float GetRandomValue(Random r, float maximum, float minimum) {
            return (float)(r.NextDouble() * (maximum - minimum) + minimum);
        }

        public static Vector2 MeasureText(string text, int fontSize) => Render.DefaultFont.MeasureString(text) * fontSize;
        public static Vector2 MeasureText(SpriteFont font, string text, float fontSize) => font.MeasureString(text) * fontSize;

        public static void OpenURL(string url) {
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = url;
            System.Diagnostics.Process.Start(psi);
        }

        #endregion Misc. functions

        #region  Files management functions

        public static void TakeScreenShot(string filename, string extension) {
            using(Texture2D texture = new Texture2D(Graphics.GraphicsDevice, WindowSize.X, WindowSize.Y)) {
                Color[] data = new Color[WindowSize.X * WindowSize.Y];
                Graphics.GraphicsDevice.GetBackBufferData(data);
                texture.SetData(data);
                using (Stream stream = new FileStream($"{filename}.{extension}", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite)) {
                    switch (extension) {
                        case "png":
                            texture.SaveAsPng(stream, texture.Width, texture.Height);
                            Println("The screenshot was saved", LOG.DEBUG);
                            break;
                        case "jpeg":
                            texture.SaveAsJpeg(stream, texture.Width, texture.Height);
                            Println("The screenshot was saved", LOG.DEBUG);
                            break;
                        default:
                            Println("Only png and jpeg are saved", LOG.WARNING);
                            break;
                    }
                }
            }
        }
        public static void SaveTexture2D(Color[] data, int width, int height, string filename, string extension) {
            using (Texture2D texture = new Texture2D(Graphics.GraphicsDevice, width, height)) {
                texture.SetData(data);
                using (Stream dest = new FileStream($"{filename}.{extension}", FileMode.Create, FileAccess.Write, FileShare.None)) {
                    switch (extension) {
                        case "png":
                            texture.SaveAsPng(dest, width, height);
                            Println("The screenshot was saved", LOG.DEBUG);
                            break;
                        case "jpg":
                            texture.SaveAsJpeg(dest, width, height);
                            Println("The screenshot was saved", LOG.DEBUG);
                            break;
                        default:
                            Println("Only png and jpeg are saved", LOG.WARNING);
                            break;
                    }
                }
            }                
        }

        public static SpriteFont LoadFontFromImage(string filename) {
            Texture2D texture = Game.Content.Load<Texture2D>(filename);
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

                // Get character rectangle  the font atlas texture
                glyphs.Add(tempCharRecs[i]);

                // NOTE: On image based fonts (XNA style), character offsets and xAdvance are not required (set to 0)
                cropping.Add(Rectangle.Empty);
                Vector3 ker = new Vector3(0, glyphs[i].Width, 1);
                kerning.Add(ker);
            }
            return new SpriteFont(texture, glyphs, cropping, characters, charHeight + lineSpacing, 0, kerning, '?');
        }

        public static string LoadStorage(byte position, string path) {
            if (File.Exists(path)) {
                using (MemoryStream ms = new MemoryStream()) {
                    BinaryFormatter bf = new BinaryFormatter();
                    string[] data;
                    byte[] dataFile = File.ReadAllBytes(path);
                    ms.Write(dataFile, 0, dataFile.Length);
                    ms.Seek(0, SeekOrigin.Begin);
                    data = (string[])bf.Deserialize(ms);
                    return data[position];
                }
            }
            return null;
        }
        public static string[] LoadStorage(string path) {
            if (File.Exists(path)) {
                using (MemoryStream ms = new MemoryStream()) {
                    BinaryFormatter bf = new BinaryFormatter();
                    string[] data;
                    byte[] dataFile = File.ReadAllBytes(path);
                    ms.Write(dataFile, 0, dataFile.Length);
                    ms.Seek(0, SeekOrigin.Begin);
                    data = (string[])bf.Deserialize(ms);
                    ms.Dispose();
                    return data;
                }
            }
            return null;
        }
        public static void SaveStorage(string path, byte position, string value) {
            using (MemoryStream ms = new MemoryStream()) {
                BinaryFormatter bf = new BinaryFormatter();
                string[] data;
                if (File.Exists(path)) {
                    byte[] dataFile = File.ReadAllBytes(path);
                    ms.Write(dataFile, 0, dataFile.Length);
                    ms.Seek(0, SeekOrigin.Begin);
                    data = (string[])bf.Deserialize(ms);
                } else {
                    data = new string[256];
                }
                data[position] = value;
                ms.Seek(0, SeekOrigin.Begin);
                bf.Serialize(ms, data);
                byte[] bytes = ms.ToArray();
                File.WriteAllBytes(path, bytes);
            }
        }

        #endregion  Files management functions

    }
}
