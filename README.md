##NQuad

`NQuad` is a set of tools to facilitate video game development with` monogame`, which is highly inspired by `raylib`.

### Characteristics
`NQuad` is a shared project, so there is no need to worry if the project is Net Core or Net Framework, the same code is for the available platforms.
`NQuad` is made up of three modules that are independent of each other:
* **Core:** this module is the main one, since it handles the window control, input, file handling, time, among other additional functions.
* **Render:** this module is for the rendering of figures, textures and text, with an efficient 2D batching, which has functions with automatic geometry and manual geometry.
* **Math:** this module consists of two sub-modules ...
    * **Collision2D:** This sub-module contains basic shape collision detection functions.
    * **Easings:** this sub-module contains stock animation functions.

### Supported platforms
The platforms on which `NQuad` has been tested are:

* Windows (DirectX)
* DesktopGL (Windows, Linux, MacOS).
* Android (API 29).

It is believed that it is compatible with all the projects that `monogame` offers, since it uses the main classes of said library.

### Build and Work with `NQuad`

1. Download the project and link to your project.
2. Define the constants in the main project depending on the platform and the input that will be used:
    1. Define the platform:
        1. **DESKTOP** for Windows (DirectX) and DesktopGL.
        This in order to activate some functions that are not in the other platforms.
        (*For the other platforms it is optional to define the platform*).
    2. Define the input that will be used:
        1. **KEYBOARD_MOUSE** to use the keyboard and mouse.
        2. **GAMEPADS** to use the gamepads
        3. **TOUCH** to use the touch.

3. Initialize and update the **Core**:
```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NQuad;
using NQuad.Utils;

namespace My_project
{

    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private Texture2D texture;
        public Game1() {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize() {

            WindowConfigFlag state = WindowConfigFlag.FIXED_TIME_STEP | WindowConfigFlag.VSYNC | WindowConfigFlag.ALLOW_ALT_F4;

            Core.InitCore(this, graphics, "Window", 512, 512, state);

            base.Initialize();
        }

        protected override void LoadContent() {
            texture = Core.ContentLoad<Texture2D>("sometexture");
        }

        protected override void Update(GameTime gameTime) {
            Core.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime) {
            Render.ClearBackground(Color.Black);

            Render.Begin();
            Render.DrawRectangle(64, 64, 64, 64, Color.White);
            Render.DrawTexture(sometexture, 100, 100, Color.Red);
            Render.End();
            
        }
    }
}

```
