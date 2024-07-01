using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RankPanel : MonoBehaviour
{
    private static RankPanel instance = null;

    public Text NumberOfEarnedPoints = null;
    public Text CurrentRankText = null;
    public Text NextRankText = null;
    public Image ProgressBar = null;

    private enum RankPoints {RANK0 = 50, RANK1 = 100, RANK2 = 150, RANK3 = 1600, RANK4 = 2500, RANK5 = 4000};
    private int CurrentTotalNumberOfPoints = 0;
    private int AchievedTotalNumberOfPoints = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }    
        else
        {
            Destroy(gameObject);
        }
    }


    public static RankPanel Instance()
    {
        return instance;
    }

    private IEnumerator UpdateProgressBar()
    {
        int delta = AchievedTotalNumberOfPoints - CurrentTotalNumberOfPoints;
        if (delta >= 0) 
        {
            delta = 1;
        }
        else
        {
            delta = -1;
        }

        while (CurrentTotalNumberOfPoints != AchievedTotalNumberOfPoints)
        {
            if (CurrentTotalNumberOfPoints < (int)RankPoints.RANK0)
            {
                CurrentRankText.text = "0";
                NextRankText.text = "1";
                ProgressBar.gameObject.GetComponent<RectTransform>().localScale = new Vector3((float)CurrentTotalNumberOfPoints / (float)RankPoints.RANK0, 1, 1);
            }
            else if (CurrentTotalNumberOfPoints < (int)RankPoints.RANK1)
            {
                CurrentRankText.text = "1";
                NextRankText.text = "2";
                int ProgressNumberOfPoints = CurrentTotalNumberOfPoints - (int)RankPoints.RANK0;
                ProgressBar.gameObject.GetComponent<RectTransform>().localScale = new Vector3((float)ProgressNumberOfPoints / (float)(RankPoints.RANK1 - RankPoints.RANK0), 1, 1);
            }
            else if (CurrentTotalNumberOfPoints < (int)RankPoints.RANK2)
            {
                CurrentRankText.text = "2";
                NextRankText.text = "3";
                int ProgressNumberOfPoints = CurrentTotalNumberOfPoints - (int)RankPoints.RANK1;
                ProgressBar.gameObject.GetComponent<RectTransform>().localScale = new Vector3((float)ProgressNumberOfPoints / (float)(RankPoints.RANK2 - RankPoints.RANK1), 1, 1);
            }
            else
            {
                CurrentRankText.text = "3";
                NextRankText.text = "max";
                ProgressBar.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }

            CurrentTotalNumberOfPoints += delta;

            yield return new WaitForSeconds(0.01f);
        }

        if (CurrentTotalNumberOfPoints < (int)RankPoints.RANK0)
        {
            CurrentRankText.text = "0";
            NextRankText.text = "1";
            ProgressBar.gameObject.GetComponent<RectTransform>().localScale = new Vector3((float)CurrentTotalNumberOfPoints / (float)RankPoints.RANK0, 1, 1);
        }
        else if (CurrentTotalNumberOfPoints < (int)RankPoints.RANK1)
        {
            CurrentRankText.text = "1";
            NextRankText.text = "2";
            int ProgressNumberOfPoints = CurrentTotalNumberOfPoints - (int)RankPoints.RANK0;
            ProgressBar.gameObject.GetComponent<RectTransform>().localScale = new Vector3((float)ProgressNumberOfPoints / (float)(RankPoints.RANK1 - RankPoints.RANK0), 1, 1);
        }
        else if (CurrentTotalNumberOfPoints < (int)RankPoints.RANK2)
        {
            CurrentRankText.text = "2";
            NextRankText.text = "3";
            int ProgressNumberOfPoints = CurrentTotalNumberOfPoints - (int)RankPoints.RANK1;
            ProgressBar.gameObject.GetComponent<RectTransform>().localScale = new Vector3((float)ProgressNumberOfPoints / (float)(RankPoints.RANK2 - RankPoints.RANK1), 1, 1);
        }
        else 
        {
            CurrentRankText.text = "3";
            NextRankText.text = "max";
            ProgressBar.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }

        yield return null;
    }

    public int UpdateProgressData(int currentTotalNumberOfPoints, int achievedTotalNumberOfPoints)
    {
        CurrentTotalNumberOfPoints = currentTotalNumberOfPoints;
        AchievedTotalNumberOfPoints = achievedTotalNumberOfPoints;

        SetEarnedNumberOfPoints(achievedTotalNumberOfPoints - currentTotalNumberOfPoints);

        StartCoroutine(UpdateProgressBar());

        if (achievedTotalNumberOfPoints < (int)RankPoints.RANK0)
        {
            return 0;
        }
        else if (achievedTotalNumberOfPoints < (int)RankPoints.RANK1)
        {
            return 1;
        }
        else if (achievedTotalNumberOfPoints < (int)RankPoints.RANK2)
        {
            return 2;
        }
        else //if(achievedTotalNumberOfPoints < (int)RankPoints.RANK3)
        {
            return 3;
        }
        /*else if (achievedTotalNumberOfPoints < (int)RankPoints.RANK4)
        {
            return 4;
        }
        else
        {
            return 5;
        }*/
    }

    public void SetEarnedNumberOfPoints(int numberOfPoints)
    {
        NumberOfEarnedPoints.text = "" + numberOfPoints;
    }
}
