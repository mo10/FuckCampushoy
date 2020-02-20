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
        public string wid;
        public string formWid;
        public string priority;
        public string subject;
        public string content;
        public string senderUserName;
        public string createTime;
        public string endTime;
        public string currentTime;
        public int isHandled;
        public int isRead;
    }
    public class Collector
    {
        public int totalSize;
        public int pageSize;
        public int pageNumber;
        public CollectorItem[] rows;
    }
    public class CollectorList
    {
        public string code;
        public string message;
        public Collector datas;
    }


    public class FormRow
    {
        public string wid;
        public string formWid;
        public int fieldType;
        public string title;
        public string description;
        public int minLength;
        public string sort;
        public int maxLength;
        public int isRequired;
        public object imageCount;
        public int hasOtherItems;
        public string colName;
        public string value;
        public object fieldItems;
    }
    public class FormData
    {
        public int totalSize;
        public int pageSize;
        public int pageNumber;
        public FormRow[] rows;
    }
    public class FormMessage
    {
        public string code;
        public string message;
        public FormData datas;
    }


    public class SendReq
    {
        public string code;
        public string message;
    }
}
