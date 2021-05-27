using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NQuad.Utils;
using System;
using System.Collections.Generic;

namespace NQuad
{
    public static class Render
    {
        private const float PI = 3.14159265358979323846f;
        private const float DEG2RAD = PI / 180.0f;
        private const float SMOOTH_CIRCLE_ERROR_RATE = 0.5f;

        private static ushort index;
        private static Vertex[] data;
        private static SpriteEffect defaultShader;
        private static Texture2D defaultTexture;
        private static Texture2D currentTexture;
        private static PrimitiveType currentPrimitive;

        private static byte DrawCall = 0;

        private static SpriteFont defaultFont;

        public static void InitRender() {
            index = 0;
            data = new Vertex[8192 * 6]; //8192
            for (int i = 0; i < 8192 * 6; i++) {
                data[i] = new Vertex(Vector2.Zero, Vector2.Zero, Color.White);
            }

            defaultTexture = new Texture2D(Core.Game.GraphicsDevice, 1, 1);
            defaultTexture.SetData(new Color[] { Color.White });
            defaultTexture.Name = "Default";
            defaultTexture.Tag = "Default";
            currentTexture = defaultTexture;

            defaultShader = new SpriteEffect(Core.Game.GraphicsDevice);
            
            currentPrimitive = PrimitiveType.TriangleList;

            Core.Game.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            Core.Game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            Core.Game.GraphicsDevice.DepthStencilState = DepthStencilState.None;
            Core.Game.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            CreateDefaultFont();

        }
        private static void CreateDefaultFont() {
            
            uint[] defaultFontData = {
                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00200020, 0x0001b000, 0x00000000, 0x00000000, 0x8ef92520, 0x00020a00, 0x7dbe8000, 0x1f7df45f,
                0x4a2bf2a0, 0x0852091e, 0x41224000, 0x10041450, 0x2e292020, 0x08220812, 0x41222000, 0x10041450, 0x10f92020, 0x3efa084c, 0x7d22103c, 0x107df7de,
                0xe8a12020, 0x08220832, 0x05220800, 0x10450410, 0xa4a3f000, 0x08520832, 0x05220400, 0x10450410, 0xe2f92020, 0x0002085e, 0x7d3e0281, 0x107df41f,
                0x00200000, 0x8001b000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000,
                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0xc0000fbe, 0xfbf7e00f, 0x5fbf7e7d, 0x0050bee8, 0x440808a2, 0x0a142fe8, 0x50810285, 0x0050a048,
                0x49e428a2, 0x0a142828, 0x40810284, 0x0048a048, 0x10020fbe, 0x09f7ebaf, 0xd89f3e84, 0x0047a04f, 0x09e48822, 0x0a142aa1, 0x50810284, 0x0048a048,
                0x04082822, 0x0a142fa0, 0x50810285, 0x0050a248, 0x00008fbe, 0xfbf42021, 0x5f817e7d, 0x07d09ce8, 0x00008000, 0x00000fe0, 0x00000000, 0x00000000,
                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x000c0180,
                0xdfbf4282, 0x0bfbf7ef, 0x42850505, 0x004804bf, 0x50a142c6, 0x08401428, 0x42852505, 0x00a808a0, 0x50a146aa, 0x08401428, 0x42852505, 0x00081090,
                0x5fa14a92, 0x0843f7e8, 0x7e792505, 0x00082088, 0x40a15282, 0x08420128, 0x40852489, 0x00084084, 0x40a16282, 0x0842022a, 0x40852451, 0x00088082,
                0xc0bf4282, 0xf843f42f, 0x7e85fc21, 0x3e0900bf, 0x00000000, 0x00000004, 0x00000000, 0x000c0180, 0x00000000, 0x00000000, 0x00000000, 0x00000000,
                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x04000402, 0x41482000, 0x00000000, 0x00000800,
                0x04000404, 0x4100203c, 0x00000000, 0x00000800, 0xf7df7df0, 0x514bef85, 0xbefbefbe, 0x04513bef, 0x14414500, 0x494a2885, 0xa28a28aa, 0x04510820,
                0xf44145f0, 0x474a289d, 0xa28a28aa, 0x04510be0, 0x14414510, 0x494a2884, 0xa28a28aa, 0x02910a00, 0xf7df7df0, 0xd14a2f85, 0xbefbe8aa, 0x011f7be0,
                0x00000000, 0x00400804, 0x20080000, 0x00000000, 0x00000000, 0x00600f84, 0x20080000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000,
                0xac000000, 0x00000f01, 0x00000000, 0x00000000, 0x24000000, 0x00000f01, 0x00000000, 0x06000000, 0x24000000, 0x00000f01, 0x00000000, 0x09108000,
                0x24fa28a2, 0x00000f01, 0x00000000, 0x013e0000, 0x2242252a, 0x00000f52, 0x00000000, 0x038a8000, 0x2422222a, 0x00000f29, 0x00000000, 0x010a8000,
                0x2412252a, 0x00000f01, 0x00000000, 0x010a8000, 0x24fbe8be, 0x00000f01, 0x00000000, 0x0ebe8000, 0xac020000, 0x00000f01, 0x00000000, 0x00048000,
                0x0003e000, 0x00000f00, 0x00000000, 0x00008000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000038, 0x8443b80e, 0x00203a03,
                0x02bea080, 0xf0000020, 0xc452208a, 0x04202b02, 0xf8029122, 0x07f0003b, 0xe44b388e, 0x02203a02, 0x081e8a1c, 0x0411e92a, 0xf4420be0, 0x01248202,
                0xe8140414, 0x05d104ba, 0xe7c3b880, 0x00893a0a, 0x283c0e1c, 0x04500902, 0xc4400080, 0x00448002, 0xe8208422, 0x04500002, 0x80400000, 0x05200002,
                0x083e8e00, 0x04100002, 0x804003e0, 0x07000042, 0xf8008400, 0x07f00003, 0x80400000, 0x04000022, 0x00000000, 0x00000000, 0x80400000, 0x04000002,
                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00800702, 0x1848a0c2, 0x84010000, 0x02920921, 0x01042642, 0x00005121, 0x42023f7f, 0x00291002,
                0xefc01422, 0x7efdfbf7, 0xefdfa109, 0x03bbbbf7, 0x28440f12, 0x42850a14, 0x20408109, 0x01111010, 0x28440408, 0x42850a14, 0x2040817f, 0x01111010,
                0xefc78204, 0x7efdfbf7, 0xe7cf8109, 0x011111f3, 0x2850a932, 0x42850a14, 0x2040a109, 0x01111010, 0x2850b840, 0x42850a14, 0xefdfbf79, 0x03bbbbf7,
                0x001fa020, 0x00000000, 0x00001000, 0x00000000, 0x00002070, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000,
                0x08022800, 0x00012283, 0x02430802, 0x01010001, 0x8404147c, 0x20000144, 0x80048404, 0x00823f08, 0xdfbf4284, 0x7e03f7ef, 0x142850a1, 0x0000210a,
                0x50a14684, 0x528a1428, 0x142850a1, 0x03efa17a, 0x50a14a9e, 0x52521428, 0x142850a1, 0x02081f4a, 0x50a15284, 0x4a221428, 0xf42850a1, 0x03efa14b,
                0x50a16284, 0x4a521428, 0x042850a1, 0x0228a17a, 0xdfbf427c, 0x7e8bf7ef, 0xf7efdfbf, 0x03efbd0b, 0x00000000, 0x04000000, 0x00000000, 0x00000008,
                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00200508, 0x00840400, 0x11458122, 0x00014210,
                0x00514294, 0x51420800, 0x20a22a94, 0x0050a508, 0x00200000, 0x00000000, 0x00050000, 0x08000000, 0xfefbefbe, 0xfbefbefb, 0xfbeb9114, 0x00fbefbe,
                0x20820820, 0x8a28a20a, 0x8a289114, 0x3e8a28a2, 0xfefbefbe, 0xfbefbe0b, 0x8a289114, 0x008a28a2, 0x228a28a2, 0x08208208, 0x8a289114, 0x088a28a2,
                0xfefbefbe, 0xfbefbefb, 0xfa2f9114, 0x00fbefbe, 0x00000000, 0x00000040, 0x00000000, 0x00000000, 0x00000000, 0x00000020, 0x00000000, 0x00000000,
                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00210100, 0x00000004, 0x00000000, 0x00000000, 0x14508200, 0x00001402, 0x00000000, 0x00000000,
                0x00000010, 0x00000020, 0x00000000, 0x00000000, 0xa28a28be, 0x00002228, 0x00000000, 0x00000000, 0xa28a28aa, 0x000022e8, 0x00000000, 0x00000000,
                0xa28a28aa, 0x000022a8, 0x00000000, 0x00000000, 0xa28a28aa, 0x000022e8, 0x00000000, 0x00000000, 0xbefbefbe, 0x00003e2f, 0x00000000, 0x00000000,
                0x00000004, 0x00002028, 0x00000000, 0x00000000, 0x80000000, 0x00003e0f, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000,
                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000,
                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000,
                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000,
                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000,
                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000,
                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000 };
            int charsHeight = 10;
            int charsDivisor = 1;    // Every char is separated from the consecutive by a 1 pixel divisor, horizontally and vertically
            int[] charsWidth = { 3, 1, 4, 6, 5, 7, 6, 2, 3, 3, 5, 5, 2, 4, 1, 7, 5, 2, 5, 5, 5, 5, 5, 5, 5, 5, 1, 1, 3, 4, 3, 6,
                            7, 6, 6, 6, 6, 6, 6, 6, 6, 3, 5, 6, 5, 7, 6, 6, 6, 6, 6, 6, 7, 6, 7, 7, 6, 6, 6, 2, 7, 2, 3, 5,
                            2, 5, 5, 5, 5, 5, 4, 5, 5, 1, 2, 5, 2, 5, 5, 5, 5, 5, 5, 5, 4, 5, 5, 5, 5, 5, 5, 3, 1, 3, 4, 4,
                            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                            1, 1, 5, 5, 5, 7, 1, 5, 3, 7, 3, 5, 4, 1, 7, 4, 3, 5, 3, 3, 2, 5, 6, 1, 2, 2, 3, 5, 6, 6, 6, 6,
                            6, 6, 6, 6, 6, 6, 7, 6, 6, 6, 6, 6, 3, 3, 3, 3, 7, 6, 6, 6, 6, 6, 6, 5, 6, 6, 6, 6, 6, 6, 4, 6,
                            5, 5, 5, 5, 5, 5, 9, 5, 5, 5, 5, 5, 2, 2, 3, 3, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 3, 5
            };

            Color[] data = new Color[128 * 128];
            for (int i = 0, counter = 0; i < 128 * 128; i += 32) {
                for (int j = 31; j >= 0; j--) {
                    if (((defaultFontData[counter]) & (1u << (j))) > 0) {
                        data[i + j] = Color.White;
                    } else {
                        data[i + j] = Color.Transparent;
                    }
                }
                counter++;
            }

            Texture2D texture = new Texture2D(Core.Game.GraphicsDevice, 128, 128);
            texture.SetData(data);
            List<Rectangle> glyphs = new List<Rectangle>(), cropping = new List<Rectangle>();
            List<char> characters = new List<char>();
            List<Vector3> kerning = new List<Vector3>();

            int currentLine = 0;
            int currentPosX = charsDivisor;
            int testPosX = charsDivisor;

            for (int i = 0; i < 224; i++) {
                characters.Add(char.ConvertFromUtf32(i + 32)[0]);

                Rectangle glyph = new Rectangle(currentPosX,
                    charsDivisor + currentLine * (charsHeight + charsDivisor),
                    charsWidth[i],
                    charsHeight);

                testPosX += glyph.Width + charsDivisor;

                if (testPosX >= texture.Width) {
                    currentLine++;
                    currentPosX = 2 * charsDivisor + charsWidth[i];
                    testPosX = currentPosX;

                    glyph.X = charsDivisor;
                    glyph.Y = (charsDivisor + currentLine * (charsHeight + charsDivisor));
                } else currentPosX = testPosX;

                glyphs.Add(glyph);
                cropping.Add(Rectangle.Empty);

                Vector3 k = new Vector3(0, glyph.Width, 1);
                kerning.Add(k);

            }

            defaultFont = new SpriteFont(texture, glyphs, cropping, characters, 15, 0, kerning, '?');
            defaultFontData = null;
            charsWidth = null;
            texture = null;
            glyphs = null;
            cropping = null;
            characters = null;
            kerning = null;
        }

        public static byte GetDrawCalls() {
            return DrawCall;
        }
        public static SpriteFont GetDefaultFont() {
            return defaultFont;
        }

        public static void ClearBackground(in Color color) {
            Core.Game.GraphicsDevice.Clear(color);
        }

        public static void BeginScissorMode(in int x, in int y, in int width, in int height) {
            Core.Game.GraphicsDevice.ScissorRectangle = new Rectangle(x, y, width, height);
        }
        public static void BeginRenderTarget(ref RenderTarget2D renderTarget) {
            Core.Game.GraphicsDevice.SetRenderTarget(renderTarget);
        }
        public static void BeginCamera(in Camera camera) {
            defaultShader.TransformMatrix = camera.Matrix;
        }
        public static void Begin() {
            index = 0;
            DrawCall = 0;
            defaultShader.CurrentTechnique.Passes[0].Apply();
            Core.Game.GraphicsDevice.Textures[0] = currentTexture;
        }
        public static void Begin(in BlendState? blendState = null) {
            index = 0;
            DrawCall = 0;

            Core.Game.GraphicsDevice.BlendState = blendState ?? BlendState.AlphaBlend;

            defaultShader.CurrentTechnique.Passes[0].Apply();
            Core.Game.GraphicsDevice.Textures[0] = currentTexture;
        }
        public static void Begin(in BlendState? blendState = null, in SamplerState? samplerState = null) {
            index = 0;
            DrawCall = 0;

            Core.Game.GraphicsDevice.BlendState = blendState ?? BlendState.AlphaBlend;
            Core.Game.GraphicsDevice.SamplerStates[0] = samplerState ?? SamplerState.PointClamp;

            defaultShader.CurrentTechnique.Passes[0].Apply();
            Core.Game.GraphicsDevice.Textures[0] = currentTexture;
        }
        public static void Begin(in BlendState? blendState = null, in RasterizerState? rasterizerState = null, in SamplerState? samplerState = null) {
            index = 0;
            DrawCall = 0;

            Core.Game.GraphicsDevice.BlendState = blendState ?? BlendState.AlphaBlend;
            Core.Game.GraphicsDevice.RasterizerState = rasterizerState ?? RasterizerState.CullNone;
            Core.Game.GraphicsDevice.SamplerStates[0] = samplerState ?? SamplerState.PointClamp;

            defaultShader.CurrentTechnique.Passes[0].Apply();
            Core.Game.GraphicsDevice.Textures[0] = currentTexture;
        }

        public static void End() {
            if(index != 0) {
                int mod = (currentPrimitive == PrimitiveType.TriangleList) ? 3 : 2;
                Core.Game.GraphicsDevice.DrawUserPrimitives(currentPrimitive, data, 0, index / mod);
                defaultShader.TransformMatrix = null;
                DrawCall++;
                index = 0;
            }
        }
        public static void EndCamera() {
            defaultShader.TransformMatrix = null;
        }
        public static void EndRenderTarget() {
            Core.Game.GraphicsDevice.SetRenderTarget(null);
        }
        public static void EndScissorMode() {
            Core.Game.GraphicsDevice.ScissorRectangle = new Rectangle(0, 0, Core.GetWindowWidth(), Core.GetWindowHeight());
        }

        private static void CheckBufferLimitMode(in int additionalVertex, in PrimitiveType type, in Texture2D texture) {
            if (texture != currentTexture || currentPrimitive != type) {
                if (texture == null || texture.IsDisposed)
                    throw new Exception("No exist texture");
                End();
                currentTexture = texture;
                currentPrimitive = type;
                Core.Game.GraphicsDevice.Textures[0] = currentTexture;
            } else if (index + additionalVertex > data.Length)
                End();
        }

        #region DRAW FUNCTIONS

        public static void DrawPixel(in float posX, in float posY, in Color color) {
            CheckBufferLimitMode(6, PrimitiveType.TriangleList, defaultTexture);
            float right = posX + 1;
            float down = posY + 1;
            data[index++].Set(posX, posY, 0, 0, color);
            data[index++].Set(right, posY, 0, 0, color);
            data[index++].Set(posX, down, 0, 0, color);

            data[index++].Set(right, posY, 0, 0, color);
            data[index++].Set(right, down, 0, 0, color);
            data[index++].Set(posX, down, 0, 0, color);
        }
        public static void DrawPixelV(in Vector2 position, in Color color) {
            CheckBufferLimitMode(6, PrimitiveType.TriangleList, defaultTexture);
            float right = position.X + 1;
            float down = position.Y + 1;
            data[index++].Set(position.X, position.Y, 0, 0, color);
            data[index++].Set(right, position.Y, 0, 0, color);
            data[index++].Set(position.X, down, 0, 0, color);

            data[index++].Set(right, position.Y, 0, 0, color);
            data[index++].Set(right, down, 0, 0, color);
            data[index++].Set(position.X, down, 0, 0, color);
        }

        public static void DrawLine(in float startPosX, in float startPosY, in float endPosX, in float endPosY, in Color color) {
            CheckBufferLimitMode(2, PrimitiveType.LineList, defaultTexture);
            data[index++].Set(startPosX, startPosY, 0, 0, color);
            data[index++].Set(endPosX, endPosY, 0, 0, color);
        }
        public static void DrawLineV(in Vector2 startPos, in Vector2 endPos, in Color color) {
            CheckBufferLimitMode(2, PrimitiveType.LineList, defaultTexture);
            data[index++].Set(startPos.X, startPos.Y, 0, 0, color);
            data[index++].Set(endPos.X, endPos.Y, 0, 0, color);
        }
        public static void DrawLineEx(Vector2 startPos, Vector2 endPos, in float thick, in Color color) {
            CheckBufferLimitMode(6, PrimitiveType.TriangleList, defaultTexture);
            if (startPos.X > endPos.X) {
                Vector2 tempPos = startPos;
                startPos = endPos;
                endPos = tempPos;
            }
            Vector2 origin = new Vector2(0f, 0.5f);
            float dx = endPos.X - startPos.X;
            float dy = endPos.Y - startPos.Y;
            float d = (float)Math.Sqrt(dx * dx + dy * dy);
            float angle = (float)Math.Atan(dy / dx);
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);


            Vector2 TL = new Vector2(startPos.X + origin.X * cos - origin.Y * sin, startPos.Y + origin.X * sin + origin.Y * cos);
            Vector2 TR = new Vector2(startPos.X + (origin.X + d) * cos - origin.Y * sin, startPos.Y + (origin.X + d) * sin + origin.Y * cos);
            Vector2 BL = new Vector2(startPos.X + origin.X * cos - (origin.Y + thick) * sin, startPos.Y + origin.X * sin + (origin.Y + thick) * cos);
            Vector2 BR = new Vector2(startPos.X + (origin.X + d) * cos - (origin.Y + thick) * sin, startPos.Y + (origin.X + d) * sin + (origin.Y + thick) * cos);

            data[index++].Set(TL.X, TL.Y, 0, 0, color);
            data[index++].Set(TR.X, TR.Y, 0, 0, color);
            data[index++].Set(BL.X, BL.Y, 0, 0, color);

            data[index++].Set(TR.X, TR.Y, 0, 0, color);
            data[index++].Set(BR.X, BR.Y, 0, 0, color);
            data[index++].Set(BL.X, BL.Y, 0, 0, color);
        }
        public static void DrawLineBezier(in Vector2 startPos, in Vector2 endPos, in float thick, in Color color) {
            const int BEZIER_LINE_DIVISIONS = 24;
            Vector2 previous = startPos;
            Vector2 current;

            for (int i = 1; i <= BEZIER_LINE_DIVISIONS; i++) {
                // Cubic easing in-out
                // NOTE: Easing is calculated only for y position value
                current.Y = Easings.EaseCubicInOut(i, startPos.Y, endPos.Y - startPos.Y, BEZIER_LINE_DIVISIONS);
                current.X = previous.X + (endPos.X - startPos.X) / BEZIER_LINE_DIVISIONS;

                DrawLineEx(previous, current, thick, color);

                previous = current;
            }
        }
        public static void DrawLineStrip(in Vector2[] points, in Color color) {
            if (points.Length >= 2) {
                CheckBufferLimitMode(points.Length, PrimitiveType.LineList, defaultTexture);
                for (int i = 0; i < points.Length - 1; i++) {
                    data[index++].Set(points[i].X, points[i].Y, 0, 0, color);
                    data[index++].Set(points[i + 1].X, points[i + 1].Y, 0, 0, color);
                }
            }
        }

        public static void DrawCircle(in float centerX, in float centerY, in float radius, in Color color) {
            CheckBufferLimitMode(108, PrimitiveType.TriangleList, defaultTexture);
            const float stepLength = 10f;
            float angle = 0f;
            for (int i = 0; i < 36; i++) {
                data[index++].Set(centerX, centerY, 0, 0, color);
                data[index++].Set(centerX + (float)Math.Sin(DEG2RAD * angle) * radius, centerY + (float)Math.Cos(DEG2RAD * angle) * radius, 0, 0, color);
                data[index++].Set(centerX + (float)Math.Sin(DEG2RAD * (angle + stepLength)) * radius, centerY + (float)Math.Cos(DEG2RAD * (angle + stepLength)) * radius, 0, 0, color);
                angle += stepLength;
            }
        }
        public static void DrawCircleSector(in Vector2 center, float radius, int startAngle, int endAngle, int segments, in Color color) {
            CheckBufferLimitMode(segments * 3, PrimitiveType.TriangleList, defaultTexture);
            if (radius <= 0.0f) radius = 0.1f;
            if (endAngle < startAngle) {
                int tmp = startAngle;
                startAngle = endAngle;
                endAngle = tmp;
            }

            if (segments < 4) {
                float th = (float)Math.Acos(2 * Math.Pow(1 - SMOOTH_CIRCLE_ERROR_RATE / radius, 2) - 1);
                segments = (int)((endAngle - startAngle) * (float)Math.Ceiling(2 * PI / th) / 360);

                if (segments <= 0) segments = 4;
            }
            float stepLength = (endAngle - startAngle) / (float)segments;
            float angle = startAngle;
            for (int i = 0; i < segments; i++) {
                data[index++].Set(center.X, center.Y, 0, 0, color);
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * angle) * radius, center.Y + (float)Math.Cos(DEG2RAD * angle) * radius, 0, 0, color);
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * (angle + stepLength)) * radius, center.Y + (float)Math.Cos(DEG2RAD * (angle + stepLength)) * radius, 0, 0, color);
                angle += stepLength;
            }
        }
        public static void DrawCircleSectorLines(in Vector2 center, float radius, int startAngle, int endAngle, int segments, in Color color) {
            if (radius <= 0.0f) radius = 0.1f;

            if (endAngle < startAngle) {
                int tmp = startAngle;
                startAngle = endAngle;
                endAngle = tmp;
            }

            if (segments < 4) {
                float th = (float)Math.Acos(2 * Math.Pow(1 - SMOOTH_CIRCLE_ERROR_RATE / radius, 2) - 1);
                segments = (int)((endAngle - startAngle) * Math.Ceiling(2 * PI / th) / 360);

                if (segments <= 0) segments = 4;
            }

            float stepLength = (float)(endAngle - startAngle) / (float)segments;
            float angle = (float)startAngle;

            bool showCapLines = true;
            int limit = 2 * (segments + 2);
            if ((endAngle - startAngle) % 360 == 0) { limit = 2 * segments; showCapLines = false; }

            CheckBufferLimitMode(limit, PrimitiveType.LineList, defaultTexture);

            if (showCapLines) {
                data[index++].Set(center.X, center.Y, 0, 0, color);
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * angle) * radius, center.Y + (float)Math.Cos(DEG2RAD * angle) * radius, 0, 0, color);
            }

            for (int i = 0; i < segments; i++) {
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * angle) * radius, center.Y + (float)Math.Cos(DEG2RAD * angle) * radius, 0, 0, color);
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * (angle + stepLength)) * radius, center.Y + (float)Math.Cos(DEG2RAD * (angle + stepLength)) * radius, 0, 0, color);

                angle += stepLength;
            }

            if (showCapLines) {
                data[index++].Set(center.X, center.Y, 0, 0, color);
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * angle) * radius, center.Y + (float)Math.Cos(DEG2RAD * angle) * radius, 0, 0, color);
            }
        }
        public static void DrawCircleGradient(in float centerX, in float centerY, in float radius, in Color color1, in Color color2) {
            CheckBufferLimitMode(108, PrimitiveType.TriangleList, defaultTexture);
            for (int i = 0; i < 360; i += 10) {
                data[index++].Set(centerX, centerY, 0, 0, color1);
                data[index++].Set(centerX + (float)Math.Sin(DEG2RAD * i) * radius, (float)centerY + (float)Math.Cos(DEG2RAD * i) * radius, 0, 0, color2);
                data[index++].Set(centerX + (float)Math.Sin(DEG2RAD * (i + 10)) * radius, (float)centerY + (float)Math.Cos(DEG2RAD * (i + 10)) * radius, 0, 0, color2);
            }
        }
        public static void DrawCircleV(in Vector2 center, in float radius, in Color color) {
            CheckBufferLimitMode(108, PrimitiveType.TriangleList, defaultTexture);
            const float stepLength = 10f;
            float angle = 0f;
            for (int i = 0; i < 36; i++) {
                data[index++].Set(center.X, center.Y, 0, 0, color);
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * angle) * radius, center.Y + (float)Math.Cos(DEG2RAD * angle) * radius, 0, 0, color);
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * (angle + stepLength)) * radius, center.Y + (float)Math.Cos(DEG2RAD * (angle + stepLength)) * radius, 0, 0, color);
                angle += stepLength;
            }
        }
        public static void DrawCircleLines(in float centerX, in float centerY, in float radius, in Color color) {
            CheckBufferLimitMode(72, PrimitiveType.LineList, defaultTexture);
            for (int i = 0; i < 360; i += 10) {
                data[index++].Set(centerX + (float)Math.Sin(DEG2RAD * i) * radius, centerY + (float)Math.Cos(DEG2RAD * i) * radius, 0, 0, color);
                data[index++].Set(centerX + (float)Math.Sin(DEG2RAD * (i + 10)) * radius, centerY + (float)Math.Cos(DEG2RAD * (i + 10)) * radius, 0, 0, color);
            }
        }

        public static void DrawEllipse(in float centerX, in float centerY, in float radiusH, in float radiusV, in Color color) {
            CheckBufferLimitMode(108, PrimitiveType.TriangleList, defaultTexture);
            for (int i = 0; i < 360; i += 10) {
                data[index++].Set((float)centerX, (float)centerY, 0, 0, color);
                data[index++].Set((float)centerX + (float)Math.Sin(DEG2RAD * i) * radiusH, (float)centerY + (float)Math.Cos(DEG2RAD * i) * radiusV, 0, 0, color);
                data[index++].Set((float)centerX + (float)Math.Sin(DEG2RAD * (i + 10)) * radiusH, (float)centerY + (float)Math.Cos(DEG2RAD * (i + 10)) * radiusV, 0, 0, color);
            }

        }
        public static void DrawEllipseLines(in float centerX, in float centerY, in float radiusH, in float radiusV, in Color color) {
            CheckBufferLimitMode(72, PrimitiveType.LineList, defaultTexture);

            for (int i = 0; i < 360; i += 10) {
                data[index++].Set(centerX + (float)Math.Sin(DEG2RAD * i) * radiusH, centerY + (float)Math.Cos(DEG2RAD * i) * radiusV, 0, 0, color);
                data[index++].Set(centerX + (float)Math.Sin(DEG2RAD * (i + 10)) * radiusH, centerY + (float)Math.Cos(DEG2RAD * (i + 10)) * radiusV, 0, 0, color);
            }
        }

        public static void DrawRing(in Vector2 center, float innerRadius, float outerRadius, int startAngle, int endAngle, int segments, in Color color) {
            if (startAngle == endAngle) return;

            if (outerRadius < innerRadius) {
                float tmp = outerRadius;
                outerRadius = innerRadius;
                innerRadius = tmp;

                if (outerRadius <= 0.0f) outerRadius = 0.1f;
            }

            if (endAngle < startAngle) {
                int tmp = startAngle;
                startAngle = endAngle;
                endAngle = tmp;
            }

            if (segments < 4) {
                float th = (float)Math.Acos(2 * (float)Math.Pow(1 - SMOOTH_CIRCLE_ERROR_RATE / outerRadius, 2) - 1);
                segments = (int)((endAngle - startAngle) * Math.Ceiling(2 * PI / th) / 360);

                if (segments <= 0) segments = 4;
            }

            if (innerRadius <= 0.0f) {
                DrawCircleSector(center, outerRadius, startAngle, endAngle, segments, color);
                return;
            }

            float stepLength = (endAngle - startAngle) / (float)segments;
            float angle = startAngle;

            CheckBufferLimitMode(6 * segments, PrimitiveType.TriangleList, defaultTexture);

            for (int i = 0; i < segments; i++) {

                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * angle) * innerRadius, center.Y + (float)Math.Cos(DEG2RAD * angle) * innerRadius, 0, 0, color);
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * angle) * outerRadius, center.Y + (float)Math.Cos(DEG2RAD * angle) * outerRadius, 0, 0, color);
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * (angle + stepLength)) * innerRadius, center.Y + (float)Math.Cos(DEG2RAD * (angle + stepLength)) * innerRadius, 0, 0, color);

                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * (angle + stepLength)) * innerRadius, center.Y + (float)Math.Cos(DEG2RAD * (angle + stepLength)) * innerRadius, 0, 0, color);
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * angle) * outerRadius, center.Y + (float)Math.Cos(DEG2RAD * angle) * outerRadius, 0, 0, color);
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * (angle + stepLength)) * outerRadius, center.Y + (float)Math.Cos(DEG2RAD * (angle + stepLength)) * outerRadius, 0, 0, color);

                angle += stepLength;
            }

        }
        public static void DrawRingLines(in Vector2 center, float innerRadius, float outerRadius, int startAngle, int endAngle, int segments, in Color color) {
            if (startAngle == endAngle) return;

            if (outerRadius < innerRadius) {
                float tmp = outerRadius;
                outerRadius = innerRadius;
                innerRadius = tmp;

                if (outerRadius <= 0.0f) outerRadius = 0.1f;
            }

            if (endAngle < startAngle) {
                int tmp = startAngle;
                startAngle = endAngle;
                endAngle = tmp;
            }

            if (segments < 4) {
                float th = (float)Math.Acos(2 * Math.Pow(1 - SMOOTH_CIRCLE_ERROR_RATE / outerRadius, 2) - 1);
                segments = (int)((endAngle - startAngle) * Math.Ceiling(2 * PI / th) / 360);

                if (segments <= 0) segments = 4;
            }

            if (innerRadius <= 0.0f) {
                DrawCircleSectorLines(center, outerRadius, startAngle, endAngle, segments, color);
                return;
            }

            float stepLength = (endAngle - startAngle) / (float)segments;
            float angle = startAngle;

            bool showCapLines = true;
            int limit = 4 * (segments + 1);
            if ((endAngle - startAngle) % 360 == 0) { limit = 4 * segments; showCapLines = false; }

            CheckBufferLimitMode(limit, PrimitiveType.LineList, defaultTexture);

            if (showCapLines) {
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * angle) * outerRadius, center.Y + (float)Math.Cos(DEG2RAD * angle) * outerRadius, 0, 0, color);
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * angle) * innerRadius, center.Y + (float)Math.Cos(DEG2RAD * angle) * innerRadius, 0, 0, color);
            }

            for (int i = 0; i < segments; i++) {
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * angle) * outerRadius, center.Y + (float)Math.Cos(DEG2RAD * angle) * outerRadius, 0, 0, color);
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * (angle + stepLength)) * outerRadius, center.Y + (float)Math.Cos(DEG2RAD * (angle + stepLength)) * outerRadius, 0, 0, color);
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * angle) * innerRadius, center.Y + (float)Math.Cos(DEG2RAD * angle) * innerRadius, 0, 0, color);
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * (angle + stepLength)) * innerRadius, center.Y + (float)Math.Cos(DEG2RAD * (angle + stepLength)) * innerRadius, 0, 0, color);

                angle += stepLength;
            }

            if (showCapLines) {
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * angle) * outerRadius, center.Y + (float)Math.Cos(DEG2RAD * angle) * outerRadius, 0, 0, color);
                data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * angle) * innerRadius, center.Y + (float)Math.Cos(DEG2RAD * angle) * innerRadius, 0, 0, color);
            }

        }

        public static void DrawRectangle(in float positionX, in float positionY, in float sizeX, in float sizeY, in Color color) {
            CheckBufferLimitMode(6, PrimitiveType.TriangleList, defaultTexture);
            float right = positionX + sizeX;
            float down = positionY + sizeY;
            data[index++].Set(positionX, positionY, 0, 0, color);
            data[index++].Set(right, positionY, 0, 0, color);
            data[index++].Set(positionX, down, 0, 0, color);

            data[index++].Set(right, positionY, 0, 0, color);
            data[index++].Set(right, down, 0, 0, color);
            data[index++].Set(positionX, down, 0, 0, color);
        }
        public static void DrawRectangleV(in Vector2 position, in Vector2 size, in Color color) {
            CheckBufferLimitMode(6, PrimitiveType.TriangleList, defaultTexture);
            float right = position.X + size.X;
            float down = position.Y + size.Y;
            data[index++].Set(position.X, position.Y, 0, 0, color);
            data[index++].Set(right, position.Y, 0, 0, color);
            data[index++].Set(position.X, down, 0, 0, color);

            data[index++].Set(right, position.Y, 0, 0, color);
            data[index++].Set(right, down, 0, 0, color);
            data[index++].Set(position.X, down, 0, 0, color);
        }
        public static void DrawRectangleRec(in Rectangle rec, in Color color) {
            CheckBufferLimitMode(6, PrimitiveType.TriangleList, defaultTexture);

            data[index++].Set(rec.X, rec.Y, 0, 0, color);
            data[index++].Set(rec.Right, rec.Y, 0, 0, color);
            data[index++].Set(rec.X, rec.Bottom, 0, 0, color);

            data[index++].Set(rec.Right, rec.Y, 0, 0, color);
            data[index++].Set(rec.Right, rec.Bottom, 0, 0, color);
            data[index++].Set(rec.X, rec.Bottom, 0, 0, color);
        }
        public static void DrawRectanglePro(in Rectangle rec, Vector2 origin, in float rotation, in Color color) {
            CheckBufferLimitMode(6, PrimitiveType.TriangleList, defaultTexture);
            origin = -origin;
            float cos = (float)Math.Cos(rotation * DEG2RAD);
            float sin = (float)Math.Sin(rotation * DEG2RAD);

            Vector2 TL = new Vector2(rec.X + origin.X * cos - origin.Y * sin, rec.Y + origin.X * sin + origin.Y * cos);
            Vector2 TR = new Vector2(rec.X + (origin.X + rec.Width) * cos - origin.Y * sin, rec.Y + (origin.X + rec.Width) * sin + origin.Y * cos);
            Vector2 BL = new Vector2(rec.X + origin.X * cos - (origin.Y + rec.Height) * sin, rec.Y + origin.X * sin + (origin.Y + rec.Height) * cos);
            Vector2 BR = new Vector2(rec.X + (origin.X + rec.Width) * cos - (origin.Y + rec.Height) * sin, rec.Y + (origin.X + rec.Width) * sin + (origin.Y + rec.Height) * cos);


            data[index++].Set(TL.X, TL.Y, 0, 0, color);  //TL
            data[index++].Set(TR.X, TR.Y, 0, 0, color);  //TR
            data[index++].Set(BL.X, BL.Y, 0, 0, color);  //DL

            data[index++].Set(TR.X, TR.Y, 0, 0, color);  //TR
            data[index++].Set(BR.X, BR.Y, 0, 0, color);  //DR
            data[index++].Set(BL.X, BL.Y, 0, 0, color);  //DL
        }
        public static void DrawRectangleGradientV(in float posX, in float posY, in float width, in float height, in Color color1, in Color color2) {
            DrawRectangleGradientEx(new Rectangle((int)posX, (int)posY, (int)width, (int)height), color1, color2, color2, color1);
        }
        public static void DrawRectangleGradientH(in float posX, in float posY, in float width, in float height, in Color color1, in Color color2) {
            DrawRectangleGradientEx(new Rectangle((int)posX, (int)posY, (int)width, (int)height), color1, color1, color2, color2);
        }
        public static void DrawRectangleGradientEx(in Rectangle rec, in Color col1, in Color col2, in Color col3, in Color col4) {
            CheckBufferLimitMode(6, PrimitiveType.TriangleList, defaultTexture);

            data[index++].Set(rec.X, rec.Y, 0, 0, col1);               //TL
            data[index++].Set(rec.Right, rec.Y, 0, 0, col4);           //TR
            data[index++].Set(rec.X, rec.Bottom, 0, 0, col2);          //DL

            data[index++].Set(rec.Right, rec.Y, 0, 0, col4);           //TR
            data[index++].Set(rec.Right, rec.Bottom, 0, 0, col3);      //DR
            data[index++].Set(rec.X, rec.Bottom, 0, 0, col2);          //DL
        }
        public static void DrawRectangleLines(in float posX, in float posY, in float width, in float height, in Color color) {
            CheckBufferLimitMode(8, PrimitiveType.LineList, defaultTexture);

            data[index++].Set(posX + 1, posY + 1, 0, 0, color);
            data[index++].Set(posX + width, posY + 1, 0, 0, color);

            data[index++].Set(posX + width, posY + 1, 0, 0, color);
            data[index++].Set(posX + width, posY + height, 0, 0, color);

            data[index++].Set(posX + width, posY + height, 0, 0, color);
            data[index++].Set(posX + 1, posY + height, 0, 0, color);

            data[index++].Set(posX + 1, posY + height, 0, 0, color);
            data[index++].Set(posX + 1, posY + 1, 0, 0, color);
        }
        public static void DrawRectangleLinesEx(in Rectangle rec, int lineThick, in Color color) {
            if (lineThick > rec.Width || lineThick > rec.Height) {
                if (rec.Width > rec.Height) lineThick = (int)rec.Height / 2;
                else if (rec.Width < rec.Height) lineThick = (int)rec.Width / 2;
            }

            DrawRectangle(rec.X, rec.Y, rec.Width, lineThick, color);
            DrawRectangle((rec.X - lineThick + rec.Width), (rec.Y + lineThick), lineThick, (rec.Height - lineThick * 2.0f), color);
            DrawRectangle(rec.X, (rec.Y + rec.Height - lineThick), rec.Width, lineThick, color);
            DrawRectangle(rec.X, (rec.Y + lineThick), lineThick, (rec.Height - lineThick * 2), color);
        }
        public static void DrawRectangleRounded(in Rectangle rec, float roundness, int segments, in Color color) {
            if ((roundness <= 0.0f) || (rec.Width < 1) || (rec.Height < 1)) {
                DrawRectangleRec(rec, color);
                return;
            }

            if (roundness >= 1.0f) roundness = 1.0f;

            float radius = (rec.Width > rec.Height) ? (rec.Height * roundness) / 2 : (rec.Width * roundness) / 2;
            if (radius <= 0.0f) return;

            if (segments < 4) {
                float th = (float)Math.Acos(2 * Math.Pow(1 - SMOOTH_CIRCLE_ERROR_RATE / radius, 2) - 1);
                segments = (int)(Math.Ceiling(2 * PI / th) / 4.0f);
                if (segments <= 0) segments = 4;
            }

            float stepLength = 90.0f / segments;

            /*  Quick sketch to make sense of all of this (there are 9 parts to draw, also mark the 12 points we'll use below)
             *  Not my best attempt at ASCII art, just preted it's rounded rectangle :)
             *     P0                    P1
             *       ____________________
             *     /|                    |\
             *    /1|          2         |3\
             *P7 /__|____________________|__\ P2
             *  |   |P8                P9|   |
             *  | 8 |          9         | 4 |
             *  | __|____________________|__ |
             *P6 \  |P11              P10|  / P3
             *    \7|          6         |5/
             *     \|____________________|/
             *     P5                    P4
             */

            Vector2[] point = {
                new Vector2(rec.X + radius, rec.Y), new Vector2((rec.X + rec.Width) - radius, rec.Y), new Vector2( rec.X + rec.Width, rec.Y + radius ), // PO, P1, P2
                new Vector2(rec.X + rec.Width, (rec.Y + rec.Height) - radius), new Vector2((rec.X + rec.Width) - radius, rec.Y + rec.Height), // P3, P4
                new Vector2(rec.X + radius, rec.Y + rec.Height), new Vector2( rec.X, rec.Y + rec.Height - radius), new Vector2(rec.X, rec.Y + radius), // P5, P6, P7
                new Vector2(rec.X + radius, rec.Y + radius), new Vector2((rec.X + rec.Width) - radius, rec.Y + radius), // P8, P9
                new Vector2(rec.X + rec.Width - radius, (rec.Y + rec.Height) - radius), new Vector2(rec.X + radius, (rec.Y + rec.Height) - radius) // P10, P11
            };

            Vector2[] centers = { point[8], point[9], point[10], point[11] };
            float[] angles = { 180.0f, 90.0f, 0.0f, 270.0f };

            CheckBufferLimitMode(12 * segments + 30, PrimitiveType.TriangleList, defaultTexture);
            for (int k = 0; k < 4; ++k) {
                float angle = angles[k];
                Vector2 center = centers[k];
                for (int i = 0; i < segments; i++) {
                    data[index++].Set(center.X, center.Y, 0, 0, color);
                    data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * angle) * radius, center.Y + (float)Math.Cos(DEG2RAD * angle) * radius, 0, 0, color);
                    data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * (angle + stepLength)) * radius, center.Y + (float)Math.Cos(DEG2RAD * (angle + stepLength)) * radius, 0, 0, color);
                    angle += stepLength;
                }
            }

            data[index++].Set(point[0].X, point[0].Y, 0, 0, color);
            data[index++].Set(point[8].X, point[8].Y, 0, 0, color);
            data[index++].Set(point[9].X, point[9].Y, 0, 0, color);
            data[index++].Set(point[1].X, point[1].Y, 0, 0, color);
            data[index++].Set(point[0].X, point[0].Y, 0, 0, color);
            data[index++].Set(point[9].X, point[9].Y, 0, 0, color);

            data[index++].Set(point[9].X, point[9].Y, 0, 0, color);
            data[index++].Set(point[10].X, point[10].Y, 0, 0, color);
            data[index++].Set(point[3].X, point[3].Y, 0, 0, color);
            data[index++].Set(point[2].X, point[2].Y, 0, 0, color);
            data[index++].Set(point[9].X, point[9].Y, 0, 0, color);
            data[index++].Set(point[3].X, point[3].Y, 0, 0, color);

            data[index++].Set(point[11].X, point[11].Y, 0, 0, color);
            data[index++].Set(point[5].X, point[5].Y, 0, 0, color);
            data[index++].Set(point[4].X, point[4].Y, 0, 0, color);
            data[index++].Set(point[10].X, point[10].Y, 0, 0, color);
            data[index++].Set(point[11].X, point[11].Y, 0, 0, color);
            data[index++].Set(point[4].X, point[4].Y, 0, 0, color);

            data[index++].Set(point[7].X, point[7].Y, 0, 0, color);
            data[index++].Set(point[6].X, point[6].Y, 0, 0, color);
            data[index++].Set(point[11].X, point[11].Y, 0, 0, color);
            data[index++].Set(point[8].X, point[8].Y, 0, 0, color);
            data[index++].Set(point[7].X, point[7].Y, 0, 0, color);
            data[index++].Set(point[11].X, point[11].Y, 0, 0, color);

            data[index++].Set(point[8].X, point[8].Y, 0, 0, color);
            data[index++].Set(point[11].X, point[11].Y, 0, 0, color);
            data[index++].Set(point[10].X, point[10].Y, 0, 0, color);
            data[index++].Set(point[9].X, point[9].Y, 0, 0, color);
            data[index++].Set(point[8].X, point[8].Y, 0, 0, color);
            data[index++].Set(point[10].X, point[10].Y, 0, 0, color);

        }
        public static void DrawRectangleRoundedLines(in Rectangle rec, float roundness, int segments, int lineThick, in Color color) {
            if (lineThick < 0) lineThick = 0;

            // Not a rounded rectangle
            if (roundness <= 0.0f) {
                DrawRectangleLinesEx(new Rectangle(rec.X - lineThick, rec.Y - lineThick, rec.Width + 2 * lineThick, rec.Height + 2 * lineThick), lineThick, color);
                return;
            }

            if (roundness >= 1.0f) roundness = 1.0f;

            // Calculate corner radius
            float radius = (rec.Width > rec.Height) ? (rec.Height * roundness) / 2 : (rec.Width * roundness) / 2;
            if (radius <= 0.0f) return;

            // Calculate number of segments to use for the corners
            if (segments < 4) {
                // Calculate the maximum angle between segments based on the error rate (usually 0.5f)
                float th = (float)Math.Acos(2 * Math.Pow(1 - SMOOTH_CIRCLE_ERROR_RATE / radius, 2) - 1);
                segments = (int)(Math.Ceiling(2 * PI / th) / 2.0f);
                if (segments <= 0) segments = 4;
            }

            float stepLength = 90.0f / segments;
            float outerRadius = radius + (float)lineThick, innerRadius = radius;

            Vector2[] point = {
                new Vector2(rec.X + innerRadius, rec.Y - lineThick), new Vector2((rec.X + rec.Width) - innerRadius, rec.Y - lineThick), new Vector2( rec.X + rec.Width + lineThick, rec.Y + innerRadius ), // PO, P1, P2
                new Vector2(rec.X + rec.Width + lineThick, (rec.Y + rec.Height) - innerRadius), new Vector2((rec.X + rec.Width) - innerRadius, rec.Y + rec.Height + lineThick), // P3, P4
                new Vector2(rec.X + innerRadius, rec.Y + rec.Height + lineThick), new Vector2(rec.X - lineThick, (rec.Y + rec.Height) - innerRadius), new Vector2(rec.X - lineThick, rec.Y + innerRadius), // P5, P6, P7
                new Vector2(rec.X + innerRadius, rec.Y), new Vector2((rec.X + rec.Width) - innerRadius, rec.Y), // P8, P9
                new Vector2( rec.X + rec.Width, rec.Y + innerRadius ), new Vector2(rec.X + rec.Width, (rec.Y + rec.Height) - innerRadius), // P10, P11
                new Vector2((rec.X + rec.Width) - innerRadius, rec.Y + rec.Height), new Vector2(rec.X + innerRadius, rec.Y + rec.Height), // P12, P13
                new Vector2(rec.X, (rec.Y + rec.Height) - innerRadius), new Vector2(rec.X, rec.Y + innerRadius) // P14, P15
            };

            Vector2[] centers = {
                new Vector2(rec.X + innerRadius, rec.Y + innerRadius), new Vector2(rec.X + rec.Width - innerRadius, rec.Y + innerRadius), // P16, P17
                new Vector2(rec.X + rec.Width - innerRadius, rec.Y + rec.Height - innerRadius), new Vector2(rec.X + innerRadius, rec.Y + rec.Height  - innerRadius) // P18, P19
            };

            float[] angles = { 180.0f, 90.0f, 0.0f, 270.0f };

            if (lineThick > 1) {
                CheckBufferLimitMode(24 * segments + 24, PrimitiveType.TriangleList, defaultTexture);
                for (int k = 0; k < 4; ++k) {
                    float angle = angles[k];
                    Vector2 center = centers[k];

                    for (int i = 0; i < segments; i++) {

                        data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * angle) * innerRadius, center.Y + (float)Math.Cos(DEG2RAD * angle) * innerRadius, 0, 0, color);
                        data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * angle) * outerRadius, center.Y + (float)Math.Cos(DEG2RAD * angle) * outerRadius, 0, 0, color);
                        data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * (angle + stepLength)) * innerRadius, center.Y + (float)Math.Cos(DEG2RAD * (angle + stepLength)) * innerRadius, 0, 0, color);
                        data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * (angle + stepLength)) * innerRadius, center.Y + (float)Math.Cos(DEG2RAD * (angle + stepLength)) * innerRadius, 0, 0, color);
                        data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * angle) * outerRadius, center.Y + (float)Math.Cos(DEG2RAD * angle) * outerRadius, 0, 0, color);
                        data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * (angle + stepLength)) * outerRadius, center.Y + (float)Math.Cos(DEG2RAD * (angle + stepLength)) * outerRadius, 0, 0, color);

                        angle += stepLength;
                    }
                }

                data[index++].Set(point[0].X, point[0].Y, 0, 0, color);
                data[index++].Set(point[8].X, point[8].Y, 0, 0, color);
                data[index++].Set(point[9].X, point[9].Y, 0, 0, color);
                data[index++].Set(point[1].X, point[1].Y, 0, 0, color);
                data[index++].Set(point[0].X, point[0].Y, 0, 0, color);
                data[index++].Set(point[9].X, point[9].Y, 0, 0, color);

                data[index++].Set(point[10].X, point[10].Y, 0, 0, color);
                data[index++].Set(point[11].X, point[11].Y, 0, 0, color);
                data[index++].Set(point[3].X, point[3].Y, 0, 0, color);
                data[index++].Set(point[2].X, point[2].Y, 0, 0, color);
                data[index++].Set(point[10].X, point[10].Y, 0, 0, color);
                data[index++].Set(point[3].X, point[3].Y, 0, 0, color);

                data[index++].Set(point[13].X, point[13].Y, 0, 0, color);
                data[index++].Set(point[5].X, point[5].Y, 0, 0, color);
                data[index++].Set(point[4].X, point[4].Y, 0, 0, color);
                data[index++].Set(point[12].X, point[12].Y, 0, 0, color);
                data[index++].Set(point[13].X, point[13].Y, 0, 0, color);
                data[index++].Set(point[4].X, point[4].Y, 0, 0, color);

                data[index++].Set(point[7].X, point[7].Y, 0, 0, color);
                data[index++].Set(point[6].X, point[6].Y, 0, 0, color);
                data[index++].Set(point[14].X, point[14].Y, 0, 0, color);
                data[index++].Set(point[15].X, point[15].Y, 0, 0, color);
                data[index++].Set(point[7].X, point[7].Y, 0, 0, color);
                data[index++].Set(point[14].X, point[14].Y, 0, 0, color);

            } else {
                CheckBufferLimitMode(8 * segments + 8, PrimitiveType.LineList, defaultTexture);

                for (int k = 0; k < 4; ++k) {
                    float angle = angles[k];
                    Vector2 center = centers[k];

                    for (int i = 0; i < segments; i++) {
                        data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * angle) * outerRadius, center.Y + (float)Math.Cos(DEG2RAD * angle) * outerRadius, 0, 0, color);
                        data[index++].Set(center.X + (float)Math.Sin(DEG2RAD * (angle + stepLength)) * outerRadius, center.Y + (float)Math.Cos(DEG2RAD * (angle + stepLength)) * outerRadius, 0, 0, color);
                        angle += stepLength;
                    }
                }

                for (int i = 0; i < 8; i += 2) {
                    data[index++].Set(point[i].X, point[i].Y, 0, 0, color);
                    data[index++].Set(point[i + 1].X, point[i + 1].Y, 0, 0, color);
                }

            }
        }

        public static void DrawTriangle(in Vector2 v1, in Vector2 v2, in Vector2 v3, in Color color) {
            CheckBufferLimitMode(3, PrimitiveType.TriangleList, defaultTexture);

            data[index++].Set(v1.X, v1.Y, 0, 0, color);
            data[index++].Set(v2.X, v2.Y, 0, 0, color);
            data[index++].Set(v3.X, v3.Y, 0, 0, color);
        }
        public static void DrawTriangleLines(in Vector2 v1, in Vector2 v2, in Vector2 v3, in Color color) {
            CheckBufferLimitMode(6, PrimitiveType.LineList, defaultTexture);

            data[index++].Set(v1.X, v1.Y, 0, 0, color);
            data[index++].Set(v2.X, v2.Y, 0, 0, color);
            data[index++].Set(v2.X, v2.Y, 0, 0, color);

            data[index++].Set(v3.X, v3.Y, 0, 0, color);
            data[index++].Set(v3.X, v3.Y, 0, 0, color);
            data[index++].Set(v1.X, v1.Y, 0, 0, color);
        }
        public static void DrawTriangleFan(in Vector2[] points, in Color color) {
            if (points.Length >= 3) {
                CheckBufferLimitMode((points.Length - 2) * 6, PrimitiveType.TriangleList, defaultTexture);
                for (int i = 1; i < points.Length - 1; i++) {

                    data[index++].Set(points[0].X, points[0].Y, 0, 0, color);           //TL
                    data[index++].Set(points[i + 1].X, points[i + 1].Y, 0, 0, color);   //TR
                    data[index++].Set(points[i].X, points[i].Y, 0, 0, color);           //DL

                    data[index++].Set(points[i + 1].X, points[i + 1].Y, 0, 0, color);   //TR
                    data[index++].Set(points[i + 1].X, points[i + 1].Y, 0, 0, color);   //DR
                    data[index++].Set(points[i].X, points[i].Y, 0, 0, color);           //DL
                }

            }
        }
        public static void DrawTriangleStrip(in Vector2[] points, in Color color) {
            if (points.Length >= 3) {
                CheckBufferLimitMode(3 * (points.Length - 2), PrimitiveType.TriangleList, defaultTexture);

                for (int i = 2; i < points.Length; i++) {
                    if ((i % 2) == 0) {
                        data[index++].Set(points[i].X, points[i].Y, 0, 0, color);
                        data[index++].Set(points[i - 2].X, points[i - 2].Y, 0, 0, color);
                        data[index++].Set(points[i - 1].X, points[i - 1].Y, 0, 0, color);
                    } else {
                        data[index++].Set(points[i].X, points[i].Y, 0, 0, color);
                        data[index++].Set(points[i - 1].X, points[i - 1].Y, 0, 0, color);
                        data[index++].Set(points[i - 2].X, points[i - 2].Y, 0, 0, color);
                    }
                }
            }
        }

        public static void DrawPoly(in Vector2 center, int sides, in float radius, in float rotation, in Color color) {
            if (sides < 3) sides = 3;
            float centralAngle = 0.0f;
            float cos = (float)Math.Cos(rotation * DEG2RAD);
            float sin = (float)Math.Sin(rotation * DEG2RAD);
            CheckBufferLimitMode(4 * (360 / sides), PrimitiveType.TriangleList, defaultTexture);
            Vector2 M1;

            for (int i = 0; i < sides; i++) {
                data[index++].Set(center.X, center.Y, 0, 0, color); // const M1
                M1 = new Vector2((float)Math.Sin(DEG2RAD * centralAngle) * radius, (float)Math.Cos(DEG2RAD * centralAngle) * radius);
                data[index++].Set((M1.X * cos) - (M1.Y * sin) + center.X, (M1.X * sin) + (M1.Y * cos) + center.Y, 0, 0, color); //M1

                centralAngle += 360.0f / sides;
                M1 = new Vector2((float)Math.Sin(DEG2RAD * centralAngle) * radius, (float)Math.Cos(DEG2RAD * centralAngle) * radius);
                data[index++].Set((M1.X * cos) - (M1.Y * sin) + center.X, (M1.X * sin) + (M1.Y * cos) + center.Y, 0, 0, color); //M1
            }

        }
        public static void DrawPolyLines(in Vector2 center, int sides, in float radius, in float rotation, in Color color) {
            if (sides < 3) sides = 3;
            float centralAngle = 0.0f;
            float cos = (float)Math.Cos(rotation * DEG2RAD);
            float sin = (float)Math.Sin(rotation * DEG2RAD);
            CheckBufferLimitMode(3 * (360 / sides), PrimitiveType.LineList, defaultTexture);
            Vector2 M1;
            for (int i = 0; i < sides; i++) {
                M1 = new Vector2((float)Math.Sin(DEG2RAD * centralAngle) * radius, (float)Math.Cos(DEG2RAD * centralAngle) * radius);
                data[index++].Set((M1.X * cos) - (M1.Y * sin) + center.X, (M1.X * sin) + (M1.Y * cos) + center.Y, 0, 0, color); //M1

                centralAngle += 360.0f / sides;
                M1 = new Vector2((float)Math.Sin(DEG2RAD * centralAngle) * radius, (float)Math.Cos(DEG2RAD * centralAngle) * radius);
                data[index++].Set((M1.X * cos) - (M1.Y * sin) + center.X, (M1.X * sin) + (M1.Y * cos) + center.Y, 0, 0, color); //M1
            }

        }

        public static void DrawTexture(in Texture2D texture, in float positionX, in float positionY, in Color color) {
            CheckBufferLimitMode(6, PrimitiveType.TriangleList, texture);
            float right = positionX + texture.Width;
            float down = positionY + texture.Height;

            data[index++].Set(positionX, positionY, 0, 0, color);
            data[index++].Set(right, positionY, 1, 0, color);
            data[index++].Set(positionX, down, 0, 1, color);

            data[index++].Set(right, positionY, 1, 0, color);
            data[index++].Set(right, down, 1, 1, color);
            data[index++].Set(positionX, down, 0, 1, color);

        }
        private static void DrawTextureInternal(in float positionX, in float positionY, in float right, in float down, in Color color, in TextureFlip flip = TextureFlip.None) {

            switch (flip) {
                case TextureFlip.None:
                    data[index++].Set(positionX, positionY, 0, 0, color);
                    data[index++].Set(right, positionY, 1, 0, color);
                    data[index++].Set(positionX, down, 0, 1, color);

                    data[index++].Set(right, positionY, 1, 0, color);
                    data[index++].Set(right, down, 1, 1, color);
                    data[index++].Set(positionX, down, 0, 1, color);
                    break;
                case TextureFlip.Horizontal:
                    data[index++].Set(positionX, positionY, 1, 0, color);
                    data[index++].Set(right, positionY, 0, 0, color);
                    data[index++].Set(positionX, down, 1, 1, color);

                    data[index++].Set(right, positionY, 0, 0, color);
                    data[index++].Set(right, down, 0, 1, color);
                    data[index++].Set(positionX, down, 1, 1, color);
                    break;
                case TextureFlip.Vertical:
                    data[index++].Set(positionX, positionY, 0, 1, color);
                    data[index++].Set(right, positionY, 1, 1, color);
                    data[index++].Set(positionX, down, 0, 0, color);

                    data[index++].Set(right, positionY, 1, 1, color);
                    data[index++].Set(right, down, 1, 0, color);
                    data[index++].Set(positionX, down, 0, 0, color);
                    break;
                case TextureFlip.Horizontal | TextureFlip.Vertical:
                    data[index++].Set(positionX, positionY, 1, 1, color);
                    data[index++].Set(right, positionY, 0, 1, color);
                    data[index++].Set(positionX, down, 1, 0, color);

                    data[index++].Set(right, positionY, 0, 1, color);
                    data[index++].Set(right, down, 0, 0, color);
                    data[index++].Set(positionX, down, 1, 0, color);
                    break;
            }
        }
        public static void DrawTexture(in Texture2D texture, in float positionX, in float positionY, in Color color, in TextureFlip flip = TextureFlip.None) {
            CheckBufferLimitMode(6, PrimitiveType.TriangleList, texture);
            float right = positionX + texture.Width;
            float down = positionY + texture.Height;

            DrawTextureInternal(positionX, positionY, right, down, color, flip);

        }
        public static void DrawTexture(in Texture2D texture, in Vector2 position, in Color color, in TextureFlip flip = TextureFlip.None) {
            CheckBufferLimitMode(6, PrimitiveType.TriangleList, texture);
            float right = position.X + texture.Width;
            float down = position.Y + texture.Height;
            DrawTextureInternal(position.X, position.Y, right, down, color, flip);
        }
        public static void DrawTexture(in Texture2D texture, in Vector2 position, in Vector2 origin, in float scale, in Color color, in TextureFlip flip = TextureFlip.None) {
            CheckBufferLimitMode(6, PrimitiveType.TriangleList, texture);
            float posX = position.X - origin.X;
            float posY = position.Y - origin.Y;
            float right = posX + (texture.Width * scale);
            float down = posY + (texture.Height * scale);
            DrawTextureInternal(posX, posY, right, down, color, flip);

        }
        public static void DrawTexture(in Texture2D texture, in Vector2 position, in Vector2 origin, in Vector2 scale, in Color color, in TextureFlip flip = TextureFlip.None) {
            CheckBufferLimitMode(6, PrimitiveType.TriangleList, texture);
            float posX = position.X - origin.X;
            float posY = position.Y - origin.Y;
            float right = posX + (texture.Width * scale.X);
            float down = posY + (texture.Height * scale.Y);
            DrawTextureInternal(posX, posY, right, down, color, flip);
        }
        public static void DrawTexture(in Texture2D texture, in Vector2 position, Vector2 origin, in Vector2 scale, in float rotation, in Color color, in TextureFlip flip = TextureFlip.None) {
            CheckBufferLimitMode(6, PrimitiveType.TriangleList, texture);
            origin = -origin;
            float cos = (float)Math.Cos(rotation * DEG2RAD);
            float sin = (float)Math.Sin(rotation * DEG2RAD);

            Vector2 TL = new Vector2(position.X + origin.X * cos - origin.Y * sin, position.Y + origin.X * sin + origin.Y * cos);
            Vector2 TR = new Vector2(position.X + (origin.X + (texture.Width * scale.X)) * cos - origin.Y * sin, position.Y + (origin.X + (texture.Width * scale.X)) * sin + origin.Y * cos);
            Vector2 BL = new Vector2(position.X + origin.X * cos - (origin.Y + (texture.Height * scale.Y)) * sin, position.Y + origin.X * sin + (origin.Y + (texture.Height * scale.Y)) * cos);
            Vector2 BR = new Vector2(position.X + (origin.X + (texture.Width * scale.X)) * cos - (origin.Y + (texture.Height * scale.Y)) * sin, position.Y + (origin.X + (texture.Width * scale.X)) * sin + (origin.Y + (texture.Height * scale.Y)) * cos);


            switch (flip) {
                case TextureFlip.None:
                    data[index++].Set(TL.X, TL.Y, 0, 0, color);
                    data[index++].Set(TR.X, TR.Y, 1, 0, color);
                    data[index++].Set(BL.X, BL.Y, 0, 1, color);

                    data[index++].Set(TR.X, TR.Y, 1, 0, color);
                    data[index++].Set(BR.X, BR.Y, 1, 1, color);
                    data[index++].Set(BL.X, BL.Y, 0, 1, color);
                    break;
                case TextureFlip.Horizontal:
                    data[index++].Set(TL.X, TL.Y, 1, 0, color);
                    data[index++].Set(TR.X, TR.Y, 0, 0, color);
                    data[index++].Set(BL.X, BL.Y, 1, 1, color);

                    data[index++].Set(TR.X, TR.Y, 0, 0, color);
                    data[index++].Set(BR.X, BR.Y, 0, 1, color);
                    data[index++].Set(BL.X, BL.Y, 1, 1, color);
                    break;
                case TextureFlip.Vertical:
                    data[index++].Set(TL.X, TL.Y, 0, 1, color);
                    data[index++].Set(TR.X, TR.Y, 1, 1, color);
                    data[index++].Set(BL.X, BL.Y, 0, 0, color);

                    data[index++].Set(TR.X, TR.Y, 1, 1, color);
                    data[index++].Set(BR.X, BR.Y, 1, 0, color);
                    data[index++].Set(BL.X, BL.Y, 0, 0, color);
                    break;
                case TextureFlip.Horizontal | TextureFlip.Vertical:
                    data[index++].Set(TL.X, TL.Y, 1, 1, color);
                    data[index++].Set(TR.X, TR.Y, 0, 1, color);
                    data[index++].Set(BL.X, BL.Y, 1, 0, color);

                    data[index++].Set(TR.X, TR.Y, 0, 1, color);
                    data[index++].Set(BR.X, BR.Y, 0, 0, color);
                    data[index++].Set(BL.X, BL.Y, 1, 0, color);
                    break;
            }

        }
        public static void DrawTexture(in Texture2D texture, in Rectangle? source, in Rectangle dest, in Color color, in TextureFlip flip = TextureFlip.None) {
            CheckBufferLimitMode(6, PrimitiveType.TriangleList, texture);

            Vector2 _texCoordTL;
            Vector2 _texCoordBR;
            if (source.HasValue) {
                Rectangle srcRect = source.GetValueOrDefault();
                _texCoordTL.X = srcRect.X / texture.Width;
                _texCoordTL.Y = srcRect.Y / texture.Height;
                _texCoordBR.X = (srcRect.X + srcRect.Width) / texture.Width;
                _texCoordBR.Y = (srcRect.Y + srcRect.Height) / texture.Height;
            } else {
                _texCoordTL = Vector2.Zero;
                _texCoordBR = Vector2.One;
            }

            if ((flip & TextureFlip.Vertical) != 0) {
                float temp = _texCoordBR.Y;
                _texCoordBR.Y = _texCoordTL.Y;
                _texCoordTL.Y = temp;
            }
            if ((flip & TextureFlip.Horizontal) != 0) {
                float temp = _texCoordBR.X;
                _texCoordBR.X = _texCoordTL.X;
                _texCoordTL.X = temp;
            }

            data[index++].Set(dest.X, dest.Y, _texCoordTL.X, _texCoordTL.Y, color);
            data[index++].Set(dest.Right, dest.Y, _texCoordBR.X, _texCoordTL.Y, color);
            data[index++].Set(dest.X, dest.Bottom, _texCoordTL.X, _texCoordBR.Y, color);

            data[index++].Set(dest.Right, dest.Y, _texCoordBR.X, _texCoordTL.Y, color);
            data[index++].Set(dest.Right, dest.Bottom, _texCoordBR.X, _texCoordBR.Y, color);
            data[index++].Set(dest.X, dest.Bottom, _texCoordTL.X, _texCoordBR.Y, color);
        }
        public static void DrawTexture(in Texture2D texture, in Rectangle? source, in Rectangle dest, in Vector2 origin, in float rotation, in Color color, in TextureFlip flip = TextureFlip.None) {
            CheckBufferLimitMode(6, PrimitiveType.TriangleList, texture);

            Vector2 _texCoordTL;
            Vector2 _texCoordBR;
            if (source.HasValue) {
                Rectangle srcRect = source.GetValueOrDefault();
                _texCoordTL.X = srcRect.X / texture.Width;
                _texCoordTL.Y = srcRect.Y / texture.Height;
                _texCoordBR.X = (srcRect.X + srcRect.Width) / texture.Width;
                _texCoordBR.Y = (srcRect.Y + srcRect.Height) / texture.Height;
            } else {
                _texCoordTL = Vector2.Zero;
                _texCoordBR = Vector2.One;
            }

            if ((flip & TextureFlip.Vertical) != 0) {
                float temp = _texCoordBR.Y;
                _texCoordBR.Y = _texCoordTL.Y;
                _texCoordTL.Y = temp;
            }
            if ((flip & TextureFlip.Horizontal) != 0) {
                float temp = _texCoordBR.X;
                _texCoordBR.X = _texCoordTL.X;
                _texCoordTL.X = temp;
            }

            float cos = (float)Math.Cos(rotation * DEG2RAD);
            float sin = (float)Math.Sin(rotation * DEG2RAD);

            Vector2 TL = new Vector2(dest.X + origin.X * cos - origin.Y * sin, dest.Y + origin.X * sin + origin.Y * cos);
            Vector2 TR = new Vector2(dest.X + (origin.X + dest.Width) * cos - origin.Y * sin, dest.Y + (origin.X + dest.Width) * sin + origin.Y * cos);
            Vector2 BL = new Vector2(dest.X + origin.X * cos - (origin.Y + dest.Height) * sin, dest.Y + origin.X * sin + (origin.Y + dest.Height) * cos);
            Vector2 BR = new Vector2(dest.X + (origin.X + dest.Width) * cos - (origin.Y + dest.Height) * sin, dest.Y + (origin.X + dest.Width) * sin + (origin.Y + dest.Height) * cos);

            data[index++].Set(TL.X, TL.Y, _texCoordTL.X, _texCoordTL.Y, color);
            data[index++].Set(TR.X, TR.Y, _texCoordBR.X, _texCoordTL.Y, color);
            data[index++].Set(BL.X, BL.Y, _texCoordTL.X, _texCoordBR.Y, color);

            data[index++].Set(TR.X, TR.Y, _texCoordBR.X, _texCoordTL.Y, color);
            data[index++].Set(BR.X, BR.Y, _texCoordBR.X, _texCoordBR.Y, color);
            data[index++].Set(BL.X, BL.Y, _texCoordTL.X, _texCoordBR.Y, color);

        }

        public static void DrawText(in string text, in float posX, in float posY, float fontSize, in Color color) {
            DrawText(defaultFont, text, posX, posY, fontSize, color);
        }
        public static void DrawText(in SpriteFont font, in string text, in float posX, in float posY, float fontSize, in Color color) {

            Vector2 offset = Vector2.Zero;
            bool firstGlyphOfLine = true;

            for (int i = 0; i < text.Length; i++) {
                char c = text[i];

                if (c == '\n') {
                    offset.X = 0;
                    offset.Y += font.LineSpacing * fontSize;
                    firstGlyphOfLine = true;
                    continue;
                }

                SpriteFont.Glyph glyph = font.GetGlyphs().GetValueOrDefault(c);

                if (firstGlyphOfLine) {
                    offset.X = Math.Max(glyph.LeftSideBearing, 0);
                    firstGlyphOfLine = false;
                } else {
                    offset.X += font.Spacing + glyph.LeftSideBearing;
                }
                
                Vector2 position = offset;
                position.X += (glyph.Cropping.X * fontSize) + posX;
                position.Y += (glyph.Cropping.Y * fontSize) + posY;

                float right = position.X + (glyph.BoundsInTexture.Width * fontSize);
                float down = position.Y + (glyph.BoundsInTexture.Height * fontSize);

                float TLx = glyph.BoundsInTexture.X / (float)font.Texture.Width;
                float TLy = glyph.BoundsInTexture.Y / (float)font.Texture.Height;
                float BRx = (glyph.BoundsInTexture.X + glyph.BoundsInTexture.Width) / (float)font.Texture.Width;
                float BRy = (glyph.BoundsInTexture.Y + glyph.BoundsInTexture.Height) / (float)font.Texture.Height;

                CheckBufferLimitMode(6, PrimitiveType.TriangleList, font.Texture);

                data[index++].Set(position.X, position.Y, TLx, TLy, color); //TL
                data[index++].Set(right, position.Y, BRx, TLy, color);  //TR
                data[index++].Set(position.X, down, TLx, BRy, color); //BL

                data[index++].Set(right, position.Y, BRx, TLy, color); //TR
                data[index++].Set(right, down, BRx, BRy, color); //BR
                data[index++].Set(position.X, down, TLx, BRy, color); //BL

                offset.X += (glyph.Width + glyph.RightSideBearing) * fontSize;
            }

        }

        public static void DrawTextRec(in string text, in Rectangle rec, in float fontSize, in bool wordWrap, in Color color) {
            DrawTextRecEx(defaultFont, text, rec, fontSize, wordWrap, color);
        }
        public static void DrawTextRecEx(in SpriteFont font, in string text, in Rectangle rec, in float fontSize, in bool wordWrap, in Color color) {
            float textOffsetY = 0;            // Offset between lines (on line break '\n')
            float textOffsetX = 0.0f;       // Offset X to next character to draw

            int startLine = -1;         // Index where to begin drawing (where a line begins)
            int endLine = -1;           // Index where to stop drawing (where a line ends)
            int lastk = -1;             // Holds last value of the character position

            Warp state = wordWrap ? Warp.MEASURE_STATE : Warp.DRAW_STATE;
            for (int i = 0, k = 0; i < text.Length; i++, k++) {
                
                char c = text[i];
                SpriteFont.Glyph glyph = font.GetGlyphs().GetValueOrDefault(c);
                
                float glyphWidth = 0;
                if (c != '\n') {
                    glyphWidth = (glyph.BoundsInTexture.Width * fontSize) + font.Spacing;
                }

                if (state == Warp.MEASURE_STATE) {
                    if ((c == ' ') || (c == '\t') || (c == '\n')) endLine = i;

                    if ((textOffsetX + glyphWidth + 1) >= rec.Width) {
                        
                        endLine = (endLine < 1) ? i : endLine;
                        state = Warp.DRAW_STATE;

                    } else if ((i + 1) == text.Length) {
                        endLine = i;

                        state = Warp.DRAW_STATE;
                    } else if (c == '\n') state = Warp.DRAW_STATE;

                    if (state == Warp.DRAW_STATE) {
                        textOffsetX = 0;
                        i = startLine;
                        glyphWidth = 0;

                        // Save character position when we switch states
                        int tmp = lastk;
                        lastk = k - 1;
                        k = tmp;
                    }
                } else {
                    if (c == '\n') {
                        if (!wordWrap) {
                            textOffsetY += (font.LineSpacing * fontSize);
                            textOffsetX = 0;
                        }
                    } else {
                        if (!wordWrap && ((textOffsetX + glyphWidth + 1) >= rec.Width)) {
                            textOffsetY += (font.LineSpacing * fontSize);
                            textOffsetX = 0;
                        }

                        if ((textOffsetY + (font.LineSpacing * fontSize)) > rec.Height) break;
                        
                        if ((c != ' ') && (c != '\t')) {

                            Vector2 position = new Vector2(rec.X + textOffsetX, rec.Y + textOffsetY);
                            float right = position.X + (glyph.BoundsInTexture.Width * fontSize);
                            float down = position.Y + (glyph.BoundsInTexture.Height * fontSize);

                            float TLx = glyph.BoundsInTexture.X / (float)font.Texture.Width;
                            float TLy = glyph.BoundsInTexture.Y / (float)font.Texture.Height;
                            float BRx = (glyph.BoundsInTexture.X + glyph.BoundsInTexture.Width) / (float)font.Texture.Width;
                            float BRy = (glyph.BoundsInTexture.Y + glyph.BoundsInTexture.Height) / (float)font.Texture.Height;

                            CheckBufferLimitMode(6, PrimitiveType.TriangleList, font.Texture);

                            data[index++].Set(position.X, position.Y, TLx, TLy, color); //TL
                            data[index++].Set(right, position.Y, BRx, TLy, color);  //TR
                            data[index++].Set(position.X, down, TLx, BRy, color); //BL

                            data[index++].Set(right, position.Y, BRx, TLy, color); //TR
                            data[index++].Set(right, down, BRx, BRy, color); //BR
                            data[index++].Set(position.X, down, TLx, BRy, color); //BL

                        }

                    }

                    if (wordWrap && (i == endLine)) {
                        textOffsetY += (font.LineSpacing * fontSize);
                        textOffsetX = 0;
                        startLine = endLine;
                        endLine = -1;
                        glyphWidth = 0;
                        k = lastk;

                        state = Warp.MEASURE_STATE;
                    }

                }

                textOffsetX += glyphWidth + glyph.RightSideBearing;
            }

        }

        #endregion DRAW FUNCTIONS

        #region SHADER FUNCTIONS

        public static void BeginShader(in Effect effect) {
            index = 0;
            DrawCall = 0;
            effect.CurrentTechnique.Passes[0].Apply();
        }
        public static void BeginShader(in Effect effect, in BlendState? blendState = null) {
            index = 0;
            DrawCall = 0;
            Core.Game.GraphicsDevice.BlendState = blendState ?? BlendState.AlphaBlend;
            effect.CurrentTechnique.Passes[0].Apply();
        }
        public static void BeginShader(in Effect effect, in BlendState? blendState = null, in SamplerState? samplerState = null) {
            index = 0;
            DrawCall = 0;
            Core.Game.GraphicsDevice.BlendState = blendState ?? BlendState.AlphaBlend;
            Core.Game.GraphicsDevice.SamplerStates[0] = samplerState ?? SamplerState.PointClamp;
            effect.CurrentTechnique.Passes[0].Apply();
        }
        public static void BeginShader(in Effect effect, in BlendState? blendState = null, in RasterizerState? rasterizerState = null, in SamplerState? samplerState = null) {
            index = 0;
            DrawCall = 0;
            Core.Game.GraphicsDevice.BlendState = blendState ?? BlendState.AlphaBlend;
            Core.Game.GraphicsDevice.RasterizerState = rasterizerState ?? RasterizerState.CullNone;
            Core.Game.GraphicsDevice.SamplerStates[0] = samplerState ?? SamplerState.PointClamp;
            effect.CurrentTechnique.Passes[0].Apply();
        }

        public static void GenVertex(in int vertexCount, in PrimitiveType type) {
            if (currentPrimitive != type) {
                End();
                currentPrimitive = type;
            } else if (index + vertexCount > data.Length)
                End();
        }
        public static void AddVertex(in float PositionX, in float PositionY, in float TexcoordX, in float TexcoordY, in Color color) {
            data[index++].Set(PositionX, PositionY, TexcoordX, TexcoordY, color);
        }

        #endregion SHADER FUNCTIONS

    }
}
