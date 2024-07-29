using System;
using System.Collections.Generic;
using System.Timers;

namespace MeetingApp
{
    /// <summary>
    /// Класс, отвечающий за отправку уведомлений о предстоящих встречах.
    /// </summary>
    public class NotificationMeeting
    {
        private readonly List<Meeting> _meetingList; // Список встреч
        private readonly Timer _timer;
        private readonly object _consoleLock = new object(); // Объект для блокировки при выводе в консоль
        private volatile bool _sleeping = false; // Флаг для выключения уведолмений



        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="NotificationMeeting"/>.
        /// </summary>
        /// <param name="meetings">Список встреч, для которых будут отправляться уведомления.</param>
        public NotificationMeeting(List<Meeting> meetings)
        {
            _meetingList = meetings;

            _timer = new Timer(15000);
            _timer.Elapsed += CheckMeetings2;
            _timer.AutoReset = true;
            _timer.Enabled = true;

        }

        /// <summary>
        /// Функция проверки уведолмения.
        /// </summary>
        private void CheckMeetings2(Object source, ElapsedEventArgs e)
        {
            if (_sleeping)
            {
                DateTime currentTime = DateTime.Now;

                foreach (var meeting in _meetingList.ToArray())
                {
                    if (meeting.ReminderTime!=null)
                    {
                        DateTime? notificationTime = meeting.StartTime - meeting.ReminderTime;

                        if (currentTime >= notificationTime && currentTime < meeting.StartTime && !meeting.NotificationSent)
                        {
                            lock (_consoleLock)
                            {
                                Console.WriteLine($"\n*****************************");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"Уведомление: Встреча '{meeting.Title}' начнется в {meeting.StartTime.ToString("dd.MM.yyyy HH.mm")}");
                                Console.ResetColor();
                                Console.WriteLine($"*****************************");
                            }
                            meeting.NotificationSent = true; // Отмечаем что уведомление отправлено
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Временно выключает уведомления.
        /// </summary>
        public void SleepOn()
        {
            _sleeping = false;
        }

        /// <summary>
        /// Включает уведомления. 
        /// </summary>
        public void SleepOff()
        {
            _sleeping = true;
        }
    }
}
