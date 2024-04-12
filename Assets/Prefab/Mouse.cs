

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    private static Card _SelectedCard = null;
    public static GameObject _HoverringObject;
    private Vector3 _CurrPosition;
    private Vector3 _PrevPosition;

    private static bool _IsMouseMove = false;

    public static Card SelectedCard
    {
        get { return _SelectedCard; }
        set
        {
            if (_SelectedCard == null) _SelectedCard = value;
            else { if (value == null) _SelectedCard = null; }
        }
    }

    public Vector3 MousePoint { get; }
    public static bool IsMouseMove { get { return _IsMouseMove; } }

    void Update()
    {
        _IsMouseMove = false;

        _CurrPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
               Input.mousePosition.y, -Camera.main.transform.position.z));
        if(_PrevPosition != _CurrPosition)
        {
            _PrevPosition = _CurrPosition;
            transform.position = _CurrPosition;
            _IsMouseMove = true;
        }
    }
}
