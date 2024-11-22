namespace VictorDev.IAQ
{

    public abstract class IAQ_DataConverter
    {

        /// <summary>
        /// 轉換原始值單位
        /// </summary>
        public static void ConvertToDisplayValue(ref Data_IAQ data)
        {/**
            float displayValue = 0;
            string displayUnitName = "";
            
             * 判斷哪種欄位，再進行值換算
             * Switch columnName:
             * {
             * }
             * ****/

            #region [暫時用原始值與單位]
            /* displayValue = data.SourceValue;
             displayUnitName = data.SourceUnitName;*/
            #endregion

            //   data.SetDisplayData(displayValue, displayUnitName);
        }
    }
}
