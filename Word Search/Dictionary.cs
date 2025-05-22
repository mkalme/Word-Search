using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Word_Search
{
    class Dictionary
    {
        public static Form1 form;

        public static string[] arrayOfWords;
        public static string[] chosenWords;

        public static char[] chars;

        static int maxNumberOfThreads = 10;
        static int numberOfWordsPerThread = 2000;

        public static void setupList() {
            arrayOfWords = File.ReadAllText(Form1.path).Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static void chooseWords() {
            chars = getChars();

            string words = "";
            words = manageThreads();

            chosenWords = words.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            form.wordsRichTextBox.Text = string.Join("\n", chosenWords);
        }

        public static string manageThreads() {
            string words = "";
            int overall = (int)Math.Ceiling((double)arrayOfWords.Length / (double)(maxNumberOfThreads * numberOfWordsPerThread));

            for (int i = 0; i < overall; i++) {
                int start = (maxNumberOfThreads * numberOfWordsPerThread) * i;
                int end = start + (maxNumberOfThreads * numberOfWordsPerThread) > arrayOfWords.Length ? arrayOfWords.Length : start + (maxNumberOfThreads * numberOfWordsPerThread);
                int remaining = end - start;

                int numberOfThreads = (int)Math.Ceiling((double)remaining / (double)numberOfWordsPerThread);

                string[] array = new string[numberOfThreads];

                List<Task> allTasks = new List<Task>();
                for (int b = 0; b < numberOfThreads; b++) {
                    int copyB = b;
                    Task task = new Task(() =>
                    {
                        int start1 = numberOfWordsPerThread * copyB;
                        start1 = start1 > remaining ? remaining + start: start1 + start;
                        int end1 = (numberOfWordsPerThread * copyB) + numberOfWordsPerThread;
                        end1 = end1 > remaining ? remaining + start : end1 + start;

                        array[copyB] = loopWords(start1, end1);
                    });
                    allTasks.Add(task);
                    task.Start();
                }

                Task.WaitAll(allTasks.ToArray());

                words += "\n" + string.Join("\n", array);
            }
            return words;
        }

        public static string loopWords(int start, int end) {
            string words = "";

            for (int i = start; i < end; i++)
            {
                words += checkIfWordMatches(arrayOfWords[i]) + (i == arrayOfWords.Length - 1 ? "" : "\n");
            }

            return words;
        }

        public static string checkIfWordMatches(string word) {
            string result = "";

            if (word.Length == chars.Length) {
                for (int i = 0; i < chars.Length; i++) {
                    if (chars[i] != word[i] && chars[i] != '-')
                    {
                        goto after_loop;
                    }
                }

                result = word;
            }

            after_loop:

            return result;
        }

        public static char[] getChars() {
            char[] charsTemp = new char[form.wordTextBox.Text.Length];
            for (int i = 0; i < charsTemp.Length; i++) {
                charsTemp[i] = form.wordTextBox.Text[i] == '-' ? '-' : form.wordTextBox.Text[i];
            }

            return charsTemp;
        }
    }
}
