using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ItemGamePopupAchCpt : BaseMonoBehaviour
{
    public Text tvName;
    public Slider sliderPro;

    public Image sliderFill;

    public Sprite spSliderFull;
    public Sprite spSliderUnFull;

    public Color colorStatusCompleted;
    public Color colorStatusProcessing;

    public void SetData(string name, float pro)
    {
        if (tvName != null)
            tvName.text = name;
        if (sliderPro != null)
            sliderPro.value = pro;
        if (pro >= 1)
        {
            sliderFill.sprite = spSliderFull;
            tvName.color = colorStatusCompleted;
        }
        else
        {
            sliderFill.sprite = spSliderUnFull;
            tvName.color = colorStatusProcessing;
        }
    }
}