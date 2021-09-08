using System;

namespace YolaTestProject
{
    class Program
    {
        private static MapPoint[] _mapPoints;

        private static IGeoService _geoService = new OpenStreetMapService();
        private static IFileSaver _fileSaver = new CommonFileSaver();

        static void Main(string[] args)
        {
            #region Header
            Console.WriteLine("================================================================");
            Console.WriteLine("===== Вас приветствует программа поиска полигона на карте! =====");
            Console.WriteLine("================================================================");
            Console.WriteLine();
            #endregion

            #region Query
            bool queryGot = false;

            Console.WriteLine("----- Введите запрос и нажмите клавишу \"Enter\"");

            while (!queryGot)
            {
                string query = Console.ReadLine();

                _mapPoints = _geoService.GetMapPoints(query);

                if (_mapPoints.Length > 0)
                {
                    queryGot = true;

                    Console.WriteLine($"----- Найдено {_mapPoints.Length} точек");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("----- Ничего не найдено... Попробуйте ввести запрос еще раз");
                }
            }
            #endregion

            #region Interval
            bool vertexCountGot = false;
            int interval = 0;

            Console.WriteLine();
            Console.WriteLine("----- Введите интервал между точками (1 - каждая первая точка,");
            Console.WriteLine("----- 2 - каждая вторая точка и т.д.) и нажмите клавишу \"Enter\"");

            while (!vertexCountGot)
            {
                int.TryParse(Console.ReadLine(), out interval);

                if (interval > 0)
                {
                    vertexCountGot = true;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("----- Некорректный интервал. Попробуйте ввести значение еще раз");
                }
            }

            MapPointsGapper pointsGapper = new MapPointsGapper();

            _mapPoints = pointsGapper.GapMapPoints(_mapPoints, interval);
            #endregion

            #region Saving
            Console.WriteLine();
            Console.WriteLine("----- Введите название файла");

            string fileName = Console.ReadLine();

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"export_{DateTime.Now}";
            }

            string finalPath = _fileSaver.Save(_mapPoints, fileName);

            Console.WriteLine($"----- Файл сохранен по пути {finalPath}");
            #endregion

            #region Ending
            Console.WriteLine();
            Console.WriteLine("=======================================");
            Console.WriteLine("===== Программа завершила работу! =====");
            Console.WriteLine("=======================================");
            #endregion

            Console.ReadKey();
        }
    }
}