using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TitleChange : MonoBehaviour
{
    [SerializeField] TMP_Text _selfText;
    [SerializeField] List<string> _wordsThatStartWithL;
    [SerializeField] List<string> _wordsThatStartWithS;
    [SerializeField] List<string> _wordsThatStartWithD;
    string _firstPart;
    string _secondPart;
    string _thirdPart;

    IEnumerator ChangeL() {
        int num = 0;
        while (true) {
            num = Random.Range(0, _wordsThatStartWithL.Count - 1);
            _firstPart = _wordsThatStartWithL[num];
            Write();
            yield return new WaitForSeconds(Random.Range(1f, 8f));
        }
    }

    IEnumerator ChangeS() {
        int num = 0;
        while (true) {
            num = Random.Range(0, _wordsThatStartWithS.Count - 1);
            _secondPart = _wordsThatStartWithS[num];
            Write();
            yield return new WaitForSeconds(Random.Range(1f, 8f));
        }
    }

    IEnumerator ChangeD() {
        int num = 0;
        while (true) {
            num = Random.Range(0, _wordsThatStartWithD.Count - 1);
            _thirdPart = _wordsThatStartWithD[num];
            Write();
            yield return new WaitForSeconds(Random.Range(1f, 8f));
        }
    }

    public void Stop() {
        StopAllCoroutines();
    }

    private void Write() {
        _selfText.text = _firstPart + _secondPart + _thirdPart;
    }

    private void Awake() {
        StartCoroutine(ChangeL());
        StartCoroutine(ChangeS());
        StartCoroutine(ChangeD());
    }
}
