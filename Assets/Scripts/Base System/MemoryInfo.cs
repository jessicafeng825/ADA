using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum Memory
{
    None, BishopMemory, Mansion, StreetCorner, LawyerOffice, Nightclub, VoidBase, SecretLaboratory
}
public class MemoryInfo : MonoBehaviour
{
    [field: SerializeField]
    public Memory memory 
    { get; private set; }

    [field: SerializeField]
    public List<Rooms> Rooms
    { get; private set; }

    [field: SerializeField]
    public List<DoorInfo> Doors
    { get; private set; }

    public int totalInterestPoints 
    { get; private set; }
    
    [field: SerializeField]
    public int interestPointCount
    { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        foreach(Transform child in this.transform)
        {
            if(child.tag == "Room")
            {
                Rooms.Add(child.GetComponent<Rooms>());
                foreach(Transform point in child.Find("InterestPoints"))
                {
                    totalInterestPoints++;
                    
                    Debug.Log("Interest Point: " + point.name + " added");
                    InvestigationManager.Instance.AddInterestPoint(point.name, point.gameObject);
                }
                foreach(Transform door in child.Find("Doors"))
                {
                    Doors.Add(door.GetComponent<DoorInfo>());
                }
            }
        }
        interestPointCount = totalInterestPoints;
    }

    private void Start() 
    {
        if(memory != InvestigationManager.Instance.startMemory.GetComponent<MemoryInfo>().memory)
            this.gameObject.SetActive(false);
    }

    public bool UpdateInterestPointCount(int n)
    {
        interestPointCount += n;
        if(memory == Memory.BishopMemory && interestPointCount == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
