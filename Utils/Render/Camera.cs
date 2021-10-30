using Microsoft.Xna.Framework;

namespace NQuad.Utils.Render {
    public class Camera {
        public Matrix Matrix => Matrix.CreateRotationZ(Rotation) * Matrix.CreateScale(Zoom) * Matrix.CreateTranslation(Target.X - Offset.X, Target.Y - Offset.Y, 0);
        
        public Vector2 Offset;
        public Vector2 Target;
        
        public float Rotation { get; set; }

        private float zoom;
        public float Zoom {
            get { return zoom; }
            set { zoom = MathHelper.Clamp(value, -10f, 10f); }
        }

        public Camera(Vector2 off, Vector2 targ, float rot = 0f, float _zoom = 1f) {
            Offset = off;
            Target = targ;
            Rotation = rot;
            zoom = MathHelper.Clamp(_zoom, -10f, 10f);
        }

    }
}
