using System;

/// <summary>
/// DateTime日期格式
/// </summary>
public static class DateTimeFormatter
{
    /// <summary>
    /// 2024-11-24 13:33:22
    /// </summary>
    public static string FullDateTimeFormat => $"{FullDateFormat} {FullTimeFormat}";

    /// <summary>
    /// 2024-11-24T13:33:22
    /// </summary>
    public static string FullDateTimeFormatWithT => $"{FullDateFormat}T{FullTimeFormat}";

    /// <summary>
    /// 2024-11-24
    /// </summary>
    public static string FullDateFormat => "yyyy-MM-dd";
    /// <summary>
    /// 13:33:22
    /// </summary>
    public static string FullTimeFormat => "HH:mm:ss";
}
