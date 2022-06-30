using System.Collections;
    // 敵側の手札
    [SerializeField] Transform PlayerNumbersHandTransform, PlayerOperatorsHandTransform;
    // プレイヤーの手札
    [SerializeField] Transform EnemySelectedCardsTransform, PlayerSelectedCardsTransform;
        

        CardController [] operatorsCardList = EnemyOperatorsHandTransform.GetComponentsInChildren<CardController>();
        CardController [] numbersCardList = EnemyNumbersHandTransform.GetComponentsInChildren<CardController>();

        // カードを選ぶ
        CardController operatorCard = operatorsCardList[0];
        CardController numberCard = numbersCardList[0];
       

        // カードを配置する
        operatorCard.movement.SetCardTransform(EnemySelectedCardsTransform);
        numberCard.movement.SetCardTransform(EnemySelectedCardsTransform);


        CaluculateScore(EnemySelectedCardsTransform);
       
    }
        if (IsPlayerTurn) {
            // プレイヤーの行動処理
           
        }
        else {
            Debug.Log(IsPlayerTurn);
            EnemyTurn();
            if (IsGameFinished()) {
                Debug.Log("enemyの勝利");
            }
    }
        if (IsPlayerTurn) {
            CaluculateScore(PlayerSelectedCardsTransform);
            if (IsGameFinished()) {
                Debug.Log("playerの勝利");

            }
        }
    }

        List<CardController> selectedCards = GetSelectedCards(SelectedCardsTransform);
        if(selectedCards.Count != 2) {
            Debug.LogWarning("2まいのカードを選択してください");
        }

        CardController operatorCard = selectedCards[0];
        CardController numberCard = selectedCards[1];
        if (IsPlayerTurn) PlayerNumber.text = EvaluateFormula(PlayerNumber.text, operatorCard.card.value, numberCard.card.value).ToString();
        else EnemyNumber.text = EvaluateFormula(EnemyNumber.text, operatorCard.card.value, numberCard.card.value).ToString();

        operatorCard.Vanish();
        numberCard.Vanish();
        // カードを引く
        string OpetatorCardName = DrawCard(allOperatorsCardList);
        string NumberCardName = DrawCard(allNumbersCardList);
        if (IsPlayerTurn) {
            AddCardToHand(PlayerOperatorsHandTransform, OpetatorCardName);
            AddCardToHand(PlayerNumbersHandTransform, NumberCardName);
        }
        else {
            AddCardToHand(EnemyOperatorsHandTransform, OpetatorCardName);
            AddCardToHand(EnemyNumbersHandTransform, NumberCardName);
        }

        IsPlayerTurn = !IsPlayerTurn;
        GameTurnFlow();
    }
        List<CardController> selectedCards = new List<CardController>(SelectedCardTransform.GetComponentsInChildren<CardController>());
        return selectedCards;
    }
        int result = 0;
        int selectedNum = 0;
        try {
            result = int.Parse(correntScore);
            selectedNum = int.Parse(selectedNumber);
        }
        catch (System.FormatException) {
            Debug.Log("Invalid Value");
        }

        if (selectedOperator == "+") result = result + selectedNum;
        else if (selectedOperator == "-") result = Mathf.Max(result - selectedNum, 1);
        else if (selectedOperator == "*") result = result * selectedNum;
        else Debug.LogWarning("Unsupported Operator");

        return result;
    }