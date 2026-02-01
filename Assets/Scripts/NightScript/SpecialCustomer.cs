using System.Collections.Generic;
using UnityEngine;

public class SpecialCustomer : ICustomer
{
    [SerializeField] private ListenUI listenUI;

    public override void Order()
    {
        base.Order();
        listenUI.OnSpecialCustomer();
    }
}
