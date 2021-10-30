#if KEYBOARD_MOUSE
using System;

namespace NQuad.Utils.Input
{
    [Flags]
    public enum MouseButton
    {
        Left = 0,
        Right = 1,
        Middle = 2,
        XButton1 = 4,
        XButton2 = 8
    }
}
#endif