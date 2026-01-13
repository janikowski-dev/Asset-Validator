using System.Numerics;
using System.Text.Json;

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

    internal static bool TryReadValue(Asset asset, string key, out int value)
    {
        value = -1;

        if (!asset.Metadata.TryGetValue(key, out JsonElement jsonValue))
        {
            return false;
        }

        if (jsonValue.ValueKind != JsonValueKind.Number)
        {
            return false;
        }

        if (!jsonValue.TryGetInt32(out int intValue))
        {
            return false;
        }
        
        value = intValue;
        return true;
    }

    internal static bool TryReadValue(Asset asset, string key, out Vector3 value)
    {
        value = Vector3.NaN;

        if (!asset.Metadata.TryGetValue(key, out JsonElement eulerElement))
        {
            return false;
        }

        if (eulerElement.ValueKind != JsonValueKind.Array)
        {
            return false;
        }

        if (eulerElement.GetArrayLength() != 3)
        {
            return false;
        }

        value = new Vector3(
            eulerElement[0].GetSingle(),
            eulerElement[1].GetSingle(),
            eulerElement[2].GetSingle()
        );
        return true;
    }
}