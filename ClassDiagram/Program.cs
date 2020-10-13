using System;
using System.Threading;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace ClassDiagram
{
    class Program
    {
        static bool threadCondition = true;
        static void Main(string[] args)
        {
          //declaring variables
            int intChoice;
            List<Tamagotchi> tamagotchiList = new List<Tamagotchi>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Tamagotchi>));
            //loads old save file if it exists
            if (File.Exists("tamagotchis.xml"))
            {
                tamagotchiList = LoadInstances(tamagotchiList, serializer);
            }
            while (true)
            {
              //InstanceStart() lets you choose to create or load your tamagotchi pets 
                intChoice = InstanceStart(tamagotchiList, serializer);
                //threadCondition is a static bool that is used for a while loop in the timeTick thread, lock not needed for bool
                threadCondition = true;
                //Starts thread, thread is responsible for saving and keeping track of time, it does this every 10~ seconds
                Thread timeTick = new Thread(()=>ThreadTick(tamagotchiList, serializer, intChoice));
                timeTick.Start();
                //Uistart let's you control actions for your pet or return to the main menu
                UiStart(tamagotchiList, intChoice);
                /*if user choose to return we will cancel the current thread (since it ticks for the wrong variables)
                and then we use timetick.Join to wait for the thread to stop, we use this to make sure SaveInstances doesn't complain
                about the file being used by another process*/
                threadCondition = false;
                System.Console.WriteLine("Loading...");
                timeTick.Join();
            } 
        }

        static void UiStart(List<Tamagotchi> tamagotchiList, int intChoice)
        {
            int x = 0;
        while (true)
        {
          //if pet dies it will remove the pet from the list
          if (tamagotchiList[intChoice].GetAlive() == false)
          {
              Console.Clear();
              System.Console.WriteLine("Oh no! it looks like your pet died... Press Enter to bury your pet");
              tamagotchiList.RemoveAt(intChoice);
              Console.ReadLine();
              return;
          }
          //UI that let's you choose actions for pet
          Console.Clear();
          string[] array = new string[]{$"Teach {tamagotchiList[intChoice].name} a new word", $"Say hello to {tamagotchiList[intChoice].name}", $"Feed {tamagotchiList[intChoice].name}","Check Health","Main Menu"};
          for (int i = 0; i < x; i++)
          {
            System.Console.WriteLine("    " + array[i]);
          }
          Console.BackgroundColor = ConsoleColor.White;
          Console.ForegroundColor = ConsoleColor.Black;
          Console.WriteLine("--> " + array[x]);
          Console.BackgroundColor = ConsoleColor.Black;
          Console.ForegroundColor = ConsoleColor.White;
          for (int i = x +1; i < array.Length; i++)
          {
            System.Console.WriteLine("    " + array[i]);
          }
          ConsoleKeyInfo Ui = Console.ReadKey();
          if (Ui.Key == ConsoleKey.DownArrow && x != array.Length-1|| Ui.Key == ConsoleKey.S && x != array.Length-1)
          {
            x++;
          }
          else if (Ui.Key == ConsoleKey.UpArrow && x != 0|| Ui.Key == ConsoleKey.W && x != 0)
          {
            x--;
          }
          else if (Ui.Key == ConsoleKey.Enter)
          {
            switch (x)
            {
              case 0:
                Console.Write("New word: ");
                tamagotchiList[intChoice].Teach(Console.ReadLine());
                break;
              case 1:
                tamagotchiList[intChoice].Hi();
                break;
              case 2:
                tamagotchiList[intChoice].Feed();
                break;
              case 3:
                tamagotchiList[intChoice].PrintStats();
              break;
              case 4:

                return;
            }
            System.Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
          }
        }
        }
        static int InstanceStart(List<Tamagotchi> tamagotchiList, XmlSerializer serializer)
        {
            while (true)
            {
              int x = 0;
              //UI that lets user choose if user wants to create tamagochi or load a previously created one
                while (true)
                {
                  Console.Clear();
                  string[] array = new string[]{"1.Create new Tamagotchi","2.Load Tamagotchi"};
                  for (int i = 0; i < x; i++)
                  {
                    System.Console.WriteLine("    "+array[i]);
                  }
                  Console.BackgroundColor = ConsoleColor.White;
                  Console.ForegroundColor = ConsoleColor.Black;
                  Console.WriteLine("--> " +array[x]);
                  Console.BackgroundColor = ConsoleColor.Black;
                  Console.ForegroundColor = ConsoleColor.White;
                  for (int i = x +1; i < array.Length; i++)
                  {
                    System.Console.WriteLine("    "+array[i]);
                  }
                  ConsoleKeyInfo Ui = Console.ReadKey();
                  if (Ui.Key == ConsoleKey.DownArrow && x != array.Length-1|| Ui.Key == ConsoleKey.S && x != array.Length-1)
                  {
                    x++;
                  }
                  else if (Ui.Key == ConsoleKey.UpArrow && x != 0|| Ui.Key == ConsoleKey.W && x != 0)
                  {
                    x--;
                  }
                  else if (Ui.Key == ConsoleKey.Enter)
                  {
                    switch (x)
                    {
                      case 0:
                      //while loop with UI to let user create pet, name it and select difficulty
                       while (true)
                       {
                          Console.Clear();
                          System.Console.Write("Name your character: ");
                          string nameTamagotchi = Console.ReadLine();
                          x = 0;
                          int difficulty = 0;
                          while (true)
                          {
                            Console.Clear();
                            System.Console.WriteLine("Select difficulty!");
                            array = new string[]{"Easy", "Normal", "Hard", "Extreme", "ULTRA CHAOTIC SUPER HARD"};
                            for (int i = 0; i < x; i++)
                            {
                              System.Console.WriteLine("    "+array[i]);
                            }
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine("--> "+array[x]);
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                            for (int i = x +1; i < array.Length; i++)
                            {
                              System.Console.WriteLine("    "+array[i]);
                            }
                            Ui = Console.ReadKey();
                            if (Ui.Key == ConsoleKey.DownArrow && x != array.Length-1|| Ui.Key == ConsoleKey.S && x != array.Length-1)
                            {
                              x++;
                            }
                            else if (Ui.Key == ConsoleKey.UpArrow && x != 0|| Ui.Key == ConsoleKey.W && x != 0)
                            {
                              x--;
                            }
                            else if (Ui.Key == ConsoleKey.Enter)
                            {
                              switch (x)
                              {
                                case 0:
                                  difficulty = 0;
                                break;
                                case 1:
                                difficulty = 1;
                                break;
                                case 2:
                                difficulty = 2;
                                break;
                                case 3:
                                difficulty = 3;
                                break;
                                case 4:
                                difficulty = 4;
                                break;
                              }
                              x = 0;
                              break;
                            }
                          }
                          if (nameTamagotchi != "")
                          {
                            nameTamagotchi = SecurityElement.Escape(nameTamagotchi);
                            tamagotchiList.Add(new Tamagotchi{name = nameTamagotchi.Trim(), difficulty = difficulty});
                            SaveInstances(tamagotchiList,serializer);
                            break;
                          }
                          else
                          {
                            System.Console.WriteLine("Invalid Name! Press Enter to try again");
                            Console.ReadLine();
                          }
                      }
                    break;
                    case 1:
                          //UI that lets user load previous tamagochis
                          int xPet = 0;
                          if (tamagotchiList.Count == 0)
                          {
                            System.Console.WriteLine("You don't have any pets! Press Enter to try again...");
                            Console.ReadLine();
                              break;
                          }
                          while (true)
                          {
                            System.Console.WriteLine("Choose a pet!");
                            Console.Clear();
                            for (int i = 0; i < xPet; i++)
                            {
                              System.Console.WriteLine("    "+tamagotchiList[i].name);
                            }
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.WriteLine("--> "+tamagotchiList[xPet].name);
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                            for (int i = xPet +1; i < tamagotchiList.Count; i++)
                            {
                              System.Console.WriteLine("    "+tamagotchiList[i].name);
                            }
                            ConsoleKeyInfo petUi = Console.ReadKey();
                            if (petUi.Key == ConsoleKey.DownArrow && xPet != tamagotchiList.Count-1|| petUi.Key == ConsoleKey.S && x != tamagotchiList.Count-1)
                            {
                              xPet++;
                            }
                            else if (petUi.Key == ConsoleKey.UpArrow && xPet != 0|| petUi.Key == ConsoleKey.W && xPet != 0)
                            {
                              xPet--;
                            }
                            else if (petUi.Key == ConsoleKey.Enter)
                            {
                              return xPet;
                            }
                            else if(petUi.Key == ConsoleKey.Escape)
                            {
                              break;
                            }
                          }
                          break;
                    }
                  }
                }
            }
        }
        static List<Tamagotchi> LoadInstances(List<Tamagotchi> tamagotchiList, XmlSerializer serializer)
        {
          //filestream closes with using statement. Opens file, deserialize it to List with tamagochis and returns it.
            using (FileStream tamagotchiStream = File.OpenRead("tamagotchis.xml"))
            {
                tamagotchiList = (List<Tamagotchi>)serializer.Deserialize(tamagotchiStream);
                return tamagotchiList;
            }
            
        }
        static void SaveInstances(List<Tamagotchi> tamagotchiList, XmlSerializer serializer)
        {
          //filestream closes safely with using statement. Open or creates file and serializes the list inputed in parameter.
          using (FileStream tamagotchiFile = File.Open("tamagotchis.xml", FileMode.OpenOrCreate))
          {
              serializer.Serialize(tamagotchiFile, tamagotchiList);
          }
        }
        public static void ThreadTick(List<Tamagotchi> tamagotchis, XmlSerializer serializer, int intChoice)
        {
          //the thread longs when the static bool is true (check main to see when threadcondition is true)
            while (threadCondition == true)
            {
              /*runs tick for the chosed tamagochi and saves tamagochis, because of Thread.Sleep(), it might take a while
              waiting for the thread to close, since Thread.Sleep() will delay the closing of while loop*/
              tamagotchis[intChoice].Tick();
              SaveInstances(tamagotchis, serializer);
              Thread.Sleep(5000);
            }
        }
    }
}
