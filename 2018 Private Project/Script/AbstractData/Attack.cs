using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType { Melee, Range } // 공격 타입 : 근접 형태, 원거리 형태

public interface AttackBehaviour
{
    // '공격 행동'에 대한 정보를 가지는 인터페이스
    IEnumerator AttackAnim(GameObject target, float damage);
}
