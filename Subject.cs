using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutyiMutyiDB
{
    class Subject
    {
        public int ID { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public int Credit { get; set; }
        public string TypeOfSubjectRequirement { get; set; }
        public string TeacherID { get; set; }
        public string InstituteResponsibleForTheSubject { get; set; }
		
		//ez itten a tárggy
    }
}
