using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VictorDev._FakeData
{
    /// <summary>
    /// [帳戶資料]
    /// </summary>
    public abstract class _FakeData_Users
    {
        /// <summary>
        /// 隨機取得N筆用戶資料 {UserName、Account、Password、EMail}
        /// <para>+ 先用OrderBy(數字)打亂順序，再取前N筆資料</para>
        /// <para>+ Random範圍越大，隨機排序的組合越多；過大會影響效能，所以數字合理即可</para>
        /// </summary>
        public static List<Dictionary<string, string>> GetRandomeUsers(int count) => userList.OrderBy(x => Random.Range(0, userList.Count)).Take(count).ToList();

        #region [假資料]
        /// <summary>
        /// [假資料] 用戶資料 - UserName、Account、Password、EMail
        /// <para> ChatGPT: 請幫我列出50筆常見的中文姓名與其帳號、密碼、EMail，並用List<Dictionary<string, string>>的方式儲存，Dictionary的key欄位要用英文，中文姓名要三個字，中間不要有"小"這個字 </para>
        /// </summary>
        private static List<Dictionary<string, string>> userList = new List<Dictionary<string, string>>()
        {
            new Dictionary<string, string> { { "UserName", "李明亮" }, { "Account", "li_mingliang" }, { "Password", "password123" }, { "EMail", "li_mingliang@example.com" } },
            new Dictionary<string, string> { { "UserName", "王小華" }, { "Account", "wang_xiaohua" }, { "Password", "password123" }, { "EMail", "wang_xiaohua@example.com" } },
            new Dictionary<string, string> { { "UserName", "張靜雅" }, { "Account", "zhang_jingya" }, { "Password", "password123" }, { "EMail", "zhang_jingya@example.com" } },
            new Dictionary<string, string> { { "UserName", "陳偉豪" }, { "Account", "chen_weihao" }, { "Password", "password123" }, { "EMail", "chen_weihao@example.com" } },
            new Dictionary<string, string> { { "UserName", "林子豪" }, { "Account", "lin_zihao" }, { "Password", "password123" }, { "EMail", "lin_zihao@example.com" } },
            new Dictionary<string, string> { { "UserName", "劉志強" }, { "Account", "liu_zhiqiang" }, { "Password", "password123" }, { "EMail", "liu_zhiqiang@example.com" } },
            new Dictionary<string, string> { { "UserName", "黃文華" }, { "Account", "huang_wenhua" }, { "Password", "password123" }, { "EMail", "huang_wenhua@example.com" } },
            new Dictionary<string, string> { { "UserName", "徐佳琪" }, { "Account", "xu_jiaqi" }, { "Password", "password123" }, { "EMail", "xu_jiaqi@example.com" } },
            new Dictionary<string, string> { { "UserName", "鄭欣怡" }, { "Account", "zheng_xinyi" }, { "Password", "password123" }, { "EMail", "zheng_xinyi@example.com" } },
            new Dictionary<string, string> { { "UserName", "許志賢" }, { "Account", "xu_zhi_xian" }, { "Password", "password123" }, { "EMail", "xu_zhi_xian@example.com" } },
            new Dictionary<string, string> { { "UserName", "郭瑋玲" }, { "Account", "guo_weiling" }, { "Password", "password123" }, { "EMail", "guo_weiling@example.com" } },
            new Dictionary<string, string> { { "UserName", "謝麗華" }, { "Account", "xie_lihua" }, { "Password", "password123" }, { "EMail", "xie_lihua@example.com" } },
            new Dictionary<string, string> { { "UserName", "羅建國" }, { "Account", "luo_jianguo" }, { "Password", "password123" }, { "EMail", "luo_jianguo@example.com" } },
            new Dictionary<string, string> { { "UserName", "沈美君" }, { "Account", "shen_meijun" }, { "Password", "password123" }, { "EMail", "shen_meijun@example.com" } },
            new Dictionary<string, string> { { "UserName", "杜志明" }, { "Account", "du_zhiming" }, { "Password", "password123" }, { "EMail", "du_zhiming@example.com" } },
            new Dictionary<string, string> { { "UserName", "江浩然" }, { "Account", "jiang_haoran" }, { "Password", "password123" }, { "EMail", "jiang_haoran@example.com" } },
            new Dictionary<string, string> { { "UserName", "賴文君" }, { "Account", "lai_wenjun" }, { "Password", "password123" }, { "EMail", "lai_wenjun@example.com" } },
            new Dictionary<string, string> { { "UserName", "姚碧霞" }, { "Account", "yao_bixia" }, { "Password", "password123" }, { "EMail", "yao_bixia@example.com" } },
            new Dictionary<string, string> { { "UserName", "韋嘉豪" }, { "Account", "wei_jiahao" }, { "Password", "password123" }, { "EMail", "wei_jiahao@example.com" } },
            new Dictionary<string, string> { { "UserName", "鄧文博" }, { "Account", "deng_wenbo" }, { "Password", "password123" }, { "EMail", "deng_wenbo@example.com" } },
            new Dictionary<string, string> { { "UserName", "張宇寧" }, { "Account", "zhang_yuning" }, { "Password", "password123" }, { "EMail", "zhang_yuning@example.com" } },
            new Dictionary<string, string> { { "UserName", "黃建華" }, { "Account", "huang_jianhua" }, { "Password", "password123" }, { "EMail", "huang_jianhua@example.com" } },
            new Dictionary<string, string> { { "UserName", "程文彥" }, { "Account", "cheng_wenyan" }, { "Password", "password123" }, { "EMail", "cheng_wenyan@example.com" } },
            new Dictionary<string, string> { { "UserName", "吳瑋君" }, { "Account", "wu_weijun" }, { "Password", "password123" }, { "EMail", "wu_weijun@example.com" } },
            new Dictionary<string, string> { { "UserName", "蔡嘉穎" }, { "Account", "cai_jiaying" }, { "Password", "password123" }, { "EMail", "cai_jiaying@example.com" } },
            new Dictionary<string, string> { { "UserName", "黎偉倫" }, { "Account", "li_weile" }, { "Password", "password123" }, { "EMail", "li_weile@example.com" } },
            new Dictionary<string, string> { { "UserName", "邱曼君" }, { "Account", "qiu_manjun" }, { "Password", "password123" }, { "EMail", "qiu_manjun@example.com" } },
            new Dictionary<string, string> { { "UserName", "郭信宏" }, { "Account", "guo_xinhong" }, { "Password", "password123" }, { "EMail", "guo_xinhong@example.com" } },
            new Dictionary<string, string> { { "UserName", "余欣妍" }, { "Account", "yu_xinyan" }, { "Password", "password123" }, { "EMail", "yu_xinyan@example.com" } },
            new Dictionary<string, string> { { "UserName", "唐建新" }, { "Account", "tang_jianxin" }, { "Password", "password123" }, { "EMail", "tang_jianxin@example.com" } },
            new Dictionary<string, string> { { "UserName", "何志偉" }, { "Account", "he_zhiwei" }, { "Password", "password123" }, { "EMail", "he_zhiwei@example.com" } },
            new Dictionary<string, string> { { "UserName", "蔣婷婷" }, { "Account", "jiang_tingting" }, { "Password", "password123" }, { "EMail", "jiang_tingting@example.com" } },
            new Dictionary<string, string> { { "UserName", "高志強" }, { "Account", "gao_zhiqiang" }, { "Password", "password123" }, { "EMail", "gao_zhiqiang@example.com" } },
            new Dictionary<string, string> { { "UserName", "邵小玲" }, { "Account", "shao_xiaoling" }, { "Password", "password123" }, { "EMail", "shao_xiaoling@example.com" } },
            new Dictionary<string, string> { { "UserName", "呂靜雯" }, { "Account", "lv_jingwen" }, { "Password", "password123" }, { "EMail", "lv_jingwen@example.com" } },
            new Dictionary<string, string> { { "UserName", "廖美麗" }, { "Account", "liao_meili" }, { "Password", "password123" }, { "EMail", "liao_meili@example.com" } },
            new Dictionary<string, string> { { "UserName", "蘇貞慧" }, { "Account", "su_zhenghui" }, { "Password", "password123" }, { "EMail", "su_zhenghui@example.com" } },
            new Dictionary<string, string> { { "UserName", "彭耀文" }, { "Account", "peng_yaowen" }, { "Password", "password123" }, { "EMail", "peng_yaowen@example.com" } },
            new Dictionary<string, string> { { "UserName", "鍾秀華" }, { "Account", "zhong_xiuhua" }, { "Password", "password123" }, { "EMail", "zhong_xiuhua@example.com" } },
            new Dictionary<string, string> { { "UserName", "蔡韋志" }, { "Account", "cai_weizhi" }, { "Password", "password123" }, { "EMail", "cai_weizhi@example.com" } },
            new Dictionary<string, string> { { "UserName", "賴美珍" }, { "Account", "lai_meizhen" }, { "Password", "password123" }, { "EMail", "lai_meizhen@example.com" } },
            new Dictionary<string, string> { { "UserName", "鄭俊宇" }, { "Account", "zheng_junyu" }, { "Password", "password123" }, { "EMail", "zheng_junyu@example.com" } },
            new Dictionary<string, string> { { "UserName", "梁文彥" }, { "Account", "liang_wenyan" }, { "Password", "password123" }, { "EMail", "liang_wenyan@example.com" } },
            new Dictionary<string, string> { { "UserName", "蔡子涵" }, { "Account", "cai_zihan" }, { "Password", "password123" }, { "EMail", "cai_zihan@example.com" } },
            new Dictionary<string, string> { { "UserName", "王俊傑" }, { "Account", "wang_junjie" }, { "Password", "password123" }, { "EMail", "wang_junjie@example.com" } },
            new Dictionary<string, string> { { "UserName", "陳雅玲" }, { "Account", "chen_yaling" }, { "Password", "password123" }, { "EMail", "chen_yaling@example.com" } },
            new Dictionary<string, string> { { "UserName", "張馨文" }, { "Account", "zhang_xinwen" }, { "Password", "password123" }, { "EMail", "zhang_xinwen@example.com" } },
            new Dictionary<string, string> { { "UserName", "劉光輝" }, { "Account", "liu_guanghui" }, { "Password", "password123" }, { "EMail", "liu_guanghui@example.com" } },
            new Dictionary<string, string> { { "UserName", "李怡君" }, { "Account", "li_yijun" }, { "Password", "password123" }, { "EMail", "li_yijun@example.com" } },
            new Dictionary<string, string> { { "UserName", "吳志雄" }, { "Account", "wu_zhi_xiong" }, { "Password", "password123" }, { "EMail", "wu_zhi_xiong@example.com" } },
            new Dictionary<string, string> { { "UserName", "胡子傑" }, { "Account", "hu_zijie" }, { "Password", "password123" }, { "EMail", "hu_zijie@example.com" } },
            new Dictionary<string, string> { { "UserName", "譚婉君" }, { "Account", "tan_wanjun" }, { "Password", "password123" }, { "EMail", "tan_wanjun@example.com" } },
            new Dictionary<string, string> { { "UserName", "廖美珍" }, { "Account", "liao_meizhen" }, { "Password", "password123" }, { "EMail", "liao_meizhen@example.com" } },
            new Dictionary<string, string> { { "UserName", "洪雅惠" }, { "Account", "hong_yahui" }, { "Password", "password123" }, { "EMail", "hong_yahui@example.com" } },
            new Dictionary<string, string> { { "UserName", "施建宏" }, { "Account", "shi_jianhong" }, { "Password", "password123" }, { "EMail", "shi_jianhong@example.com" } },
            new Dictionary<string, string> { { "UserName", "雷子涵" }, { "Account", "lei_zihan" }, { "Password", "password123" }, { "EMail", "lei_zihan@example.com" } },
            new Dictionary<string, string> { { "UserName", "顧家豪" }, { "Account", "gu_jiahao" }, { "Password", "password123" }, { "EMail", "gu_jiahao@example.com" } },
            new Dictionary<string, string> { { "UserName", "賴文清" }, { "Account", "lai_wenqing" }, { "Password", "password123" }, { "EMail", "lai_wenqing@example.com" } },
            new Dictionary<string, string> { { "UserName", "李欣怡" }, { "Account", "li_xinyi" }, { "Password", "password123" }, { "EMail", "li_xinyi@example.com" } },
            new Dictionary<string, string> { { "UserName", "杜宇寧" }, { "Account", "du_yuning" }, { "Password", "password123" }, { "EMail", "du_yuning@example.com" } },
            new Dictionary<string, string> { { "UserName", "曾秀雯" }, { "Account", "zeng_xiuwen" }, { "Password", "password123" }, { "EMail", "zeng_xiuwen@example.com" } }
        };
        #endregion
    }
}
