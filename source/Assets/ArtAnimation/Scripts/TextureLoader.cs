using UnityEngine;

public static class TextureLoader
{
    public static void Copy(Texture2D source, Texture2D dest)
    {
        Color32[] sourceColors = source.GetPixels32();
        Color32[] destColors = dest.GetPixels32();

        int width = Mathf.Min(source.width, dest.width);
        int height = Mathf.Min(source.height, dest.height);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int sourceIndex = GetIndex(source, i, j);
                int destIndex = GetIndex(dest, i, j);

                destColors[destIndex].r = sourceColors[sourceIndex].r;
                destColors[destIndex].g = sourceColors[sourceIndex].g;
                destColors[destIndex].b = sourceColors[sourceIndex].b;
            }
        }

        dest.SetPixels32(destColors);
        dest.Apply();
    }

    public static void ClearOverlap(Texture2D target, Texture2D mask)
    {
        Color32[] targetColors = target.GetPixels32();
        Color32[] maskColors = mask.GetPixels32();

        int width = Mathf.Min(target.width, mask.width);
        int height = Mathf.Min(target.height, mask.height);

        for (int j = 0; j < height; j++)
        {
            Color32 ClearColor = targetColors[GetIndex(target, width - 1, j)];

            for (int i = 0; i < width; i++)
            {
                int targetIndex = GetIndex(target, i, j);
                int maskIndex = GetIndex(mask, i, j);

                if (maskColors[maskIndex].a != 0)
                    targetColors[targetIndex] = ClearColor;
            }
        }

        target.SetPixels32(targetColors);
        target.Apply();
    }

    public static int GetIndex(Texture2D texture, int x, int y)
    {
        return x + (y * texture.width);
    }
}