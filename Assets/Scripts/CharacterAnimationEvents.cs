using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEvents : MonoBehaviour
{
    Character character;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponentInParent<Character>();
    }

    void DoDamage()
    {
        Character targetCharacter = character.target.GetComponent<Character>();
        targetCharacter.DoDamage();
    }

    void AttackEnd()
    {
        character.SetState(Character.State.RunningFromEnemy);
    }

    void PunchEnd()
    {
        character.SetState(Character.State.RunningFromEnemy);
    }

    void ShootEnd()
    {
        character.SetState(Character.State.Idle);
    }
}
