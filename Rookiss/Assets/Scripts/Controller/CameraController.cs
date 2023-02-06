using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuaterView;

    [SerializeField]
    public Vector3 delta = new Vector3(0, 7, -5);

    [SerializeField]
    public GameObject player;

    void LateUpdate()
    {
        if(_mode == Define.CameraMode.QuaterView)
        {
            if (!player.IsValid()) return;

            RaycastHit hit;
            if(Physics.Raycast(player.transform.position, delta, out hit, delta.magnitude, 1 << (int)Define.Layer.Block))
            {
                float dist = (hit.point - player.transform.position).magnitude * 0.8f;
                transform.position = player.transform.position + delta.normalized * dist;
            }
            else
            {
                transform.position = player.transform.position + delta;
                transform.LookAt(player.transform);
            }
        }
    }

    public void SetQuaterView(Vector3 _delta)
    {
        _mode = Define.CameraMode.QuaterView;
        this.delta = _delta;
    }

    public void SetPlayer(GameObject go) { this.player = go; }
}
