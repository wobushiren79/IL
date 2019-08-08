using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface ITextInfoView
{
    void GetTextInfoSuccess(List<TextInfoBean> listData);

    void GetTextInfoFail();
}