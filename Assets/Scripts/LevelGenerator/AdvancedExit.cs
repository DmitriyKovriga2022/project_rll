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
            // Exit.SetActive(true);
            // DeadEnd.SetActive(false);
            var exit = GetComponentInChildren<Exit>();
            exit.Entrance = entrance.GetComponentInChildren<Exit>();
            roomController.RegisterExit(exit);
        }

        public void SetDeadEnd()
        {
            CurrentState = State.close;
            Exit.SetActive(false);
            DeadEnd.SetActive(true);
        }
    }
}
