using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Constraints;

public class UIManeger : MonoBehaviour{


    // ���݂��̃J�[�h�|�C���g�e�L�X�g
    [SerializeField] TextMeshProUGUI EnemyCardPointText, PlayerCardPointText;

    // ���ꂼ��̐���
    [SerializeField] TextMeshProUGUI EnemyScoreText, PlayerScoreText;

    // �ڕW�̐���
    [SerializeField] TextMeshProUGUI TargetScoreText;

    public void SetTargetScoreText(int targetValue) {
        TargetScoreText.text = targetValue.ToString();
    }

    public void SetPlayerScoreText(int playerScore) {
        PlayerScoreText.text = playerScore.ToString();
    }

    public void SetEnemyScoreText(int enemyScore) {
        EnemyScoreText.text = enemyScore.ToString();
    }

    public void SetPlayerCardPointText(int playerCardPoint) {
        PlayerCardPointText.text = "CP : " + playerCardPoint.ToString() + "/" + Const.MAX_CARD_POINT.ToString();
    }

    public void SetEnemyCardPointText(int enemyCardPoint) {
        EnemyCardPointText.text = "CP : " + enemyCardPoint.ToString() + "/" + Const.MAX_CARD_POINT.ToString();
    }
}
