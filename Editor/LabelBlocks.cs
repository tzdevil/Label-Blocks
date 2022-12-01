#if (UNITY_EDITOR)
using UnityEditor;
using UnityEngine;

namespace tzdevil.LabelBlocks
{
    [InitializeOnLoad]
    public class LabelBlocks : MonoBehaviour
    {
        static LabelBlocks() => EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;

        private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID);

            if (obj != null)
            {
                var objName = obj.name;

                if (objName[0] == '-' && objName[1] == '-')
                {
                    var fontColor = Color.white;
                    var backgroundColor = objName[2] switch
                    {
                        'r' => Color.red,
                        'g' => Color.green,
                        'b' => Color.blue,
                        '#' when ColorUtility.TryParseHtmlString(objName[2..9], out Color col) => col,
                        _ => Color.gray
                    };

                    Rect offsetRect = new(selectionRect.position, selectionRect.size);
                    EditorGUI.DrawRect(selectionRect, backgroundColor);

                    var newobjName = objName[(objName[2] != '#' ? 3 : 9)..];
                    EditorGUI.LabelField(offsetRect, newobjName, new GUIStyle()
                    {
                        normal = new GUIStyleState() { textColor = fontColor },
                        richText = true,
                        alignment = TextAnchor.UpperCenter
                    });
                }
            }
        }
    }
}
#endif