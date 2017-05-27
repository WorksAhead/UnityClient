using System.Collections.Generic;
using ArkCrossEngine;

public class GameObjectManager
{
    List<UnityEngine.GameObject> m_AttackEffectList = new List<UnityEngine.GameObject>();
    int m_IdxAttackEffectList = -1;

    List<UnityEngine.GameObject> m_CriticalDamageList = new List<UnityEngine.GameObject>();
    int m_IdxCriticalDamageList = -1;

    List<UnityEngine.GameObject> m_DamageForAddHeroList = new List<UnityEngine.GameObject>();
    int m_IdxDamageForAddHeroList = -1;

    List<UnityEngine.GameObject> m_DamageForCutHeroList = new List<UnityEngine.GameObject>();
    int m_IdxDamageForCutHeroList = -1;


    List<UnityEngine.GameObject> m_EnergyAddList = new List<UnityEngine.GameObject>();
    int m_IdxEnergyAddList = -1;

    List<UnityEngine.GameObject> m_EnergyCutList = new List<UnityEngine.GameObject>();
    int m_IdxEnergyCutList = -1;


    List<UnityEngine.GameObject> m_ScreenTipList = new List<UnityEngine.GameObject>();
    int m_IdxScreenTipList = -1;

    List<UnityEngine.GameObject> m_GainMoneyList = new List<UnityEngine.GameObject>();
    int m_IdxGainMoneyList = -1;


    #region Singleton
    private static GameObjectManager s_Instance = new GameObjectManager();
    public static GameObjectManager Instance
    {
        get { return s_Instance; }
    }
    #endregion

    private GameObjectManager()
    {
        ClearAllObjectList();
    }

    public void ClearAllObjectList()
    {
        ClearObjectList("AttackEffect");
        ClearObjectList("CriticalDamage");
        ClearObjectList("DamageForAddHero");
        ClearObjectList("DamageForCutHero");
        ClearObjectList("EnergyAdd");
        ClearObjectList("EnergyCut");
        ClearObjectList("ScreenTip");
        ClearObjectList("GainMoney");
    }

    public void ClearObjectList(string objType)
    {
        switch (objType)
        {
            case "AttackEffect":
                for (int i = 0; i < m_AttackEffectList.Count; i++)
                {
                    if (m_AttackEffectList[i] != null)
                    {
                        UnityEngine.GameObject.Destroy(m_AttackEffectList[i]);
                    }
                }
                m_AttackEffectList.Clear();
                m_IdxAttackEffectList = -1;
                break;
            case "CriticalDamage":
                for (int i = 0; i < m_CriticalDamageList.Count; i++)
                {
                    if (m_CriticalDamageList[i] != null)
                    {
                        UnityEngine.GameObject.Destroy(m_CriticalDamageList[i]);
                    }
                }
                m_CriticalDamageList.Clear();
                m_IdxCriticalDamageList = -1;
                break;
            case "DamageForAddHero":
                for (int i = 0; i < m_DamageForAddHeroList.Count; i++)
                {
                    if (m_DamageForAddHeroList[i] != null)
                    {
                        UnityEngine.GameObject.Destroy(m_DamageForAddHeroList[i]);
                    }
                }
                m_DamageForAddHeroList.Clear();
                m_IdxDamageForAddHeroList = -1;
                break;
            case "DamageForCutHero":
                for (int i = 0; i < m_DamageForCutHeroList.Count; i++)
                {
                    if (m_DamageForCutHeroList[i] != null)
                    {
                        UnityEngine.GameObject.Destroy(m_DamageForCutHeroList[i]);
                    }
                }
                m_DamageForCutHeroList.Clear();
                m_IdxDamageForCutHeroList = -1;
                break;
            case "EnergyAdd":
                for (int i = 0; i < m_EnergyAddList.Count; i++)
                {
                    if (m_EnergyAddList[i] != null)
                    {
                        UnityEngine.GameObject.Destroy(m_EnergyAddList[i]);
                    }
                }
                m_EnergyAddList.Clear();
                m_IdxEnergyAddList = -1;
                break;
            case "EnergyCut":
                for (int i = 0; i < m_EnergyCutList.Count; i++)
                {
                    if (m_EnergyCutList[i] != null)
                    {
                        UnityEngine.GameObject.Destroy(m_EnergyCutList[i]);
                    }
                }
                m_EnergyCutList.Clear();
                m_IdxEnergyCutList = -1;
                break;
            case "ScreenTip":
                for (int i = 0; i < m_ScreenTipList.Count; i++)
                {
                    if (m_ScreenTipList[i] != null)
                    {
                        UnityEngine.GameObject.Destroy(m_ScreenTipList[i]);
                    }
                }
                m_ScreenTipList.Clear();
                m_IdxScreenTipList = -1;
                break;
            case "GainMoney":
                for (int i = 0; i < m_GainMoneyList.Count; i++)
                {
                    if (m_GainMoneyList[i] != null)
                    {
                        UnityEngine.GameObject.Destroy(m_GainMoneyList[i]);
                    }
                }
                m_GainMoneyList.Clear();
                m_IdxGainMoneyList = -1;
                break;
            default:
                break;
        }
    }

    public void NewObject(string objType, int number, UnityEngine.GameObject parent)
    {
        List<UnityEngine.GameObject> tempList = null;
        switch (objType)
        {
            case "AttackEffect":
                tempList = m_AttackEffectList;
                break;
            case "CriticalDamage":
                tempList = m_CriticalDamageList;
                break;
            case "DamageForAddHero":
                tempList = m_DamageForAddHeroList;
                break;
            case "DamageForCutHero":
                tempList = m_DamageForCutHeroList;
                break;
            case "EnergyAdd":
                tempList = m_EnergyAddList;
                break;
            case "EnergyCut":
                tempList = m_EnergyCutList;
                break;
            case "ScreenTip":
                tempList = m_ScreenTipList;
                break;
            case "GainMoney":
                tempList = m_GainMoneyList;
                break;
            default:
                break;
        }
        if (number > 0 && tempList != null)
        {
            UnityEngine.Object prefab = CrossObjectHelper.TryCastObject<UnityEngine.GameObject>(ArkCrossEngine.ResourceSystem.GetSharedResource(UIManager.Instance.GetPathByName(objType)));
            if (prefab != null)
            {
                UnityEngine.GameObject go = null;
                UnityEngine.Transform tf = null;
                BloodAnimationScript bas = null;
                for (int i = 0; i < number; i++)
                {
                    go = UnityEngine.GameObject.Instantiate(prefab) as UnityEngine.GameObject;
                    if (go != null)
                    {
                        tf = go.transform;
                        if (parent != null)
                        {
                            tf.parent = parent.transform;
                        }
                        else
                        {
                            tf.parent = null;
                        }
                        tf.localScale = UnityEngine.Vector3.one;
                        bas = go.GetComponent<BloodAnimationScript>();
                        if (bas != null)
                        {
                            //bas.InitState(); //StopAnimation()里面已经调用该接口
                            bas.StopAnimation();
                        }
                        tempList.Add(go);
                    }
                }
            }
        }
    }


    private int PlusOne(int idx, List<UnityEngine.GameObject> reflst)
    {
        if (reflst.Count == 0)
        {
            return 0;
        }

        if (idx + 1 == reflst.Count)
        {
            return 0;
        }
        else
        {
            return idx + 1;
        }

    }


    private int PlusIdx(string objType, List<UnityEngine.GameObject> reflst)
    {
        int ret = 0;
        switch (objType)
        {
            case "AttackEffect":
                m_IdxAttackEffectList = PlusOne(m_IdxAttackEffectList, m_AttackEffectList);
                ret = m_IdxAttackEffectList;
                break;
            case "CriticalDamage":
                m_IdxCriticalDamageList = PlusOne(m_IdxCriticalDamageList, m_CriticalDamageList);
                ret = m_IdxCriticalDamageList;
                break;
            case "DamageForAddHero":
                m_IdxDamageForAddHeroList = PlusOne(m_IdxDamageForAddHeroList, m_DamageForAddHeroList);
                ret = m_IdxDamageForAddHeroList;
                break;
            case "DamageForCutHero":
                m_IdxDamageForCutHeroList = PlusOne(m_IdxDamageForCutHeroList, m_DamageForCutHeroList);
                ret = m_IdxDamageForCutHeroList;
                break;
            case "EnergyAdd":
                m_IdxEnergyAddList = PlusOne(m_IdxEnergyAddList, m_EnergyAddList);
                ret = m_IdxEnergyAddList;
                break;
            case "EnergyCut":
                m_IdxEnergyCutList = PlusOne(m_IdxEnergyCutList, m_EnergyCutList);
                ret = m_IdxEnergyCutList;
                break;
            case "ScreenTip":
                m_IdxScreenTipList = PlusOne(m_IdxScreenTipList, m_ScreenTipList);
                ret = m_IdxScreenTipList;
                break;
            case "GainMoney":
                m_IdxGainMoneyList = PlusOne(m_IdxGainMoneyList, m_GainMoneyList);
                ret = m_IdxGainMoneyList;
                break;
            default:
                break;
        }
        return ret;
    }

    public UnityEngine.GameObject NewObject(string objType, float timeRecycle = 0.0f)
    {
        UnityEngine.GameObject ret = null;
        List<UnityEngine.GameObject> tempList = null;
        switch (objType)
        {
            case "AttackEffect":
                tempList = m_AttackEffectList;
                break;
            case "CriticalDamage":
                tempList = m_CriticalDamageList;
                break;
            case "DamageForAddHero":
                tempList = m_DamageForAddHeroList;
                break;
            case "DamageForCutHero":
                tempList = m_DamageForCutHeroList;
                break;
            case "EnergyAdd":
                tempList = m_EnergyAddList;
                break;
            case "EnergyCut":
                tempList = m_EnergyCutList;
                break;
            case "ScreenTip":
                tempList = m_ScreenTipList;
                break;
            case "GainMoney":
                tempList = m_GainMoneyList;
                break;
            default:
                break;
        }
        if (tempList != null)
        {
            if (tempList.Count > 0)
            {
                int newidx = PlusIdx(objType, tempList);
                if (tempList[newidx] != null)
                {
                    ret = tempList[newidx];
                    BloodAnimationScript bas = ret.GetComponent<BloodAnimationScript>();
                    if (bas != null)
                    {
                        bas.SetTimeRecycle(timeRecycle);
                        if (bas.IsActive())
                        {
                            UnityEngine.Transform tf = ret.transform;
                            tf.localScale = UnityEngine.Vector3.one;
                            bas.StopAnimation();
                        }
                    }
                }
            }
        }

        return ret;
    }
}
