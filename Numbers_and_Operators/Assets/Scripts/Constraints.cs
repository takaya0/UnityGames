using System;
using System.Collections;
using System.Collections.Generic;

using System.Collections.ObjectModel;

namespace Constraints {
    public static class Const {
        // 手札の最大値
        public const int MAX_HAND_NUM = 5;
        // カードポイント(CP) の最大値
        public const int MAX_CARD_POINT = 16;
        //　演算カードのリスト
        public static readonly ReadOnlyCollection<string> OperatorsCardList =
                    Array.AsReadOnly(new string[] { "Plus", "Minus", "Product", "Quotient" });
        //　演算カードのリスト
        public static readonly ReadOnlyCollection<string> NumbersCardList =
                    Array.AsReadOnly(new string[] { "1", "2", "3", "4", "5", "6", "7", "8" });
    }
}