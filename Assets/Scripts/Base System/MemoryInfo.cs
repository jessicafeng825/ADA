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
    public List<DoorInfo> Doors
    { get; private set; }

    public int totalInterestPoints 
    { get; private set; }
    
    [field: SerializeField]
    public int interestPointCount
    { get; set; }

    private PhotonView pv;
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
        if(memory != InvestigationManager.Instance.startMemory.GetComponent<MemoryInfo>().memory)
            this.gameObject.SetActive(false);
    }

    private void Start() 
    {
        pv = GetComponent<PhotonView>();
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
