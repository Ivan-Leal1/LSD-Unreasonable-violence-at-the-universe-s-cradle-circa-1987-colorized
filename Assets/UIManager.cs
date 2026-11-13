using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text _actionInfo;
    [SerializeField] TMP_Text _actionLog;
    [SerializeField] TMP_Text _miscInfo;
    [SerializeField] TMP_Text _statusInfo;

    public void WriteThere(int where, int firstValue, int secondValue, int thirdValue) {
        switch (where) {
            // 0 = action info; 1 = action log; 2 = misc info; 3 = status info
            default:
                break;
        }
    }
}
