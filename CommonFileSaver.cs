using System;
using System.IO;

namespace YolaTestProject
{
    class CommonFileSaver : IFileSaver
    {
        // сделал так, потому что нет требований по сохранению
        // если что можно будет написать отдельный класс, чтобы можно
        // было передавать в конструкторе путь и формат
        // а пока что пусть будет такой класс для сохранения в txt

        private string _path;
        private string _folderName = "Yola";
        private string _fileName;

        public string Save(MapPoint[] points, string fileName)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();

            foreach (char invalidChar in invalidChars)
            {
                fileName = fileName.Replace(invalidChar, '_'); // удаляем символы, которые не разрешены в именах файлов
            }

            _fileName = fileName;

            _path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _path = Path.Combine(_path, _folderName);

            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            _path = Path.Combine(_path, _fileName);
            _path += ".txt";

            FileStream fileStream = new FileStream(_path, FileMode.OpenOrCreate);
            StreamWriter streamWriter = new StreamWriter(fileStream, System.Text.Encoding.Default);

            foreach (MapPoint point in points)
            {
                streamWriter.WriteLine($"X = {point.X}, Y = {point.Y}");
            }

            streamWriter.Close();
            fileStream.Close();

            return _path;
        }
    }
}