using SP.DataLayer.Context;
using SP.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP.Scheduler
{
    public class KeywordScore
    {
        public string keyword { set; get; }
        public decimal score { set; get; }

        public KeywordScore(string keyword, decimal score)
        {
            this.keyword = keyword;
            this.score = score;
        }
    }

    public class SchedulerExtensions
    {
        public static string[] split100(string sentence)
        {
            char[] NonWordCharacters = new char[] { ',', '،', '.', ':', ';', ' ' };
            return string.Join(" ", (sentence ?? "").Split(NonWordCharacters).Select(s => s.Length > 100 ? string.Join(" ", Enumerable.Range(0, s.Length / 100).Select(i => s.Substring(i, 100))) : s)).Split();
        }

        public static void addListScore(IUnitOfWork _uow, List<KeywordScore> listKeywords, string keyword, decimal score, List<string> KeywordExceptions, bool addTraslate = true)
        {
            if (!string.IsNullOrEmpty(keyword) && !KeywordExceptions.Any(a => a == keyword))
            {
                if(addTraslate)
                {
                    var translate = _uow.Set<Rel_KeywordTranslate>().AsQueryable().Where(w => w.keywordTranslateParentWord == keyword).Select(s => s.keywordTranslateChildWord).ToList();
                    if (translate.Count > 0)
                    {
                        foreach (var k in translate)
                        {
                            addListScore(_uow, listKeywords, split100(k), score, KeywordExceptions, false);
                        }
                    }
                }
               
                var list = split100(keyword);
                if (list.Length == 1)
                {
                    decimal allScore = listKeywords.Where(w => w.keyword == keyword).Sum(s => s.score) + score;
                    listKeywords.RemoveAll(f => f.keyword == keyword);
                    listKeywords.Add(new KeywordScore(keyword, allScore));
                }
                else
                    addListScore(_uow, listKeywords, list, score, KeywordExceptions, addTraslate);
            }
        }

        public static void addListScore(IUnitOfWork _uow, List<KeywordScore> listKeywords, string[] keywords, decimal score, List<string> KeywordExceptions, bool addTraslate = true)
        {
            foreach (var keyword in keywords)
            {
                var list = split100(keyword);
                if (list.Length == 1)
                    addListScore(_uow, listKeywords, keyword, score, KeywordExceptions, addTraslate);
                else
                    addListScore(_uow, listKeywords, list, score, KeywordExceptions, addTraslate);
            }

        }
    }
}
