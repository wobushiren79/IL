using UnityEngine;
using UnityEditor;

public class GameSubstitutionInfo 
{
    //用户名字
    public static readonly string User_Name = "{name}";
    //用户别名 XX 哥哥 XX姐姐
    public static readonly string User_Other_Name = "{othername}";
    //客栈名字
    public static readonly string Inn_Name = "{innname}";

    //迷你游戏 玩家姓名列表
    public static readonly string MiniGame_UserNameList= "{minigame_usernamelist}";
    //迷你游戏 敌对玩家姓名列表
    public static readonly string MiniGame_EnemyNameList = "{minigame_enemynamelist}";

    //迷你游戏 烹饪 评审角色姓名列表
    public static readonly string MiniGame_Cooking_AuditerNameList = "{minigame_cooking_auditernamelist}";
    //迷你游戏 烹饪 烹饪主题
    public static readonly string MiniGame_Cooking_Theme = "{minigame_cooking_theme}";
    //迷你游戏 烹饪 玩家烹饪食物名称
    public static readonly string MiniGame_Cooking_UserFoodName = "{minigame_cooking_user_cooking_foodname}";


}