using System.Collections;
using UnityEngine;

class ProjectileUnit : Unit
{
    public GameObject Owner
    {  get; set; }

    private void Awake()
    {
        GameManager.Instance.GetSystem<StageSystem>().RoundEnd += DeleteSelf;
    }

    private void FixedUpdate()
    {
    }

    private void OnDestroy()
    {
        GameManager.Instance.GetSystem<StageSystem>().RoundEnd -= DeleteSelf;
    }

    private void DeleteSelf()
    {
        if (Owner == null)
        {
            Destroy(gameObject);
        }
    }
}
