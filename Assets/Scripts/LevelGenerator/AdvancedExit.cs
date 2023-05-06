using UnityEngine;

namespace LevelGenerator.Scripts
{
    public class AdvancedExit : MonoBehaviour
    {
        public string[] CreatesTags;
        public ExitDirection ExitDirection;
        [SerializeField] private GameObject Exit, DeadEnd;
        public enum State {free, busy, close};
        public State CurrentState = State.free;
 
        public void SetExit(AdvancedExit entrance, RoomController roomController)
        {
            CurrentState = State.busy;
            var door = GetComponentInChildren<DoorController>();
            door.Entrance = entrance.GetComponentInChildren<DoorController>();
            roomController.RegisterExit(door);
        }

        public void SetDeadEnd()
        {
            CurrentState = State.close;
            Exit.SetActive(false);
            DeadEnd.SetActive(true);
        }
    }
}
