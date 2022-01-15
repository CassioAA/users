namespace Manager.API.ViewModels
{
    public class ResultViewModel {

        public string Message { get; set; }

        public bool Success { get; set; }

        // type equivalent to "var" type
        public dynamic Data { get; set; }

    }
}