using UnityEngine;

[RequireComponent(typeof(UnityEngine.BoxCollider))]
public class MoodBox : UnityEngine.MonoBehaviour
{

    void OnDrawGizmos()
    {
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.color = new UnityEngine.Color(0.5f, 0.9f, 1.0f, 0.35f);
        Gizmos.DrawCube(UnityEngine.Vector3.zero, UnityEngine.Vector3.one);

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.color = new UnityEngine.Color(0.5f, 0.9f, 1.0f, 0.75f);
        Gizmos.DrawCube(GetComponent<Collider>().bounds.center, GetComponent<Collider>().bounds.size);
    }
}
