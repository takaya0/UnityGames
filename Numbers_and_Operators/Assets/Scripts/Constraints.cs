using System;
using System.Collections;
using System.Collections.Generic;

using System.Collections.ObjectModel;

namespace Constraints {
    public static class Const {
        // 初期演算カード枚数
        public const int INIT_OPERATER_CARD_NUM = 3;
        // 初期数字カード枚数
        public const int INIT_NUMBER_CARD_NUM = 3;
        // 目標値の最小値
        public const int MIN_TARGET_VALUE = 30;
        // 目標値の最大値
        public const int MAX_TARGET_VALUE = 80;
        // 2演算式が可能かどうかの閾値
        public const int DOUBLE_OPERATE_THRESHOLD = 6;
        

        // 手札の最大枚数
        public const int MAX_HAND_NUM = 5;
        // カードポイント(CP)の最大値
        public const int MAX_CARD_POINT = 16;
        // 演算カードのリスト
        public static readonly ReadOnlyCollection<string> OperatorsCardList =
                    Array.AsReadOnly(new string[] { "Plus", "Minus", "Product", "Quotient" });
        //　数字カードのリスト
        public static readonly ReadOnlyCollection<string> NumbersCardList =
                    Array.AsReadOnly(new string[] { "1", "2", "3", "4", "5", "6", "7", "8" });

       
    }

    public static class SkillCost {
        // 数の交換スキル
        public const int exchangeSkillCost = 12;
        // ドロースキル
        public const int drawSkillCost = 4;
        // 相手の数減少スキル
        public const int downSkillCost = 6;
        // 相手の数増加スキル
        public const int upSkillCost = 6;
    }
}