using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace NDPTassignment3
{
    [ServiceBehavior]
    public class WordScrambleGame : IWordScrambleGame
    {
        // the maximum number of players allowed playing simultaneously
        private const int MAX_PLAYERS = 5;
        // the user hosting the game. If it’s null nobody is hosting the game.
        private static String userHostingTheGame = null;
        // the Word object that contains the scrambled and unscrambled words
        private static Word gameWords;
        // the list of players playing the game
        private static List<String> activePlayers = new List<string>();

        [OperationBehavior]
        public bool isGameBeingHosted()
        {
            // TO BE COMPLETED BY YOU: Add exception and program logic
            return false;
        }

        [OperationBehavior]
        public string hostGame(String playerName, String wordToScramble)
        {
            // TO BE COMPLETED BY YOU: Add exception and program logic
            if (userHostingTheGame != null)
            {
                AlreadyHostedFault fault = new AlreadyHostedFault();
                fault.hostName = userHostingTheGame;
                fault.reason = fault.hostName + " is already hosting the game.";
                throw new FaultException<AlreadyHostedFault>(fault, fault.reason);
            }
            
            userHostingTheGame = playerName;
            string scrambledWord = scrambleWord(wordToScramble);
            gameWords = new Word();
            gameWords.scrambledWord = scrambledWord;
            gameWords.unscrambledWord = wordToScramble;


            return gameWords.scrambledWord;
        }

        [OperationBehavior]
        public Word join(string playerName)
        {
            // TO BE COMPLETED BY YOU: Add exception and program logic
            if(userHostingTheGame == null)
            {
                NobodyHostingFault fault = new NobodyHostingFault();
                fault.reason = "Nobody hosting this game.";
                throw new FaultException<NobodyHostingFault>(fault, fault.reason);
            }
            if(userHostingTheGame == playerName)
            {
                HostJoinFault fault = new HostJoinFault();
                fault.hostName = userHostingTheGame;
                fault.reason = fault.hostName + " is hosting and cannot join the game";
                throw new FaultException<HostJoinFault>(fault, fault.reason);
            }
            if(activePlayers.Count >= MAX_PLAYERS)
            {
                FullGameFault fault = new FullGameFault();
                fault.maxNumber = MAX_PLAYERS;
                fault.reason = "Player count has exceede the maximum number(" + fault.maxNumber + ").";
                throw new FaultException<FullGameFault>(fault, fault.reason);
            }
            activePlayers.Add(playerName);
            return gameWords;
        }

        [OperationBehavior]
        public bool guessWord(string playerName, string guessedWord, string unscrambledWord)
        {
            // TO BE COMPLETED BY YOU: Add exception and program logic
            if(!activePlayers.Contains(playerName))
            {
                NobodyPlayingFault fault = new NobodyPlayingFault();
                fault.reason = "Player is not playing this game.";
                throw new FaultException<NobodyPlayingFault>(fault, fault.reason);
            }
            if (guessedWord == unscrambledWord)
                return true;
            
            return false;
        }

        // Utility function to scramble a word
        private string scrambleWord(string word)
        {
            char[] chars = word.ToArray();
            Random r = new Random(2011);
            for (int i = 0; i < chars.Length; i++)
            {
                int randomIndex = r.Next(0, chars.Length);
                char temp = chars[randomIndex];
                chars[randomIndex] = chars[i];
                chars[i] = temp;
            }
            return new string(chars);
        }

    }
}
