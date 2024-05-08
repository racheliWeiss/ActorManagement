using Microsoft.AspNetCore.Http;
using System;

namespace ActorManagement.Models
{
    public class ActorResponse
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public Actor Actor { get; set; }
    }
}
