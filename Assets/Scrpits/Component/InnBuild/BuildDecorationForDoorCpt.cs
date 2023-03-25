using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildDecorationForDoorCpt : BuildDecorationCpt
{
    public Sprite spLeftAndRightOpen;
    public Sprite spUpAndDownOpen;

    public GameObject objTableLeftPosition;
    public GameObject objTableRightPosition;
    public GameObject objTableDownPosition;
    public GameObject objTableUpPosition;

    public override void SetData(BuildItemBean buildItemData, Sprite spIcon)
    {
        base.SetData(buildItemData);

        List<string> listIconStr = buildItemData.GetIconList();
        Sprite spLeftAndRight = InnBuildHandler.Instance.manager.GetFurnitureSpriteByName(listIconStr[0] + "_0");
        Sprite spUpAndDown = InnBuildHandler.Instance.manager.GetFurnitureSpriteByName(listIconStr[0] + "_2");

        spLeftAndRightOpen = InnBuildHandler.Instance.manager.GetFurnitureSpriteByName(listIconStr[0] + "_1");
        spUpAndDownOpen = InnBuildHandler.Instance.manager.GetFurnitureSpriteByName(listIconStr[0] + "_3");
        SetSprite(spLeftAndRight, spLeftAndRight, spUpAndDown, spUpAndDown);
    }

    public override void SetDirection(Direction2DEnum direction)
    {
        base.SetDirection(direction);
        switch (direction)
        {
            case Direction2DEnum.Left:
                srMainBuild.transform.position = objTableLeftPosition.transform.position;
                srShadow.transform.position = objTableLeftPosition.transform.position+new Vector3(0.05f,0);
                break;
            case Direction2DEnum.Right:
                srMainBuild.transform.position = objTableRightPosition.transform.position;
                srShadow.transform.position = objTableRightPosition.transform.position + new Vector3(0.05f, 0);
                break;
            case Direction2DEnum.Down:
                srMainBuild.transform.position = objTableDownPosition.transform.position;
                srShadow.transform.position = objTableDownPosition.transform.position + new Vector3(0.05f, 0);
                break;
            case Direction2DEnum.UP:
                srMainBuild.transform.position = objTableUpPosition.transform.position;
                srShadow.transform.position = objTableUpPosition.transform.position + new Vector3(0.05f, 0);
                break;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.name.Contains("Body"))
        {
            return;
        }
        Sprite spIcon = null;
        switch (direction)
        {
            case Direction2DEnum.Left:
            case Direction2DEnum.Right:
                spIcon = spLeftAndRightOpen;
                break;
            case Direction2DEnum.UP:
            case Direction2DEnum.Down:
                spIcon = spUpAndDownOpen;
                break;
        }
        if (srMainBuild != null)
            srMainBuild.sprite = spIcon;
        if (srShadow != null)
            srShadow.sprite = spIcon;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.name.Contains("Body"))
        {
            return;
        }
        SetSprite();
    }

}