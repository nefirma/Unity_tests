using UnityEngine;
using System.Collections.Generic;

public class SubwayMap : MonoBehaviour
{
    public List<Line> lines;

    [System.Serializable]
    public class Line
    {
        public string lineName;
        public List<Station> stations;
    }
}

public class Line : MonoBehaviour
{
    [SerializeField] 
    private int lineId;
	
    [SerializeField]
    private List<Node> nodes = new List<Node>();
    public Line(int lineId, List<Node> nodes)
    {
        this.lineId = lineId;
        this.nodes = nodes;
    }
}