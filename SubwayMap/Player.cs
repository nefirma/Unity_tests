using System.Collections.Generic;
using System.Linq;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using TMPro;
using Unity.UI;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Station startStation;
    public Station finishStation;
    public SubwayMap subwayMap;
    public float speed;
    float totalPathDistance = 0;
    int totalTransfers = 0;

    private List<Station> path;
    private int currentStationIndex;
    public Text textMeshPro;

    private void Start()
    {
        transform.position = startStation.transform.position;
        FindPath();
    }

    private void Update()
    {
        if (path == null || currentStationIndex >= path.Count) return;
        transform.position = Vector3.MoveTowards(transform.position, path[currentStationIndex].transform.position, Time.deltaTime * speed);
        if (transform.position == path[currentStationIndex].transform.position)
        {
            currentStationIndex++;
        }
    }

    private void FindPath()
    {
        Dictionary<Station, float> distances = new Dictionary<Station, float>();
        Dictionary<Station, Station> previousStations = new Dictionary<Station, Station>();
        List<Station> unvisitedStations = new List<Station>();

        // Initialize distances and previous stations for all stations
        foreach (var line in subwayMap.lines)
        {
            foreach (var station in line.stations)
            {
                distances[station] = float.MaxValue;
                previousStations[station] = null;
                unvisitedStations.Add(station);
            }
        }

        // Set the distance for the start station to 0
        distances[startStation] = 0;

        while (unvisitedStations.Count > 0)
        {
            // Find the unvisited station with the shortest distance
            Station currentStation = null;
            float shortestDistance = float.MaxValue;
            foreach (var station in unvisitedStations)
            {
                if (!(distances[station] < shortestDistance)) continue;
                shortestDistance = distances[station];
                currentStation = station;
            }

            if (currentStation == finishStation)
            {
                break;
            }

            unvisitedStations.Remove(currentStation);

            // Calculate the distance to all neighboring stations
            if (currentStation == null) continue;
            foreach (var connection in currentStation.connections)
            {
                float distance = Vector3.Distance(currentStation.transform.position, connection.transform.position);
                SubwayMap.Line currentLine = FindLine(currentStation);
                SubwayMap.Line connectionLine = FindLine(connection);

                if (currentLine.lineName != connectionLine.lineName)// && distance > 0.1f)
                {
                    distance += currentStation.transferPenalty;
                }
                float newDistance = distances[currentStation] + distance;
                if (!(newDistance < distances[connection])) continue;
                distances[connection] = newDistance;
                previousStations[connection] = currentStation;
            }
        }

        // Construct the path
        path = new List<Station>();
        Station current = finishStation;
        while (current != startStation)
        {
            path.Add(current);
            current = previousStations[current];
        }
        path.Reverse();

        // Create a string to store the names of the stations in the path
        string pathString = "Path: " + startStation.name[0];
        SubwayMap.Line currentLine2 = FindLine(startStation);

        // Print out the entire route, including intermediate stations
        for (int i = 0; i < path.Count; i++)
        {
            Station currentStation = path[i];
            SubwayMap.Line nextLine = FindLine(path[i]);

            // Check if the player needs to transfer lines
            if (currentLine2.lineName != nextLine.lineName)
            {
                pathString += " -> Transfer to " + nextLine.lineName + " line @ " + currentStation.name[0];
                currentLine2 = nextLine;
            }
            else
            {
                pathString += " -> " + currentStation.name[0];
            }
        }
        
        for (int i = 0; i < path.Count - 1; i++)
        {
            Station currentStation = path[i];
            Station nextStation = path[i + 1];
            totalPathDistance += Vector3.Distance(currentStation.transform.position, nextStation.transform.position);

            SubwayMap.Line currentLine = FindLine(currentStation);
            SubwayMap.Line nextLine = FindLine(nextStation);
            if (currentLine.lineName != nextLine.lineName)
            {
                totalTransfers++;
            }
        }
        string totalTransferString = IntegerToString.ConvertIntToString(totalTransfers);
        Debug.Log("Total " + pathString + " with " + totalTransferString + " line changing and the total distance ~" + Mathf.RoundToInt(totalPathDistance));
        textMeshPro.text = "Total " + pathString + " with " + totalTransferString +
                           " line changing and the total distance ~" + Mathf.RoundToInt(totalPathDistance);
    }   

    private SubwayMap.Line FindLine(Station station)
    {
        foreach (var line in subwayMap.lines)
        {
            if (line.stations.Contains(station)) return line;
        }

        return null;
    }
}

