# NQuad

`NQuad` is a set of tools to facilitate video game development with `monogame`, which is highly inspired by `raylib`.

### Features
`NQuad` is a .NetStandar library, so there is no need to worry if the project is .NetCore or .NetFramework, the same code is compatible for the available platforms.
`NQuad` consists of four modules:
* **Core:** This is the main module, as it handles window control, input, file management, time, among other additional functions.
* **Render:** Module for rendering figures, textures and text, with efficient 2D batch processing, which has functions with automatic geometry and manual geometry.
* **Collision:** Module with basic 2D shape collision detection functions.
* **Easings:** Module with animation curves functions.

### Supported platforms
The platforms on which `NQuad` has been tested are:

* Windows (DirectX).
* DesktopGL (Windows, Linux, MacOS).
* UWP (DirectX, XAML).
* Android (API 29).

It is believed that it is compatible with all the projects that `monogame` offers, since it uses the main classes of said library.

### Build and Work with `NQuad`

1. Download the project and link to your project.
2. Define the constants in the main project depending on the platform and the input that will be used:
    1. Define the platform:
        1. **DESKTOP** for Windows (DirectX), DesktopGL and UWP.
        This in order to activate some functions that are not in the other platforms.
        (*For the other platforms it is optional to define the platform*).
    2. Define the input that will be used:
        1. **KEYBOARD_MOUSE**.
        2. **GAMEPADS**.
        3. **TOUCH**.

3. Initialize and update the **Core**:
```csharp
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NQuad;
using NQuad.Utils.Core;

namespace MyProject
{

    public class MainGame : Game
    {
        private Texture2D texture;
        public MainGame() {
            WindowConfigFlag state = WindowConfigFlag.FIXED_TIME_STEP | WindowConfigFlag.VSYNC | WindowConfigFlag.ALLOW_ALT_F4;

            Core.InitCore(this, "Window", 800, 450, state);
        }

        protected override void Initialize() {

            base.Initialize();
        }

        protected override void LoadContent() {
            texture = Core.ContentLoad<Texture2D>("sometexture");
        }

        protected override void Update(GameTime gameTime) {
            Core.Update(gameTime);
            //Code here
        }

        protected override void Draw(GameTime gameTime) {
            Render.ClearBackground(Color.Black);

            Render.Begin();
            Render.Rectangle(64, 64, 64, 64, Color.White);
            Render.Texture(sometexture, 100, 100, Color.Red);
            Render.End();
            
        }
    }
}

```
