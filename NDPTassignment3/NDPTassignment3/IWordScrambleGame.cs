/**
 * PROG 3170 Assignment 3
 * Nicole Dahlquist and Peter Thomson
 * IWordScrambleGame.cs
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
    [ServiceContract]
    public interface IWordScrambleGame
    {
        // Returns true if the game is already being hosted or false otherwise 
        [OperationContract]
        bool isGameBeingHosted();
                
        
        // Exception: game is already being hosted by someone else
        [FaultContract(typeof(AlreadyHostedFault))]
        [OperationContract]
        // The function returns the name of the person hosting the game 
        // User ‘userName’ tries to host the game with word ‘wordToScramble’
        string hostGame(string userName, string wordToScramble);

        
        // Exception: maximum number of players reached
        [FaultContract(typeof(FullGameFault))]
        // Exception: host cannot join the game
        [FaultContract(typeof(HostJoinFault))]
        // Exception: nobody is hosting the game      
        [FaultContract(typeof(NobodyHostingFault))]
        [OperationContract]
        // Player ‘playerName’ tries to join the game
        // The function returns a Word object containing the host’s (un)scrambled words
        Word join(string playerName);

        
        // Exception: user is not playing the game 
        [FaultContract(typeof(PlayerNotFoundFault))]
        [OperationContract]
        // Player ‘playerName’ guesses word ‘guessedWord’ compared with word ‘unscrambledWord’
        // Returns true if ‘guessedWord’ is identical to ‘unscrambledWord’ or false otherwise
        // The function returns the name of the person hosting the game 
        bool guessWord(string playerName, string guessedWord, string unscrambledWord);

    }

    [DataContract]
    public class Word
    {
        [DataMember]
        public string unscrambledWord;
        [DataMember]
        public string scrambledWord;
    }

    // Exception: game is already being hosted by someone else
    [DataContract]
    public class AlreadyHostedFault
    {
        [DataMember]
        public string hostName;

        [DataMember]
        public string reason;
    }

    // Exception: maximum number of players reached
    [DataContract]
    public class FullGameFault
    {
        [DataMember]
        public string reason;

        [DataMember]
        public int maxNumber;
    }

    // Exception: host cannot join the game
    [DataContract]
    public class HostJoinFault
    {
        [DataMember]
        public string hostName;

        [DataMember]
        public string reason;
    }

    // Exception: nobody is hosting the game  
    [DataContract]
    public class NobodyHostingFault
    {
        [DataMember]
        public string reason;
    }

    // Exception: user is not playing the game 
    [DataContract]
    public class PlayerNotFoundFault
    {
        [DataMember]
        public string reason;
    }
}
