namespace ActorManagement.Models
{
    public class ActorsResponses
    {
        public List<Error> Errors { get; set; }
        public int StatusCode { get; set; }
        public string TraceId { get; set; }
        public bool IsSuccess { get; set; }
        public List<Actor> Actors { get; set; }
    }
}
