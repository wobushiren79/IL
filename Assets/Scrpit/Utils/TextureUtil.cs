using UnityEditor;
using UnityEngine;

public class TextureUtil
{
    /// <summary>
    /// 获取像素
    /// </summary>
    /// <param name="tex"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Color GetPixel(Texture2D tex, Vector2Int position)
    {
        return tex.GetPixel(position.x, position.y);
    }

    /// <summary>
    /// sprite转Texture2d 用于图集里的图片
    /// </summary>
    /// <param name="sprite"></param>
    /// <returns></returns>
    //public static Texture2D SpriteToTexture2D(Sprite sprite)
    //{
    //    Texture2D texture = new Texture2D
    //    (
    //        (int)sprite.rect.width,
    //        (int)sprite.rect.height
    //    );

    //    Color[] pixels = sprite.texture.GetPixels(
    //        (int)sprite.textureRect.x,
    //        (int)sprite.textureRect.y,
    //        (int)sprite.textureRect.width,
    //        (int)sprite.textureRect.height
    //    );
    //    texture.SetPixels(pixels);
    //    texture.filterMode = FilterMode.Point;
    //    texture.Apply();
    //    return texture;
    //}

    public static Texture2D SpriteToTexture2D(Sprite sprite)
    {
        try
        {
            if (sprite.rect.width != sprite.texture.width)
            {
                Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                Color[] newColors = sprite.texture.GetPixels((int)(sprite.textureRect.x),
                                                             (int)(sprite.textureRect.y),
                                                             (int)(sprite.textureRect.width),
                                                             (int)(sprite.textureRect.height));
                newText.SetPixels(newColors);
                newText.filterMode = FilterMode.Point;
                newText.Apply();
                return newText;
            }
            else
                return sprite.texture;
        }
        catch
        {
            return sprite.texture;
        }
    }

}