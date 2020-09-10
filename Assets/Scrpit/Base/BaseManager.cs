using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
public class BaseManager : BaseMonoBehaviour
{
    /// <summary>
    /// 根据名字获取音频
    /// </summary>
    /// <param name="name"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public virtual GameObject GetGameObjectByName(string name, GameObjectDictionary map)
    {
        if (name == null)
            return null;
        if (map.TryGetValue(name, out GameObject obj))
            return obj;
        else
            return null;
    }

    /// <summary>
    /// 根据名字获取音频
    /// </summary>
    /// <param name="name"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public virtual AudioClip GetAudioClipByName(string name,AudioBeanDictionary map)
    {
        if (name == null)
            return null;
        if (map.TryGetValue(name, out AudioClip audioClip))
            return audioClip;
        else
            return null;
    }

    /// <summary>
    /// 根据名字获取动画
    /// </summary>
    /// <param name="name"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public virtual AnimationClip GetAnimClipByName(string name, AnimBeanDictionary map)
    {
        if (name == null)
            return null;
        if (map.TryGetValue(name, out AnimationClip animClip))
            return animClip;
        else
            return null;
    }


    /// <summary>
    /// 根据名字获取tile
    /// </summary>
    /// <param name="name"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public virtual TileBase GetTileBaseByName(string name, TileBeanDictionary map)
    {
        if (name == null)
            return null;
        if (map.TryGetValue(name, out TileBase tile))
            return tile;
        else
            return null;
    }

    /// <summary>
    /// 根据名字获取图标
    /// </summary>
    /// <param name="name"></param>
    /// <param name="listdata"></param>
    /// <returns></returns>
    public virtual Sprite GetSpriteByName(string name, List<IconBean> listdata)
    {
        IconBean iconData = BeanUtil.GetIconBeanByName(name, listdata);
        if (iconData == null)
            return null;
        return iconData.value;
    }

    /// <summary>
    /// 根据位置获取图标
    /// </summary>
    /// <param name="positon"></param>
    /// <param name="listdata"></param>
    /// <returns></returns>
    public virtual Sprite GetSpriteByPosition(int position, List<IconBean> listdata) {
        IconBean iconData = BeanUtil.GetIconBeanByPosition(position, listdata);
        if (iconData == null)
            return null;
        return iconData.value;
    }

    /// <summary>
    /// 通过名字获取Icon
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public virtual Sprite GetSpriteByName(string name,IconBeanDictionary map)
    {
        if (name == null)
            return null;
        if (map.TryGetValue(name, out Sprite spIcon))
            return spIcon;
        else
            return null;  
    }

    public virtual Sprite GetSpriteByName(string name, SpriteAtlas spriteAtlas)
    {
       return spriteAtlas.GetSprite(name);
    }

    /// <summary>
    /// 通过ID获取数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public virtual T GetDataById<T>(long name, Dictionary<long,T> map) where T : class
    {
        if (map == null)
            return null;
        if (map.TryGetValue(name, out T itemData))
            return itemData;
        else
            return null;
    }
}