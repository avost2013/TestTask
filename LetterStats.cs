using System;

namespace TestTask
{
    /// <summary>
    /// Статистика вхождения буквы/пары букв
    /// </summary>
    public struct LetterStats : IComparable<LetterStats>
    {
        /// <summary>
        /// Буква/Пара букв для учёта статистики.
        /// </summary>
        public string Letter;

        /// <summary>
        /// Кол-во вхождений буквы/пары.
        /// </summary>
        public int Count;

        public int CompareTo(LetterStats letterStats)
        {
            return Letter.CompareTo(letterStats.Letter);
        }
    }

}
