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

        private string _sourcePath;
        private DateTime _dateTaken = DateTime.MinValue;

        #endregion

        #region Constructors

        public MediaFile(string sourcePath)
        {
            _sourcePath = sourcePath;
        }

	    #endregion

        #region Private Methods

        private DateTime GetDateTaken()
        {
            DateTime dateTaken = DateTime.MinValue;

            if (Path.GetExtension(this.SourcePath).ToUpper() == ".JPG")
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

        #region Public Properties

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
    }
}
