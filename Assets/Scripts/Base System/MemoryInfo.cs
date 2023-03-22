using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Memory
{
    None, BishopMemory, Mansion, StreetCorner, LawyerOffice, NightClub, VoidBase, SecretLaboratory
}
public class MemoryInfo : MonoBehaviour
{
    [field: SerializeField]
    public Memory memory 
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
                foreach(Transform point in child.Find("InterestPoints"))
                {
                    totalInterestPoints++;
                    
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
