using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    private LevitationProperty levitationProperty;
    void Start()
    {
        levitationProperty = this.GetComponentInParent<LevitationProperty>();
    }

    void OnCollisionEnter(Collision collision)
    {



        if (collision.gameObject.name == "Player")
            return;

        levitationProperty.StopLevitating();
    }

}
