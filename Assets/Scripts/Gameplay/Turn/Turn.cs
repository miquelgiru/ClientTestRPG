using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Turn", menuName = "Gameplay/Turn", order = 1)]
public class Turn : ScriptableObject
{
    public enum PlayerType { STANDART_PLAYER, AI_PLAYER}
    public PlayerType Type;
    private int turnCount = 0;

    public PlayerHolder Player;
   

    public void StartTurn()
    {
        switch (Type)
        {
            case PlayerType.STANDART_PLAYER:
                ((PlayerFSM)Player.fSM).ChangeState(PlayerFSM.PlayerStates.IDLE);
                break;

            case PlayerType.AI_PLAYER:
                ((AiFSM)Player.fSM).ChangeState(AiFSM.AiStates.MANAGE_TURN);
                break;
        }

        ++turnCount;
    }

    public void EndTurn()
    {
        switch (Type)
        {
            case PlayerType.STANDART_PLAYER:
                ((PlayerFSM)Player.fSM).ChangeState(PlayerFSM.PlayerStates.WAIT_FOR_TURN);
                break;

            case PlayerType.AI_PLAYER:
                ((AiFSM)Player.fSM).ChangeState(AiFSM.AiStates.WAIT_FOR_TURN);
                break;
        }
    }

   
}