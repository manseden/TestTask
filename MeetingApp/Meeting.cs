using System;

namespace MeetingApp
{
    public class Meeting
    {

        /// <summary>
        /// Ид встречи.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Название встречи.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Время начала встречи.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Время окончания встречи.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Время напоминания перед началом встречи.
        /// Значение может быть null, если напоминание не установлено.
        /// </summary>
        public TimeSpan? ReminderTime { get; set; }

        /// <summary>
        /// Проверка отправлено ли уведолмение
        /// </summary>
        public bool NotificationSent { get; set; } = false;

        public override string ToString()
        {
            string reuslt = $"Встреча - {Title}\n Время начала - {StartTime.ToString("dd.MM.yyyy HH.mm")}\n Время окончания - {EndTime.ToString("dd.MM.yyyy HH.mm")}\n";

            if (ReminderTime.HasValue)
            {
                reuslt += $" Время напоминания - {ReminderTime.Value}\n";
            }

            return reuslt;
        }
    }
}
