using UnityEditor;
using UnityEngine;

public class BoardEditor : EditorWindow
{
    private const int BoardSize = 50; // 50 x 50 보드
    private bool[,] selectedCells = new bool[BoardSize, BoardSize]; // 선택된 칸 저장
    private string prefabName = "NewPrefab";

    [MenuItem("Tools/Board Creator")]
    public static void ShowWindow()
    {
        GetWindow<BoardEditor>("Board Creator");
    }

    private void OnGUI()
    {
        // 칸 선택 그리드
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

        // 이름 입력
        GUILayout.Space(10);
        prefabName = EditorGUILayout.TextField("Prefab Name", prefabName);

        // 생성 버튼
        if (GUILayout.Button("Generate"))
        {
            GeneratePrefab();
        }
    }

    private void GeneratePrefab()
    {
        // 선택된 칸에 기반하여 버튼 생성
        GameObject parentObject = new GameObject("GeneratedButtons");

        for (int y = 0; y < BoardSize; y++)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                if (selectedCells[x, y])
                {
                    GameObject button = GameObject.CreatePrimitive(PrimitiveType.Cube); // 버튼 대신 큐브 생성
                    button.transform.position = new Vector3(x, 0, y);
                    button.transform.parent = parentObject.transform;
                }
            }
        }

        // 프리팹으로 저장
        string path = $"Assets/{prefabName}.prefab";
        PrefabUtility.SaveAsPrefabAsset(parentObject, path);
        DestroyImmediate(parentObject);

        Debug.Log($"Prefab saved at {path}");
    }
}
