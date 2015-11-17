/**
 * PROG 3170 Assignment 3
 * Nicole Dahlquist and Peter Thomson
 * WordScrambleClient.cs
 * 
 * Created: November 17, 2015 
 * 
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDPTassignment3Client.WordScrambleGameServiceReference;
using System.ServiceModel;


namespace NDPTassignment3Client
{
    class WordScrambleClient
    {
        static void Main(string[] args)
        {
            WordScrambleGameClient proxy = new WordScrambleGameClient();

            bool canPlayGame = true;

            Console.WriteLine("Player's name?");
            String playerName = Console.ReadLine();

            if (!proxy.isGameBeingHosted())
            {
                Console.WriteLine("Welcome " + playerName +
                           "! Do you want to host the game?(yes/no)");
                if (Console.ReadLine().CompareTo("yes") == 0)
                {
                    Console.WriteLine("Type the word to scramble.");
                    string inputWord = Console.ReadLine();

                    //Error handling for hostGame method
                    try
                    {
                        string scrambledWord = proxy.hostGame(playerName, inputWord);
                        canPlayGame = false;
                        Console.WriteLine("You're hosting the game with word '" + inputWord + "' scrambled as '" + scrambledWord + "'");
                        Console.ReadKey();
                    }
                    catch (FaultException e)
                    {
                        Console.WriteLine("{0}:{1}", e.Code.Name, e.Reason);
                        Console.ReadKey();
                    }
                    
                }
            }

            if (canPlayGame)
            {
                Console.WriteLine("Do you want to play the game?(yes/no)");
                if (Console.ReadLine().CompareTo("yes") == 0)
                {
                    //Error handling for join method
                    try
                    {
                        Word gameWords = proxy.join(playerName);
                        Console.WriteLine("Can you unscramble this word? => " + gameWords.scrambledWord);
                        String guessedWord;
                        bool gameOver = false;
                        while (!gameOver)
                        {
                            guessedWord = Console.ReadLine();

                            //Error handling for guessWord method
                            try
                            {
                                gameOver = proxy.guessWord(playerName, guessedWord, gameWords.unscrambledWord);
                                if (!gameOver)
                                {
                                    Console.WriteLine("Nope, try again...");
                                }                                   
                            }
                            catch(FaultException e)
                            {
                                Console.WriteLine("{0}:{1}", e.Code.Name, e.Reason);
                                Console.ReadKey();
                            }
                            
                        }
                        Console.WriteLine("You WON!!!");
                        Console.ReadKey();
                    }

                    catch(FaultException e)
                    {
                        Console.WriteLine("{0}:{1}", e.Code.Name, e.Reason);
                        Console.ReadKey();
                    }
                   
                }
            }
        }
    }
}
