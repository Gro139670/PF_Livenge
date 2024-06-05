using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : UnitHelper
{
    [SerializeField] private Sprite[] _Front, _Behind,_Left, _Right, _ETC, _Effect;
    private Sprite[] _CurrSprite = null;
    private SpriteRenderer _SpriteRenderer;

    private bool _IsFinish = false;


    private void Start()
    {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
       

    }

    private void Update()
    {
        if (_OwnerInfo.IsDamaged == false)
        {
            _SpriteRenderer.color = Color.white;
        }
        else
        {
            _OwnerInfo.IsDamaged = false;
            _SpriteRenderer.color = Color.red;
        }

        Action<Sprite[]> setDir = sprite =>
        {
            if(sprite.Length > 0)
            {
                _CurrSprite = sprite;
            }
        };

        switch (_OwnerInfo.LookDir)
        {
            case Unit.Direction.Up:
                setDir(_Behind);
                break;
            case Unit.Direction.Left:
                setDir(_Left);
                break;
            case Unit.Direction.Right:
                setDir(_Right);
                break;
            case Unit.Direction.Down:
                setDir(_Front);
                break;
            case Unit.Direction.None:
                setDir(_Behind);
                break;
        }
        if(_CurrSprite == null)
        {
            setDir(_ETC);
        }
        else
        {
            _SpriteRenderer.sprite = _CurrSprite[0];
        }
    }
}
