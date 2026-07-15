using System;
using System.IO;
using System.Net;
using WinSCP;

namespace EDDY.IS.DeliveryEngine.Workflow.Activities.Helper
{
    public class FTPSender
    {
        public string FileName { get; set; }
        public string FTPServerIP { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string FTPDirectory { get; set; }
        public bool UseSecureProtocol { get; set; }

        public string FileProtocol { get; set; }
        public string HostKey { get; set; }

        public FTPSender()
        {
            UseSecureProtocol = false;
            FTPDirectory = "";
        }

        public bool Upload()
        {          

            if (FileProtocol == "SFTP")
                return UploadSFTP();
            else if (FileProtocol == "FTP")
                return UploadFTP();
            else throw new Exception("No FTP File Protocol associated!");            
        }

        #region privates

        private string StripLeadingAndEndingSlashes(string inputString)
        {
            string returnString = inputString;

            //if left "fttp://" hack it off
            if (returnString.Length > 6)
            {
                if (returnString.ToLower().Substring(0, 6) == "ftp://")
                {
                    returnString = returnString.Substring(6);
                }
            }

            //if left is "/" hack it off
            if (returnString.Substring(0, 1) == "/")
            {
                returnString = returnString.Substring(1);
            }

            //if right is "/" hack it off
            if (returnString.Substring(returnString.Length - 1, 1) == "/")
            {
                returnString = returnString.Substring(0, returnString.Length - 1);
            }

            return returnString;
        }

        private bool UploadSFTP()
        {
            bool success = false;

            // Setup session options
            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = FTPServerIP,
                UserName = UserId,
                Password = Password,
                SshHostKeyFingerprint = HostKey
            };

            using (Session session = new Session())
            {
                session.Open(sessionOptions);
                TransferOptions transferOptions = new TransferOptions();
                transferOptions.TransferMode = TransferMode.Binary;
                TransferOperationResult transferResult;
                transferResult = session.PutFiles(FileName, FTPDirectory, false, transferOptions);

                success = transferResult.IsSuccess;
            }

            return success;
        }

        private bool UploadFTP()
        {
            bool success = false;

            //Get File Info
            FileInfo fileInfo = new FileInfo(FileName);

            //Build URI
            //Add FTP protocol and IP
            string uri = "ftp://" + StripLeadingAndEndingSlashes(FTPServerIP);
            //Add Directory (if present)
            if (!String.IsNullOrWhiteSpace(FTPDirectory))
            {
                uri += "/" + StripLeadingAndEndingSlashes(FTPDirectory);
            }
            //Add file Name
            uri += "/" + fileInfo.Name;

            FtpWebRequest ftpWebRequest;

            // Create FtpWebRequest object from the Uri provided
            ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

            //Create Credentials if given
            if (UserId != null && Password != null)
            {
                // Provide the WebPermission Credintials
                ftpWebRequest.Credentials = new NetworkCredential(UserId, Password);
            }

            // By default KeepAlive is true, where the control connection is not closed after a command is executed.
            ftpWebRequest.KeepAlive = false;

            // Specify the command to be executed.
            ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;

            // Specify the data transfer type.
            ftpWebRequest.UseBinary = true;

            // Notify the server about the size of the uploaded file
            ftpWebRequest.ContentLength = fileInfo.Length;

            // The buffer size is set to 2kb
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;

            // Opens a file stream (System.IO.FileStream) to read the file to be uploaded
            FileStream fs = fileInfo.OpenRead();

            // Stream to which the file to be upload is written
            Stream strm = ftpWebRequest.GetRequestStream();

            // Read from the file stream 2kb at a time
            contentLen = fs.Read(buff, 0, buffLength);

            // until Stream content ends
            while (contentLen != 0)
            {
                // Write Content from the file stream to the FTP Upload Stream
                strm.Write(buff, 0, contentLen);
                contentLen = fs.Read(buff, 0, buffLength);
            }

            // Close the file stream and the Request Stream
            strm.Close();
            fs.Close();

            //success
            success = true;



            return success;
        }

        #endregion

    }
}