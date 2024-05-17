using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHover : MonoBehaviour
{
    protected bool _IsMouseHover = false;
    private BoxCollider2D _BoxCollider;

    public bool IsMouseHover
    {
        get
        {
            return _IsMouseHover;
        }
    }


    protected virtual void Awake()
    {
        _BoxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        _IsMouseHover = false;
        if (_BoxCollider.OverlapPoint(Mouse.Instance.MousePoint) == true)
        {
            _IsMouseHover = true;
        }
    }
}
