using UnityEngine;
using System.Collections;
using UnityEditor;

public class MeshInfo : EditorWindow
{

    private int vertexCount;
    private int submeshCount;
    private int triangleCount;

    [MenuItem("Tools/Mesh Info")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MeshInfo window = (MeshInfo)EditorWindow.GetWindow(typeof(MeshInfo));
        window.titleContent.text = "Mesh Info";
    }

    void OnSelectionChange()
    {
        Repaint();
    }



    void OnGUI()
    {
        if (Selection.activeGameObject)
        {
            MeshFilter[] filters = Selection.activeGameObject.GetComponentsInChildren<MeshFilter>();

            vertexCount = submeshCount = triangleCount = 0;

            foreach (var f in filters)
            {
                vertexCount += f.sharedMesh.vertexCount;
                triangleCount += f.sharedMesh.triangles.Length / 3;
                submeshCount += f.sharedMesh.subMeshCount;
            }

            EditorGUILayout.LabelField(Selection.activeGameObject.name);
            EditorGUILayout.LabelField("Vertices: ", vertexCount.ToString());
            EditorGUILayout.LabelField("Triangles: ", triangleCount.ToString());
            EditorGUILayout.LabelField("SubMeshes: ", submeshCount.ToString());
        }
    }

}