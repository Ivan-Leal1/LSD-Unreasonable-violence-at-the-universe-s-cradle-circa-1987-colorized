using UnityEngine;
using System.Collections.Generic;

public class OverworldManager : MonoBehaviour
{
    [SerializeField] List<GameObject> _roomList;
    public List<RoomBase> _currentRoomPool;

    private void Awake() {
        _currentRoomPool.Add(_roomList[/*Random.Range(0, _roomList.Count -1)*/0].GetComponent<RoomBase>());
        _currentRoomPool.Add(_roomList[/*Random.Range(0, _roomList.Count -1)*/1].GetComponent<RoomBase>());
        _currentRoomPool.Add(_roomList[/*Random.Range(0, _roomList.Count -1)*/2].GetComponent<RoomBase>());
        GameManager.Singleton.ReceiveRooms(_currentRoomPool);
    }
}