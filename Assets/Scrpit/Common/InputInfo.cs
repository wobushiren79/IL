using UnityEngine;
using UnityEditor;

public class InputInfo : ScriptableObject
{
    //上下移动
    public static string Horizontal = "Horizontal";
    public static string Vertical = "Vertical";

    //取消 鼠标右键
    public static string Cancel = "Cancel";

    //鼠标左键 鼠标左键 
    public static string Confirm = "Confirm";

    //选择按钮E
    public static string Rotate_Right = "Rotate_Right";

    //选择按钮Q
    public static string Rotate_Left = "Rotate_Left";

    //互动按钮E
    public static string Interactive_E = "Interactive_E";
}