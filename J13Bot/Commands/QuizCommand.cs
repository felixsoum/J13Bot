using System;
using System.Collections.Generic;
using Discord.WebSocket;

namespace J13Bot.Commands
{
    class QuizEntry
    {
        public string Question { get; set; }
        public string Answer { get; set; }

        public QuizEntry(string question, string answer)
        {
            Question = question;
            Answer = answer;
        }
    }

    class QuizCommand : BaseCommand
    {
        List<QuizEntry> entries = new List<QuizEntry>()
        {
            new QuizEntry("What is the worst-case running time of insertion sort?\n__A__: O(1),    __B__: O(n),    __C__: O(nlog(n)),    __D__: O(n^2)", "D"),
            new QuizEntry("What is the best-case running time of insertion sort?\n__A__: O(1),    __B__: O(n),    __C__: O(nlog(n)),    __D__: O(n^2)", "B"),
            new QuizEntry("What is the worst-case running time of merge sort?\n__A__: O(1),    __B__: O(n),    __C__: O(nlog(n)),    __D__: O(n^2)", "C"),
            new QuizEntry("What is the best-case running time of merge sort?\n__A__: O(1),    __B__: O(n),    __C__: O(nlog(n)),    __D__: O(n^2)", "C"),
            new QuizEntry("What is the worst-case running time of heapsort?\n__A__: O(1),    __B__: O(n),    __C__: O(nlog(n)),    __D__: O(n^2)", "C"),
            new QuizEntry("Which of the following cannot sort in-place?\n__A__: insertion sort,    __B__: merge sort,    __C__: heapsort,    __D__: quicksort", "C"),
            new QuizEntry("In analysis, what does the notation O() represent?\n__A__: Asymptotic upper bound,    __B__: Asymptotic lower bound,    __C__: both", "A"),
            new QuizEntry("In analysis, what does the notation Ω() represent?\n__A__: Asymptotic upper bound,    __B__: Asymptotic lower bound,    __C__: both", "B"),
            new QuizEntry("In analysis, what does the notation Θ() represent?\n__A__: Asymptotic upper bound,    __B__: Asymptotic lower bound,    __C__: both", "C"),
            new QuizEntry("Who was Gauss?", "mathematician"),
            new QuizEntry("0101 + 1111 = ?", "10100"),
            new QuizEntry("log2(512) = ?", "9"),
            new QuizEntry("log2(1024) = ?", "10"),
            new QuizEntry("log2(2048) = ?", "11"),
            new QuizEntry("What is the name of this symbol: Θ", "theta"),
            new QuizEntry("What is the name of this symbol: Ω", "omega"),
        };

        Random random = new Random();

        public QuizCommand() : base("quiz")
        {
        }

        public override void OnCommand(List<string> stringParams, SocketUserMessage message)
        {
            int index = random.Next(entries.Count);
            var quizEntry = entries[index];
            gameData.CurrentQuizAnswer = quizEntry.Answer;
            string reply = $"Q{index + 1}) {quizEntry.Question}";
            reply += "\nReply using the command: answer ____";
            message.Channel.SendMessageAsync(reply);
        }
    }
}
