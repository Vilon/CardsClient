using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class UIUtils
{
    static Material greyButton;
    static Material GreyButton
    {
        get
        {
            if (greyButton == null)
                greyButton = Resources.Load<Material>("Materials/GreyButton");
            return greyButton;
        }
    }
    /** wrapper for activating/de-activating a game object */
    static public void SetActive(GameObject gameObj, bool on)
    {
        gameObj.SetActive(on);
    }

    /** wrapper for testing if a game object is active */
    static public bool GetActive(GameObject go)
    {
        return go && go.activeInHierarchy;
    }

    static public GameObject AddChild(Component parent, Component prefab)
    {
        return AddChild(parent.gameObject, prefab.gameObject);
    }
    static public GameObject AddChild(Component parent, GameObject prefab)
    {
        return AddChild(parent.gameObject, prefab);
    }

    static public GameObject AddChild(GameObject parent) { return AddChild(parent, true); }

    /// <summary>
    /// Add a new child game object.
    /// </summary>
    static public GameObject AddChild(GameObject parent, bool undo)
    {
        GameObject go = new GameObject();
#if UNITY_EDITOR
        if (undo) UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
        if (parent != null)
        {
            Transform t = go.transform;
            t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
            go.SetActive(true);
        }
        return go;
    }

    /// <summary>
    /// Instantiate an object and add it to the specified parent.
    /// </summary>

    static public GameObject AddChild(GameObject parent, GameObject prefab)
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;
#if UNITY_EDITOR
        UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.SetParent(parent.transform);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
            go.SetActive(true);
        }
        return go;
    }

    public static GameObject AddChild(GameObject parent, string Path)
    {
        GameObject go = GameObject.Instantiate(Resources.Load(Path)) as GameObject;
        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.SetParent(parent.transform);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
        }
        return go;
    }

    public static void SetButtonActive(Transform button, bool state)
    {
        var array = button.GetComponentsInChildren<Graphic>();
        foreach (var graphic in array)
        {
            graphic.raycastTarget = state;
            graphic.material = state ? null : GreyButton;
        }
    }

    /// <summary>
    /// 指定一个 item让其定位到ScrollRect中间
    /// </summary>
    /// <param name="target">需要定位到的目标</param>
    public static void CenterOnItem(RectTransform target, ScrollRect scrollRect)
    {
        var contentTransform = scrollRect.content;
        var viewPointTransform = scrollRect.viewport;
        // Item is here
        var itemCenterPositionInScroll = GetWorldPointInWidget(scrollRect.GetComponent<RectTransform>(), GetWidgetWorldPoint(target));
        Debug.Log("Item Anchor Pos In Scroll: " + itemCenterPositionInScroll);
        // But must be here
        var targetPositionInScroll = GetWorldPointInWidget(scrollRect.GetComponent<RectTransform>(), GetWidgetWorldPoint(viewPointTransform));
        Debug.Log("Target Anchor Pos In Scroll: " + targetPositionInScroll);
        // So it has to move this distance
        var difference = targetPositionInScroll - itemCenterPositionInScroll;
        difference.z = 0f;

        var newNormalizedPosition = new Vector2(difference.x / (contentTransform.rect.width - viewPointTransform.rect.width),
            difference.y / (contentTransform.rect.height - viewPointTransform.rect.height));

        newNormalizedPosition = scrollRect.normalizedPosition - newNormalizedPosition;

        newNormalizedPosition.x = Mathf.Clamp01(newNormalizedPosition.x);
        newNormalizedPosition.y = Mathf.Clamp01(newNormalizedPosition.y);

        DOTween.To(() => scrollRect.normalizedPosition, x => scrollRect.normalizedPosition = x, newNormalizedPosition, 1);
    }

    static Vector3 GetWidgetWorldPoint(RectTransform target)
    {
        //pivot position + item size has to be included
        var pivotOffset = new Vector3(
            (0.5f - target.pivot.x) * target.rect.size.x,
            (0.5f - target.pivot.y) * target.rect.size.y,
            0f);
        var localPosition = target.localPosition + pivotOffset;
        return target.parent.TransformPoint(localPosition);
    }

    static Vector3 GetWorldPointInWidget(RectTransform target, Vector3 worldPoint)
    {
        return target.InverseTransformPoint(worldPoint);
    }

    public static void ClearChild(MonoBehaviour parent)
    {
        ClearChild(parent.transform);
    }

    public static void ClearChild(GameObject parent)
    {
        ClearChild(parent.transform);
    }

    public static void ClearChild(Transform parent)
    {
        while (parent.childCount > 0)
        {
            GameObject.DestroyImmediate(parent.GetChild(0).gameObject);
        }
    }
}
