using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.QLTVExceptions
{
    public class PhieuMuon_SachException : Exception
    {
       public PhieuMuon_SachException() { }
        public PhieuMuon_SachException(string message) : base(message) { 
        
        }
    }
}
