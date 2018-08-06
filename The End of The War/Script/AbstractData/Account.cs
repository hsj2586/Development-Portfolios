using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Account
{
    // '계정'에 대한 정보를 가지는 추상 클래스.

    [SerializeField]
    private string username;
    [SerializeField]
    private int level;
    [SerializeField]
    private int gold;
    [SerializeField]
    private int numofCharacter;
    [SerializeField]
    private int current_stage;
    [SerializeField]
    private List<GameObject> character;

    public Account(string username, int level, int gold, int numofCharacter, int current_stage, List<GameObject> character)
    {
        this.username = username;
        this.level = level;
        this.gold = gold;
        this.numofCharacter = numofCharacter;
        this.current_stage = current_stage;
        this.character = character;
    }

    public string Access_name
    {
        get { return this.username; }
        set { this.username = value; }
    }

    public int Access_gold
    {
        get { return this.gold; }
        set { this.gold = value; }
    }

    public int Access_level
    {
        get { return this.level; }
        set { this.level = value; }
    }

    public int Access_numofcharacter
    {
        get { return this.numofCharacter; }
        set { this.numofCharacter = value; }
    }

    public List<GameObject> Access_characters
    {
        get { return this.character; }
        set { this.character = value; }
    }

    public GameObject Access_character(int idx)
    {
        return this.character[idx];
    }

    public int Access_currentstage
    {
        get { return this.current_stage; }
        set { this.current_stage = value; }
    }
}
