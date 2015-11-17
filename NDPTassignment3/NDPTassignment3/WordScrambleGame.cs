/**
 * PROG 3170 Assignment 3
 * Nicole Dahlquist and Peter Thomson
 * WordScrambleGame.cs
 * 
 * Created: November 17, 2015 
 * 
 **/

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
            //Return true if a user is hosting the game, false otherwise
            return (userHostingTheGame != null);
        }

        [OperationBehavior]
        public string hostGame(String playerName, String wordToScramble)
        {
            //Throw exception if a host already exists
            if (userHostingTheGame != null)
            {
                AlreadyHostedFault fault = new AlreadyHostedFault();
                fault.hostName = userHostingTheGame;
                fault.reason = fault.hostName + " is already hosting the game.";
                throw new FaultException<AlreadyHostedFault>(fault, fault.reason);
            }
            
            //Set the host and the game word
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
            //Throw exception if noone is hosting the game
            if(userHostingTheGame == null)
            {
                NobodyHostingFault fault = new NobodyHostingFault();
                fault.reason = "Nobody hosting this game.";
                throw new FaultException<NobodyHostingFault>(fault, fault.reason);
            }

            //Throw exception if the player trying to join is the same as the host
            if(userHostingTheGame == playerName)
            {
                HostJoinFault fault = new HostJoinFault();
                fault.hostName = userHostingTheGame;
                fault.reason = fault.hostName + " is hosting and cannot join the game";
                throw new FaultException<HostJoinFault>(fault, fault.reason);
            }

            //Throw exception if maxmimum number of concurrent players have already joined
            if(activePlayers.Count >= MAX_PLAYERS)
            {
                FullGameFault fault = new FullGameFault();
                fault.maxNumber = MAX_PLAYERS;
                fault.reason = "Player count has exceeded the maximum number(" + fault.maxNumber + ").";
                throw new FaultException<FullGameFault>(fault, fault.reason);
            }

            //Add new player to the activePlayers list
            activePlayers.Add(playerName);
            return gameWords;
        }

        [OperationBehavior]
        public bool guessWord(string playerName, string guessedWord, string unscrambledWord)
        {
            //Throw exception if the player is not in the game
            if(!activePlayers.Contains(playerName))
            {
                PlayerNotFoundFault fault = new PlayerNotFoundFault();
                fault.reason = "Player is not playing this game.";
                throw new FaultException<PlayerNotFoundFault>(fault, fault.reason);
            }

            //return true if the guessed word matches the original word
            if (guessedWord == unscrambledWord) return true;

            //return false if guess did not match
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
