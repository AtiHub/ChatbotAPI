using ChatbotAPI.Data;
using ChatbotAPI.Models;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatbotAPI.DbFill {
    public class CQA {
        public string Category { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        public static List<CQA> ReadCSVFile(string location) {
            try {
                using (var reader = new StreamReader(location, Encoding.Default))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)) {
                    var records = csv.GetRecords<CQA>().ToList();
                    return records;
                }
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }
        }

        public static void ReadAndPrint() {
            List<CQA> List = CQA.ReadCSVFile("./DbFill/SorularCevaplar.csv");
            foreach (var Cqa in List) {
                string Row = Cqa.Category + ", " + Cqa.Question + ", " + Cqa.Answer;
                System.Diagnostics.Debug.WriteLine(Row);
            }
        }

        public static void ReadAndAdd(ChatbotAPIContext context) {
            if (context is null) {
                throw new ArgumentNullException(nameof(context));
            }

            List<CQA> List = CQA.ReadCSVFile("./DbFill/SorularCevaplar.csv");

            var Categories = List.Select(o => o.Category);
            var CategoriesDistinct = Categories.Distinct().ToList();
            var Questions = List.Select(o => o.Question);
            var Answers = List.Select(o => o.Answer);
            /*
            int x = 1;
            foreach (var c in CategoriesDistinct) {
                var Category = new Category { Id = x, Text = c };
                context.Category.Add(Category);
                x += 1;
            }
            context.SaveChangesAsync();
            
            for(int y = 1; y <= Questions.Count(); y += 1) {
                int CategoryId = CategoriesDistinct.FindIndex(a => a == Categories.ElementAt(y)) + 1;
                var Category = new Category { Id = CategoryId, Text = Categories.ElementAt(y) + "" };
                var Answer = new Answer { Id = y, Text = Answers.ElementAt(y) + "" };
                var Question = new Question { Id = y, CategoryId = CategoryId, Answer = Answer, Text = Questions.ElementAt(y) + "" };
                context.Answer.Add(Answer);
                context.Question.Add(Question);
            }
            
            int z = 1;
            foreach (var a in Answers) {
                var Answer = new Answer { Id = z, Text = a };
                context.Answer.Add(Answer);
                z += 1;
            }
            context.SaveChanges();*/
            /*
            int k = 1;
            foreach (var qca in List) {
                int CategoryId = CategoriesDistinct.FindIndex(a => a == Categories.ElementAt(k - 1)) + 1;
                var Question = new Question { Id = k, CategoryId = CategoryId, AnswerId = k, Text = Questions.ElementAt(k - 1) + "" };
                context.Question.Add(Question);
                k += 1;
            }
            context.SaveChanges();*/
        }
    }
}
