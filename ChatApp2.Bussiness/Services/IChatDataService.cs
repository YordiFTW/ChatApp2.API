﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp2.Domain.Lists;
using ChatApp2.Domain.Models;

namespace ChatApp2.Bussiness.Services
{
    public class ChatDataService : IChatDataService
    {
        ProfanityList profanityList = new ProfanityList();


        public Message ProfanityChecker(Message message)
        {

            foreach (string word in profanityList._wordList)
            {
                if (message.Content.Contains(word))
                {
                    string reformed = word;

                    foreach (char letter in word)
                    {

                        reformed = reformed.Replace(letter, '*');
                    }

                    message.Content = message.Content.Replace(word, reformed);
                }
                else
                {
                    //donothing
                }
            }

            return message;
        }

        public Chat TogglePrivateChat(Chat chat)
        {
            if (chat.Private == true)
                chat.Private = false;
            else
                chat.Private = true;



            return chat;
        }


    }
}
