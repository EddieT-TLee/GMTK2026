using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SegmentedBar))]
public class SegmentedBarEditor : Editor
{
    private SerializedProperty numSegmentsProperty;
    private int previousNumSegments;
    private bool needsRebuild;

    private void OnEnable()
    {
        numSegmentsProperty = serializedObject.FindProperty("numSegments");
        previousNumSegments = numSegmentsProperty.intValue;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        if (numSegmentsProperty.intValue != previousNumSegments)
        {
            previousNumSegments = numSegmentsProperty.intValue;
            needsRebuild = true;
        }

        if (needsRebuild)
        {
            EditorGUILayout.Space();

            EditorGUILayout.HelpBox(
                "The number of segments has changed. Rebuild the health bar to apply the changes.",
                MessageType.Info);

            if (GUILayout.Button("Rebuild Segments"))
            {
                foreach (Object targetObject in targets)
                {
                    SegmentedBar bar = (SegmentedBar)targetObject;
                    Undo.RegisterFullObjectHierarchyUndo(bar.gameObject, "Rebuild Segments");

                    bar.RebuildSegments();

                    EditorUtility.SetDirty(bar);
                }

                needsRebuild = false;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}