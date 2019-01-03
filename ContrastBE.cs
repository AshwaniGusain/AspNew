using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contrast_Web
{
    public class ContrastBE
    {
        private Int32 _CustomerCode;
        public Int32 CustomerCode
        {
            get { return _CustomerCode; }
            set { _CustomerCode = value; }
        }

        private Int32 _JournalCode;
        public Int32 JournalCode
        {
            get { return _JournalCode; }
            set { _JournalCode = value; }
        }

        private string _ArticleCode;
        public string ArticleCode
        {
            get { return _ArticleCode; }
            set { _ArticleCode = value; }
        }

        private string _Volume;
        public string Volume
        {
            get { return _Volume; }
            set { _Volume = value; }
        }

        private string _Issue;
        public string Issue
        {
            get { return _Issue; }
            set { _Issue = value; }
        }

        private Int32 _ProcessId;
        public Int32 ProcessId
        {
            get { return _ProcessId; }
            set { _ProcessId = value; }
        }
        private Int32 _StageId;
        public Int32 StageId
        {
            get { return _StageId; }
            set { _StageId = value; }
        }

        private Int32 _WinRefID;
        public Int32 WinRefID
        {
            get { return _WinRefID; }
            set { _WinRefID = value; }
        }

        private Int32 _RoundCount;
        public Int32 RoundCount
        {
            get { return _RoundCount; }
            set { _RoundCount = value; }
        }
    }
}
