using System;
using System.Collections.Generic;
using System.Security;

namespace ClassDiagram
{
    public class Tamagotchi
    {
        //Variables
        bool isAlive = true;
        public int difficulty = 0;
        Random generator = new Random();
        public string name = "";

        List<string> words = new List<string>();
        public int hunger = 0;
        public int bordedom = 0;
        public DateTime oldTime = DateTime.Now;
        long minutesToTick = 1;

        //Methods
        public bool GetAlive()
        {
            return isAlive;
        }
        public void Feed()
        {
            int randomNumber = generator.Next(1,4);
            if (hunger - randomNumber >= 0)
            {
                hunger -= randomNumber;
            }
            else
            {
                hunger = 0;
            }
            System.Console.WriteLine("Fed " + name);
        }
        public void Hi()
        {
            if (words.Count == 0)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("You: Hi");
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.Write(name);
                Console.ForegroundColor = ConsoleColor.White;
                System.Console.Write(": tja"); 
                System.Console.WriteLine();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine();
                System.Console.WriteLine("You: Hi");
                System.Console.Write(name);
                Console.ForegroundColor = ConsoleColor.White;
                System.Console.Write(": " + words[generator.Next(0, words.Count)]); 
                System.Console.WriteLine();
            }
            ReduceBoredom();
        }
        public void Teach(string word)
        {
            if (word.Length != 0)
            {
                words.Add(word.Trim());
                ReduceBoredom();
                System.Console.WriteLine("New word learned!");
            }
            else
            {
                System.Console.WriteLine("Your word can't be 0 characters long!");
            }
        }
        public void PrintStats()
        {
            System.Console.WriteLine($"hunger: {hunger} \nboredom: {bordedom}\nAlive: {isAlive}");
        }
        private void ReduceBoredom()
        {
            if (bordedom != 0)
            {
                bordedom -= 1;
            }
        }
        public void Tick()
        {
            switch (difficulty)
            {
                case 0:
                minutesToTick = 120;
                break;
                case 1:
                minutesToTick = 60;
                break;
                case 2:
                minutesToTick = 30;
                break;
                case 3:
                minutesToTick = 5;
                break;
                case 4:
                minutesToTick = 1;
                break;
            }
            //check differential time between latest saved point and current point (in time)
            DateTime currentTime = DateTime.Now;
            long diffTicks = currentTime.Ticks - oldTime.Ticks;
            TimeSpan diffTime = new TimeSpan(diffTicks);
            if (minutesToTick <= diffTime.Minutes)
            {
                for (int i = 0; i < diffTime.Minutes; i++)
                {
                    hunger += 1;
                    bordedom += 1;   
                }
                oldTime = DateTime.Now;
            }
            //if hunger or boredom is or is over 10 it will die
            if (hunger >= 10 || bordedom >= 10)
            {
                isAlive = false;
            }
        }
    }
}
