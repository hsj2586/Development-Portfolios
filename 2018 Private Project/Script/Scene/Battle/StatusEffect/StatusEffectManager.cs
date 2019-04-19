using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    // 캐릭터에 부착하는 상태 변화 리스트를 관리하는 컴포넌트 형태의 스크립트.

    Dictionary<int, IEnumerator> status_effects;
    int key_idx; // 버프를 검색할 수 있도록 하는 인덱스 변수

    void Awake()
    {
        status_effects = new Dictionary<int, IEnumerator>();
        key_idx = 0;
    }

    public int StatusEffectListPush(IEnumerator status_effect) // 키 인덱스 값을 증가시키고 딕셔너리에 추가, 그 후에 해당 인덱스를 반환.
    {
        key_idx++;
        status_effects.Add(key_idx, status_effect);
        return key_idx;
    }

    public void StatusEffectListPop(int key)
    {
        for (int i = 0; status_effects.Count > i; i++)
        {
            if (status_effects.ContainsKey(key))
            {
                status_effects.Remove(key);
                print(status_effects.Count);
                return;
            }
        }
    }
}
