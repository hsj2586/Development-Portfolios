using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocalAccount : MonoBehaviour
{
    // 로컬의 계정에 귀속되는 데이터들을 저장하고 있는 객체(클래스)
    private static LocalAccount localAccount = null;
    [SerializeField]
    int numOfLux; // 럭스의 개수
    [SerializeField]
    int totalExperienceOfSunGirl; // 태양소녀의 경험치
    List<ConstellationDictionary> listOfConstellation; // 보유 별자리 리스트

    public static LocalAccount Instance
    {
        get
        {
            if (localAccount == null)
            {
                localAccount = FindObjectOfType(typeof(LocalAccount)) as LocalAccount;
            }
            return localAccount;
        }
    }

    public int NumOfLux
    {
        get
        {
            return numOfLux;
        }

        set
        {
            numOfLux = value;
        }
    }

    public List<ConstellationDictionary> ListOfConstellation
    {
        get
        {
            return listOfConstellation;
        }

        set
        {
            listOfConstellation = value;
        }
    }

    public int TotalExperienceOfSunGirl
    {
        get
        {
            return totalExperienceOfSunGirl;
        }

        set
        {
            totalExperienceOfSunGirl = value;
        }
    }

    void Awake()
    {
        if (!localAccount)
        {
            localAccount = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);

        // 인게임 씬 테스트용 코드
        NewAccount();
    }

    public int GetLevelOfSun() // 태양소녀의 레벨
    {
        int level = 1;
        float temp = TotalExperienceOfSunGirl;

        for (int n = 1; ; n++) // 레벨이 3까지, 각각 100, 300의 경험치로 모델을 가정.
        {
            temp = temp - 100 * (Mathf.Pow(3, n - 1)); // 등비수열 공식 적용 ( 100, 300, 900 ... 이라 가정 )
            if (temp >= 0)
                level++;
            else
                return level;
        }
    }

    public float GetRatioExpOfLevel() // 현재 레벨의 경험치 비율
    {
        float temp = TotalExperienceOfSunGirl;

        for (int n = 1; ; n++) // 레벨이 3까지, 각각 100, 300의 경험치로 모델을 가정.
        {
            temp = temp - 100 * (Mathf.Pow(3, n - 1)); // 등비수열 공식 적용 ( 100, 300, 900 ... 이라 가정 )
            if (temp < 0)
            {
                return ((temp + 100 * (Mathf.Pow(3, n - 1))) / (100 * (Mathf.Pow(3, n - 1))));
            }
        }
    }

    public bool UpdateLux(int lux) // 럭스 소비 및 획득
    {
        if ((numOfLux + lux) < 0)
            return false;
        else
        {
            numOfLux += lux;
            return true;
        }
    }

    public void SaveAccount() // 게임 저장
    {
        AccountData account;
        account = new AccountData(numOfLux, totalExperienceOfSunGirl, listOfConstellation);
        BinaryFile.BinarySerialize<AccountData>(account, Application.persistentDataPath + "/SaveFile/LocalAccount/LocalAccount");
    }

    public void NewAccount() // 새 게임(새 계정)
    {
        AccountData account;
        listOfConstellation = new List<ConstellationDictionary>();
        listOfConstellation.Add(new ConstellationDictionary("Aries", 0, string.Format("양자리\n\n전후방의 적들에게 일격을 가합니다."), true));
        listOfConstellation.Add(new ConstellationDictionary("Sagittarius", 0, string.Format("사수자리\n\n활을 쏘아 원거리로 공격합니다."), false));
        listOfConstellation.Add(new ConstellationDictionary("Sagittarius", 0, string.Format("사수자리\n\n활을 쏘아 원거리로 공격합니다."), false));
        listOfConstellation.Add(new ConstellationDictionary("Sagittarius", 0, string.Format("사수자리\n\n활을 쏘아 원거리로 공격합니다."), false));
        account = new AccountData(1000, 0, listOfConstellation);

        BinaryFile.BinarySerialize<AccountData>(account, Application.persistentDataPath + "/SaveFile/LocalAccount/LocalAccount");
        account = BinaryFile.BinaryDeserialize<AccountData>(Application.persistentDataPath + "/SaveFile/LocalAccount/LocalAccount");

        numOfLux = account.numOfLux;
        listOfConstellation = account.listOfConstellation;
    }

    public void LoadAccount() // 불러오기
    {
        AccountData account = BinaryFile.BinaryDeserialize<AccountData>(Application.persistentDataPath + "/SaveFile/LocalAccount/LocalAccount");
        numOfLux = account.numOfLux;
        listOfConstellation = account.listOfConstellation;
    }

    public int GetSkillLevel(string constellationName)
    {
        for (int i = 0; i < ListOfConstellation.Count; i++)
        {
            if (ListOfConstellation[i].constellationName.Equals(constellationName))
            {
                return ListOfConstellation[i].GetLevelOfSkill();
            }
        }
        Debug.Log("존재하지 않는 스킬입니다.");
        return 0;
    }
}
