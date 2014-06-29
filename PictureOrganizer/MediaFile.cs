using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureOrganizer
{
    public class MediaFile
    {
        #region Constants

        public const int DateTakenPropertyId = 36867;

	    #endregion

        #region Private Fields

        private Lazy<FileTypes> _fileType;
        private string _sourcePath;
        private DateTime _dateTaken = DateTime.MinValue;

        #endregion

        #region Constructors

        public MediaFile(string sourcePath)
        {
            _fileType = new Lazy<FileTypes>(this.GetFileType, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
            _sourcePath = sourcePath;
        }

	    #endregion

        #region Public Properties

        public static List<string> PictureExtensions = new List<string>() { ".JPG", ".PNG", ".GIF" };
        public static List<string> VideoExtensions = new List<string>() { ".MOV" };

        public FileTypes FileType
        {
            get
            {
                return _fileType.Value;
            }
        }

        public string SourcePath
        {
            get
            {
                return _sourcePath;
            }
        }

        public string SourceFileName
        {
            get
            {
                return System.IO.Path.GetFileName(this.SourcePath);
            }
        }

        public bool AllowCreateDestinationFolder
        {
            get
            {
                return true;
            }
        }

        public DateTime DateTaken
        {
            get
            {
                if (_dateTaken == DateTime.MinValue)
                {
                    _dateTaken = GetDateTaken();
                }

                return _dateTaken;
            }
        }

        #endregion

        #region Private Methods

        private FileTypes GetFileType()
        {
            FileTypes ft = FileTypes.Unknown;
            string extension = System.IO.Path.GetExtension(this.SourceFileName);

            if (MediaFile.PictureExtensions.Any(pe => string.Compare(pe, extension, true) == 0))
            {
                ft = FileTypes.Picture;
            }
            else if (MediaFile.VideoExtensions.Any(pe => string.Compare(pe, extension, true) == 0))
            {
                ft = FileTypes.Video;
            }

            return ft;
        }

        private DateTime GetDateTaken()
        {
            DateTime dateTaken = DateTime.MinValue;

            if (this.FileType == FileTypes.Picture)
            {
                using (Image myImage = Image.FromFile(this.SourcePath))
                {
                    if (myImage.PropertyIdList.Contains(DateTakenPropertyId))
                    {
                        PropertyItem propItem = myImage.GetPropertyItem(DateTakenPropertyId);

                        //Convert date taken metadata to a DateTime object
                        string sdate = Encoding.UTF8.GetString(propItem.Value).Trim();
                        string secondhalf = sdate.Substring(sdate.IndexOf(" "), (sdate.Length - sdate.IndexOf(" ")));
                        string firsthalf = sdate.Substring(0, 10);
                        firsthalf = firsthalf.Replace(":", "-");
                        sdate = firsthalf + secondhalf;
                        dateTaken = DateTime.Parse(sdate);
                    }
                }
            }

            if (dateTaken == DateTime.MinValue)
            {
                var dateCreated = File.GetCreationTime(this.SourcePath);
                var dateModified = File.GetLastWriteTime(this.SourcePath);
                
                if (dateCreated < dateModified)
                {
                    dateTaken = dateCreated;
                }
                else
                {
                    dateTaken = dateModified;
                }
            }

            return dateTaken;
        }

        #endregion
    }
}
