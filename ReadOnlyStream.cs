using System;
using System.IO;
using System.Windows;

namespace TestTask
{
    public class ReadOnlyStream : IReadOnlyStream
    {
        private StreamReader _localStream;
        
        /// <summary>
        /// Конструктор класса. 
        /// Т.к. происходит прямая работа с файлом, необходимо 
        /// обеспечить ГАРАНТИРОВАННОЕ закрытие файла после окончания работы с таковым!
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        public ReadOnlyStream(string fileFullPath)
        {
            IsEof = false;

            // TODO : Заменить на создание реального стрима для чтения файла!
            //_localStream = null;
            //_localStream = new FileStream(fileFullPath,FileMode.OpenOrCreate);
            _localStream = new StreamReader(fileFullPath);
        }

        /// <summary>
        /// Флаг окончания файла.
        /// </summary>
        private bool _isEof = true;
        public bool IsEof
        {
            get => _isEof;
            // TODO : Заполнять данный флаг при достижении конца файла/стрима при чтении
            private set
            {
                _isEof = value;
            }
        }


        /// <summary>
        /// Ф-ция чтения следующего символа из потока.
        /// Если произведена попытка прочитать символ после достижения конца файла, метод 
        /// должен бросать соответствующее исключение
        /// </summary>
        /// <returns>Считанный символ.</returns>
        public char ReadNextChar()
        {
            // TODO : Необходимо считать очередной символ из _localStream
            IsEof = _localStream.EndOfStream;
            if (!IsEof)
            {
                try
                {
                    char ch = Convert.ToChar(_localStream.Read());

                    if(_localStream.EndOfStream) IsEof = true;
                    return char.IsLetter(ch) ? ch : ' ';
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка чтения символа: " + ex.Message);
                    return (char)0x00;
                }
            }
            else
                throw new EndOfStreamException();

        }

        /// <summary>
        /// Сбрасывает текущую позицию потока на начало.
        /// </summary>
        public void ResetPositionToStart()
        {
            if (_localStream == null)
            {
                IsEof = true;
                return;
            }

            _localStream.BaseStream.Position = 0;
            IsEof = false;
        }

        public void CloseFile()
        {
            _localStream.Close();
        }

        ~ReadOnlyStream()
        {
            CloseFile();
        }
    }
}
