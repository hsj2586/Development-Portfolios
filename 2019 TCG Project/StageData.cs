using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StageData
{
    public List<object> dataList;

    public StageData(List<object> dataList)
    {
        this.dataList = dataList;
    }
}
