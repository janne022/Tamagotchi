using System;
using System.Collections.Generic;
using System.Security;

namespace ClassDiagram
{
    public class Tamagotchi
    {
        bool isAlive = true;
        public int difficulty = 0;
        Random generator = new Random();
        public string name = SecurityElement.Escape("");

        List<string> words = new List<string>();
        public int hunger = 0;
        public int bordedom = 0;
        public DateTime oldTime = DateTime.Now;
        long minutesToTick = 1;


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
        }
        public void Hi()
        {
            if (words.Count == 0)
            {
               System.Console.WriteLine(name + ": tja"); 
            }
            else
            {
                System.Console.WriteLine(name + ": " + words[generator.Next(0, words.Count)]);
            }
            ReduceBoredom();
        }
        public void Teach(string word)
        {
            words.Add(word.Trim());
            ReduceBoredom();
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
            if (hunger >= 10 || bordedom >= 10)
            {
                isAlive = false;
            }
        }
    }
}
