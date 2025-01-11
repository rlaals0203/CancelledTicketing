using UnityEditor;
using UnityEngine;

public class BoardEditor : EditorWindow
{
    private const int BoardSize = 50; // 50 x 50 ����
    private bool[,] selectedCells = new bool[BoardSize, BoardSize]; // ���õ� ĭ ����
    private string prefabName = "NewPrefab";

    [MenuItem("Tools/Board Creator")]
    public static void ShowWindow()
    {
        GetWindow<BoardEditor>("Board Creator");
    }

    private void OnGUI()
    {
        // ĭ ���� �׸���
        GUILayout.Label("Select Cells", EditorStyles.boldLabel);
        for (int y = 0; y < BoardSize; y++)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < BoardSize; x++)
            {
                selectedCells[x, y] = GUILayout.Toggle(selectedCells[x, y], "", GUILayout.Width(10), GUILayout.Height(10));
            }
            GUILayout.EndHorizontal();
        }

        // �̸� �Է�
        GUILayout.Space(10);
        prefabName = EditorGUILayout.TextField("Prefab Name", prefabName);

        // ���� ��ư
        if (GUILayout.Button("Generate"))
        {
            GeneratePrefab();
        }
    }

    private void GeneratePrefab()
    {
        // ���õ� ĭ�� ����Ͽ� ��ư ����
        GameObject parentObject = new GameObject("GeneratedButtons");

        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                if (selectedCells[x, y])
                {
                    GameObject button = GameObject.CreatePrimitive(PrimitiveType.Cube); // ��ư ��� ť�� ����
                    button.transform.position = new Vector3(x, 0, y);
                    button.transform.parent = parentObject.transform;
                }
            }
        }

        // ���������� ����
        string path = $"Assets/{prefabName}.prefab";
        PrefabUtility.SaveAsPrefabAsset(parentObject, path);
        DestroyImmediate(parentObject);

        Debug.Log($"Prefab saved at {path}");
    }
}
