using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface ICookingThemeView 
{
     void GetAllCookingThemeSuccess(List<CookingThemeBean> listData);

     void GetAllCookingThemeFail();    
}