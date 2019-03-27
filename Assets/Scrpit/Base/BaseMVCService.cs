using UnityEngine;
using UnityEditor;

public abstract class BaseMVCService<T>
{
    /// <summary>
    /// 增加数据
    /// </summary>
    /// <param name="data"></param>
    public abstract T Add(T data);

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="data"></param>
    public abstract T Delete(T data);

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    public abstract T Update(T data);

    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="data"></param>
    public abstract T Query(T data);
}