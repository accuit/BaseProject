using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.PersistenceLayer.Repository.Entities
{
   public class AnswerAttribute
    {
        public string Answer { get; set; }
        public string Sequence { get; set; }
        public bool IsAffirmative { get; set; }// Manoranjan SDCE-4241
    }
}
