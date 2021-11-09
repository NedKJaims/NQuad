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

        public Camera(Vector2 offset, Vector2 target, float rotation = 0f, float zoom = 1f) {
            Offset = offset;
            Target = target;
            Rotation = rotation;
            this.zoom = MathHelper.Clamp(zoom, -10f, 10f);
        }

    }
}
