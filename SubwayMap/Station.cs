using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    public int stationId;
    public bool IsTransferStation;
    public List<Station> connections;
    public int transferPenalty;

    public void Connect(Station other)
    {
        connections.Add(other);
        other.connections.Add(this);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.0f);
    }
}

