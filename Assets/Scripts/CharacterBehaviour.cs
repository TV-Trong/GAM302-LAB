using Fusion;
using UnityEngine;

public abstract class CharacterBehaviour : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(ChangeDirection))]
    protected float FacingDirection { get; set; }

    protected virtual void ChangeDirection() { }
}
