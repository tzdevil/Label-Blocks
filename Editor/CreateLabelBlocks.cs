using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace tzdevil.LabelBlocks
{
    public class CreateLabelBlocks : EditorWindow
    {
        private Color color = Color.black;
        private string text = "GameObject";

        public static List<GameObject> Labels;

        [MenuItem("GameObject/Create Label Blocks", false, 10)]
        public static void CreateNewLabelBlocks()
            => GetWindow(typeof(CreateLabelBlocks), false, "Create Label Blocks")
                        .position = new Rect(360, 120, 320, 180 + Labels.Count * 20);

        private void Awake() => UpdateLabels();

        private static void UpdateLabels() => Labels = FindObjectsOfType<GameObject>().Where(k => k.name[0] == '-' && k.name[1] == '-').OrderByDescending(k => k.GetInstanceID()).ToList();

        private void OnGUI()
        {
            color = EditorGUILayout.ColorField("Label Color", color);
            text = EditorGUILayout.TextField("Label Text", text);

            EditorGUILayout.Space(10);

            var tempLabel = Labels;

            if (GUILayout.Button("Create New Label Block"))
            {
                var label = new GameObject
                {
                    name = $"--#{ColorUtility.ToHtmlStringRGB(color)}{text}"
                };
                Labels.Add(label);
                CreateNewLabelBlocks();
            }

            GUILayout.Label("Grouping", EditorStyles.boldLabel);
            if (GUILayout.Button("Reset Grouping"))
            {
                UpdateLabels();
                CreateNewLabelBlocks();
            }

            if (GUILayout.Button("Group by Color"))
            {
                UpdateLabels();
                Labels = Labels.Where(k => k.name[3..9] == ColorUtility.ToHtmlStringRGB(color)).ToList();
                CreateNewLabelBlocks();
            }

            if (GUILayout.Button("Group by Name"))
            {
                UpdateLabels();
                Labels = Labels.Where(k => k.name[9..] == text).ToList();
                CreateNewLabelBlocks();
            }

            EditorGUILayout.Space(5);
            GUILayout.Label("Label Blocks", EditorStyles.boldLabel);

            EditorGUI.BeginDisabledGroup(true);
            foreach (var label in Labels)
            {
                EditorGUILayout.BeginHorizontal();

                ColorUtility.TryParseHtmlString(label.name[2..9], out Color col);
                EditorGUILayout.ColorField(col);

                EditorGUILayout.ObjectField(label, typeof(GameObject), true);

                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}