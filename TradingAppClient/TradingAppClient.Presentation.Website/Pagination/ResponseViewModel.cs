using NPOI.SS.Formula.Functions;

namespace TradingAppClient.Presentacion.Website.Pagination
{
    public class ResponseViewModel<T>
    {
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }

        public ResponseViewModel(T data)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }
        public ResponseViewModel()
        {
        }
    }
}
