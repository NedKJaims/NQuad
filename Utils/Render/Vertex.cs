using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace NQuad.Utils
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct Vertex : IVertexType
    {
        [DataMember]
        private Vector2 Position;
        [DataMember]
        private Vector2 Texcoord;
        [DataMember]
        private Color Color;
        
        public static readonly VertexDeclaration VertexDeclaration;

        public Vertex(in Vector2 position, in Vector2 texcoord, in Color color) {
            Position = position;
            Texcoord = texcoord;
            Color = color;
        }

        public void Set(in float x, in float y, in float TXx, in float TYy, in Color col) {
            Position.X = x;
            Position.Y = y;
            Texcoord.X = TXx;
            Texcoord.Y = TYy;
            Color = col;
        }

        VertexDeclaration IVertexType.VertexDeclaration {
            get {
                return VertexDeclaration;
            }
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = Position.GetHashCode();
                hashCode = (hashCode * 397) ^ Texcoord.GetHashCode();
                hashCode = (hashCode * 397) ^ Color.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString() {
            return "{{Position:" + this.Position + " Color:" + this.Color + " TextureCoordinate:" + this.Texcoord + "}}";
        }

        public static bool operator ==(in Vertex left, in Vertex right) {
            return (left.Position == right.Position) && (left.Color == right.Color) && (left.Texcoord == right.Texcoord);
        }

        public static bool operator !=(in Vertex left, in Vertex right) {
            return !(left == right);
        }

        public override bool Equals(object obj) {
            if (obj == null)
                return false;

            if (obj.GetType() != base.GetType())
                return false;

            return (this == ((Vertex)obj));
        }

        static Vertex() {
            var elements = new VertexElement[]
            {
                new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
                new VertexElement(8, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                new VertexElement(16, VertexElementFormat.Color, VertexElementUsage.Color, 0)
            };
            VertexDeclaration = new VertexDeclaration(elements);
        }

    }

}
