using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatbotAPI.DbFill {
        public sealed class CQAMap : ClassMap<CQA> {
            public CQAMap() {
                Map(x => x.Category).Name("Category");
                Map(x => x.Question).Name("Question");
                Map(x => x.Answer).Name("Answer");
            }
        }
    }
