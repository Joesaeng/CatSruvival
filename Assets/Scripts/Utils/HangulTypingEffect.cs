using System;

public static class HangulTypingEffect
{
    // 한글 초성, 중성, 종성 분리
    private static readonly char[] ChoSung = { 'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };
    private static readonly char[] JungSung = { 'ㅏ', 'ㅐ', 'ㅑ', 'ㅒ', 'ㅓ', 'ㅔ', 'ㅕ', 'ㅖ', 'ㅗ', 'ㅘ', 'ㅙ', 'ㅚ', 'ㅛ', 'ㅜ', 'ㅝ', 'ㅞ', 'ㅟ', 'ㅠ', 'ㅡ', 'ㅢ', 'ㅣ' };
    private static readonly char[] JongSung = { ' ', 'ㄱ', 'ㄲ', 'ㄳ', 'ㄴ', 'ㄵ', 'ㄶ', 'ㄷ', 'ㄹ', 'ㄺ', 'ㄻ', 'ㄼ', 'ㄽ', 'ㄾ', 'ㄿ', 'ㅀ', 'ㅁ', 'ㅂ', 'ㅄ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ' };

    // 한글을 자모로 분리하는 함수
    public static string SplitHangul(char hangul)
    {
        if (hangul < 0xAC00 || hangul > 0xD7A3)
            return hangul.ToString(); // 한글이 아니면 그대로 리턴

        int unicode = hangul - 0xAC00;
        int cho = unicode / (21 * 28);
        int jung = (unicode % (21 * 28)) / 28;
        int jong = (unicode % 28);

        return $"{ChoSung[cho]}{JungSung[jung]}{(jong != 0 ? JongSung[jong].ToString() : "")}";
    }

    // 자모를 다시 한글로 조합하는 함수
    public static char CombineHangul(char cho, char jung, char jong = ' ')
    {
        int choIndex = Array.IndexOf(ChoSung, cho);
        int jungIndex = Array.IndexOf(JungSung, jung);
        int jongIndex = Array.IndexOf(JongSung, jong);

        if (choIndex < 0 || jungIndex < 0)
            return ' ';
        return (char)(0xAC00 + (choIndex * 21 * 28) + (jungIndex * 28) + jongIndex);
    }

}

