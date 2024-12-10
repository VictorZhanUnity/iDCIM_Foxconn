using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;

namespace VictorDev.Managers
{
    public class ModuleLoader : SingletonMonoBehaviour<ModuleLoader>
    {
        [Header(">>> 模組項目")]
        [SerializeField] private List<Module> modules;

        [Header(">>> 當所有模組皆初始化完成時")]
        public UnityEvent onAllModulesInitComplete = new UnityEvent();

        private void Awake() => InitAllModules();

        /// <summary>
        /// 初始化所有模組
        /// </summary>
        private void InitAllModules()
        {
            int counter = -1;
            void onInitHandler()
            {
                if (++counter < modules.Count)
                {
                    if (modules[counter] != null) modules[counter].OnInit(onInitHandler);
                    else onInitHandler();
                }
                else onAllModulesInitComplete?.Invoke();
            }
            onInitHandler();
        }
    }
}
