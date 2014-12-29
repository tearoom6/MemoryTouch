using UnityEngine;
using System.Collections;

/// <summary>
/// ランキングボードの表示を制御します。
/// </summary>
[System.Obsolete("Deprecated.", true)]
public class RecordBoard : MonoBehaviour
{

    public GUIText recordLabel;
    public GUIText recordName;
    public GUIText recordPointLabel;
    public GUIText recordPoint;
    public GUIText recordRankLabel;
    public GUIText recordRank;
    public GUIText rankingLabel;
    public GUIText[] rankingRanks;
    public GUIText[] rankingNames;
    public GUIText[] rankingPoints;

    public GUIText rankingText;

    private static int rankingNum = 10;

    void Awake()
    {
        float rankingTextPos_Left = .15f;
        float rankingTextPos_Left2 = .25f;
        float rankingTextPos_Right = .88f;
        float rankingTextPos_Top = .58f;
        // create RankingRanks
        rankingRanks = new GUIText[rankingNum];
        for (int i = 0; i < rankingNum; i++)
        {
            GUIText rank = Instantiate(rankingText, new Vector3(rankingTextPos_Left, rankingTextPos_Top - .04f * i, 3f), Quaternion.identity) as GUIText;
            rank.transform.parent = this.transform;
            rankingRanks[i] = rank;
        }
        // create RankingNames
        rankingNames = new GUIText[rankingNum];
        for (int i = 0; i < rankingNum; i++)
        {
            GUIText rankName = Instantiate(rankingText, new Vector3(rankingTextPos_Left2, rankingTextPos_Top - .04f * i, 3f), Quaternion.identity) as GUIText;
            rankName.transform.parent = this.transform;
            rankingNames[i] = rankName;
        }
        // create RankingPoints
        rankingPoints = new GUIText[rankingNum];
        for (int i = 0; i < rankingNum; i++)
        {
            GUIText rankPoint = Instantiate(rankingText, new Vector3(rankingTextPos_Right, rankingTextPos_Top - .04f * i, 3f), Quaternion.identity) as GUIText;
            rankPoint.anchor = TextAnchor.UpperRight;
            rankPoint.transform.parent = this.transform;
            rankingPoints[i] = rankPoint;
        }
    }

    public void SetLabels(string record, string point, string rank, string ranking)
    {
        recordLabel.text = record;
        recordPointLabel.text = point;
        recordRankLabel.text = rank;
        rankingLabel.text = ranking;
    }
	
    public void SetMyRecord(string name, int point, int rank)
    {
        recordName.text = name;
        recordPoint.text = point.ToString();
        recordRank.text = rank.ToString();
    }

    public void SetRankingRecords(RankingRecord[] records)
    {
        for (int i = 0; i < rankingNum; i++)
        {
            if (records.Length <= i)
            {
                break;
            }
            this.rankingRanks[i].text = records[i].rank.ToString();
            this.rankingNames[i].text = records[i].name;
            this.rankingPoints[i].text = records[i].point.ToString();
        }
    }

}

/// <summary>
/// ランキング表示における個々のレコードのエンティティクラスです。
/// </summary>
[System.Serializable]
public class RankingRecord
{
    public string category;
    public string name;
    public int rank;
    public int point;

    public RankingRecord()
    {
        // デフォルトコンストラクタはJSONのデシリアライズに必要
    }

    public RankingRecord(string category, string name, int rank, int point)
    {
        this.category = category;
        this.name = name;
        this.rank = rank;
        this.point = point;
    }
}
