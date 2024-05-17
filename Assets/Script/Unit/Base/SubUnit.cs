using System.Collections;
using UnityEngine;


public class SubUnit : Unit
{

    public GameObject Owner
    { get; set; }


    private void FixedUpdate()
    {
        if (Owner == null)
        {
            gameObject.SetActive(false);
        }

        if(GameManager.Instance.GetSystem<StageSystem>().IsBattle == false)
        {
            gameObject.SetActive(false);
        }
    }
}
