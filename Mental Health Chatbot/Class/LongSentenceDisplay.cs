using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mental_Health_Chatbot.Class
{
    public class LongSentenceDisplay
    {
        public static void printLongSentence(String longSentence)
        {
            int maxLineWidth = 100;
            string[] words = longSentence.Split(' ');

            int currentLineWidth = 0;
            string formattedSentence = "";

            formattedSentence += "[MH Chatbot] ";

            // Loop through each word
            foreach (string word in words)
            {
                // Check if the word fits in the current line or if it exceeds the maximum line width
                if (currentLineWidth + word.Length + 1 > maxLineWidth)
                {
                    formattedSentence += Environment.NewLine;  // Start a new line
                    formattedSentence += "             ";
                    currentLineWidth = 0;  // Reset the current line width
                }

                // Append the word to the formatted sentence
                formattedSentence += word + " ";

                // Update the current line width
                currentLineWidth += word.Length + 1;
            }
            formattedSentence += Environment.NewLine;

            Console.WriteLine(formattedSentence);
        }
    }
}

