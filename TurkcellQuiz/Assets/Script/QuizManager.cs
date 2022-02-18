using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public TextAsset textAssetData;

    public List<QuestionsAndAnswers> QnA;
    public List<QuestionsAndAnswers> QnAExcel;
    public GameObject[] options;
    public int currentQuestion;
    public int tableSize;

    public GameObject QuizPanel;
    public GameObject GOPanel;

    public TextMeshProUGUI QuestionText;
    public TextMeshProUGUI ScoreText;

    [SerializeField]
    private float timeBetweenQuestions = 1f;

    int totalQuestion = 0;
    public int score = 0;

    private void Start()
    {
        totalQuestion = QnA.Count;
        GOPanel.SetActive(false);
        //ReadQnAExcel();
        generateQuestion();
        
    }

    public void ReadQnAExcel()
    {
        string[] data = textAssetData.text.Split(new string[] { ",", "/n" }, System.StringSplitOptions.None);

        tableSize = ((data.Length - 1) / 7) - 1;

        /*Debug.Log(tableSize);
        Debug.Log(data.Length);

        Debug.Log(data[7 * (0 + 1) + 5]);
        Debug.Log(data[7 * (1 + 1) + 5]);
        Debug.Log(data[7 * (2 + 1) + 5]);*/

        //QnAExcel = new List<QuestionsAndAnswers>[tableSize];
        //QnAExcel.Capacity = tableSize;

        for (int i = 0; i < tableSize; i++)
        {
            QnA[i].Question = data[7* (i+1)];
            QnA[i].Answers[0] = data[7 * (i + 1) + 1];
            QnA[i].Answers[1] = data[7 * (i + 1) + 2];
            QnA[i].Answers[2] = data[7 * (i + 1) + 3];
            QnA[i].Answers[3] = data[7 * (i + 1) + 4];
            QnA[i].CorrectAnswer = int.Parse(data[7 * (i + 1) + 5]);
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        QuizPanel.SetActive(false);
        GOPanel.SetActive(true);
        ScoreText.text = score + "/" + totalQuestion;
    }

    public void correct()
    {
        score += 1;
        //QnA.RemoveAt(currentQuestion);
        QnA.RemoveAt(currentQuestion);
        StartCoroutine(TransitionToNextQuestion());
    }

    public void wrong()
    {
        //when you answer wrong
        //QnA.RemoveAt(currentQuestion);
        QnA.RemoveAt(currentQuestion);
        StartCoroutine(TransitionToNextQuestion());
    }

    void SetAnswers()
    {
        for (int i = 0; i<options.Length;i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            //options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = QnA[currentQuestion].Answers[i];
            options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = QnA[currentQuestion].Answers[i];

            options[i].GetComponent<AnswerScript>().GetComponent<Image>().color = options[i].GetComponent<AnswerScript>().startColor;

            /*if (QnA[currentQuestion].CorrectAnswer == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }*/

            if (QnA[currentQuestion].CorrectAnswer == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    IEnumerator TransitionToNextQuestion()
    {
        yield return new WaitForSeconds(timeBetweenQuestions);
        generateQuestion();
    }

    void generateQuestion()
    {
        if(QnA.Count>0)
        {
            currentQuestion = Random.Range(0, QnA.Count);
            

            QuestionText.text = QnA[currentQuestion].Question;
            
            SetAnswers();

        }
        else
        {
            Debug.Log("Out of Question");
            GameOver();
        }

        /*if (tableSize > 0)
        {
            currentQuestion = Random.Range(0, tableSize);

            QuestionText.text = QnA[currentQuestion].Question;

            SetAnswers();

        }
        else
        {
            Debug.Log("Out of Question");
            GameOver();
        }*/


    }
}
