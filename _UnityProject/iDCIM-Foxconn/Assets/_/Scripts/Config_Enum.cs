using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config_Enum 
{
    public enum enumAccountRole
    {
        全部, 最大權限管理者, 巡檢員, 機保人員, BA管理者, 機房管理者, 機房一般使用者, 一般訪客
    }
    public enum enumLanguage
    {
        簡体中文, English, 繁體中文
    }
    public enum enumAccountStatus
    {
        全部, 啟用, 停用
    }
    public enum enumNetType
    {
        local
    }
}
