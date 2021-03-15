﻿using ChatbotAPI.Data;
using ChatbotAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatbotAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase {
        private readonly ChatbotAPIContext _context;

        public ChatbotController(ChatbotAPIContext context) {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Answer>> AskQuestion(QuestionText questionText) {
            //TODO: QUESTION WILL RUN THROUGH THE CHATBOT HERE; FOR NOW, RANDOM ANSWER WILL BE GIVEN
            Random _random = new Random();
            int num = _random.Next(196) + 1;
            var answer = await _context.Answer.FindAsync(num);

            if (answer == null) {
                return NotFound();
            }

            return answer;
        }
        public class QuestionText {
            public string Text { get; set; }
        }
        public class AnswerText {
            public string Text { get; set; }
        }
    }
}

