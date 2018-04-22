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
    public float PlayerVerticalSpeed;
    public float PlayerHorizontalSpeed;
    public float PlayerSlowVerticalSpeed;
    public float PlayerSlowHorizontalSpeed;
    public float PlayerShotInterval;
    public int PlayerBulletDamage;
    public float PlayerBulletSpeed;

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
        PlayerEliminateRadius = player.m_checkBound;
        PlayerVerticalSpeed = player.m_verticalSpeed;
        PlayerHorizontalSpeed = player.m_horizontalSpeed;
        PlayerSlowVerticalSpeed = player.m_slowVerticalSpeed;
        PlayerSlowHorizontalSpeed = player.m_slowHorizontalSpeed;
        PlayerShotInterval = player.m_shootInterval;
        PlayerBulletDamage = player.m_bulletDamage;
        PlayerBulletSpeed = player.m_bulletSpeed;
    } 

    public void Load (PlayerProperty player)
    {
        player.m_playerLife = PlayerLife;
        player.m_playerHealth = PlayerHealth;
        player.m_playerBlackPoint = PlayerBlackPoint;
        player.m_playerWhitePoint = PlayerWhitePoint;
        player.m_maxPlayerPoint = PlayerMaxPoint;
        player.m_addValue = PlayerAddPointWhenHit;
        player.m_checkBound = PlayerEliminateRadius;
        player.m_verticalSpeed = PlayerVerticalSpeed;
        player.m_horizontalSpeed = PlayerHorizontalSpeed;
        player.m_slowVerticalSpeed = PlayerSlowVerticalSpeed;
        player.m_slowHorizontalSpeed = PlayerSlowHorizontalSpeed;
        player.m_shootInterval = PlayerShotInterval;
        player.m_bulletDamage = PlayerBulletDamage;
        player.m_bulletSpeed = PlayerBulletSpeed;
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