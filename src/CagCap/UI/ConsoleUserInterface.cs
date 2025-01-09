// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.UI
{
    internal class ConsoleUserInterface: IUserInterface
    {
        public void Start()
        {
            Console.WriteLine("CAN and GPS data capturing has started.");
            Console.WriteLine("Press 'q' to quit.");
        }

        public void WaitForExit()
        {
            while (Console.ReadKey(true).Key != ConsoleKey.Q)
            {
                // Wait for the user to press 'q'
            }
        }
    }
}
