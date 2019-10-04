using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class ModulesBO
    {
        public int ModuleID
        {
            get;
            set;
        }

        public int ModuleCode
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public bool IsQuestionModule
        {
            get;
            set;
        }
        public Nullable<bool> IsStoreWise
        {
            get;
            set;
        }

        // Added by Navneet on 2/1/2015

        public byte ModuleType
        {
            get;
            set;
        }

        public string ModuleDescription
        {
            get;
            set;
        }
    }
}
