using System;
using System.Buffers.Text;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProperties : NetworkBehaviour
{
    [Networked]
    [HideInInspector]
    public float CurrentHP { get; set; }

    [Networked]
    [HideInInspector]
    public float BaseHP { get; set; }

    [Networked]
    [HideInInspector]
    public float MoveSpeed { get; set; }

    [SerializeField] float localHP;
    [SerializeField] float localSpeed;

    Image HPImageFill;

    public override void Spawned()
    {
        BaseHP = CurrentHP = localHP;
        MoveSpeed = localSpeed;

        HPImageFill = transform.GetChild(0).GetChild(0).GetChild(0).GetComponentInChildren<Image>();
    }

    private void Update()
    {
        if (HasStateAuthority && Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamageRpc(1);
        }
    }

    public override void Render()
    {
        HPImageFill.fillAmount = CurrentHP / BaseHP;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void TakeDamageRpc(float damage)
    {
        CurrentHP -= damage;

        HPImageFill.fillAmount = CurrentHP / BaseHP;

        if (CurrentHP <= 0)
        {
            Runner.Despawn(Object);
        }
    }
}
