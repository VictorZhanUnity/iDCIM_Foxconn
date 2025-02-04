using System;
using System.Collections.Generic;
using Debug = VictorDev.Common.Debug;

/// IDCMP 機電系統管線種類
public static class IDcimpPipeHelper
{
    public static List<string> GetSubTypeEnumList(PipeType pipeType)
    {
        List<string> result = new List<string>();
        Type subType = GetSubTypeEnum(pipeType);
        if (subType != null)
        {
            Array values = Enum.GetValues(subType);
            foreach (object value in values)
            {
                result.Add(value.ToString());
            }
        }

        return result;
    }


    public static Type GetSubTypeEnum(PipeType pipeType)
    {
        Debug.Log($"pipeType: {pipeType}", true);
        return pipeType switch
        {
            PipeType.電力 => typeof(ElectricityType),
            PipeType.冷卻水 => typeof(CoolingWaterType),
            PipeType.空調 => typeof(AirConditioningType),
            PipeType.給排水 => typeof(WaterType),
            PipeType.消防 => typeof(FireType),
            _ => null
        };
    }
}

/// 管線種類
public enum PipeType
{
    電力 = 0,
    空調 = 1,
    給排水 = 2,
    冷卻水 = 3,
    消防 = 4,
}

/// 電力系統
public enum ElectricityType
{
    高低壓變電站 = 0,
    發電機 = 1,
}

/// 空調系統
public enum AirConditioningType
{
    一般空調 = 0,
    機房暖通空調HVAC = 1,
    地下停車場通風 = 2,
}

/// 給排水系統
public enum WaterType
{
    生活用水 = 0,
    污水 = 1,
    雨水 = 2,
}

/// 冷卻水系統
public enum CoolingWaterType
{
    冷凝水 = 0,
    冷卻廢水 = 1,
}

/// 消防系統
public enum FireType
{
    消防用水 = 0,
    排煙管線 = 1
}