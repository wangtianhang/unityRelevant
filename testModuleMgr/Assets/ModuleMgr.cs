using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 理念上分为一级模块和二级模块 二级模块依赖一级模块
interface IModule
{
    //bool IsScene();
    void LeaveSelfToNextUI();

    void LeaveSelfToNextScene();

    void LeaveSelfToPrevUI();

    void LeaveSelfToPrevScene();

    void ForwardJumpToSelf(System.Object param);

    void BackJumpToSelf();
}

public enum ModuleId
{
    None,

    Scene1,
    Scene2,
    Scene3,

    UIModule1,
    UIModule2,
    UIModule3,
    UIModule4,
    UIModule5,
    UIModule6,
    UIModule7,
    UIModule8,
    UIModule9,
}

class ModuleData
{
    public ModuleId m_moduleId;
    public System.Object m_param;
    public IModule m_module;
    public GameObject m_go;

    public ModuleId m_comeFromScene = ModuleId.None;
}

public interface IESCHandle
{
    void EscCallback();
}

/// <summary>
/// 不在负责uidepth之类的处理
/// </summary>
public class ModuleMgr : MonoBehaviour {

    //List<ModuleData> m_sceneModuleList = new List<ModuleData>();
    List<ModuleData> m_moduleList = new List<ModuleData>();

    List<IESCHandle> m_innerWindowList = new List<IESCHandle>();

    public void Start()
    {
        Test();
    }

    public void Test()
    {
        System.Random random = new System.Random(0);
        for(int i = 0; i <  100; ++i)
        {
            int index = random.Next(0, 100);
            if(index < 50)
            {
                int module = random.Next(1, (int)ModuleId.UIModule9 + 1);
                JumpToModule((ModuleId)module, module);
            }
            else
            {
                BackModule();
            }
        }
    }

    public void JumpToModule(ModuleId id, System.Object param)
    {
        //todo 需要考虑四种情况 
        // scene to scene 
        // scene to ui
        // ui to scene
        // ui to ui

        if(m_moduleList.Count == 0)
        {
            // 直接推入队列
        }
        else
        {
            ModuleData lastModule = m_moduleList[m_moduleList.Count -1];
            if (IsScene(lastModule.m_moduleId) && IsScene(id))
            {
                lastModule.m_module.LeaveSelfToNextScene();
                ModuleData moduleData = CreateModule(id, param);
                m_moduleList.Add(moduleData);
            }
            else if (IsScene(lastModule.m_moduleId) && !IsScene(id))
            {
                if(IsUIModuleDependencyScene(id, lastModule.m_moduleId))
                {
                    lastModule.m_module.LeaveSelfToNextUI();
                    // 依赖当前场景, 直接弹出
                    ModuleData moduleData = CreateModule(id, param);
                    moduleData.m_comeFromScene = GetCurScene();
                    m_moduleList.Add(moduleData);
                }
                else
                {
                    lastModule.m_module.LeaveSelfToNextScene();
                    ModuleId dependencyScene = GetDependencyScene(id);
                    // 不依赖当前场景, 跳转场景并跳转到指定界面
                    ModuleData moduleData = CreateSceneAndJumpUI(dependencyScene, id, param);
                    m_moduleList.Add(moduleData);
                }
            }
            else if(!IsScene(lastModule.m_moduleId) && IsScene(id))
            {
                if(id != GetCurScene())
                {
                    // todo 跳到不同的场景
                    for (int i = m_moduleList.Count - 1; i >= 0; i++)
                    {
                        if (m_moduleList[i].m_moduleId == GetCurScene())
                        {
                            m_moduleList[i].m_module.LeaveSelfToNextUI();
                        }
                    }
                    lastModule.m_module.LeaveSelfToNextScene();
                    ModuleData moduleData = CreateModule(id, param);
                    m_moduleList.Add(moduleData);
                }
                else
                {
                    // todo 跳回当前场景
                    for(int i = m_moduleList.Count - 1; i >= 0; i++ )
                    {
                        if(m_moduleList[i].m_moduleId == GetCurScene())
                        {
                            m_moduleList[i].m_module.ForwardJumpToSelf(param);

                            ModuleData moduleData = new ModuleData();
                            moduleData.m_moduleId = m_moduleList[i].m_moduleId;
                            moduleData.m_param = m_moduleList[i].m_param;
                            moduleData.m_go = m_moduleList[i].m_go;
                            moduleData.m_module = m_moduleList[i].m_module;
                            m_moduleList.Add(moduleData);

                            break;
                        }
                    }

                }
            }
            else if(!IsScene(lastModule.m_moduleId) && !IsScene(id))
            {
                ModuleId dependencyScene = GetDependencyScene(id);
                if(dependencyScene == GetCurScene())
                {
                    lastModule.m_module.LeaveSelfToNextUI();
                    ModuleData moduleData = CreateModule(id, param);
                    moduleData.m_comeFromScene = GetCurScene();
                    m_moduleList.Add(moduleData);
                }
                else
                {
                    lastModule.m_module.LeaveSelfToNextScene();
                    ModuleData moduleData = CreateSceneAndJumpUI(dependencyScene, id, param);
                    m_moduleList.Add(moduleData);
                }
            }
        }
    }

    ModuleId GetCurScene()
    {
        return ModuleId.None;
    }

    bool IsScene(ModuleId id)
    {
        return false;
    }

    bool IsUIModuleDependencyScene(ModuleId uiModule, ModuleId sceneModule)
    {
        return false;
    }

    ModuleId GetDependencyScene(ModuleId uiModule)
    {
        return ModuleId.None;
    }

    public void BackModule()
    {
        //todo 需要考虑四种情况 
        // scene to scene 
        // scene to ui
        // ui to scene
        // ui to ui
        ModuleData curModule = m_moduleList[m_moduleList.Count - 1];
        ModuleData lastModule = m_moduleList[m_moduleList.Count - 2];
        m_moduleList.RemoveAt(m_moduleList.Count - 1);
        if (IsScene(curModule.m_moduleId) && IsScene(lastModule.m_moduleId))
        {
            curModule.m_module.LeaveSelfToPrevScene();
            ModuleData moduleData = CreateModule(lastModule.m_moduleId, lastModule.m_param);
            lastModule.m_go = moduleData.m_go;
            lastModule.m_module = moduleData.m_module;
        }
        else if (IsScene(curModule.m_moduleId) && !IsScene(lastModule.m_moduleId))
        {
            if (lastModule.m_comeFromScene == curModule.m_moduleId)
            {
                curModule.m_module.LeaveSelfToPrevUI();
                ModuleData moduleData = CreateModule(lastModule.m_moduleId, lastModule.m_param);
                lastModule.m_go = moduleData.m_go;
                lastModule.m_module = moduleData.m_module;
            }
            else
            {
                // 还原上一个场景的go
                curModule.m_module.LeaveSelfToPrevScene();
                ModuleData moduleData = null;
                for (int i = m_moduleList.Count - 1; i >= 0; i++)
                {
                    if (m_moduleList[i].m_moduleId == lastModule.m_comeFromScene)
                    {
                        moduleData = CreateSceneAndBackUI(m_moduleList[i].m_moduleId, m_moduleList[i].m_param, lastModule.m_moduleId, lastModule.m_param);
                        break;
                    }
                }

                for (int i = m_moduleList.Count - 1; i >= 0; i++)
                {
                    if (m_moduleList[i].m_moduleId == lastModule.m_comeFromScene)
                    {
                        m_moduleList[i].m_go = moduleData.m_go;
                        m_moduleList[i].m_module = moduleData.m_module;
                    }
                }
            }
        }
        else if (!IsScene(curModule.m_moduleId) && IsScene(lastModule.m_moduleId))
        {
            if (curModule.m_comeFromScene == lastModule.m_moduleId)
            {
                curModule.m_module.LeaveSelfToPrevScene();
                lastModule.m_module.BackJumpToSelf();
            }
            else
            {
                throw new System.Exception("应该不存在从ui返回场景来源却不是场景");
            }
        }
        else if (!IsScene(curModule.m_moduleId) && !IsScene(lastModule.m_moduleId))
        {
            if (curModule.m_comeFromScene == lastModule.m_comeFromScene)
            {
                curModule.m_module.LeaveSelfToPrevUI();
                ModuleData moduleData = CreateModule(lastModule.m_moduleId, lastModule.m_param);
                lastModule.m_go = moduleData.m_go;
                lastModule.m_module = moduleData.m_module;
            }
            else
            {
                throw new System.Exception("应该不存在从ui返回ui来源却不是场景");
            }
        }
    }

    /// <summary>
    /// 从scene返回ui后调用
    /// </summary>
    /// <param name="uiId"></param>
    /// <param name="uiParam"></param>
    public void _ShowUIModuleForBackScene(ModuleId uiId, System.Object uiParam)
    {
        ModuleData moduleData = CreateModule(uiId, uiParam);
        m_moduleList[m_moduleList.Count - 1].m_go = moduleData.m_go;
        m_moduleList[m_moduleList.Count - 1].m_module = moduleData.m_module;
    }

    public void AddInnerModuleEscWindow(IESCHandle escCallback)
    {

    }

    ModuleData CreateModule(ModuleId id, System.Object param)
    {
        // 如果为场景 创建modulego module内部自己去切换场景啊 加载啊什么的 
        return null;
    }

    /// <summary>
    /// 各场景应该支持默认参数
    /// 等场景加载完成后 场景辅助进行二次跳转
    /// </summary>
    /// <param name="sceneId"></param>
    /// <param name=""></param>
    /// <returns></returns>
    ModuleData CreateSceneAndJumpUI(ModuleId sceneId, ModuleId uiId, System.Object param)
    {
        return null;
    }

    ModuleData CreateSceneAndBackUI(ModuleId sceneId, System.Object sceneParam, ModuleId uiId, System.Object uiParam)
    {
        return null;
    }
}
