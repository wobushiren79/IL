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
    //上档键
    public static string Shift = "Shift";
    //选择按钮E
    public static string Rotate_Right = "Rotate_Right";

    //选择按钮Q
    public static string Rotate_Left = "Rotate_Left";

    //互动按钮E
    public static string Interactive_E = "Interactive_E";

    //互动按钮E
    public static string Interactive_Space = "Interactive_Space";

    //方向 左
    public static string Direction_Left = "Direction_Left";
    //方向 右
    public static string Direction_Right = "Direction_Right";
    //方向 右
    public static string Direction_Up = "Direction_Up";
    //方向 右
    public static string Direction_Down = "Direction_Down";

    //数字键
    public static string Number_1 = "Number_1";
    public static string Number_2 = "Number_2";
    public static string Number_3 = "Number_3";
    public static string Number_4 = "Number_4";

    //缩放 zc
    public static string Zoom_In = "ZoomIn"; 
    public static string Zoom_Out = "ZoomOut";
    public static string Zoom_Mouse= "Mouse ScrollWheel";
}