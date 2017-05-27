using UnityEngine;
using System.Collections;

public class FS_AddSome : UnityEngine.MonoBehaviour
{

  // Use this for initialization
  void Start()
  {
    StartCoroutine(MakeSomeNewOnes());
  }

  IEnumerator MakeSomeNewOnes()
  {
    while (true) {
      try {
        UnityEngine.GameObject go = (UnityEngine.GameObject)Instantiate(gameObject);
        Destroy(go.GetComponent<FS_AddSome>());
      } catch (System.Exception ex) {
        ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
      }
      yield return new WaitForSeconds(1f);
    }
  }
}
