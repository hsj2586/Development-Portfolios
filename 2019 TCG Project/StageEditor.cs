using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

#if UNITY_EDITOR
public class StageEditor : EditorWindow
{
    enum DataType { Int, Float, String, GameObject, Sprite }
    public string[] fileList = new string[] { };
    public string[] deckList_Excel;
    public string[] enemyList_Excel;
    public string[] options = new string[] { "말풍선 띄우기", "툴팁 띄우기", "턴 시작", "조건부 대기",
        "턴 버튼", "배틀 시스템", "캐릭터 능력치 조정", "카드 능력치 조정", "캐릭터 추가", "턴 종료" ,"게임 종료" };
    public string[] speechBubblesOptions = new string[] { "클릭할 때까지 대기", "대기 안함" };
    public string[] characterTagOptions = new string[] { "아군", "적군" };
    public string[] animationOptions = new string[] { "있음", "없음" };
    public string[] turnOptions = new string[] { "아군의 턴", "적군의 턴" };
    public string[] conditionalWaitingOptions = new string[] { "말풍선이 꺼질 때까지 대기", "일정 시간 대기", "턴 종료시까지 대기", "특정 적군 피격시까지 대기" };
    public string[] turnButtonOptions = new string[] { "턴 버튼 켜기", "턴 버튼 끄기" };
    public string[] battleSystemOptions = new string[] { "일반 아군", "특정 아군" };
    public string[] battleSystemSecondOptions = new string[] { "어떤 카드를", "랜덤 카드를" };
    public string[] characterAdjustmentOptions = new string[] { "공격력", "방어력", "스피드" };
    public string[] cardAdjustmentOptions = new string[] { "정확성", "코스트" };
    public string[] addCharacterOptions = new string[] { "아군 추가", "적군 추가" };


    Vector2 scrollPos;

    string SaveFileNameInput; // 데이터 저장 경로 입력
    int LoadFileNameInput = 0; // 데이터 로드 경로 입력
    int ExcelFileNameInput = 0; // 엑셀 데이터 import 경로 입력
    Excel_format sampleDeckList; // 견본덱
    Excel_format enemies; // 적

    List<List<object>> itemList;
    List<object> spawnerList;

    public int Language_Num;

    private static StageEditor instance;
    public static StageEditor Instance()
    {
        if (instance == null)
        {
            instance = new StageEditor();

        }
        return instance;
    }

    public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }

    [MenuItem("Window/Stage Editor")]
    public static void Open()
    {
        GetWindow<StageEditor>().titleContent = new GUIContent("Stage Editor");
    }

    void OnEnable()
    {
        GUILayout.Width(3000);
        GUILayout.Height(6000);
        sampleDeckList = Resources.Load("Data/" + "SampleDeck_kor") as Excel_format;
        enemies = Resources.Load("Data/" + "Enemy_kor") as Excel_format;
        itemList = new List<List<object>>();
        spawnerList = new List<object>(4) { 0, 0, 0, 0 };

        deckList_Excel = new string[sampleDeckList.list.Count];
        for (int i = 0; i < sampleDeckList.list.Count; i++)
        {
            deckList_Excel[i] = sampleDeckList.list[i].string_value[6];
        }
        enemyList_Excel = new string[enemies.list.Count + 1];
        for (int i = 0; i < enemyList_Excel.Length - 1; i++)
        {
            enemyList_Excel[i] = string.Format(enemies.list[i].string_value[1] + " " + enemies.list[i].string_value[0]);
        }
        enemyList_Excel[enemyList_Excel.Length - 1] = "none";
    }

    void OnGUI()
    {
        FileListUpdate();
        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, true, false, GUILayout.Height(Screen.height - 10));
        GUILayout.Label(" ### Stage Editor ### ", EditorStyles.boldLabel);
        DrawUILine(Color.gray);
        GUILayout.Label("This is a convenient tool for editing the Stage.", EditorStyles.boldLabel);
        ButtonCheck();
        DrawUILine(Color.gray);
        GUILayout.Label(" ### Spawners ### ", EditorStyles.boldLabel, GUILayout.Width(140));
        SpawnersUpdate();
        DrawUILine(Color.gray);
        GUILayout.Label(" ### EventList ### ", EditorStyles.boldLabel, GUILayout.Width(140));
        for (int i = 0; i < itemList.Count; i++)
        {
            ChangeLabels(i);
            GUILayout.BeginHorizontal(GUILayout.Width(150), GUILayout.Height(10));
            itemList[i][0] = EditorGUILayout.Popup((int)itemList[i][0], options, GUILayout.Width(140));
            ExtensionParameter(i, (int)itemList[i][0]);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.EndScrollView();
    }

    void SpawnersUpdate()
    {
        GUILayout.BeginHorizontal(GUILayout.Width(700), GUILayout.Height(10));
        GUILayout.Label("Allies", EditorStyles.boldLabel, GUILayout.Width(200));
        GUILayout.Label("Index", EditorStyles.boldLabel, GUILayout.Width(50));
        GUILayout.Label("Enemies", EditorStyles.boldLabel, GUILayout.Width(200));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(GUILayout.Width(800), GUILayout.Height(10));
        spawnerList[0] = EditorGUILayout.Popup((int)spawnerList[0], deckList_Excel, GUILayout.Width(200));
        GUILayout.Label("   [0]", EditorStyles.boldLabel, GUILayout.Width(50));
        spawnerList[1] = EditorGUILayout.Popup((int)spawnerList[1], enemyList_Excel, GUILayout.Width(200));
        GUILayout.EndHorizontal();

        for (int i = 2; i < 4; i++)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(700), GUILayout.Height(10));
            GUILayout.Label("", EditorStyles.boldLabel, GUILayout.Width(200));
            GUILayout.Label(string.Format("   [" + i.ToString() + "]"), EditorStyles.boldLabel, GUILayout.Width(50));
            spawnerList[i] = EditorGUILayout.Popup((int)spawnerList[i], enemyList_Excel, GUILayout.Width(200));
            GUILayout.EndHorizontal();
        }
        GUILayout.Space(10);
    }

    void FileListUpdate() // 로드 가능한 파일 목록 갱신
    {
        if (Directory.Exists("Assets/Resources/Data/StageData"))
        {
            List<string> fileNamesInDirectory = new List<string>();
            DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Data/StageData");
            FileInfo[] files = dir.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                if (!files[i].Name.Contains(".meta") && !files[i].Name.Contains("Spawner") && !files[i].Name.Contains("Language"))
                    fileNamesInDirectory.Add(files[i].Name);
            }
            fileList = fileNamesInDirectory.ToArray();
        }
    }

    void ButtonCheck() // 버튼 기능
    {
        GUILayout.BeginHorizontal(GUILayout.Width(200));
        GUILayout.Label("File name", EditorStyles.boldLabel);
        LoadFileNameInput = EditorGUILayout.Popup(LoadFileNameInput, fileList, GUILayout.Width(200));

        if (GUILayout.Button("Load", GUILayout.Width(140)))
        {
            if (!Directory.Exists("Assets/Resources/Data/StageData"))
            {
                Directory.CreateDirectory("Assets/Resources/Data/StageData");
            }
            List<List<object>> tempList = BinaryFile.BinaryDeserialize<List<List<object>>>(string.Format("Assets/Resources/Data/StageData/" + fileList[LoadFileNameInput]));
            spawnerList = BinaryFile.BinaryDeserialize<List<object>>(string.Format("Assets/Resources/Data/StageData/" + fileList[LoadFileNameInput] + "Spawner"));
            itemList = BinaryFile.SettingDeserializeData(tempList);

            //scripts = LoadScriptsDataFromExcel(fileList[LoadFileNameInput]);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(GUILayout.Width(200));
        GUILayout.Label("File name", EditorStyles.boldLabel);
        SaveFileNameInput = EditorGUILayout.TextField(SaveFileNameInput, GUILayout.Width(200));
        if (GUILayout.Button("Create", GUILayout.Width(140)))
        {
            if (!Directory.Exists("Assets/Resources/Data/StageData"))
            {
                Directory.CreateDirectory("Assets/Resources/Data/StageData");
            }
            List<List<object>> tempItemList = BinaryFile.SettingSerializeData(itemList);
            BinaryFile.BinarySerialize<List<List<object>>>(tempItemList, string.Format("Assets/Resources/Data/StageData/" + SaveFileNameInput));
            BinaryFile.BinarySerialize<List<object>>(spawnerList, string.Format("Assets/Resources/Data/StageData/" + SaveFileNameInput + "Spawner"));
            SaveScriptsDataToExcel(SaveFileNameInput);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(GUILayout.Width(200));
        GUILayout.Label("Excel name", EditorStyles.boldLabel);
        ExcelFileNameInput = EditorGUILayout.Popup(ExcelFileNameInput, fileList, GUILayout.Width(200));
        if (GUILayout.Button("Import", GUILayout.Width(140)))
        {
            if (!Directory.Exists("Assets/Resources/Data/StageData"))
            {
                Directory.CreateDirectory("Assets/Resources/Data/StageData");
            }
            ImportLanguageDataToBinary(fileList[ExcelFileNameInput]);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(GUILayout.Width(200));
        if (GUILayout.Button("Add Event", GUILayout.Width(140)))
        {
            AddList();
        }
        if (GUILayout.Button("Remove Event", GUILayout.Width(140)))
        {
            RemoveList();
        }
        if (GUILayout.Button("Clear", GUILayout.Width(140)))
        {
            ClearEditor();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }

    void ClearEditor() // 에디터 초기화
    {
        LoadFileNameInput = 0;
        SaveFileNameInput = null;
        itemList = new List<List<object>>();
    }

    void AddList()
    {
        itemList.Add(new List<object>() { 0, 0, 0, 0, 0 });
    }

    void RemoveList()
    {
        if (!itemList.Count.Equals(0))
            itemList.RemoveAt(itemList.Count - 1);
    }

    void InitializeElement(List<object> list, int index, DataType dataType)
    {
        switch (dataType)
        {
            case DataType.Int:
                {
                    if (list[index] == null || !list[index].GetType().Equals(typeof(int)))
                        list[index] = 0;
                }
                break;
            case DataType.Float:
                {
                    if (list[index] == null || !list[index].GetType().Equals(typeof(float)))
                        list[index] = (float)0;
                }
                break;
            case DataType.String:
                {
                    if (list[index] == null || !list[index].GetType().Equals(typeof(string)))
                        list[index] = "";
                }
                break;
            case DataType.GameObject:
                {
                    if (list[index] != null && !list[index].GetType().Equals(typeof(GameObject)))
                        list[index] = null;
                }
                break;
            case DataType.Sprite:
                {
                    if (list[index] != null && !list[index].GetType().Equals(typeof(Sprite)))
                        list[index] = null;
                }
                break;
        }
    }

    void ChangeLabels(int index) // 매개 변수에 따라 레이블 변경
    {
        GUILayout.BeginHorizontal(GUILayout.Width(500));
        GUILayout.Label("Event" + (index + 1).ToString(), EditorStyles.boldLabel, GUILayout.Width(140));

        switch (itemList[index][0])
        {
            case 0: // 말풍선 띄우기
                {
                    GUILayout.Label("대기 여부", EditorStyles.boldLabel, GUILayout.Width(140));
                    GUILayout.Label("피아 구분", EditorStyles.boldLabel, GUILayout.Width(140));
                    GUILayout.Label("누구의", EditorStyles.boldLabel, GUILayout.Width(70));
                    GUILayout.Label("텍스트", EditorStyles.boldLabel, GUILayout.Width(140));
                }
                break;
            case 1: // 툴팁 띄우기
                {
                    GUILayout.Label("애니메이션", EditorStyles.boldLabel, GUILayout.Width(140));
                    if (itemList[index][1].Equals(0))
                    {
                        GUILayout.Label("프리팹", EditorStyles.boldLabel, GUILayout.Width(140));
                    }
                    else
                    {
                        GUILayout.Label("스프라이트", EditorStyles.boldLabel, GUILayout.Width(140));
                    }
                    GUILayout.Label("텍스트", EditorStyles.boldLabel, GUILayout.Width(140));
                }
                break;
            case 2: // 턴 시작
                {
                    if (itemList[index][1].Equals(0))
                    {
                        GUILayout.Label("피아 구분", EditorStyles.boldLabel, GUILayout.Width(140));
                        GUILayout.Label("누구의", EditorStyles.boldLabel, GUILayout.Width(70));
                        GUILayout.Label("카드번호", EditorStyles.boldLabel, GUILayout.Width(70));
                    }
                }
                break;
            case 3: // 조건부 대기
                {
                    if (itemList[index][1].Equals(1))
                    {
                        GUILayout.Label("대기시간(초)", EditorStyles.boldLabel, GUILayout.Width(140));
                    }
                    else if (itemList[index][1].Equals(3))
                    {
                        GUILayout.Label("", EditorStyles.boldLabel, GUILayout.Width(140));
                        GUILayout.Label("누구의", EditorStyles.boldLabel, GUILayout.Width(70));
                    }
                }
                break;
            case 4: // 턴 버튼
                {
                    GUILayout.Label("버튼 활성화", EditorStyles.boldLabel, GUILayout.Width(140));
                }
                break;
            case 5: // 배틀 시스템
                {
                    GUILayout.Label("타입 설정", EditorStyles.boldLabel, GUILayout.Width(140));
                    if (itemList[index][1].Equals(1))
                    {
                        GUILayout.Label("누구의", EditorStyles.boldLabel, GUILayout.Width(70));
                        GUILayout.Label("카드 설정", EditorStyles.boldLabel, GUILayout.Width(140));
                        if (itemList[index][3].Equals(0))
                            GUILayout.Label("카드번호", EditorStyles.boldLabel, GUILayout.Width(70));
                        else if (itemList[index][3].Equals(1))
                            GUILayout.Label("얼마나", EditorStyles.boldLabel, GUILayout.Width(70));
                    }
                    else
                    {
                        GUILayout.Label("얼마나", EditorStyles.boldLabel, GUILayout.Width(70));
                    }
                }
                break;
            case 6: // 캐릭터 능력치 조정
                {
                    GUILayout.Label("피아 구분", EditorStyles.boldLabel, GUILayout.Width(140));
                    GUILayout.Label("누구의", EditorStyles.boldLabel, GUILayout.Width(70));
                    GUILayout.Label("어떤 능력치", EditorStyles.boldLabel, GUILayout.Width(140));
                    GUILayout.Label("얼마나", EditorStyles.boldLabel, GUILayout.Width(70));
                }
                break;
            case 7: // 카드 능력치 조정
                {
                    GUILayout.Label("피아 구분", EditorStyles.boldLabel, GUILayout.Width(140));
                    GUILayout.Label("누구의", EditorStyles.boldLabel, GUILayout.Width(70));
                    GUILayout.Label("어떤 능력치", EditorStyles.boldLabel, GUILayout.Width(140));
                    GUILayout.Label("얼마나", EditorStyles.boldLabel, GUILayout.Width(70));
                }
                break;
            case 8: // 아군/적군 추가
                {
                    GUILayout.Label("피아 구분", EditorStyles.boldLabel, GUILayout.Width(140));
                    GUILayout.Label("캐릭터 번호", EditorStyles.boldLabel, GUILayout.Width(70));
                }
                break;
            case 9: // 턴 종료
                break;
            case 10: // 게임 종료
                break;
        }
        GUILayout.EndHorizontal();
    }

    void ExtensionParameter(int index, int value) // 확장 매개 변수 표시
    {
        switch (value)
        {
            case 0: // 말풍선 띄우기
                {
                    InitializeElement(itemList[index], 1, DataType.Int);
                    InitializeElement(itemList[index], 2, DataType.Int);
                    InitializeElement(itemList[index], 3, DataType.Int);
                    InitializeElement(itemList[index], 4, DataType.String);

                    itemList[index][1] = EditorGUILayout.Popup((int)itemList[index][1], speechBubblesOptions, GUILayout.Width(140));
                    itemList[index][2] = EditorGUILayout.Popup((int)itemList[index][2], characterTagOptions, GUILayout.Width(140));
                    itemList[index][3] = EditorGUILayout.IntField((int)itemList[index][3], GUILayout.Width(70));
                    itemList[index][4] = EditorGUILayout.TextField((string)itemList[index][4], GUILayout.Width(300));
                }
                break;
            case 1: // 툴팁 띄우기
                {
                    InitializeElement(itemList[index], 1, DataType.Int);
                    InitializeElement(itemList[index], 3, DataType.String);

                    itemList[index][1] = EditorGUILayout.Popup((int)itemList[index][1], animationOptions, GUILayout.Width(140));
                    if (itemList[index][1].Equals(0))
                    {
                        InitializeElement(itemList[index], 2, DataType.GameObject);
                        itemList[index][2] = (GameObject)EditorGUILayout.ObjectField((GameObject)itemList[index][2], typeof(GameObject), true, GUILayout.Width(140));
                    }
                    else
                    {
                        InitializeElement(itemList[index], 2, DataType.Sprite);
                        itemList[index][2] = (Sprite)EditorGUILayout.ObjectField((Sprite)itemList[index][2], typeof(Sprite), true, GUILayout.Width(140));
                    }
                    itemList[index][3] = EditorGUILayout.TextField((string)itemList[index][3], GUILayout.Width(300));
                }
                break;
            case 2: // 턴 시작
                {
                    InitializeElement(itemList[index], 1, DataType.Int);
                    InitializeElement(itemList[index], 2, DataType.Int);
                    InitializeElement(itemList[index], 3, DataType.String);

                    itemList[index][1] = EditorGUILayout.Popup((int)itemList[index][1], turnOptions, GUILayout.Width(140));
                    if (itemList[index][1].Equals(0))
                    {
                        itemList[index][2] = EditorGUILayout.IntField((int)itemList[index][2], GUILayout.Width(70));
                        itemList[index][3] = EditorGUILayout.TextField((string)itemList[index][3], GUILayout.Width(70));
                    }
                }
                break;
            case 3: // 조건부 대기
                {
                    InitializeElement(itemList[index], 1, DataType.Int);

                    itemList[index][1] = EditorGUILayout.Popup((int)itemList[index][1], conditionalWaitingOptions, GUILayout.Width(140));
                    if (itemList[index][1].Equals(1))
                    {
                        InitializeElement(itemList[index], 2, DataType.Float);
                        itemList[index][2] = EditorGUILayout.FloatField((float)itemList[index][2], GUILayout.Width(70));
                    }
                    else if (itemList[index][1].Equals(3))
                    {
                        InitializeElement(itemList[index], 2, DataType.Int);
                        itemList[index][2] = EditorGUILayout.IntField((int)itemList[index][2], GUILayout.Width(70));
                    }
                }
                break;
            case 4: // 턴 버튼
                {
                    InitializeElement(itemList[index], 1, DataType.Int);

                    itemList[index][1] = EditorGUILayout.Popup((int)itemList[index][1], turnButtonOptions, GUILayout.Width(140));
                }
                break;
            case 5: // 배틀 시스템
                {
                    InitializeElement(itemList[index], 1, DataType.Int);
                    InitializeElement(itemList[index], 2, DataType.Int);
                    InitializeElement(itemList[index], 3, DataType.Int);

                    itemList[index][1] = EditorGUILayout.Popup((int)itemList[index][1], battleSystemOptions, GUILayout.Width(140));
                    if (itemList[index][1].Equals(1)) // 특정 아군 설정
                    {
                        itemList[index][2] = EditorGUILayout.IntField((int)itemList[index][2], GUILayout.Width(70));
                        itemList[index][3] = EditorGUILayout.Popup((int)itemList[index][3], battleSystemSecondOptions, GUILayout.Width(140));
                        if (itemList[index][3].Equals(0)) // 어떤 카드를
                        {
                            InitializeElement(itemList[index], 4, DataType.String);
                            itemList[index][4] = EditorGUILayout.TextField((string)itemList[index][4], GUILayout.Width(70));
                        }
                        else if (itemList[index][3].Equals(1)) // 랜덤 카드를
                        {
                            InitializeElement(itemList[index], 4, DataType.Int);
                            itemList[index][4] = EditorGUILayout.IntField((int)itemList[index][4], GUILayout.Width(70));
                        }
                    }
                    else if (itemList[index][1].Equals(0)) // 일반 아군 설정
                    {
                        itemList[index][2] = EditorGUILayout.IntField((int)itemList[index][2], GUILayout.Width(70));
                    }
                }
                break;
            case 6: // 캐릭터 능력치 조정
                {
                    InitializeElement(itemList[index], 1, DataType.Int);
                    InitializeElement(itemList[index], 2, DataType.Int);
                    InitializeElement(itemList[index], 3, DataType.Int);
                    InitializeElement(itemList[index], 4, DataType.Int);

                    itemList[index][1] = EditorGUILayout.Popup((int)itemList[index][1], characterTagOptions, GUILayout.Width(140));
                    itemList[index][2] = EditorGUILayout.IntField((int)itemList[index][2], GUILayout.Width(70));
                    itemList[index][3] = EditorGUILayout.Popup((int)itemList[index][3], characterAdjustmentOptions, GUILayout.Width(140));
                    itemList[index][4] = EditorGUILayout.IntField((int)itemList[index][4], GUILayout.Width(70));
                }
                break;
            case 7: // 카드 능력치 조정
                {
                    InitializeElement(itemList[index], 1, DataType.Int);
                    InitializeElement(itemList[index], 2, DataType.Int);
                    InitializeElement(itemList[index], 3, DataType.Int);
                    InitializeElement(itemList[index], 4, DataType.Float);

                    itemList[index][1] = EditorGUILayout.Popup((int)itemList[index][1], characterTagOptions, GUILayout.Width(140));
                    itemList[index][2] = EditorGUILayout.IntField((int)itemList[index][2], GUILayout.Width(70));
                    itemList[index][3] = EditorGUILayout.Popup((int)itemList[index][3], cardAdjustmentOptions, GUILayout.Width(140));
                    itemList[index][4] = EditorGUILayout.FloatField((float)itemList[index][4], GUILayout.Width(70));
                }
                break;
            case 8: // 아군/적군 추가
                {
                    InitializeElement(itemList[index], 1, DataType.Int);
                    InitializeElement(itemList[index], 2, DataType.Int);

                    itemList[index][1] = EditorGUILayout.Popup((int)itemList[index][1], addCharacterOptions, GUILayout.Width(140));
                    itemList[index][2] = EditorGUILayout.IntField((int)itemList[index][2], GUILayout.Width(70));
                }
                break;
            case 9: // 턴 종료
                break;
            case 10: // 게임 종료
                break;
        }
    }

    void SaveScriptsDataToExcel(string fileName) // 챕터의 스크립트 정보들을 엑셀로 저장하는 메소드
    {
        List<string> tempList = new List<string>();
        int chapter = 0;

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i][0].Equals(0)) // 말풍선 띄우기일 경우
            {
                tempList.Add((string)itemList[i][4]);
            }
            else if (itemList[i][0].Equals(1)) // 툴팁일 경우
            {
                tempList.Add((string)itemList[i][3]);
            }
        }

        if (fileName.Contains("TutorialChapter"))
        {
            chapter = int.Parse(fileName[15].ToString());
        }
        else if (fileName.Contains("Stage"))
        {
            // 싱글 컨텐츠 스테이지 추가 구현내용이 들어갈 것.
        }

        StageScriptExcel.WriteStageScript(chapter, tempList.ToArray());
        Debug.Log("Create Complete.");
    }

    void ImportLanguageDataToBinary(string fileName) // Import 버튼을 눌렀을 경우, 엑셀의 스크립트 정보를 바이너리로 저장
    {
        string[][] languageData = new string[3][];

        int chapter = 0;
        if (fileName.Contains("TutorialChapter"))
        {
            chapter = int.Parse(fileName[15].ToString());
        }
        else if (fileName.Contains("Stage"))
        {
            // 싱글 컨텐츠 스테이지 추가 구현내용이 들어갈 것.
        }

        for (int i = 0; i < 3; i++)
        {
            languageData[i] = StageScriptExcel.ReadStageScript(chapter, i);
        }

        BinaryFile.BinarySerialize<string[][]>(languageData, string.Format("Assets/Resources/Data/StageData/" + fileName + "Language"));
        Debug.Log(string.Format("Assets/Resources/Data/StageData/" + fileName + "Language file generated."));
        Debug.Log("Import Complete.");
    }
}
#endif