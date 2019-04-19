using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 튜토리얼 메니져
/// </summary>
public class Tutorial_Manager : MonoBehaviour
{
    public bool isTuto; // 튜토 중인지 
    //public MagicBoxAni MagicBox; // 메인메뉴에 있는 
    public int step; // 튜토리얼 단계

    Battle_Manager Battle_Manager; // 인게임 배틀을 전반적으로 관리하는 클래스
    //액셀 데이터들
    Excel_format SampleDeck_list; // 견본덱
    Excel_format Character_list;// 캐릭터
    Excel_format scripts_list;// 대사 스크립트
    Excel_format enemy_list;

    public static bool objectLock = false;
    Deck deck;
    int[] enemyDeck;

    public Excel_format Scripts_list { get => scripts_list; set => scripts_list = value; }

    private void Awake()
    {
        if (isTuto)
        {
            //MagicBox.SetState(0);
            DataManager.inst().AllLoad();
            step = DataManager.tutorial.Tuto_stage;
            if (step.Equals(0) || step.Equals(1))
            {
                DataManager.tutorial.isTuto = true;
                DataManager.tutorial.Tuto_stage = 1;
                DataManager.inst().AllSave();
                step = DataManager.tutorial.Tuto_stage;
            }

            if (!objectLock)
            {
                DontDestroyOnLoad(gameObject);
                objectLock = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            //MagicBox.SetState(1);
            Destroy(gameObject);
        }

        SampleDeck_list = ExcelDataManager.Inst().getData("SampleDeck", DataManager.inst().get_LanguageNumber());
        Character_list = ExcelDataManager.Inst().getData("Character", DataManager.inst().get_LanguageNumber());
        Scripts_list = ExcelDataManager.Inst().getData("TutorialScripts", DataManager.inst().get_LanguageNumber());
        enemy_list = ExcelDataManager.Inst().getData("Enemy", DataManager.inst().get_LanguageNumber());
    }

    int MakeDeck(int DeckNum)
    {
        // 덱 불러오기
        deck = new Deck();
        List<char_dataset> character = new List<char_dataset>();
        int numOfCharacter = 0;
        for (int i = 0; i < 3; i++)
        {
            if (!SampleDeck_list.list[DeckNum].string_value[i].Equals("none"))
            {
                numOfCharacter++;
            }
        }

        for (int i = 0; i < numOfCharacter; i++)
        {

            for (int j = 0; j < Character_list.list.Count; j++)
            {
                if (Character_list.list[j].string_value[0].Equals(SampleDeck_list.list[DeckNum].string_value[i]))
                {
                    char_dataset _character = new char_dataset();
                    _character.set_char_num(j + 1);
                    character.Add(_character);
                }
            }

            string[] Skills = SampleDeck_list.list[DeckNum].string_value[i + 3].Split(';');

            for (int j = 0; j < Skills.Length; j++)
            {
                character[i].set_skill(j, Convert.ToInt32(Skills[j]));
            }
            deck.setDeck(i, character[i]);
        }
        return numOfCharacter;
    }

    // 튜토리얼 시작을위해 덱세팅하고 Battle_Manager에 요청
    public void StartTutorialInGame()
    {
        List<object> spawnerList =
            BinaryFile.BinaryDeserialize<List<object>>(string.Format("Assets/Resources/Data/StageData/TutorialChapter" + DataManager.tutorial.Tuto_stage + "Spawner"));
        List<int> enemyDeck_ = new List<int>();
        int makeDeck = MakeDeck((int)spawnerList[0]);

        for (int i = 1; i < spawnerList.Count; i++)
        {
            if (((int)spawnerList[i]) < enemy_list.list.Count)
            {
                enemyDeck_.Add((int)spawnerList[i] + 1);
            }
        }

        enemyDeck = enemyDeck_.ToArray();
        Battle_Manager = Battle_Manager.Inst();
        Battle_Manager.Tutorial_start(makeDeck, deck, enemyDeck);
    }

    //Battle_Manager에서 플레이 세팅이 끝나면 호출됨, 튜토리얼 시작
    public void startTutoGame()
    {
        string tutorialName = string.Format("TutorialChapter" + DataManager.tutorial.Tuto_stage);

        if (!GetComponent<StageLoader>())
            gameObject.AddComponent<StageLoader>();

        GetComponent<StageLoader>().InitStage(tutorialName, DataManager.inst().get_LanguageNumber());
        GetComponent<StageLoader>().StartStage();
    }
}