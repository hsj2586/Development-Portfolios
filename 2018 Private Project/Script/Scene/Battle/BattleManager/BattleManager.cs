using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    // 전투 시에 전투의 흐름을 총괄하는 기능의 스크립트.

    private bool battlecond = true;
    bool turn = false;

    List<GameObject> allies = new List<GameObject>();
    List<GameObject> enemies = new List<GameObject>();
    Queue<IEnumerator> turnQueue = new Queue<IEnumerator>();
    Queue<Character> whoseTurnQueue = new Queue<Character>();

    [SerializeField]
    AudioClip battle_clip;

    Account account;
    string current_stage;
    Stage stage;

    public bool Access_turn
    {
        get { return this.turn; }
        set { this.turn = value; }
    }

    public bool Access_battlecond
    {
        get { return this.battlecond; }
        set { this.battlecond = value; }
    }

    void Start()
    {
        InitBattle();
    }

    void InitBattle()
    {
        SoundManager.Instance.PlayMainSound(battle_clip);
        account = FileManager.AccountDataLoad("SaveFile/AccountData.txt");
        current_stage = "Stage" + (account.Access_currentstage + 1).ToString();
        stage = FileManager.StageDataLoad("SaveFile/StageData/" + current_stage + ".txt");

        #region 아군 캐릭터 리스트 생성
        for (int i = 0; i < account.Access_numofcharacter; i++)
        {
            allies.Add(account.Access_character(i));
            allies[i].tag = "Ally";

            if (allies[i].GetComponent<BarAnimation>() == null)
                allies[i].AddComponent<BarAnimation>();

            if (allies[i].GetComponent<AllyCharacter>().Access_atktype == AttackType.Melee)
            {
                if (allies[i].GetComponent<MeleeAttack>() == null)
                    allies[i].AddComponent<MeleeAttack>();
            }
            else
            {
                if (allies[i].GetComponent<RangeAttack>() == null)
                    allies[i].AddComponent<RangeAttack>();
            }

            if (allies[i].GetComponent<StatusEffectManager>() == null)
                allies[i].AddComponent<StatusEffectManager>();
        }
        #endregion

        #region 적군 캐릭터 리스트 생성
        for (int i = 0; i < stage.Access_enemylist.Count; i++)
        {
            enemies.Add(FileManager.CharacterDataLoad("SaveFile/EnemyData/" + stage.Access_enemylist[i] + ".txt"));
            enemies[i].tag = "Enemy";
            if (enemies[i].GetComponent<BarAnimation>() == null)
                enemies[i].AddComponent<BarAnimation>();
            if (enemies[i].GetComponent<EnemyCharacter>().Access_atktype == AttackType.Melee)
            {
                if (enemies[i].GetComponent<MeleeAttack>() == null)
                    enemies[i].AddComponent<MeleeAttack>();
            }
            else
            {
                if (enemies[i].GetComponent<RangeAttack>() == null)
                    enemies[i].AddComponent<RangeAttack>();
            }
            if (enemies[i].GetComponent<StatusEffectManager>() == null)
                enemies[i].AddComponent<StatusEffectManager>();
        }
        #endregion

        SpawnCharacters(allies); // 아군, 적군 리스트를 씬에 소환
        SpawnCharacters(enemies);

        GetComponent<BattleUIManager>().Init(); // 전투 직전 모든 초기화 진행
        foreach (GameObject ally in allies)
        {
            ally.GetComponent<AllyCharacter>().Init();
        }
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyCharacter>().Init();
        }
        StartCoroutine(update());
    }

    public void Check() // 게임 종료 조건 확인
    {
        if (battlecond == true)
        {
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        int i;
        GameObject[] allyList = GameObject.FindGameObjectsWithTag("Ally");
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        if (allyList.Length == 0 || enemyList.Length == 0) // 아군 혹은 적군의 수가 0일 경우
        {
            battlecond = false;
            yield return null;

            if (enemyList.Length == 0) // 플레이어가 이겼을 경우
            {
                AllyCharacter temp;
                List<EquipmentData> temp_list = new List<EquipmentData>();
                List<int> exp_data = FileManager.ListDataLoad<int>("SaveFile/ExpData.txt"); // 경험치 차트 로드
                int[,] param_data = new int[allies.Count, 2]; // 종료 UI 애니메이션을 위한 매개변수 데이터, (level, exp) 형태

                for (i = 0; i < allies.Count; i++)
                {
                    temp = allies[i].GetComponent<AllyCharacter>();
                    param_data[i, 0] = temp.Access_level;
                    param_data[i, 1] = temp.Access_exp;
                    temp.Access_exp += stage.Access_exp;

                    while (temp.Access_exp >= exp_data[temp.Access_level]) // 레벨 업
                    {
                        temp.Access_exp -= exp_data[temp.Access_level];
                        temp.Access_level += 1;
                        temp.LevelUp();
                    }

                    for (int j = 0; j < temp.Access_equipments.Count; j++) // 장비 능력치 해제
                    {
                        if (temp.Access_equipment(j) != null)
                        {
                            temp.Access_atkpower -= temp.Access_equipment(j).atkpower;
                            temp.Access_atkspeed -= temp.Access_equipment(j).atkspeed;
                            temp.Access_defpower -= temp.Access_equipment(j).defpower;
                        }
                    }
                }

                temp_list = GetComponent<RewardManager>().TryGetReward(stage);
                yield return StartCoroutine(GetComponent<BattleUIManager>().EndGame_Animation(true, param_data, temp_list)); // 종료 UI 애니메이션 코루틴 실행

                account.Access_currentstage += 1; // 스테이지 + 1
                account.Access_characters = allies; // 계정 정보에 경험치가 증가된 캐릭터 입력 후 직렬화
                FileManager.AccountDataGenerate("SaveFile/AccountData.txt", account);
            }
            else // 플레이어가 졌을 경우
            {
                EnemyCharacter temp;
                for (i = 0; i < enemyList.Length; i++)
                {
                    temp = enemyList[i].GetComponent<EnemyCharacter>();
                    temp.StopCoroutine("Idle");
                    temp.StopCoroutine("AttackAnimation");
                    temp.Access_animator.SetInteger("State", 3);
                }
                yield return StartCoroutine(GetComponent<BattleUIManager>().EndGame_Animation(false, null, null));
            }
        }
    }

    void SpawnCharacters(List<GameObject> characters)
    {
        int count = characters.Count;
        float temp_value = (count - 1) * 1.5f;
        string temp_tag = characters[0].tag;
        int i;

        if (temp_tag == "Ally")
        {
            for (i = 0; i < count; i++)
            {
                characters[i] = Instantiate(characters[i], new Vector3(-(0.5f + (i % 2 + 1) * 1), 0, temp_value), characters[i].transform.rotation); // 자리 배치 및 캐릭터 생성
                characters[i].transform.Rotate(new Vector3(0, 90, 0), Space.World);
                temp_value -= 3;
            }
        }
        else
        {
            for (i = 0; i < characters.Count; i++)
            {
                characters[i] = Instantiate(characters[i], new Vector3((0.5f + (i % 2 + 1) * 1), 0, temp_value), characters[i].transform.rotation); // 자리 배치 및 캐릭터 생성
                characters[i].transform.Rotate(new Vector3(0, -90, 0), Space.World);
                temp_value -= 3;
            }
        }
    }

    public void GetTurn(IEnumerator behavior, Character character)
    {
        turnQueue.Enqueue(behavior);
        whoseTurnQueue.Enqueue(character);
    }

    IEnumerator update() // 턴제 공격 메커니즘의 핵심 코루틴, Queue를 통해 공격 순서를 동기화함
    {
        IEnumerator temp;
        while (true)
        {
            yield return null;
            if (turnQueue.Count != 0) // 턴큐에 행동이 들어있을 경우
            {
                if (whoseTurnQueue.Dequeue().Access_State != CharacterState.Dead) // 해당 코루틴의 주체가 죽은 상태가 아닌 경우
                {
                    turn = true;
                    temp = turnQueue.Dequeue();
                    yield return StartCoroutine(temp);
                    yield return new WaitForSeconds(0.2f);
                    turn = false;
                }
                else // 해당 코루틴의 주체가 죽었을 경우 턴 넘김
                    turnQueue.Dequeue();
            }
        }
    }
}
