using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LeaderboardDatabaseSO", menuName = "ScriptableObjects/LeaderboardDatabaseSO", order = 1)]
public class LeaderboardDatabaseSO : ScriptableObject
{
    public LeaderboardState leaderboardState; //Mock data currently Player
    public List<LeaderboardState> leaderboardStatesList = new List<LeaderboardState>(); //Mock data
    
}