using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ���˹���
/// </summary>
public class EnemyManager
{
    public static EnemyManager Instacne = new EnemyManager();



    public List<Enemy> enemyList;

    public void Init()
    {
        for (int i = 0; i < enemyList.Count; ++i)
        {
            enemyList[i].Hit(99999999);
        }
        enemyList.Clear();
    }

    /// <summary>
    /// ���ص�����Դ
    /// </summary>
    /// <param name="id">�ؿ�ID</param>
    public void LoadRes(string id)
    {
        enemyList = new List<Enemy>();

        //�ؿ���
        Dictionary<string, string> levelData = GameConfigManager.Instance.GetById(ConfigType.Level, id);

        //����id
        string[] enemyIds = levelData["EnemyIds"].Split('=');

        //����λ��
        string[] enemyPos = levelData["Pos"].Split('=');

        for (int i = 0; i < enemyIds.Length; ++i)
        {
            string enemyId = enemyIds[i];
            string[] posArr = enemyPos[i].Split(',');

            Vector3 pos = new Vector3(float.Parse(posArr[0]), float.Parse(posArr[1]), float.Parse(posArr[2]));

            Dictionary<string, string> enemyData = GameConfigManager.Instance.GetById(ConfigType.Enemy, enemyId);

            GameObject obj = Object.Instantiate(Resources.Load(enemyData["Model"])) as GameObject;
            obj.GetComponent<SpriteRenderer>().sortingOrder = 5;

            Enemy enemy = obj.AddComponent<Enemy>();
            enemy.Init(enemyData);
            enemyList.Add(enemy);

            obj.transform.position = pos;
        }

    }

    public void DeleteEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);
        if (enemyList.Count <= 0)
        {
            FightManager.Instance.ChangeType(FightType.Win);

        }
    }

    public IEnumerator DoAllEnemyAction()
    {
        for (int i = 0; i < enemyList.Count; ++i)
        {
            yield return FightManager.Instance.StartCoroutine(enemyList[i].DoAction());
        }
        for (int i = 0; i < enemyList.Count; ++i)
        {
            enemyList[i].SetRandomAction();
        }
        FightManager.Instance.ChangeType(FightType.Player);
    }

}
