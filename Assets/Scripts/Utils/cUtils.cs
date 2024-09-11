using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cUtils : MonoBehaviour
{
    public const int sortingOrderDefault = 5000;
        
    // Get Sorting order to set SpriteRenderer sortingOrder, higher position = lower sortingOrder
    public static int GetSortingOrder(Vector3 position, int offset, int baseSortingOrder = sortingOrderDefault) {
        return (int)(baseSortingOrder - position.y) + offset;
    }
    
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
    {
        color ??= Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }
        
    // Create Text in the World
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder) 
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
    
    public static void CreateWorldTextPopup(string text, Vector3 localPosition) 
    {
        CreateWorldTextPopup(null, text, localPosition, 20, Color.white, localPosition + new Vector3(0, 10), 1f);
    }
    
    public static void CreateWorldTextPopup(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, Vector3 finalPopupPosition, float popupTime) 
    {
        TextMesh textMesh = CreateWorldText(parent, text, localPosition, fontSize, color, TextAnchor.LowerLeft, TextAlignment.Left, sortingOrderDefault);
        Transform transform = textMesh.transform;
        Vector3 moveAmount = (finalPopupPosition - localPosition) / popupTime;
        cFunctionUpdater.Create(delegate () 
        {
            transform.position += moveAmount * Time.deltaTime;
            popupTime -= Time.deltaTime;
            if (popupTime <= 0f) 
            {
                UnityEngine.Object.Destroy(transform.gameObject);
                return true;
            } 
            else 
            {
                return false;
            }
        }, "WorldTextPopup");
    }
    
}
