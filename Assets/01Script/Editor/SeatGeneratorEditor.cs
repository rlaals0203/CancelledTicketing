using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class SeatGeneratorEditor : EditorWindow
{
    [SerializeField] private GameObject seatPrefab;
    private const int Vert = 40;
    private const int Horizon = 50;

    private bool _isMult = false;
    
    private SeatData[,] _datas = new SeatData[Vert, Horizon];
    private SeatData _currentData;
    private SeatData _prevData;

    private Button _generateSeat;
    private VisualElement _background;
    private TextField _prefabTextField;
    
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
        _generateSeat = tree.Q<Button>("generate-btn");
        _background = tree.Q<VisualElement>("seat-background");
        _prefabTextField = tree.Q<TextField>("seat-name");
        #endregion
        
        for (int y = 0; y < Vert; y++)
        {   
            for (int x = 0; x < Horizon; x++)
            {
                Button button = new Button();
                _datas[y, x] = new SeatData(false, button);
                _datas[y, x].Position = new Vector2(x, y);
                
                button.name = "seat";
                button.userData = _datas[y, x];
                button.clicked += () => OnSeatClicked(button);
                button.RegisterCallback<MouseDownEvent>((e) => OnSeatClicked(button, false));
                
                _background.Add(button);
            }
        }
        
        _generateSeat.clicked += () => GenerateSeat(_generateSeat);
    }

    private void OnSeatClicked(Button button, bool isSingle = true)
    { 
        SeatData data = button.userData as SeatData;
        if (data == null) return;
        _prevData = _currentData;
        _currentData = data;
        data.IsChecked = !data.IsChecked;

        if (_isMult) {
            SetMultiSeat();
            return;
        }

        if (!isSingle) {
            _currentData.Seat.style.backgroundColor = Color.yellow;
            _currentData.IsChecked = false;
            _isMult = true;
        }
        else {
            SetSeatSelect(_currentData);
            _isMult = false;
        }
    }
    
    private void SetMultiSeat()
    {
        if (_prevData == null) return;
        
        for (int i = (int)_prevData.Position.y; i <= (int)_currentData.Position.y; i++)
        {
            for (int j = (int)_prevData.Position.x; j <= (int)_currentData.Position.x; j++)
            {
                SetSeatSelect(_datas[i, j], true);
            }
        }
        
        _isMult = false;
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
        prefab.name = _prefabTextField.value;
        int count = 0;

        for (int i = 0; i < Vert; i++)
        {
            for (int j = 0; j < Horizon; j++)
            {
                if (_datas[i, j].IsChecked)
                {
                    GameObject newSeat = Instantiate(seatPrefab);
                    newSeat.GetComponent<RectTransform>().position = new Vector2(j * 15, i * 15);
                    newSeat.name = $"seat{++count}";
                    newSeat.transform.SetParent(prefab.transform);
                }
            }
        }
        
        string path = $"Assets/02Prefab/{_prefabTextField.value}.prefab";
        PrefabUtility.SaveAsPrefabAsset(prefab, path);
        DestroyImmediate(prefab);
    }
}