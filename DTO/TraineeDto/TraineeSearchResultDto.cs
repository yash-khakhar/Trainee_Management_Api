namespace TraineeManagement.api.DTO.TraineeDto
{
    public class TraineeSearchResultDto
    {

        public TraineeSearchResultDto(int pageNumber, int pageSize, int totalRecords, List<TraineeResponse> data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            Data = data;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public List<TraineeResponse> Data { get; set; }
    }
}
