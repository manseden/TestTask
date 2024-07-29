using System;

namespace MeetingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MeetingManager meetingManager = new MeetingManager();
            UserInterface userInterface = new UserInterface(meetingManager);

            while (true)
            {
                userInterface.DisplayMenu();
                if (userInterface.GetUserInput() == 1)
                {
                    break;
                }
            }
        }
    }
}
