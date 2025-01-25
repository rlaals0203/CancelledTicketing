using UnityEngine;

[CreateAssetMenu(fileName = "GameStatSO", menuName = "Scriptable Objects/SO")]
public class GameStatSO : ScriptableObject
{
    public float chance; //1초 기준
    public float duration; //좌석 지속시간
}
