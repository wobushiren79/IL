using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

public class SortingGroupBugFix : BaseMonoBehaviour
{
    private void Awake()
    {
        SortingGroup sortingGroup= GetComponent<SortingGroup>();
        int oldOrder= sortingGroup.sortingOrder;
        sortingGroup.sortingOrder = 0;
        sortingGroup.sortingOrder = oldOrder;
    }
}