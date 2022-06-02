using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TestTask
{
    public class Program
    {

        /// <summary>
        /// Программа принимает на входе 2 пути до файлов.
        /// Анализирует в первом файле кол-во вхождений каждой буквы (регистрозависимо). Например А, б, Б, Г и т.д.
        /// Анализирует во втором файле кол-во вхождений парных букв (не регистрозависимо). Например АА, Оо, еЕ, тт и т.д.
        /// По окончанию работы - выводит данную статистику на экран.
        /// </summary>
        /// <param name="args">Первый параметр - путь до первого файла.
        /// Второй параметр - путь до второго файла.</param>
        static void Main(string[] args)
        {
            IReadOnlyStream inputStream1 = GetInputStream(args[0]);
            IReadOnlyStream inputStream2 = GetInputStream(args[1]);

            IList<LetterStats> singleLetterStats = FillSingleLetterStats(inputStream1);
            IList<LetterStats> doubleLetterStats = FillDoubleLetterStats(inputStream2);

            RemoveCharStatsByType(singleLetterStats, CharType.Vowel);
            RemoveCharStatsByType(doubleLetterStats, CharType.Consonants);

            PrintStatistic(singleLetterStats);
            PrintStatistic(doubleLetterStats);

            // TODO : Необжодимо дождаться нажатия клавиши, прежде чем завершать выполнение программы.
            _ =Console.ReadKey();
        }

        /// <summary>
        /// Ф-ция возвращает экземпляр потока с уже загруженным файлом для последующего посимвольного чтения.
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        /// <returns>Поток для последующего чтения.</returns>
        private static IReadOnlyStream GetInputStream(string fileFullPath)
        {
            IReadOnlyStream stream = new ReadOnlyStream(fileFullPath);

            return stream;
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения каждой буквы.
        /// Статистика РЕГИСТРОЗАВИСИМАЯ!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillSingleLetterStats(IReadOnlyStream stream)
        {
            List<LetterStats> list = new List<LetterStats>();
            stream.ResetPositionToStart();
            while (!stream.IsEof)
            {
                char c;
                try
                {
                    c = stream.ReadNextChar();
                }
                catch (EndOfStreamException)
                {
                    return list;
                }

                // TODO : заполнять статистику с использованием метода IncStatistic. Учёт букв - регистрозависимый.
                if (char.IsLetter(c))
                {
                    int ind = list.FindIndex((si) => si.Letter[0] == c);
                    if (ind > -1)
                    {
                        var e = list[ind];
                        IncStatistic(ref e);
                        list[ind] = e;
                    }
                    else
                    {
                        var elem = new LetterStats();
                        elem.Letter = c.ToString();
                        elem.Count = 1;
                        list.Add(elem);
                    }
                }
            }

            return list;

            //throw new NotImplementedException();
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения парных букв.
        /// В статистику должны попадать только пары из одинаковых букв, например АА, СС, УУ, ЕЕ и т.д.
        /// Статистика - НЕ регистрозависимая!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillDoubleLetterStats(IReadOnlyStream stream)
        {
            List<LetterStats> list = new List<LetterStats>();
            char c0, c;
            stream.ResetPositionToStart();

            try
            {
                c0 = stream.ReadNextChar();
                while (!stream.IsEof)
                {
                    c = stream.ReadNextChar();

                    // TODO : заполнять статистику с использованием метода IncStatistic. Учёт букв - НЕ регистрозависимый.
                    if (char.IsLetter(c0) && char.IsLetter(c) && c0.ToString().ToUpper() == c.ToString().ToUpper())
                    {
                        int index = list.FindIndex((l) => l.Letter == c0.ToString() + c.ToString());
                        if (index>-1)
                        {
                            var elem = list[index];
                            IncStatistic(ref elem);
                            list[index] = elem;
                        }
                        else
                        {
                            LetterStats ls = new LetterStats
                            {
                                Letter = c0.ToString() + c.ToString(),
                                Count = 1
                            };
                            list.Add(ls);
                        }
                    }
                    c0 = c;
                }
            }
            catch(EndOfStreamException)
            {
                return list;
            }

            return list;

            throw new NotImplementedException();
        }

        /// <summary>
        /// Ф-ция перебирает все найденные буквы/парные буквы, содержащие в себе только гласные или согласные буквы.
        /// (Тип букв для перебора определяется параметром charType)
        /// Все найденные буквы/пары соответствующие параметру поиска - удаляются из переданной коллекции статистик.
        /// </summary>
        /// <param name="letters">Коллекция со статистиками вхождения букв/пар</param>
        /// <param name="charType">Тип букв для анализа</param>
        private static void RemoveCharStatsByType(IList<LetterStats> letters, CharType charType)
        {
            // TODO : Удалить статистику по запрошенному типу букв.
            switch (charType)
            {
                case CharType.Consonants:
                    (letters as List<LetterStats>).RemoveAll(l => !Regex.IsMatch(l.Letter, "([eEyYuUiIoOaAёЁуУЕЫАОЭЯИЮеыаоэяию])"));
                    break;
                case CharType.Vowel:
                        (letters as List<LetterStats>).RemoveAll(l => Regex.IsMatch(l.Letter, "([eEyYuUiIoOaAёЁуУЕЫАОЭЯИЮеыаоэяию])"));
                        break;
            }            
        }

        /// <summary>
        /// Ф-ция выводит на экран полученную статистику в формате "{Буква} : {Кол-во}"
        /// Каждая буква - с новой строки.
        /// Выводить на экран необходимо предварительно отсортировав набор по алфавиту.
        /// В конце отдельная строчка с ИТОГО, содержащая в себе общее кол-во найденных букв/пар
        /// </summary>
        /// <param name="letters">Коллекция со статистикой</param>
        private static void PrintStatistic(IEnumerable<LetterStats> letters)
        {
            // TODO : Выводить на экран статистику. Выводить предварительно отсортировав по алфавиту!
            (letters as List<LetterStats>).Sort(); //Для сортировки исп. внутренний метод LetterStats
            int i = 0;
            foreach(var letter in letters)
            {
                ++i;
                Console.WriteLine($"{letter.Letter} - {letter.Count}");
            }
            Console.WriteLine($"ИТОГО: {(letters as List<LetterStats>).Count} значений");
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Метод увеличивает счётчик вхождений по переданной структуре.
        /// </summary>
        /// <param name="letterStats"></param>
        private static void IncStatistic(ref LetterStats letterStats)
        {
            letterStats.Count++;
        }


    }
}
