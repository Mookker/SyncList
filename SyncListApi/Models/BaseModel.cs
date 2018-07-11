using System;
using System.ComponentModel.DataAnnotations;

namespace SyncList.SyncListApi.Models
{
    public abstract class BaseModel
    {
        [Range(1, Int32.MaxValue)]
        public int Id { get; set; }
    }
}