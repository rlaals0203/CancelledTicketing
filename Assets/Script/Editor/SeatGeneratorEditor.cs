using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class SeatGeneratorEditor : EditorWindow
{
    [SerializeField] private GameObject seatPrefab;
    private const int Vert = 40;
    private const int Horizon = 50;

    private bool isMult = false;
    
    private SeatData[,] datas = new SeatData[Vert, Horizon];
    private SeatData currentData;
    private SeatData prevData;

    private Button generateSeat;
    private VisualElement background;
    private TextField prefabTextField;
    
    [MenuItem("Tools/SeatGenerator")]
    public static void ShowWindow()
    {
        SeatGeneratorEditor window = GetWindow<SeatGeneratorEditor>();
        window.titleContent = new GUIContent("SeatGeneratorEditor");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        VisualTreeAsset asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/CustomSeatGenerator.uxml");
        VisualElement tree = asset.Instantiate();
        root.Add(tree);

        #region AssignElements
        generateSeat = tree.Q<Button>("generate-btn");
        background = tree.Q<VisualElement>("seat-background");
        prefabTextField = tree.Q<TextField>("seat-name");
        #endregion
        
        for (int y = 0; y < Vert; y++)
        {   
            for (int x = 0; x < Horizon; x++)
            {
                Button button = new Button();
                datas[y, x] = new SeatData(false, button);
                datas[y, x].Position = new Vector2(x, y);
                
                button.name = "seat";
                button.userData = datas[y, x];
                button.clicked += () => OnSeatClicked(button);
                button.RegisterCallback<MouseDownEvent>((e) => OnSeatClicked(button, false));
                
                background.Add(button);
            }
        }
        
        generateSeat.clicked += () => GenerateSeat(generateSeat);
    }

    private void OnSeatClicked(Button button, bool isSingle = true)
    { 
        SeatData data = button.userData as SeatData;
        prevData = currentData;
        currentData = data;
        data.IsChecked = !data.IsChecked;

        if (isMult) {
            SetMultiSeat();
            return;
        }

        if (!isSingle) {
            currentData.Seat.style.backgroundColor = Color.yellow;
            currentData.IsChecked = false;
            isMult = true;
        }
        else {
            SetSeatSelect(currentData);
            isMult = false;
        }
    }
    
    private void SetMultiSeat()
    {
        if (prevData == null) return;
        
        for (int i = (int)prevData.Position.y; i <= (int)currentData.Position.y; i++)
        {
            for (int j = (int)prevData.Position.x; j <= (int)currentData.Position.x; j++)
            {
                SetSeatSelect(datas[i, j], true);
            }
        }
        
        isMult = false;
    }
    
    private void SetSeatSelect(SeatData data, bool isMulti = false)   
    {
        data.Seat.style.backgroundColor = data.IsChecked ? Color.green : Color.white;

        if (isMulti)
        {
            data.Seat.style.backgroundColor = Color.green;
            data.IsChecked = true;
        }
    }

    private void GenerateSeat(Button button)
    {
        GameObject prefab = new GameObject();
        prefab.name = prefabTextField.value;
        int count = 0;

        for (int i = 0; i < Vert; i++)
        {
            for (int j = 0; j < Horizon; j++)
            {
                if (datas[i, j].IsChecked)
                {
                    GameObject newSeat = Instantiate(seatPrefab);
                    newSeat.GetComponent<RectTransform>().position = new Vector2(j * 15, i * 15);
                    newSeat.name = $"seat{++count}";
                    newSeat.transform.SetParent(prefab.transform);
                }
            }
        }
        
        string path = $"Assets/02Prefab/{prefabTextField.value}.prefab";
        PrefabUtility.SaveAsPrefabAsset(prefab, path);
        DestroyImmediate(prefab);
    }
}