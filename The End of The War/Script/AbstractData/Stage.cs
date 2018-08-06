using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage
{
    // '스테이지'에 대한 정보를 가지는 클래스.

    int stage_idx;
    StageGrade stage_grade;
    Material stage_texture;
    List<string> stage_reward;
    List<string> stage_enemylist;
    int stage_exp;

    public Stage(int stage_idx, StageGrade stage_grade, Material stage_texture, List<string> stage_reward, List<string> stage_enemylist, int stage_exp)
    {
        this.stage_idx = stage_idx;
        this.stage_grade = stage_grade;
        this.stage_texture = stage_texture;
        this.stage_reward = stage_reward;
        this.stage_enemylist = stage_enemylist;
        this.stage_exp = stage_exp;
    }

    public int Access_stageidx
    {
        get { return this.stage_idx; }
    }

    public Material Access_texture
    {
        get { return this.stage_texture; }
    }

    public List<string> Access_reward
    {
        get { return stage_reward; }
    }

    public List<string> Access_enemylist
    {
        get { return stage_enemylist; }
    }

    public int Access_exp
    {
        get { return stage_exp; }
    }

    public StageGrade Access_grade
    {
        get { return stage_grade; }

        set { stage_grade = value; }
    }
}
