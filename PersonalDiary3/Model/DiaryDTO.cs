using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalDiaryUpdater
{
    public class DiaryDTO
    {
        String title;
        String content;
        String savedate;
        String modifydate;

        public string Title { get => title; set => title = value; }
        public string Content { get => content; set => content = value; }
        public string Savedate { get => savedate; set => savedate = value; }
        public string Modifydate { get => modifydate; set => modifydate = value; }

        public DiaryDTO(string title, string content, string savedate, string modifydate)
        {
            this.title = title;
            this.content = content;
            this.savedate = savedate;
            this.modifydate = modifydate;
        }

        public DiaryDTO() { }

    }
}
