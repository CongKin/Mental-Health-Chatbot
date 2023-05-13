// See https://aka.ms/new-console-template for more information
using Mental_Health_Chatbot.Class;
using Mental_Health_Chatbot.Database;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Mental_Health_Chatbot.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n");
            LongSentenceDisplay.printLongSentence("Welcome to our mental health support chat! I'm here to provide you with guidance, " +
                "information, and support for your mental well-being. Whether you're seeking coping strategies for anxiety," +
                " tips for improving sleep, or guidance on managing stress, I'm here to help. Please feel free to ask any " +
                "questions or share your concerns, and together, we'll work towards a healthier and happier you.");

            LongSentenceDisplay.printLongSentence("Hello! How can I support you today? Feel free to share your thoughts " +
                "or ask any questions about mental health. I'm here to help!");

            List<int> QList = new List<int>();
            List<int> MHList = new List<int>();
            int questionType = 0;
            int mentalHealthType = 0;

            // reply type = leave, ask
            // words type = leave, ask, symptom, no meaning, unused

            while (true)
            {
                Console.Write("[User] ");
                string reply = Console.ReadLine();
                Console.WriteLine();

                if (reply == "admin_0112")
                {

                }
                else
                {
                    string[] words = reply.Split(' ');

                    if (words.Length > 0)
                    {

                        bool leave = false;

                        foreach (string word in words)
                        {
                            string keyword = word.ToLower();

                            if (myDatabase.isLeaving(keyword))
                            {
                                LongSentenceDisplay.printLongSentence("I understand. If you have any more questions or need further " +
                                    "assistance in the future, feel free to reach out. Take care and have a great day!");
                                leave = true;
                                break;
                            }
                            else
                            {
                                if (!myDatabase.isNoMeaning(keyword))
                                {
                                    List<int> QType = myDatabase.GetQuestionType(keyword);

                                    if (QType.Count > 0)
                                    {
                                        QList.AddRange(QType);
                                    }
                                    else
                                    {
                                        List<int> MHType = myDatabase.GetMentalHealthType(keyword);
                                        if (MHType.Count > 0)
                                        {
                                            MHList.AddRange(MHType);
                                        }
                                        else
                                        {
                                            myDatabase.addUnknownKeyword(keyword);
                                        }
                                        
                                    }
                                }
                            }
                        }

                        if (leave)
                        {
                            break;
                        }

                        if (QList.Count > 0)
                        {
                            questionType = 0;
                            var QTduplicates = QList.GroupBy(x => x).Select(g => new { Value = g.Key, Count = g.Count() });
                            int QTmaxCount = QTduplicates.Max(d => d.Count);
                            var QTmaxDuplicates = QTduplicates.Where(d => d.Count == QTmaxCount);

                            if (QTmaxDuplicates.Count() > 1)
                            {
                                questionType = -1;

                                foreach (var duplicate in QTmaxDuplicates)
                                {
                                    int id = duplicate.Value;
                                }
                            }
                            else
                            {
                                foreach (var duplicate in QTmaxDuplicates)
                                {
                                    questionType = duplicate.Value;
                                }
                            }
                        }

                        if (MHList.Count > 0)
                        {
                            var MHduplicates = MHList.GroupBy(x => x).Select(g => new { Value = g.Key, Count = g.Count() });
                            int MHmaxCount = MHduplicates.Max(d => d.Count);
                            var MHmaxDuplicates = MHduplicates.Where(d => d.Count == MHmaxCount);
                            if (MHmaxDuplicates.Count() > 1)
                            {
                                mentalHealthType = -1;

                                foreach (var duplicate in MHmaxDuplicates)
                                {
                                    int id = duplicate.Value;
                                }
                            }
                            else
                            {
                                foreach (var duplicate in MHmaxDuplicates)
                                {
                                    mentalHealthType = duplicate.Value;
                                }
                            }
                        }

                        if (questionType < 0 || mentalHealthType <= 0)
                        {
                            LongSentenceDisplay.printLongSentence("I'm sorry, but I need more information to provide a specific answer. " +
                                "Could you please provide additional details or clarify your question? This will help me understand your " +
                                "needs better and generate a more accurate response. Thank you!");
                        }
                        else
                        {
                            string botReply = myDatabase.GetReply(mentalHealthType, questionType);
                            LongSentenceDisplay.printLongSentence(botReply);

                            QList.Clear();
                            MHList.Clear();

                            questionType = 0;
                        }
                    }
                    else
                    {
                        LongSentenceDisplay.printLongSentence("It seems like you didn't enter a message. " +
                            "How can I assist you today? If you have any questions or need guidance, " +
                            "feel free to ask. I'm here to help!");
                    }
                }

                
            }
        }
    }


}



