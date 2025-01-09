using UnityEngine;
using UnityEngine.UI;

public class Seat : MonoBehaviour
{
    private Button seat;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        seat = GetComponent<Button>();
        seat.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Debug.Log("ÁÂ¼® ¼±ÅÃµÊ");
    }
}
