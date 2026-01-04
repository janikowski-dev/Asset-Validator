namespace AssetValidator.Core.Domain;

internal static class MetadataKeys
{
    internal static class Image
    {
        public const string Width = "Image.Width";
        public const string Height = "Image.Height";
    }

    internal static class Mesh
    {
        public const string VertexCount = "Mesh.VertexCount";
        public const string TriangleCount = "Mesh.TriangleCount";
        public const string NgonCount = "Mesh.NgonCount";
    }

    internal static class Transform
    {
        public const string Scale = "Transform.Scale";
        public const string RotationEuler = "Transform.RotationEuler";
    }
}