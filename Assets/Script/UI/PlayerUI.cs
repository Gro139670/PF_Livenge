using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private enum Status
    {
        HP,
        Mana,
        EXP
    }
    TextMeshProUGUI _TextInput;

    [SerializeField] private Status _Status;

    private void Awake()
    {
        _TextInput = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_Status)
        {
            case Status.HP:
                _TextInput.text = GameManager.Instance.GetSystem<PlayerSystem>().HP.ToString();
                break;
            case Status.Mana:
                _TextInput.text = GameManager.Instance.GetSystem<PlayerSystem>().Mana.ToString();
                break;
            case Status.EXP:
                break;
        }
    }
}
