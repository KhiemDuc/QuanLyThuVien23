using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Common
{
    public class PagingInput<T> where T : class // Đầu vào của paging
    {
        /// <summary>
        /// Số lượng kết quả tối đa
        /// </summary>
        [Range(-int.MaxValue, int.MaxValue)]
        public virtual int MaxResultCount { get; set; } = 10; // Số lượng bản ghi tối đa pageSize
        /// <summary>
        /// Bỏ qua số lượng (đầu trang).
        /// </summary>
        [Range(0, int.MaxValue)]
        public virtual int SkipCount { get; set; }//Số lượng bản ghi bỏ qua
        /// <summary>
        /// Sắp xếp
        /// </summary>
        public virtual string Sorting { get; set; }
        /// <summary>
        /// Bộ lọc theo từng đối tượng
        /// </summary>
        public T Filter { get; set; }
        public PagingInput(T filter = null) // Đối tượng lọc
        {
            this.Filter = filter;
        }
    }
}
