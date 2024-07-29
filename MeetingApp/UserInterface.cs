using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MeetingApp
{
    /// <summary>
    /// Класс для взамодействия пользователя с консолью.
    /// </summary>
    public class UserInterface
    {
        private MeetingManager meetingManager; // список встреч



        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UserInterface"/>.
        /// </summary>
        public UserInterface(MeetingManager manager)
        {
            meetingManager = manager;
        }



        /// <summary>
        /// Вывод меню в консоль.
        /// </summary>
        public void DisplayMenu()
        {
            Console.WriteLine("-------------------------------");
            Console.WriteLine("1. Добавить встречу");
            Console.WriteLine("2. Удалить встречу");
            Console.WriteLine("3. Изменить встречу");
            Console.WriteLine("4. Просмотреть встречи");
            Console.WriteLine("5. Просмотреть встречи на дату");
            Console.WriteLine("6. Экспортировать встречи");
            Console.WriteLine("7. Добавить тестовые данные");
            Console.WriteLine("0. Выход");
            Console.WriteLine("-------------------------------\n");
        }

        /// <summary>
        /// Считывает ввод пользователя из консоли.
        /// </summary>
        public int GetUserInput()
        {
            Console.Write("Выберите номер меню - ");

            switch (Console.ReadLine())
            {
                case "1":
                    AddNewMeeting();
                    break;
                case "2":
                    DeleteMeeting();
                    break;
                case "3":
                    UpdateMeeting();
                    break;
                case "4":
                    OutputAllMeeting();
                    break;
                case "5":
                    OutputMeetingByDate();
                    break;
                case "6":
                    OutputMeetingInFile();
                    break;
                case "7":
                    AddTestMeetings();
                    break;
                case "0":
                    return 1;
                default:
                    Console.WriteLine("Такого пункта в меню нет");
                    break;
            }

            return 0;
        }

        /// <summary>
        /// Добавление новой встречи.
        /// </summary>
        private void AddNewMeeting()
        {
            Meeting meeting = new Meeting();

            meeting.Title = InputString("Введите название встречи - ");
            meeting.StartTime = InputDate("Введите дату начала встречи(dd.MM.yyyy HH:mm) - ");
            meeting.EndTime = InputDate("Введите дату конца встречи(dd.MM.yyyy HH:mm) - ");

            Console.WriteLine("Хотите вести время напоминания встречи? (д/н)");
            if (Console.ReadLine() == "д")
            {
                meeting.ReminderTime = InputTimeSpan("Введите напомнить встречи(HH:mm) - ");
            }

            var result = meetingManager.AddMeeting(meeting);

            WriteConsoleResult("Встреча успешна добавлена", result);
        }

        /// <summary>
        /// Удаление новой встречи.
        /// </summary>
        private void DeleteMeeting()
        {
            string input = InputString("Введите название встречи - ");

            var result = meetingManager.DeleteMeeting(input);

            WriteConsoleResult("Встреча успешна удалена", result);
        }

        /// <summary>
        /// Обновление новой встречи.
        /// </summary>
        private void UpdateMeeting()
        {
            Meeting meeting = new Meeting();
            
            string input = InputString("Введите название встречи которую хотите обновить - ");

            if (meetingManager.GetMeetingByName(input) == null) {
                Console.WriteLine("Встреча не найдена");
                return;
            };

            meeting.Title = InputString("Введите название новой встречи - ");
            meeting.StartTime = InputDate("Введите дату начала встречи(dd.MM.yyyy HH:mm) - ");
            meeting.EndTime = InputDate("Введите дату конца встречи(dd.MM.yyyy HH:mm) - ");

            Console.WriteLine("Хотите вести время напоминания встречи? (д/н)");
            if (Console.ReadLine() == "д")
            {
                meeting.ReminderTime = InputTimeSpan("Введите напомнить встречи(HH:mm) - ");
            }

            var result = meetingManager.UpdateMeeting(input, meeting);

            WriteConsoleResult("Встреча успешна обновлена", result);
        }

        /// <summary>
        /// Вывод всех встреч.
        /// </summary>
        private void OutputAllMeeting() {

            List<Meeting> meetings = meetingManager.GetAllMeetings();

            if (meetings.Count == 0)
            {
                Console.WriteLine("Список встреч пуст");
                return;
            }

            foreach (var item in meetings)
            {
                Console.WriteLine(item.ToString());
            }
        }

        /// <summary>
        /// Вывод список встреч по дате.
        /// </summary>
        private void OutputMeetingByDate()
        {
            DateTime input = InputDate("Введите дату на которую хотите посмотреть встречи(dd.MM.yyyy) - ", "dd.MM.yyyy");

            List<Meeting> meetings = meetingManager.GetMeeingsByDate(input);

            if (meetings.Count == 0)
            {
                Console.WriteLine("Список встреч пуст");
                return;
            }

            foreach (var item in meetings)
            {
                Console.WriteLine(item.ToString());
            }
        }

        /// <summary>
        /// Вывод встреч по дате в файл
        /// </summary>
        private void OutputMeetingInFile()
        {
            DateTime input = InputDate("Введите дату на которую хотите вывести встречи(dd.MM.yyyy) - ", "dd.MM.yyyy");

            List<Meeting> meetings = meetingManager.GetMeeingsByDate(input);

            if (meetings.Count == 0)
            {
                Console.WriteLine("Список встреч пуст");
                return;
            }

            try
            {
                using (StreamWriter writer = new StreamWriter("output.txt"))
                {
                    foreach (var item in meetings)
                    {
                        writer.WriteLine(item.ToString());
                    }
                }
                Console.WriteLine("Данные о встречах успешно выгружены в файл output.txt");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка при записи в файл: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выгрузке: {ex.Message}");
            }
        }

        /// <summary>
        /// Ввод тестовых встреч
        /// </summary>
        private void AddTestMeetings()
        {
            meetingManager.AddMeeting(new Meeting
            {
                Title = "Встреча 1",
                StartTime = DateTime.Now.AddHours(2),
                EndTime = DateTime.Now.AddHours(3),
                ReminderTime = TimeSpan.FromMinutes(15)
            });

            meetingManager.AddMeeting(new Meeting
            {
                Title = "Встреча 2",
                StartTime = DateTime.Now.AddHours(4),
                EndTime = DateTime.Now.AddHours(5),
                ReminderTime = null
            });

            meetingManager.AddMeeting(new Meeting
            {
                Title = "Встреча 3",
                StartTime = DateTime.Now.AddHours(6),
                EndTime = DateTime.Now.AddHours(7),
                ReminderTime = TimeSpan.FromMinutes(45)
            });

            meetingManager.AddMeeting(new Meeting
            {
                Title = "Встреча 4",
                StartTime = DateTime.Now.AddHours(8),
                EndTime = DateTime.Now.AddHours(9),
                ReminderTime = TimeSpan.FromMinutes(30)
            });

            meetingManager.AddMeeting(new Meeting
            {
                Title = "Встреча 5",
                StartTime = DateTime.Now.AddHours(10),
                EndTime = DateTime.Now.AddHours(11),
                ReminderTime = TimeSpan.FromMinutes(15)
            });

            meetingManager.AddMeeting(new Meeting
            {
                Title = "Встреча 6",
                StartTime = DateTime.Now.AddHours(12),
                EndTime = DateTime.Now.AddHours(13),
                ReminderTime = null
            });

            Console.WriteLine($"Тестовые встречи успешно добавлены");
        }



        /// <summary>
        /// Вывод в консоль результата работы MeetingManager
        /// </summary>
        /// <param name="successString">Вывод строки в консоль в случае true.</param>
        /// <param name="result">Кортеж результата.</param>
        private void WriteConsoleResult(string successString, (bool success, string errorMessage) result)
        {
            if (!result.success)
            {
                Console.WriteLine(result.errorMessage);
            }
            else
            {
                Console.WriteLine(successString);
            }
        }

        /// <summary>
        /// Проверка ввода в консоль строки
        /// </summary>
        /// <param name="text">Ввод текста пользователем</param>
        private string InputString(string text)
        {
            meetingManager.SleepOnNotification();
            Console.Write(text);

            string input = Console.ReadLine();

            while (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Ввели пустое значение");
                Console.Write(text);
                input = Console.ReadLine();
            }

            meetingManager.SleepOffNotification();
            return input;
        }

        /// <summary>
        /// Проверка ввода в консоль даты в формате строки
        /// </summary>
        /// <param name="text">Ввод даты пользователем в формате строки</param>
        /// /// <param name="format">Формат ввода времени</param>
        private DateTime InputDate(string text, string format = "dd.MM.yyyy HH:mm")
        {
            meetingManager.SleepOnNotification();
            Console.Write(text);

            string input = Console.ReadLine();
            DateTime date;

            while (!DateTime.TryParseExact(input, format, null, System.Globalization.DateTimeStyles.None, out date))
            {
                Console.WriteLine("Ввели некорректное значение формат - dd.MM.yyyy HH:mm");
                Console.Write(text);
                input = Console.ReadLine();
            }

            meetingManager.SleepOffNotification();
            return date;
        }

        /// <summary>
        /// Проверка ввода в консоль даты в формате строки
        /// </summary>
        /// <param name="text">Ввод временим в формате "hh\:mm"</param>
        private TimeSpan InputTimeSpan(string text)
        {
            meetingManager.SleepOnNotification();
            Console.Write(text);

            string input = Console.ReadLine();
            TimeSpan timeSpan = new TimeSpan();

            while (!TimeSpan.TryParseExact(input, "h\\:mm", null, out timeSpan))
            {
                Console.WriteLine("Ввели некорректное значение времени - hh:mm");
                Console.Write(text);
                input = Console.ReadLine();
            }

            meetingManager.SleepOffNotification();
            return timeSpan;
        }
    }
}
