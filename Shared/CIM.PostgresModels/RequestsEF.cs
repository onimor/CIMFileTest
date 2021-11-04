using System;

namespace CIM.PostgresModels
{
    public class RequestsEF
    {
        public int Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? AddedByName {  get; set; }
        public int? AddedById {  get; set; }
        public string? CustomerName {  get; set; }
        public int? CustomerId {  get; set; }
        public string? СontactPerson { get; set; }
        public string? Telphone { get; set; }
        public string? Mail { get; set; }
        public string? Сontent {  get; set; }
        public string? ResponsibleName {  get; set; }
        public int? ResponsibleId {  get; set; }
        public string? ExecutorName {  get; set; }
        public int? ExecutorId {  get; set; }
        public string? StatusName { get; set; }
        public int? StatusId { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsRemove { get; set; }

    }
}
