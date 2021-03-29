using Microsoft.Xna.Framework;

namespace NQuad.Utils
{
    public class Camera
    {
        public Matrix Matrix => Matrix.CreateRotationZ(rotation) * Matrix.CreateScale(Zoom) * Matrix.CreateTranslation(target.X - offset.X, target.Y - offset.Y, 0);
        public Vector2 offset;
        public Vector2 target;
        public float rotation;
        private float zoom;
        public float Zoom {
            get { return zoom; }
            set { SetZoom(value); }
        }
        public Camera(in Vector2 off, in Vector2 targ, in float rot = 0f, in float _zoom = 1f) {
            offset = off;
            target = targ;
            rotation = rot;
            zoom = (_zoom > 0f) ? _zoom : 0;
        }
        private void SetZoom(in float zoom) {
            if (zoom > -10f && zoom < 10f)
                this.zoom = zoom;
        }
        public void SetRelativeZoom(in float zoom) {
            if (zoom > -10f && zoom < 10f)
                this.zoom += zoom;
        }

    }
}
