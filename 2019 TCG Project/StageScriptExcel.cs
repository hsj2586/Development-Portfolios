using UnityEngine;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Collections.Generic;

[System.Serializable]
public static class StageScriptExcel
{
    public enum ColumnType { Index, Chapter, Sequence, Script_kor, Script_eng, Script_chi }
    private const string filepath = "Assets/Resources/Data/Excel/StageScript.xlsx";
    static IWorkbook book;
    static ISheet sheet;

    public static void WriteStageScript(int chapter, string[] scripts) // 스크립트 데이터를 엑셀에 쓰는 메소드
    {
        using (FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.ReadWrite))
        {
            book = new XSSFWorkbook(stream);
            sheet = book.GetSheetAt(0);
            int startIndex = SearchChapter(chapter);
            int tempSequence;

            if (!startIndex.Equals(0)) // 챕터가 기존 엑셀에 존재하는 경우
            {
                int shift = scripts.Length - ChapterScriptCount(chapter);
                if (shift != 0)
                    ShiftRows(startIndex + ChapterScriptCount(chapter) - 1, sheet.LastRowNum, shift);
            }
            else // 챕터가 기존 엑셀에 존재하지 않는 경우
            {
                startIndex = sheet.LastRowNum + 1;
            }

            for (int i = startIndex; i < startIndex + scripts.Length; i++)
            {
                sheet.CreateRow(i);
                tempSequence = i - startIndex + 1;
                SetCellValue(i, ColumnType.Index, i);
                SetCellValue(i, ColumnType.Chapter, chapter);
                SetCellValue(i, ColumnType.Sequence, tempSequence);
                SetCellValue(i, ColumnType.Script_kor, scripts[tempSequence - 1]);
                SetCellValue(i, ColumnType.Script_eng, "-");
                SetCellValue(i, ColumnType.Script_chi, "-");
            }

            using (var fs = new FileStream(filepath, FileMode.Create))
            {
                book.Write(fs);
            }
        }
    }

    public static string[] ReadStageScript(int chapter, int language) // 스크립트 데이터를 엑셀로부터 읽는 메소드, 0=kor 1=eng 2=chi
    {
        List<string> scripts = new List<string>();
        using (FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.ReadWrite))
        {
            book = new XSSFWorkbook(stream);
            sheet = book.GetSheetAt(0);
            int startIndex = SearchChapter(chapter);
            for (int i = startIndex; i <= sheet.LastRowNum; i++)
            {
                if (chapter != (int)GetCellValue(i, ColumnType.Chapter)) break;

                scripts.Add((string)GetCellValue(i, (ColumnType)(3 + language)));
            }
        }
        return scripts.ToArray();
    }

    static object GetCellValue(int index, ColumnType colType) // 셀의 값을 읽어오는 메소드
    {
        if (colType.Equals(ColumnType.Script_kor) || colType.Equals(ColumnType.Script_eng) || colType.Equals(ColumnType.Script_chi))
        {
            return sheet.GetRow(index).GetCell((int)colType).StringCellValue;
        }
        else
        {
            return int.Parse(sheet.GetRow(index).GetCell((int)colType).StringCellValue.ToString());
        }
    }

    static void SetCellValue(int index, ColumnType colType, object value) // 셀의 값을 변경하는 메소드
    {
        sheet.GetRow(index).CreateCell((int)colType);
        switch (colType)
        {
            case ColumnType.Index:
                sheet.GetRow(index).GetCell((int)colType).SetCellValue(value.ToString());
                break;
            case ColumnType.Chapter:
                sheet.GetRow(index).GetCell((int)colType).SetCellValue(value.ToString());
                break;
            case ColumnType.Sequence:
                sheet.GetRow(index).GetCell((int)colType).SetCellValue(value.ToString());
                break;
            case ColumnType.Script_kor:
                sheet.GetRow(index).GetCell((int)colType).SetCellValue((string)value);
                break;
            case ColumnType.Script_eng:
                sheet.GetRow(index).GetCell((int)colType).SetCellValue((string)value);
                break;
            case ColumnType.Script_chi:
                sheet.GetRow(index).GetCell((int)colType).SetCellValue((string)value);
                break;
        }
    }

    static int SearchChapter(int chapter) // 해당 챕터를 찾아 맨 처음의 인덱스를 반환하는 메소드 (없을 경우 0 반환)
    {
        if (sheet.LastRowNum != 0)
        {
            for (int index = 1; index <= sheet.LastRowNum; index++)
            {
                if (chapter == (int)GetCellValue(index, ColumnType.Chapter))
                {
                    return index;
                }
            }
        }
        return 0;
    }

    static int ChapterScriptCount(int chapter) // 해당 챕터의 스크립트 개수를 반환하는 메소드
    {
        int count = 0;
        for (int i = 1; i <= sheet.LastRowNum; i++)
        {
            if (chapter == (int)GetCellValue(i, ColumnType.Chapter))
            {
                count++;
            }
        }
        return count;
    }

    static void ShiftRows(int srcIndex, int destIndex, int shift) // 엑셀의 행을 shift하는 메소드
    {
        int[] chapterCopy = new int[destIndex - srcIndex + 1];
        int[] sequenceCopy = new int[destIndex - srcIndex + 1];
        string[] scriptCopy = new string[destIndex - srcIndex + 1];

        for (int i = srcIndex; i <= destIndex; i++) // 기존 행의 정보 복사
        {
            chapterCopy[i - srcIndex] = (int)GetCellValue(i, ColumnType.Chapter);
            sequenceCopy[i - srcIndex] = (int)GetCellValue(i, ColumnType.Sequence);
            scriptCopy[i - srcIndex] = (string)GetCellValue(i, ColumnType.Script_kor);
        }

        for (int i = srcIndex; i <= destIndex; i++) // 복사한 데이터를 shift한 위치에 입력
        {
            sheet.CreateRow(i + shift);
            int tempSequence = i - srcIndex + 1;
            SetCellValue(i + shift, ColumnType.Index, i + shift);
            SetCellValue(i + shift, ColumnType.Chapter, chapterCopy[i - srcIndex]);
            SetCellValue(i + shift, ColumnType.Sequence, sequenceCopy[i - srcIndex]);
            SetCellValue(i + shift, ColumnType.Script_kor, scriptCopy[i - srcIndex]);
        }

        if (shift < 0) // 행을 위로 당겼을 경우
        {
            int tempLastRowNum = sheet.LastRowNum;
            for (int i = tempLastRowNum + shift; i < tempLastRowNum; i++)
            {
                DeleteRow(sheet.GetRow(i));
            }
        }
    }

    static void DeleteRow(IRow row) // 행을 삭제하는 메소드
    {
        sheet.RemoveRow(row);
        int rowIndex = row.RowNum;
        int lastRowNum = sheet.LastRowNum;
        if (rowIndex >= 0 && rowIndex < lastRowNum)
        {
            sheet.ShiftRows(rowIndex + 1, lastRowNum, -1);
        }
    }
}
