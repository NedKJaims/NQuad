using System;

namespace NQuad.Utils.Core
{
    [Flags]
    public enum WindowConfigFlag
    {
        NONE = 0,
        VSYNC = 1,
        FULLSCREEN_MODE = 2,
        WINDOW_RESIZABLE = 4,
        WINDOW_UNDECORATED = 8,
        MSAA = 16,
        FIXED_TIME_STEP = 32,
        ALLOW_ALT_F4 = 64,
    }
}
