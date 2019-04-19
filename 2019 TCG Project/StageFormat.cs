using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillCard;
using ccg_server_messages;

public enum ConditionalWaitingType { TimeType, TurnEndType, SpeechBubbleType, DamagedEnemyType }
public enum CharacterStatus { Attack, Defense, Speed }
public enum CardStatus { Accuracy, Cost }

public class StageFormat : MonoBehaviour
{
    // 임의의 연출이 수행되는 스테이지(튜토리얼)의 Base Class
    public Battle_Manager Battle_Manager; // 인게임 배틀을 전반적으로 관리하는 클래스
    public Excel_format scripts_list;// 대사 스크립트
    public int[] enemyDeck;
    public int stageNumber;
    public float time_count;

    protected B_character[] enemies;
    protected B_character[] allies;

    public void Init(int stageNum) // 초기화
    {
        scripts_list = GetComponent<Tutorial_Manager>().Scripts_list;
        stageNumber = stageNum;
        Battle_Manager = Battle_Manager.Inst();
        enemies = Battle_Manager.getBattlers(2).ToArray();
        allies = Battle_Manager.getBattlers(3).ToArray();
    }

    public IEnumerator GameEndCheck()
    {
        yield return new WaitUntil(() => Battle_Manager.state.Equals(Battle_Manager.TurnState.End));
        Battle_Manager.setTurnEnd(true);
        TurnEnd();
    }

    public void CountingTurn()
    {
        Battle_Manager.set_trun_cha(Battle_Manager.Turn_Ui.getTurn());
        Battle_Manager.state = Battle_Manager.TurnState.turn;
    }

    public IEnumerator BubbleScriptPlayNWait(int team, int index, string content)  // 말풍선 실행 후 대기하는 코루틴
    {
        if (team.Equals(0)) // 아군일 경우
            Battle_Manager.SpeachBB.setTransform(content, index + 1, true);
        else // 적군일 경우
            Battle_Manager.SpeachBB.setTransform(content, index + 1, false);

        yield return new WaitUntil(() => !SpeachBubble.speah_ing);
    }

    public IEnumerator BubbleScriptPlay(int team, int index, string content)  // 말풍선 실행 후 대기하는 코루틴
    {
        if (team.Equals(0)) // 아군일 경우
            Battle_Manager.SpeachBB.setTransform(content, index + 1, true);
        else // 적군일 경우
            Battle_Manager.SpeachBB.setTransform(content, index + 1, false);

        yield return null;
    }

    public IEnumerator openTooltipBox(int type, object data, string content)
    {
        Battle_Manager.SpeachBB.setTooltipBox(type, data, content);
        yield return new WaitUntil(() => !SpeachBubble.speah_ing);
    }

    public IEnumerator TurnEnd()
    {
        Battle_Manager.TurnBattler().getUI().startSpeacialSkill("TurnEnd");
        Battle_Manager.TurnBattler().startCurseSkill("TurnEnd");
        Battle_Manager.FieldManager.FieldApply("TurnEnd");
        Battle_Manager.Turn_Ui.turn_ob_off();
        Battle_Manager.Turn_Ui.NextTurn();
        Battle_Manager.TurnBattler().BModel.turn_EffectOn(false);

        yield return new WaitForSeconds(0.05f);
    }

    public void Gameover() // 게임 종료
    {
        DataManager.tutorial.Tuto_stage = stageNumber + 1;
        DataManager.inst().AllSave();
        Battle_Manager.cardsinHand.TurnButtonOb.TurnButtonON(false);
        Battle_Manager.Victory();
        DestroyImmediate(GetComponent<StageFormat>());
    }

    public IEnumerator TurnLoop(int type, params int[] args)
    {
        // type = 0 이면 일반 아군 설정, type = 1 이면 특정 아군 설정
        // (type = 0) 의 경우 args[0]에 드로우 개수를 입력,
        // (type = 1) 의 경우 args[0] = 0 이면 랜덤 카드, args[0] = 1 이면 해당 인덱스의 카드를 갖도록 설정.
        // (type = 1, args[0] = 0) 의 경우 args[1]에 아군 인덱스 설정, args[2]...[n]에 드로우할 카드의 인덱스 설정.
        // (type = 1, args[0] = 1) 의 경우 args[1]에 아군 인덱스 설정, args[2]에 드로우할 랜덤 카드 개수 설정.
        StartCoroutine(GameEndCheck());
        while (!Battle_Manager.state.Equals(Battle_Manager.TurnState.End))
        {
            CountingTurn();
            Battle_Manager.turnInit();
            if (Battle_Manager.TurnBattler().isTeam())
            {
                if (Battle_Manager.TurnBattler().getUI().Buff_Manager.buffCheck(buffType.Stun)) // 스턴이면
                {
                    Battle_Manager.turnEnd();
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    if (type.Equals(0)) // 일반 아군 설정
                    {
                        Battle_Manager.cardsinHand.cardDraw(Battle_Manager.TurnBattler().getCards(), args[0]);

                    }
                    else // 특정 아군 설정
                    {
                        if (args[1].Equals(0)) // 어떤 카드를
                        {
                            Battle_Manager.cardsinHand.cardDrawSetting(allies[args[0]].getCards());
                            for (int i = 2; i < args.Length; i++)
                            {
                                Battle_Manager.cardsinHand.CreateCardDaraw(args[i]);
                            }
                        }
                        else if (args[1].Equals(1)) // 랜덤 카드를
                        {
                            print("아군 랜덤 카드 배틀시스템 작동");
                            Battle_Manager.cardsinHand.cardDraw(allies[args[0]].getCards(), args[2]);
                        }
                    }
                }
            }
            else if (!Battle_Manager.TurnBattler().isTeam())
            {
                if (Battle_Manager.TurnBattler().getUI().Buff_Manager.buffCheck(buffType.Stun))
                {
                    Battle_Manager.Ai_Enemy.EndEnemyTurn();
                    Battle_Manager.turnEnd();
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    Battle_Manager.Ai_Enemy.startEnemyTurn((EnemyBattler)Battle_Manager.TurnBattler());
                }
            }

            yield return new WaitUntil(() => Battle_Manager.IsTurnEnd()); //턴 종료 버튼 누름
            yield return StartCoroutine(TurnEnd());
        }
    }

    public void TurnButtonSwitch(bool value) // 턴 넘기기 버튼 활성화 조정
    {
        Battle_Manager.cardsinHand.TurnButtonOb.TurnButtonON(value);
    }

    public void AllyTurnStart(int allyIndex, params int[] cards) // 아군 턴 시작
    {
        CountingTurn();
        Battle_Manager.turnInit();
        if (Battle_Manager.TurnBattler().getUI().Buff_Manager.buffCheck(buffType.Stun)) // 스턴이면
        {
            Battle_Manager.turnEnd();
            //yield return new WaitForSeconds(1f);
        }
        else
        {
            Battle_Manager.cardsinHand.cardDrawSetting(allies[allyIndex].getCards());
            for (int i = 0; i < cards.Length; i++)
            {
                Battle_Manager.cardsinHand.CreateCardDaraw(cards[i]);
            }
        }

    }

    public void EnemyTurnStart() // 적군 턴 시작
    {
        CountingTurn();
        Battle_Manager.turnInit();
        if (Battle_Manager.TurnBattler().getUI().Buff_Manager.buffCheck(buffType.Stun)) // 스턴이면
        {
            Battle_Manager.Ai_Enemy.EndEnemyTurn();
            Battle_Manager.turnEnd();
            //yield return new WaitForSeconds(1f);
        }
        else
        {
            Battle_Manager.Ai_Enemy.startEnemyTurn((EnemyBattler)Battle_Manager.TurnBattler());
        }
    }

    public IEnumerator ConditionalWaiting(ConditionalWaitingType conditionalWaitingType, params object[] args) // 조건부 대기
    {
        switch (conditionalWaitingType)
        {
            case ConditionalWaitingType.TimeType: // 일정시간 대기
                yield return new WaitForSeconds((float)args[0]);
                break;
            case ConditionalWaitingType.TurnEndType: // 턴이 끝날때까지 대기
                yield return new WaitUntil(() => Battle_Manager.IsTurnEnd());
                break;
            case ConditionalWaitingType.SpeechBubbleType: // 말풍선이 사라질때까지 대기
                yield return new WaitUntil(() => !SpeachBubble.speah_ing);
                break;
            case ConditionalWaitingType.DamagedEnemyType: // 특정 적이 피격당했을 때까지 대기
                {
                    int tempEnemyIndex = (int)args[0];
                    int enemyHp = enemies[tempEnemyIndex].getHP();
                    WaitForSeconds waitingTime = new WaitForSeconds(0.2f);
                    while (true)
                    {
                        if (enemyHp > enemies[tempEnemyIndex].getHP())
                        {
                            break;
                        }
                        yield return waitingTime;
                    }
                }
                break;
        }
    }

    public void CharacterStatusAdjustment(CharacterStatus statusAdjustment, int type, int target, int value) // 능력치 조정
    {
        // statusAdjustment = 능력치, type = 아군 or 적군, target : 어떤 대상에게, value = 얼만큼
        B_character tempCharacter = type.Equals(0) ? allies[target] : enemies[target];
        switch (statusAdjustment)
        {
            case CharacterStatus.Attack:
                tempCharacter.add_Atk(value);
                break;
            case CharacterStatus.Defense:
                tempCharacter.add_Df(value);
                break;
            case CharacterStatus.Speed:
                tempCharacter.addSpeed(value);
                break;
        }
        tempCharacter.getUI()._setStatus();
    }

    public void CardStatusAdjustment(CardStatus cardStatusAdjustment, int type, int target, float value) // 카드 능력치 조정
    {
        // statusAdjustment = 능력치, type = 아군 or 적군, target : 어떤 대상에게, value = 얼만큼
        card_data[] tempCard = type.Equals(0) ? allies[target].getCards() : enemies[target].getCards();
        switch (cardStatusAdjustment)
        {
            case CardStatus.Accuracy:
                {
                    for (int i = 0; i < tempCard.Length; i++)
                    {
                        tempCard[i].set_accuracy(value);
                    }
                }
                break;
            case CardStatus.Cost:
                {
                    for (int i = 0; i < tempCard.Length; i++)
                    {
                        tempCard[i].set_cost((int)value);
                    }
                }
                break;
        }
    }

    public void AddCharacter(int type, int value) // 전투에서 외부 적을 불러들이기
    {
        Deck deck = new Deck(); // 임시 코드
        if (type.Equals(0))
            Battle_Manager.Add_team_UIAndOb(deck, value);
        else
            Battle_Manager.Add_enemy_UIAndOb(value);
    }
}
