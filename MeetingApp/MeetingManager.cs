using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MeetingApp
{
    /// <summary>
    /// Класс, управляющий встречами.
    /// </summary>
    public class MeetingManager
    {
        private List<Meeting> _listMeetings = new List<Meeting>(); // Список встреч
        private NotificationMeeting _notificationMeeting; // Класс уведомлений



        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MeetingManager"/>.
        /// </summary>
        public MeetingManager()
        {
            _notificationMeeting = new NotificationMeeting(_listMeetings);
        }



        /// <summary>
        /// Добавляет новую встречу.
        /// </summary>
        /// <param name="meeting">Встреча для добавления.</param>
        public (bool success, string errorMessage) AddMeeting(Meeting meeting)
        {
            if (meeting.StartTime < DateTime.Now)
            {
                return (false, "Ошибка добавления! Дата начала встречи меньше текущей");
            }

            if (_listMeetings.Any(a=>meeting.StartTime < a.EndTime && meeting.EndTime > a.StartTime))
            {
                return (false, "Ошибка добавления! Встреча пересекается с существующей");
            }

            _listMeetings.Add(meeting);

            return (true,string.Empty);

        }

        /// <summary>
        /// Удаляет новую встречу.
        /// </summary>
        /// <param name="title">Название встречи для добавления.</param>
        public (bool success, string errorMessage) DeleteMeeting(string title)
        {
            Meeting meeting = GetMeetingByName(title);

            if (meeting == null)
            {
                return (false, $"Встреча - {title} не найдена");
            }

            _listMeetings.Remove(meeting);

            return (true, string.Empty);
        }

        /// <summary>
        /// Обновляет новую встречу.
        /// </summary>
        /// <param name="title">Название встречи для обновления.</param>
        /// <param name="newMeeting">Обновленная встреча.</param>
        public (bool success, string errorMessage) UpdateMeeting(string title, Meeting newMeeting)
        {
            Meeting oldMeeting = GetMeetingByName(title);
            if (oldMeeting == null)
            {
                return (false, $"Встреча - {title} не найдена");
            }
            if (_listMeetings.Any(a => a.Id != oldMeeting.Id && newMeeting.StartTime < a.EndTime && newMeeting.EndTime > a.StartTime))
            {
                return (false, "Ошибка обновления! Встреча пересекается с существующей");
            }

            _listMeetings.Remove(oldMeeting);
            _listMeetings.Add(newMeeting);

            return (true, string.Empty);
        }



        /// <summary>
        /// Возвращает список встреч по дате.
        /// </summary>
        /// <param name="date">Дата для поиска встречи.</param>
        public List<Meeting> GetMeeingsByDate(DateTime date) => _listMeetings.Where(a => a.StartTime.Date == date.Date).ToList();

        /// <summary>
        /// Возвращает встречу по имени.
        /// </summary>
        /// <param name="nameMeeting">Название встречи для поиска.</param>
        public Meeting GetMeetingByName(string nameMeeting) => _listMeetings.FirstOrDefault(a=>a.Title == nameMeeting);

        /// <summary>
        /// Возвращает все встречи.
        /// </summary>
        public List<Meeting> GetAllMeetings() => _listMeetings;



        /// <summary>
        /// Временно выключает уведомления.
        /// </summary>
        public void SleepOnNotification()
        {
            _notificationMeeting.SleepOn();
        }

        /// <summary>
        /// Включает уведомления.
        /// </summary>
        public void SleepOffNotification()
        {
            _notificationMeeting.SleepOff();
        }
    }
}
