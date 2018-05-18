using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class PlayerStatistics
{
    public int PlayerLife;
    public int PlayerHealth;
    public int PlayerBlackPoint;
    public int PlayerWhitePoint;
    public int PlayerMaxPoint;
    public int PlayerAddPointWhenHit;
    public float PlayerEliminateRadius;
    public float PlayerBlackVSpeed;
    public float PlayerBlackHSpeed;
    public float PlayerWhiteVSpeed;
    public float PlayerWhiteHSpeed;

    public PlayerStatistics ()
    {

    }

    public PlayerStatistics (PlayerProperty player)
    {
        PlayerLife = player.m_playerLife;
        PlayerHealth = player.m_playerHealth;
        PlayerBlackPoint = player.m_playerBlackPoint;
        PlayerWhitePoint = player.m_playerWhitePoint;
        PlayerMaxPoint = player.m_maxPlayerPoint;
        PlayerAddPointWhenHit = player.m_addValue;
        PlayerBlackHSpeed = player.m_blackHSpeed;
        PlayerBlackVSpeed = player.m_blackVSpeed;
        PlayerWhiteHSpeed = player.m_whiteHSpeed;
        PlayerWhiteVSpeed = player.m_whiteVSpeed;
    }

    public void Load (PlayerProperty player)
    {
        player.m_playerLife = PlayerLife;
        player.m_playerHealth = PlayerHealth;
        player.m_playerBlackPoint = PlayerBlackPoint;
        player.m_playerWhitePoint = PlayerWhitePoint;
        player.m_maxPlayerPoint = PlayerMaxPoint;
        player.m_addValue = PlayerAddPointWhenHit;
        player.m_blackHSpeed = PlayerBlackHSpeed;
        player.m_blackVSpeed = PlayerBlackVSpeed;
        player.m_whiteHSpeed = PlayerWhiteHSpeed;
        player.m_whiteVSpeed = PlayerWhiteVSpeed;
    }

    private void SavePlayerStatistics ()
    {
        if (Directory.Exists ("Saves"))
        {
            Directory.CreateDirectory ("Saves");
        }

        BinaryFormatter formatter = new BinaryFormatter ();
        FileStream saveFile = File.Create ("Saves/player.binary");
        formatter.Serialize (saveFile, this);
        saveFile.Close ();
    }

    private void LoadPlayerStatistics ()
    {
        BinaryFormatter formatter = new BinaryFormatter ();
        FileStream saveFile = File.Open ("Saves/player.binary", FileMode.Open);
        var loadData = (PlayerStatistics) formatter.Deserialize (saveFile);
        saveFile.Close ();
    }
}