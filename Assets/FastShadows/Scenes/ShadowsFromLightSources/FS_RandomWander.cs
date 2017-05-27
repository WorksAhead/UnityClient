using UnityEngine;
using System.Collections;

public class FS_RandomWander : UnityEngine.MonoBehaviour
{
  public float speed = .1f;
  public float directionChangeInterval = 1;
  public float maxHeadingChange = 30;
  public float dist = 0f;

  float heading;
  UnityEngine.Vector3 targetRotation;

  void Awake()
  {

    // Set random initial rotation
    heading = Random.Range(0, 360);
    transform.eulerAngles = new UnityEngine.Vector3(0, heading, 0);

    StartCoroutine(NewHeading());
  }

  void FixedUpdate()
  {
    transform.eulerAngles = UnityEngine.Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
    var forward = transform.TransformDirection(UnityEngine.Vector3.forward);
    transform.Translate(forward * speed);
    if (transform.position.y < 1f)
      NewHeadingRoutine();
    dist = transform.position.magnitude;
    if (transform.position.magnitude > 70f) {
      //transform.position = new UnityEngine.Vector3(0f,10f,0f);
      Destroy(gameObject);
    }
    UnityEngine.Vector3 v = transform.position;
    v.y = 10f;
    transform.position = v;
  }


  IEnumerator NewHeading()
  {
    while (true) {
      try {
        NewHeadingRoutine();
      } catch (System.Exception ex) {
        ArkCrossEngine.LogicSystem.LogicErrorLog("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
      }
      yield return new WaitForSeconds(directionChangeInterval);
    }
  }

  void NewHeadingRoutine()
  {
    var floor = UnityEngine.Mathf.Clamp(heading - maxHeadingChange, 0, 360);
    var ceil = UnityEngine.Mathf.Clamp(heading + maxHeadingChange, 0, 360);
    heading = Random.Range(floor, ceil);
    targetRotation = new UnityEngine.Vector3(0, heading, 0);
  }
}
