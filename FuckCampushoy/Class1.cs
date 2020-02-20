using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuckCampushoy
{
    public class QRLoginData
    {
        public int noLogin;
        public string qrId;
    }
    public class QRLogin
    {
        public int errCode;
        public string errMsg;

        public QRLoginData data;
    }
    
    public class QRValidation
    {
        public int status;
        public string redirectUrl;
    }
    public class QRLoginValidation
    {
        public int errCode;
        public string errMsg;

        public QRValidation data;
    }

    public class CollectorItem
    {
        public int noticeWid;
        public int priority;
        public string subject;
        public string content;
        public int isNeedManual;
        public string senderUserName;
        public string createTime;
        public string endTime;
        public string currentTime;
        public int isRead;
        public int isHandled;
        public string startTime;
        public string stopTime;
    }
    public class Collector
    {
        public int totalSize;
        public int pageSize;
        public int pageNumber;
        CollectorItem[] rows;
    }
    public class CollectorList
    {
        public string code;
        public string message;
        public Collector datas;
    }

}
