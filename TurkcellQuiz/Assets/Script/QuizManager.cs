using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public List<QuestionsAndAnswers> QnA;
    public GameObject[] options;
    public int currentQuestion;

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
        generateQuestion();
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
        QnA.RemoveAt(currentQuestion);
        StartCoroutine(TransitionToNextQuestion());
    }

    public void wrong()
    {
        //when you answer wrong
        QnA.RemoveAt(currentQuestion);
        StartCoroutine(TransitionToNextQuestion());
    }

    void SetAnswers()
    {
        for (int i = 0; i<options.Length;i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = QnA[currentQuestion].Answers[i];
            options[i].GetComponent<AnswerScript>().GetComponent<Image>().color = options[i].GetComponent<AnswerScript>().startColor;

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
       

    }
}
